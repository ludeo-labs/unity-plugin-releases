/// @cond INTERNAL
namespace LudeoSDK.MediaCapture
{
    using LudeoSDK.Internal;
    using LudeoSDK.UnityScripts;
    using LudeoVideoService;
    using LudeoVideoService.ComLib.Configuration;
    using System;
    using System.IO;
    using System.Reflection;
    using UnityEngine;

    internal class VideoCapture : ICaptureService
    {
        private const string LOG_PREFIX = "[NEW_VIDEO]";

        private static VideoCapture m_instance;

        public bool IsInitialized { get; private set; }

        private ScreenCapturer ScreenCapturer { get; set; }

        private VideoServiceConfiguration VideoServiceConfiguration { get; set; }

        private IProcessRunner VideoRecorder { get; set; }

        private bool IsDebugMode { get; set; }

        private SystemData m_systemData;

        private Action m_onVideoConfigUpdated;

        private readonly string UniqueIdForInstance = Guid.NewGuid().ToString("N");

        public VideoCapture()
        {
            m_instance = this;
            //TODO: Introduce a pre-compilation header to enable video debug mode
            IsDebugMode = false;
        }

        ~VideoCapture()
        {
            m_instance = null;
        }

        public void Initialize(SystemData systemData, string videoDirectory, Action<CaptureStatus> onCaptureStatusUpdate)
        {
            IsInitialized = true;
            m_systemData = systemData;

            if (ScreenCapturer != null)
                ScreenCapturer.CleanUp();

            if (VideoRecorder != null)
                VideoRecorder.StopProcess();

            VideoConfigurationGenerator generator = new VideoConfigurationGenerator();
            VideoServiceConfiguration = generator.GetConfiguration(systemData, videoDirectory, IsDebugMode);

            PostFixPipeNames(VideoServiceConfiguration, UniqueIdForInstance);

            string executablePath = GetServiceExePath();

            // Add launch arguments            
            string jsonConfig = JsonUtility.ToJson(VideoServiceConfiguration);
            string launchArguments = $"\"{jsonConfig.Replace("\"", "\\\"")}\"";

            string launchOptions = "-hidewindow";
            //    string launchOptions = "";

            // Runs the video service only if not in debug mode
            // (Because in debug mode you run the video service manually)
            if (!IsDebugMode)
            {
                VideoRecorder = LudeoUnityManager.LudeoInterfaceFactory.CreateIProcessRunner();

                if (VideoRecorder != null)
                    VideoRecorder.Initialize(executablePath, launchArguments, launchOptions, OnServiceError);
                else
                    throw new Exception($"Failed to initialize IProcessRunner");

                VideoRecorder.StartProcess();
            }

            ScreenCapturer = new ScreenCapturer(VideoServiceConfiguration);
            ScreenCapturer.OnCaptureInitialized = () =>
            {
                LudeoLogManager.LogInfo($"{LOG_PREFIX} Initialized!");

                onCaptureStatusUpdate(CaptureStatus.InitSuccess);
            };   
        }

        public void UpdateConfiguration(Action onComplete)
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} Sending VideoMessageType.UpdateConfig!");

            m_onVideoConfigUpdated = onComplete;

            ScreenCapturer.SendMessageImmediate(new VideoMessage
            {
                MessageType = VideoMessageType.UpdateConfig,
                Payload = new VideoConfigUpdatePayload
                {
                    CaptureDuration = m_systemData.ludeoSessionVideoData.maxHighlightDuration,
                    ApiUpdateUploadUrl = m_systemData.ludeoSessionVideoData.apiUpdateUploadUrl,
                    AppToken = m_systemData.userToken,
                }
            },
            FireCallbackFromMainThread);
        }

        private void FireCallbackFromMainThread()
        {
            LudeoUnityMainThreadDispatcher.Enqueue(m_onVideoConfigUpdated);
        }

        public void CleanUp()
        {
            if (ScreenCapturer != null)
                ScreenCapturer.CleanUp();
            else
                LudeoLogManager.LogError($"{LOG_PREFIX} ScreenCapturer already disposed!");
        }

        public void UpdateFrames(float unscaledDeltaTime)
        {
            ScreenCapturer.UpdateFrames(unscaledDeltaTime);
        }

        public int BeginCapture(SystemData systemData)
        {
            ScreenCapturer.SendMessageImmediate(new VideoMessage
            {
                MessageType = VideoMessageType.StartCapture
            }, () => ScreenCapturer.StartCapture());
            LudeoLogManager.LogInfo($"{LOG_PREFIX} Starting Capture!");

            return 0;
        }

        public int Dispose()
        {           
            ScreenCapturer?.SendMessageImmediate(new VideoMessage
            {
                MessageType = VideoMessageType.Shutdown
            });

            LudeoLogManager.LogInfo($"{LOG_PREFIX} Recording shutting down!");
            
            return 0;
        }

        public int EndCapture(SystemData systemData)
        {
            ScreenCapturer.SendMessageImmediate(new VideoMessage
            {
                MessageType = VideoMessageType.StopCapture
            }, () => ScreenCapturer.StopCapture());
            LudeoLogManager.LogInfo($"{LOG_PREFIX} Stopping Capture!");

            return 0;
        }

        public int TagCaptureWithHighlightData(string highlightId, string uploadUrl)
        {
            int currentFrame = ScreenCapturer.CurrentVideoFrameNumber + 1;

            ScreenCapturer.SendMessage(new VideoMessage
            {
                MessageType = VideoMessageType.CaptureHighlight,
                Payload = new HighlightPayload
                {
                    PublishUrl = uploadUrl,
                    HighlightId = highlightId,
                    StartFrame = Mathf.Max(currentFrame - (int)(VideoServiceConfiguration.Video.FrameRate *
                        VideoServiceConfiguration.Video.CaptureDuration), 0),
                    EndFrame = currentFrame
                }
            });

            LudeoLogManager.LogInfo($"{LOG_PREFIX} Highlight (id: {highlightId}) uploading!");

            return 0;
        }

        private void PostFixPipeNames(VideoServiceConfiguration configuration, string postFix)
        {
            configuration.Network.ImagePipeName += postFix;
            configuration.Network.MessagePipeName += postFix;
            configuration.Network.PipeConnectionSemaphore += postFix;
        }

        private void OnServiceError(string error)
        {
            LudeoLogManager.LogError($"{LOG_PREFIX} {error}");
        }

        private string GetServiceExePath()
        {
            IOSSystems osSys = LudeoUnityManager.LudeoInterfaceFactory.GetIOSSystems();
            if (osSys == null)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} failed to create IOSSystems!");
                return null;
            }

            string filePathByPlatform = osSys.GetRuntimeVideoServiceExePath();

            if (Application.isEditor)
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                string rootPath = Path.GetDirectoryName(assemblyLocation);
                rootPath = Directory.GetParent(rootPath).Parent.FullName;
                return Path.Combine(rootPath, filePathByPlatform);
            }
            else
                return Path.Combine(Application.dataPath, filePathByPlatform);
        }

        private static void StartCapture()
        {
            if (m_instance != null)
                m_instance.ScreenCapturer.StartCapture();
            else
                LudeoLogManager.LogError("May not StartCaptue without a VideoCapture instance!");
        }

        private static void StopCapture()
        {
            if (m_instance != null)
                m_instance.ScreenCapturer.StopCapture();
            else
                LudeoLogManager.LogError("May not StopCapture without a VideoCapture instance!");
        }
    }
}
/// @endcond INTERNAL