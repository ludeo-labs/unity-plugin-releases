/// @cond INTERNAL
namespace LudeoSDK.MediaCapture
{
    using LudeoSDK.Internal;
    using LudeoVideoService.ComLib.Configuration;

    internal class VideoConfigurationGenerator
    {
        private const string LOG_PREFIX = "[VID_CNFG_GEN]";

        public VideoServiceConfiguration GetConfiguration(SystemData systemData, string appDataPath, bool isDebug = false)
        {
            if (isDebug)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Running in video debug mode");
                return GenerateDefaultConfiguration(systemData, appDataPath);
            }
            VideoServiceConfiguration videoServiceConfig;

            // Since most of the video configuration is not being fetched from core (due to Legacy Video being temporary feature)
            // We use most of the video default configs.
            LudeoLogManager.LogWarning($"{LOG_PREFIX} Using partially HARDCODED NewVideo config.");
            videoServiceConfig = GenerateDefaultConfiguration(systemData, appDataPath);

            UpdateDepandantedConfigValues(systemData, appDataPath, videoServiceConfig);

            return videoServiceConfig;
        }

        private void UpdateDepandantedConfigValues(SystemData systemData, string appDataPath, VideoServiceConfiguration videoServiceConfig)
        {
            int captureDuration = systemData.ludeoSessionVideoData.maxHighlightDuration;

            videoServiceConfig.App.ApplicationPersistentDataPath = appDataPath;
            videoServiceConfig.App.AppToken = systemData.userToken;
            videoServiceConfig.Video.CaptureDuration = captureDuration;
            videoServiceConfig.Video.SaveTotalDurationInSeconds = captureDuration * 2;
            videoServiceConfig.Network.ApiUpdateUploadUrl = systemData.ludeoSessionVideoData.apiUpdateUploadUrl;
        }

        private VideoServiceConfiguration GenerateDefaultConfiguration(SystemData systemData, string appDataPath)
        {
            return new VideoServiceConfiguration
            {
                App = new ApplicationSettings
                {
                    ApplicationPersistentDataPath = appDataPath,
                    AppToken = systemData.userToken,
                    ConnectionTimeout = 5,
                    DisconnectTimeout = 1,
                    InactivityTimeout = 3,
                    MaxImageCapacity = 150
                },
                Video = new VideoCaptureSettings
                {
                    Width = 1280,
                    Height = 720,
                    PixelBytes = 3,
                    FrameRate = 30f,
                    CapturingTime = 5,
                    SaveTotalDurationInSeconds = 120f,
                    ConstantRateFactor = 28,
                    CaptureDuration = 60,
                    VideoCodec = "libx264",
                    ImageFormat = "yuv420p",
                    VideoProcessingThreadsCount = 1,
                    SegmentFileNameFormat = "SEG-%09d.mp4"
                },
                Directory = new DirectorySettings
                {
                    VideosFolderPath = "videos",
                    BinaryFolderPath = "",
                    TmpFolderPath = "tmp",
                    WorkFolderPath = "work"
                },
                Log = new LogSettings
                {
                    LogFilePath = "video-log.txt",
                    SaveLogsForXDays = 3,
                    LogLevel = 1,
                    ManageLogsBy = 3
                },
                Network = new NetworkSettings
                {
                    MessagePipeName = "LudeoMessages",
                    ImagePipeName = "LudeoImages",
                    ApiUpdateUploadUrl = "https://services.ludeo.com/api/v3/highlights/video-upload-status",
                    PipeConnectionTimeout = 60000,
                    WaitBetweenConnectionTries = 1000,
                    MaxReconnectionAttempts = 60,
                    PipeConnectionSemaphore = "PipeConnectionSemaphore"
                }
            };
        }
    }
}
/// @endcond INTERNAL