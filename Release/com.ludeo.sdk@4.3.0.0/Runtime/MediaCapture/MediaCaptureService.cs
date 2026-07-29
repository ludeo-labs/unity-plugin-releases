/// @cond INTERNAL
namespace LudeoSDK.MediaCapture
{
    using System.IO;
    using UnityEngine;
    using LudeoSDK.Internal;
    using System;
    using LudeoSDK.Ludeo.Internal.Launchers;
    using System.Runtime.InteropServices;

    internal class MediaCaptureService : IDisposable
    {
        private const string LOG_PREFIX = "[LegacyMediaCapture]";

        private const int INVALID = -1;

        private static MediaCaptureService m_instance;

        private ICaptureService m_captureService;
        private SystemData m_systemData;
        private bool m_isGotService;
        private bool disposedValue;
        private VideoCapture m_allocatedVideoCapture;
        private DummyMediaService m_dummyMediaService;

        internal static MediaCaptureService Instance
        {
            get
            {
                return m_instance;
            }
        }

        public MediaCaptureService(SystemData systemData)
        {
            m_systemData = systemData;
            m_instance = this;
            // Dummy mode is always needed: if core video - we always use it. If legacy - we use when video is not consented (non-creator/opt-out)
            m_dummyMediaService = new DummyMediaService();
            m_captureService = m_dummyMediaService;
        }

        internal void SwitchService(bool isOn, Action<CaptureStatus, bool> onCaptureStatusUpdateDone)
        {
            if (!m_captureService.IsInitialized)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} can not be switched before initialization!");
                m_systemData.captureStatus = CaptureStatus.Uninitialized;
                onCaptureStatusUpdateDone(m_systemData.captureStatus, false);
                return;
            }
            
            MediaCaptureSupport mediaCaptureToInject = MediaCaptureSupport.Dummy;

            if (isOn)
                mediaCaptureToInject = m_systemData.supportedMediaCapture;

            LudeoLogManager.LogInfo($"{LOG_PREFIX} switching to: {mediaCaptureToInject}");

            InjectService(mediaCaptureToInject);

            m_allocatedVideoCapture.UpdateConfiguration(() => 
            {
                LudeoLogManager.LogInfo($"{LOG_PREFIX} UpdateConfiguration complete!");
                m_systemData.captureStatus = CaptureStatus.InitSuccess;
                onCaptureStatusUpdateDone(m_systemData.captureStatus, isOn); 
            });
        }

        internal void InitializeCaptureServices(Action<CaptureStatus> onInitializationComplete)
        {
            LudeoLogManager.LogVerbose($"{LOG_PREFIX} InitializeCaptureServices.");

            string captureFolder = Path.Combine(Application.persistentDataPath, "MediaCaptureFolder");
            m_systemData.captureFolder = captureFolder;

            // If we are not using core video, we have to initialize the Legacy one.
            if (!m_systemData.isCoreVideoEnabled)
            {
                // Dummy mode initialization is immidiate so wrapper need not await it 
                m_dummyMediaService.Initialize(m_systemData, captureFolder, (captureStaus) => { LudeoLogManager.LogVerbose($"{LOG_PREFIX} DummyService: {captureStaus}"); });

                m_allocatedVideoCapture = new VideoCapture();
                m_allocatedVideoCapture.Initialize(m_systemData, captureFolder, (captureStatus) =>
                {
                    // Once initialized - we set back to dummy, until updated otherwise from Core SDK.
                    InjectService(MediaCaptureSupport.Dummy);

                    onInitializationComplete(captureStatus);
                });
            }
            else
                m_dummyMediaService.Initialize(m_systemData, captureFolder, onInitializationComplete);
        }

        public void BeginCapture()
        {
            m_captureService.BeginCapture(m_systemData);
        }

        public void TagVideoWithUploadData(string highlightId, string uploadUrl)
        {
            if (m_captureService.TagCaptureWithHighlightData(highlightId, uploadUrl) == INVALID)
                LudeoLogManager.LogWarning($"{LOG_PREFIX} Failed calling capture service with MarkHighlight data!");
        }

        public void EndCapture()
        {
            m_captureService.EndCapture(m_systemData);
        }

        public void DisposeIfNeeded()
        {
            m_isGotService = false;

            if (m_allocatedVideoCapture != null)
            {
                DisposeCaptureService(m_allocatedVideoCapture);
                m_allocatedVideoCapture = null;
            }

            if (m_dummyMediaService != null)
            {
                DisposeCaptureService(m_dummyMediaService);
                m_dummyMediaService = null;
            }

            m_captureService = null;
            m_instance = null;
            disposedValue = true;
        }

        internal void UpdateFrames(float unscaledDeltaTime)
        {
            if (m_isGotService)
                m_captureService.UpdateFrames(unscaledDeltaTime);
        }

        private void InjectService(MediaCaptureSupport captureServiceSupport)
        {
            switch (captureServiceSupport)
            {                
                case MediaCaptureSupport.NewVideo:
                    if (m_allocatedVideoCapture != null)
                        m_captureService = m_allocatedVideoCapture;
                    else
                    {
                        LudeoLogManager.LogError($"{LOG_PREFIX} Invalid operation! Legacy Video needs to be allocated at creation!");
                        m_captureService = m_dummyMediaService;
                    }
                    
                    break;

                case MediaCaptureSupport.Dummy:
                    m_captureService = m_dummyMediaService;
                    break;

                default:
                    LudeoLogManager.LogError($"{LOG_PREFIX} Unhandled {captureServiceSupport}");
                    m_captureService = m_dummyMediaService;
                    break;
            }

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} Set Media service to use: {captureServiceSupport}");
            m_isGotService = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeIfNeeded();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        private void DisposeCaptureService(ICaptureService captureService)
        {
            int disposeResult = captureService.Dispose();
            LudeoLogManager.LogVerbose($"Dispose {captureService.GetType()} result = {disposeResult}");

            captureService.CleanUp();
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MediaCaptureService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
/// @endcond INTERNAL