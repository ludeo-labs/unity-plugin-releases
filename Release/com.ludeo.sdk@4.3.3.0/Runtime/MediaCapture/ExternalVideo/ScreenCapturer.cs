/// @cond INTERNAL
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace LudeoSDK.MediaCapture
{
    using LudeoVideoService;
    using LudeoVideoService.ComLib.Configuration;
    using LudeoVideoService.VideoStream;
    using System;
    using System.Collections.Concurrent;
    using System.IO.Pipes;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Rendering;
    using LudeoSDK.Internal;
    using LudeoSDK.UnityScripts;

    //TODO: refactor this class - divide it into multiple classes
    internal class ScreenCapturer
    {
        private const string LOG_PREFIX = "[SCRN_CPTR]";

        public Action OnCaptureInitialized { get; set; }

        public int CurrentVideoFrameNumber { get; private set; }
        private bool IsInitialized { get; set; }
        private int ImageSize => Configuration.Video.Width * Configuration.Video.Height * Configuration.Video.PixelBytes;
        private float CaptureInterval => 1f / Configuration.Video.FrameRate;
        private float TimeSinceLastCapture { get; set; }
        private bool IsCapturing { get; set; }
        private bool HasFirstPipeCreated { get; set; }
        private bool HasCleaned { get; set; }
        private int ConnectionNumber { get; set; }
        private bool IsWaitingForConnection { get; set; }
        private VideoServiceConfiguration Configuration { get; set; }
        private CustomNamedPipeClientStream MessageStream { get; set; }
        private INativePipeServer ImageStream { get; set; }
        private MessageStreamHandler MessageStreamHandler { get; set; }
        private ConcurrentQueue<VideoMessage> MessageQueue { get; set; }
        private ConcurrentQueue<VideoFrame> FrameQueue { get; set; }
        private CancellationTokenSource CancellationTokenSource { get; set; }
        private NativeArrayPool<byte> ImagePool { get; set; }
        private CommandBuffer CommandBuffer { get; set; }
        private INativeSemaphore Semaphore { get; set; }

        private ScreenCapturer() { }

        public ScreenCapturer(VideoServiceConfiguration configuration)
        {
            INativePipeClient customNamedPipeClient = LudeoUnityManager.LudeoInterfaceFactory.CreateINativePipeClient();

            Configuration = configuration;
            CancellationTokenSource = new CancellationTokenSource();
            MessageStream = new CustomNamedPipeClientStream(customNamedPipeClient, ".", Configuration.Network.MessagePipeName, PipeDirection.Out, PipeOptions.Asynchronous);
            MessageStreamHandler = new MessageStreamHandler(MessageStream);
            MessageQueue = new ConcurrentQueue<VideoMessage>();
            CommandBuffer = new CommandBuffer();

            Semaphore = LudeoUnityManager.LudeoInterfaceFactory.CreateINativeSemaphore();

            if (Semaphore != null)
                Semaphore.Initialize(1, 1, Configuration.Network.PipeConnectionSemaphore);
            else
                throw new Exception($"Failed to initialize INativeSemaphore");

            CommandBuffer.name = "Screen Capture";
            FrameQueue = new ConcurrentQueue<VideoFrame>();
            ImagePool = new NativeArrayPool<byte>(ImageSize, 
                maxCapacity: Configuration.App.MaxImageCapacity);
            ConnectToServer().Forget();
        }

        private async Task ConnectToServer()
        {
            try
            {
                await ConnectAsync();
                IsInitialized = true;
                RunParallelProcessing();
                return;
            }
            catch (OperationCanceledException)
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} Cancelled gracefully.");

            }
            catch (Exception ex)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to connect to video service.", ex.ToString());
            }

            CleanUp();
        }

        private async Task ConnectAsync()
        {
            var attempt = 0;
            // Add initial wait - the named pipe takes sometime until actually up, and without this delay - there tends to be redundant retries
            await Task.Delay(1000);

            while (attempt < Configuration.Network.MaxReconnectionAttempts)
            {
                try
                {
                    await Task.Delay(Configuration.Network.WaitBetweenConnectionTries, CancellationTokenSource.Token);
                    MessageStream.Connect(Configuration.Network.PipeConnectionTimeout);
                    LudeoLogManager.LogInfo($"{LOG_PREFIX} connected to video service!");
                    if (OnCaptureInitialized != null)
                        LudeoUnityMainThreadDispatcher.Enqueue(OnCaptureInitialized);
                    else
                    {
                        CancellationTokenSource.Cancel();
                        throw new Exception($"{LOG_PREFIX} OnCaptureInitialized is null! Can not continue");
                    }
                        
                    return;
                }
                catch (Exception e)
                {
                    attempt++;
                    LudeoLogManager.LogWarning($"{LOG_PREFIX} Attempt '{attempt}' to connect video service failed. Retrying... (exe: {e})");

                    if (attempt >= Configuration.Network.MaxReconnectionAttempts ||
                        CancellationTokenSource.IsCancellationRequested)
                    {
                        LudeoLogManager.LogError($"{LOG_PREFIX} Connect finally failed or canceled and will not start!");
                        throw;
                    }
                }
            }
        }

        private void RunParallelProcessing()
        {
            Task.Factory.StartNew(DequeueAndProcessMessagesAsync,
                CancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default).Forget();

            Task.Factory.StartNew(DequeueAndProcessImagesAsync,
                CancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default).Forget();
        }

        internal void UpdateFrames(float unscaledDeltaTime)
        {
            if(!IsInitialized || !IsCapturing)
                return;

            TimeSinceLastCapture += unscaledDeltaTime;

            if (TimeSinceLastCapture >= CaptureInterval)
            {
                CurrentVideoFrameNumber++;
                CaptureAndStretchFrame();
                TimeSinceLastCapture -= CaptureInterval;
            }
        }

        private void CaptureAndStretchFrame()
        {
            // Clear previous commands
            CommandBuffer.Clear();

            // Create a temporary RenderTexture with the screen dimensions
            RenderTexture tempRenderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            CommandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, tempRenderTexture);

            // Create and set the target RenderTexture
            RenderTexture targetRenderTexture = RenderTexture.GetTemporary(Configuration.Video.Width,
                Configuration.Video.Height, 0, RenderTextureFormat.ARGB32);
            CommandBuffer.SetRenderTarget(targetRenderTexture);

            // Blit and stretch the screen content to the target RenderTexture
            CommandBuffer.Blit(tempRenderTexture, targetRenderTexture);

            // Execute the command buffer
            Graphics.ExecuteCommandBuffer(CommandBuffer);

            // Release the temporary RenderTexture as we no longer need it
            RenderTexture.ReleaseTemporary(tempRenderTexture);

            // Save the frame at which you started the capture
            var currentFrame = CurrentVideoFrameNumber;

            // Request an asynchronous GPU readback from the stretched RenderTexture
            var nullableArray = ImagePool.Rent();

            // If the pool is full, skip the frame
            if (!nullableArray.HasValue)
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} Image pool is full. Skipping frame.");
                return;
            }

            // Get the array from the nullable struct
            var array = nullableArray.Value;

            // Get the image bytes from GPU and send them to the pipe
            AsyncGPUReadback.RequestIntoNativeArray(ref array, targetRenderTexture, 0, TextureFormat.RGB24,
                (request) => GPUReadAndSendFrame(ref request, ref array, currentFrame, targetRenderTexture));
        }

        private void GPUReadAndSendFrame(ref AsyncGPUReadbackRequest request,
            ref NativeArray<byte> imageBytes,
            int frame,
            RenderTexture renderTexture)
        {
            // Render texture no longer needed, it has been read back.
            RenderTexture.ReleaseTemporary(renderTexture);

            // If no longer capturing, don't send the frame.
            if (!IsCapturing || request.hasError)
            {
                ImagePool.Return(imageBytes);
                return;
            }

            FrameQueue.Enqueue(new VideoFrame { Image = imageBytes, FrameNumber = frame });
        }

        private unsafe void PushImageToPipe(VideoFrame videoFrame, byte[] buffer)
        {
            var ptr = (byte*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(videoFrame.Image);

            // Write the image data first
            Marshal.Copy((IntPtr)ptr, buffer, 0, ImageSize);

            // Write the entire buffer to the pipe
            ImageStream?.Write(buffer, ImageSize);
        }

        private async Task DequeueAndProcessMessagesAsync()
        {
            while (!CancellationTokenSource.IsCancellationRequested)
            {
                while(MessageQueue.TryDequeue(out VideoMessage message))
                {
                    await MessageStreamHandler.WriteMessage(message).ConfigureAwait(false);
                    await Task.Yield();
                }

                await Task.Delay(10, CancellationTokenSource.Token).ConfigureAwait(false);
            }
        }

        private async Task DequeueAndProcessImagesAsync()
        {
            var pooledBuffer = new byte[ImageSize];
            while (!CancellationTokenSource.IsCancellationRequested)
            {
                ConnectToImageStream();
                if (FrameQueue.TryDequeue(out VideoFrame videoFrame))
                {
                    try 
                    {
                        PushImageToPipe(videoFrame, pooledBuffer);
                    }
                    catch(Exception ex)
                    {
                        LudeoLogManager.LogError(ex.ToString());
                    }
                    finally
                    {
                        ImagePool.Return(videoFrame.Image);
                    }
                    continue;
                }

                await Task.Delay(10, CancellationTokenSource.Token).ConfigureAwait(false);
            }
        }

        private void ConnectToImageStream()
        {
            if (ImageStream != null || CancellationTokenSource.IsCancellationRequested)
                return;

            CancellationTokenSource.Token.ThrowIfCancellationRequested();

            ImageStream = LudeoUnityManager.LudeoInterfaceFactory.CreateINativePipeServer();

            if (ImageStream != null )
            {
                ImageStream.Initialize($"{Configuration.Network.ImagePipeName}_{ConnectionNumber}", 
                                                                CancellationTokenSource.Token,
                                                                maxNumberOfInstances: 1, bufferOutSize: 4096, bufferInSize: 4096);
            }
            else 
                throw new Exception($"Failed to initalize INativePipeServer");

            if (HasFirstPipeCreated)
                Semaphore?.Release();

            HasFirstPipeCreated = true;
            IsWaitingForConnection = true;
            ImageStream.WaitForConnection();
            IsWaitingForConnection = false;
            ConnectionNumber++;
        }

        private void CleanImageQueue()
        {
            while (FrameQueue.TryDequeue(out VideoFrame frame)) 
            {
                ImagePool.Return(frame.Image);
            }
        }

        private void CleanMessageQueue()
        {
            while (MessageQueue.TryDequeue(out VideoMessage _)) { }
        }

        public void SendMessage(VideoMessage videoMessage)
        {
            MessageQueue.Enqueue(videoMessage);
        }

        public void SendMessageImmediate(VideoMessage videoMessage, Action callback = null)
        {
            if (!IsInitialized || HasCleaned)
            {
                callback?.Invoke();
                return;
            }

            MessageStreamHandler.WriteMessage(videoMessage)
                .ContinueWith((t) =>
                {
                    callback?.Invoke();
                });//, CancellationTokenSource.Token);
        }

        public void StartCapture()
        {
            if(IsCapturing)
                return;

            IsCapturing = true;
        }

        public void StopCapture()
        {
            if (!IsCapturing)
                return;

            ClearCapture();

            // Waits until a the service disconnects from the pipe
            Semaphore?.WaitOne();
            ImageStream?.Disconnect();
            ImageStream?.Dispose();
            ImageStream = null;
        }

        private void ClearCapture()
        {
            CleanMessageQueue();
            CleanImageQueue();
            CurrentVideoFrameNumber = 0;
            TimeSinceLastCapture = 0f;
            IsCapturing = false;
        }

        internal void CleanUp()
        {
            if(HasCleaned)
                return;

            try
            {
                CancellationTokenSource.Cancel();
                CancellationTokenSource?.Dispose();
            }
            catch(Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} CancellationTokenSource error! {e}");
            }

            try
            {
                ClearCapture();
                ImageStream?.Dispose();
                Semaphore?.Dispose();
                MessageStreamHandler?.Dispose();
                MessageStream?.Dispose();
                ImagePool?.Dispose();
                CommandBuffer?.Dispose();
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} ScreenCapture CleanUp error! {e}");
            }
            HasCleaned = true;
        }
    }
}
/// @endcond INTERNAL