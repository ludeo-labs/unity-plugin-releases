/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using LudeoSDK.UnityScripts;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;
    using Application = UnityEngine.Application;

    [Serializable]
    internal struct LocalConfigData
    {
        // External
        public LudeoLogLevel logLevel;

        // Internal
        public string gameName;
        public string gameVersion;
        public LudeoLogLevel coreLogLevel;
        public LudeoLogCategory coreLogCategory;
        public string platformUrl;
        public string apiKey;
        public string betaVersion;
        public string launcherUserId;
        public bool isRunWithoutLauncher;
        public bool isAutoStartInLudeo;
        public Guid ludeoIdToStartWith;


        public void CommitConfigs(LocalConfigData internalConfig)
        {
            gameName                    = internalConfig.gameName;
            gameVersion                 = internalConfig.gameVersion;
            coreLogLevel                = internalConfig.coreLogLevel;
            coreLogCategory             = internalConfig.coreLogCategory;
            platformUrl                 = internalConfig.platformUrl;
            apiKey                      = internalConfig.apiKey;
            launcherUserId              = internalConfig.launcherUserId;
            betaVersion                 = internalConfig.betaVersion;
            isRunWithoutLauncher        = internalConfig.isRunWithoutLauncher;
            isAutoStartInLudeo          = internalConfig.isAutoStartInLudeo;
            ludeoIdToStartWith          = internalConfig.ludeoIdToStartWith;
        }

        public bool IsForceLudeoInjection(out Guid ludeoIdToFire)
        {
            if (isRunWithoutLauncher && isAutoStartInLudeo)
            {
                ludeoIdToFire = ludeoIdToStartWith;
                return true;
            }
            else
            {
                ludeoIdToFire = Guid.Empty;
                return false;
            }
        }
    }

    [Serializable]
    internal class RunCommands
    {
        public const string COMMAND_VIDEO_ENABLED = "video-enabled";
        public const string COMMAND_VIDEO_LOCALSAVE = "video-localsave";
        public const string COMMAND_VIDEO_SWENCODER = "video-swencoder";
        public const string COMMAND_OVERLAY_ENABLED = "overlay-enabled";
        public const string COMMAND_MONITOR_ENABLED = "monitor-enabled";
        public const string COMMAND_INTERNAL_WHITELIST = "internal-whitelist_certification_authority";

        public RunCommand[] commands;

        private Dictionary<string, string> m_preDefinedCommands;
        private bool m_isParsedCommands;

        public bool IsCoreVideoEnabled()
        {
            if (!m_isParsedCommands)
                ParseCommands();

            // Is video-enable command actually set?
            if (m_preDefinedCommands.TryGetValue(COMMAND_VIDEO_ENABLED, out string isEnabledValue))
            {
                if (int.TryParse(isEnabledValue, out int isEnabled))
                    return isEnabled == 1;
                else
                    return false;
            }
            else // Not set - default to enable video
                return true;
        }

        private void ParseCommands()
        {
            m_preDefinedCommands = new Dictionary<string, string>(10);
            m_isParsedCommands = true;

            if (commands != null)
            {
                for (int i = 0; i < commands.Length; ++i)
                {
                    if (!string.IsNullOrWhiteSpace(commands[i].key))
                        m_preDefinedCommands[commands[i].key] = commands[i].value;
                }
            }
        }
    }

    [Serializable]
    internal class RunCommand
    {
        public string key;
        public string value;
    }

    [Serializable]
    internal class LudeoDllSet
    {
        public string platform;
        public string dllName;
    }

    internal static class LocalConfigSystem
    {
        private static LudeoSettings settings;

        internal const string LUDEO_FOLDER = "LudeoSDK";
        internal const string RUN_COMMANDS_FILE_NAME = "RunCommands.json";
        
        private static readonly Type TYPE_LOG_LEVEL = typeof(LudeoLogLevel);

        public static bool ReadConfig(out LocalConfigData data)
        {
            LudeoLogManager.LogInfo("Reading LocalConfigSystem.");
            LocalConfigData internalConfigData;

            bool isSuccess = SetInternalConfig(out internalConfigData);

            data = new LocalConfigData();

            if (!isSuccess)
            {
                LudeoLogManager.LogError("Error read LocalConfigSystem. Internal = " + isSuccess);
                
                return false;
            }
            else
            {
                data.CommitConfigs(internalConfigData);
                return isSuccess;
            }
        }

        private static bool SetInternalConfig(out LocalConfigData data)
        {
            bool result = true;

            settings = LudeoUnityHelpers.GetLudeoSettings();

            LocalConfigData localConfigData = new LocalConfigData();

            if (settings == null)
            {
                LudeoLogManager.LogError("Failed to load LudeoSettings.");
                LudeoLogManager.LogError("Create one by Right Clicking Unity menu bar: 'Ludeo->LudeoSDK->Setup Ludeo SDK'");
                LudeoLogManager.LogError("OR re-import LudeoSDK.");
                result = false;
            }
            else
            {
                localConfigData.gameName                = settings.gameName;
                localConfigData.gameVersion             = settings.gameVersion;
                localConfigData.platformUrl             = settings.platformUrl;
                localConfigData.apiKey                  = settings.apiKey;
                localConfigData.betaVersion             = settings.betaVersion;
                localConfigData.launcherUserId          = settings.launcherUserId;
                localConfigData.isRunWithoutLauncher    = settings.runWithoutLauncher;
                localConfigData.coreLogLevel            = settings.ludeoLogLevel;
                localConfigData.coreLogCategory         = settings.ludeoLogCategory;
                localConfigData.isAutoStartInLudeo      = settings.autoStartInLudeo;
                if (localConfigData.isRunWithoutLauncher
                    && localConfigData.isAutoStartInLudeo
                    && !Guid.TryParse(settings.ludeoToAutoStart, out localConfigData.ludeoIdToStartWith))
                    Debug.LogError($"Failed to parse ludeoToAutoStart as Ludeo Id: {localConfigData.ludeoIdToStartWith}");
            }

            data = localConfigData;
            return result;
        }

        internal static bool TryReadRunCommands(out RunCommands runCommands)
        {
            runCommands = ReadJsonFromStreamingAssets<RunCommands>(RUN_COMMANDS_FILE_NAME);
            return runCommands != default;
        }

        private static T ReadJsonFromStreamingAssets<T>(string streaminAssetsFilePath)
        {
            string filePath = GetStreamingAssetFilePath(streaminAssetsFilePath);
            T jsonObject = default;

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                jsonObject = JsonUtility.FromJson<T>(json);
                LudeoLogManager.LogInfo("Read Ludeo run commands.");
            }

            return jsonObject;
        }

        private static string GetStreamingAssetFilePath(string filePath)
        {
            string folder = Path.Combine(Application.streamingAssetsPath, LUDEO_FOLDER);
            return Path.Combine(folder, filePath);
        }
    }
}
/// @endcond INTERNAL
