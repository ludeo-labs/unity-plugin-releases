/// @cond INTERNAL
using LudeoSDK.Internal;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LudeoSDKUnityEditor
{
    [Serializable]
    internal enum LudeoSupportedPlatformType
    {
        NotSupported,
        WinRelease,
        WinDevelopment,
        Win64Release,
        Win64Development,
        OSXRelease,
        LinuxRelease,
    }

    [Serializable]
    internal class LudeoSDKSettings
    {
        public const string LUDEO_SDK_CORE_DLL_PATTERN = "LudeoSDK-*-*.dll";

        private const string LUDEO_SDK_SETTINGS_FILE_NAME = "LudeoSDKSettings.json";

        public LudeoDllSet[] ludeoDllSets = new LudeoDllSet[]
        {
            new LudeoDllSet { platform = nameof(LudeoSupportedPlatformType.NotSupported),       dllName = "" },
            new LudeoDllSet { platform = nameof(LudeoSupportedPlatformType.WinRelease),         dllName = "LudeoSDK-Win64-Release.dll" },
            new LudeoDllSet { platform = nameof(LudeoSupportedPlatformType.Win64Release),       dllName = "LudeoSDK-Win64-Release.dll" },
            new LudeoDllSet { platform = nameof(LudeoSupportedPlatformType.WinDevelopment),     dllName = "LudeoSDK-Win64-Development.dll" },
            new LudeoDllSet { platform = nameof(LudeoSupportedPlatformType.Win64Development),   dllName = "LudeoSDK-Win64-Development.dll" },
            new LudeoDllSet { platform = nameof(LudeoSupportedPlatformType.OSXRelease),         dllName = "LudeoSDK-OSX-Release.dll" },
            new LudeoDllSet { platform = nameof(LudeoSupportedPlatformType.LinuxRelease),       dllName = "LudeoSDK-linux-Release.dll" },
        };

        public bool GetDllNameForPlatform(LudeoSupportedPlatformType platform, out string dllName, bool isDeserializeFromJson=true)
        {
            if (isDeserializeFromJson)
                DeserializeOrCreateJson();

            foreach (LudeoDllSet ludeoDllSet in ludeoDllSets)
            {
                if (Enum.TryParse(ludeoDllSet.platform, out LudeoSupportedPlatformType parsedPlatform))
                {
                    if (parsedPlatform == platform)
                    {
                        dllName = ludeoDllSet.dllName;
                        return true;
                    }
                }
                else
                    Debug.LogError($"{nameof(LudeoSDKSettings)} can not parse: {ludeoDllSet.platform}");
            }

            dllName = "";
            return false;
        }


        private void DeserializeOrCreateJson()
        {
            string ludeoFolder = Path.Combine(Application.streamingAssetsPath, LudeoSystemInfo.GetSDKProjectName());
            string jsonFilePath = Path.Combine(ludeoFolder, LUDEO_SDK_SETTINGS_FILE_NAME);

            if (!Directory.Exists(ludeoFolder))
                Directory.CreateDirectory(ludeoFolder);

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                JsonUtility.FromJsonOverwrite(json, this);
            }
            else
            {
                string json = JsonUtility.ToJson(this, prettyPrint: true);
                File.WriteAllText(jsonFilePath, json);
                AssetDatabase.Refresh();
            }
        }
    }
}
/// @endcond INTERNAL