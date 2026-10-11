/// @cond INTERNAL
namespace LudeoSDK
{
    using UnityEngine;
    using LudeoSDK.MediaCapture;
    using LudeoSDK.Internal;
    using LudeoSDK.Ludeo.Internal.Launchers;
    using System;

    internal class SystemData : IDisposable
    {
        private const string LOG_PREFIX = "[SYSTEM DATA]";

        internal struct GameData
        {
            public string apiKey;
        }

        public static SystemData Instance
        {
            get
            {
                if (m_instance == null)
                    Debug.LogError("You must call Ludeo.Initialize before using LudeoSDK.");
                return m_instance;
            }
        }
        private static SystemData m_instance;

        public LocalConfigData localConfigData;
        public GameData gameData;        
        public int processInstanceId;

        #region Wrapper-Video-support
        public bool isCoreVideoEnabled;
        public MediaCaptureSupport supportedMediaCapture;
        public CaptureStatus captureStatus;
        public LudeoSessionVideoStateCallbackData ludeoSessionVideoData;
        public string captureFolder;
        public string userToken;
        #endregion Wrapper-Video-support

        public string logFile;
        public string fullConfig;

        internal LudeoTokens ludeoTokens;
        internal LaunchableProviderType launchableProviderType;
        internal ILauncherProvider launchableProvider;
        private bool disposedValue;

        internal bool IsMediaCaptureSupported()
        {
            bool supportedPlatform = (Application.platform == RuntimePlatform.WindowsPlayer) || (Application.platform == RuntimePlatform.WindowsEditor);
            bool supportedLauncher = (launchableProviderType != LaunchableProviderType.Nvidia) && (launchableProviderType != LaunchableProviderType.Gameface);

            return supportedPlatform && supportedLauncher;
        }

        public SystemData(MediaCaptureSupport forceMediaCaptureSupport)
        {
            if (m_instance != null)
                LudeoLogManager.LogFatal($"{LOG_PREFIX} SystemData is a singleton. This should not happen!");

            supportedMediaCapture = forceMediaCaptureSupport;
            ludeoSessionVideoData.maxHighlightDuration = 60;
            ludeoSessionVideoData.apiUpdateUploadUrl = "https://services.ludeo.com/api/v3/highlights/video-upload-status";

            LudeoLogManager.LogInfo($"{LOG_PREFIX} supportedMediaCapture defaults to {supportedMediaCapture}");
            LudeoLogManager.LogInfo($"{LOG_PREFIX} maxHighlightDurationInSeconds defaults to {ludeoSessionVideoData.maxHighlightDuration} seconds");
            LudeoLogManager.LogInfo($"{LOG_PREFIX} mediaCaptureUpdatesUrl defaults to {ludeoSessionVideoData.apiUpdateUploadUrl}");

            m_instance = this;
        }

        public void Deinit()
        {
            m_instance = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                Deinit();
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SystemData()
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
