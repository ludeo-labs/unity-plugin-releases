/// @cond INTERNAL
using LudeoSDK;
using LudeoSDK.Internal;
using LudeoSDK.UnityScripts;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LudeoSDKUnityEditor
{
    internal class LudeoPlatformSettings
    {
        private static Dictionary<int, LudeoSupportedPlatformType> LudeoPlatformLookup = new Dictionary<int, LudeoSupportedPlatformType>
        {
            { GetCoreLibsBitwiseKey(LudeoSDKCoreDll.Release,        BuildTarget.StandaloneWindows),     LudeoSupportedPlatformType.WinRelease},
            { GetCoreLibsBitwiseKey(LudeoSDKCoreDll.Release,        BuildTarget.StandaloneWindows64),   LudeoSupportedPlatformType.Win64Release},
            { GetCoreLibsBitwiseKey(LudeoSDKCoreDll.Development,    BuildTarget.StandaloneWindows),     LudeoSupportedPlatformType.WinDevelopment},
            { GetCoreLibsBitwiseKey(LudeoSDKCoreDll.Development,    BuildTarget.StandaloneWindows64),   LudeoSupportedPlatformType.Win64Development},

            { GetCoreLibsBitwiseKey(LudeoSDKCoreDll.Release,        BuildTarget.StandaloneOSX),         LudeoSupportedPlatformType.OSXRelease},
            { GetCoreLibsBitwiseKey(LudeoSDKCoreDll.Release,        BuildTarget.StandaloneLinux64),     LudeoSupportedPlatformType.LinuxRelease},
        };

        public static void UpdateLudeoCoreDllReference(LudeoSDKCoreDll coreDlls)
        {
            BuildTarget newTarget = EditorUserBuildSettings.activeBuildTarget;
            UpdateLudeoCoreDllReference(coreDlls, newTarget);
        }

        public static void UpdateLudeoCoreDllReference(BuildTarget newTarget)
        {
            LudeoSettings ludeoSettings = LudeoUnityHelpers.GetLudeoSettings();
            UpdateLudeoCoreDllReference(ludeoSettings.coreDllReference, newTarget);
        }

        public static bool TryGetSupportedPlatformDllName(LudeoSDKCoreDll coreDlls, BuildTarget buildTarget, out string dllName)
        {
            if (!LudeoPlatformLookup.TryGetValue(GetCoreLibsBitwiseKey(coreDlls, buildTarget), out LudeoSupportedPlatformType platformType))
                platformType = LudeoSupportedPlatformType.NotSupported;

            LudeoSDKSettings ludeoSDKSettings = new LudeoSDKSettings();

            return ludeoSDKSettings.GetDllNameForPlatform(platformType, out dllName);
        }

        private static void UpdateLudeoCoreDllReference(LudeoSDKCoreDll coreDlls, BuildTarget newTarget)
        {
            if (EditorApplication.isPlaying)
                EditorApplication.isPlaying = false;

            if (TryGetSupportedPlatformDllName(coreDlls, newTarget, out string dllName))
                DllNameUpdater.UpdateDllName(dllName);
            else
            {
                Debug.LogError($"{nameof(UpdateLudeoCoreDllReference)} failed to find dll name for: {coreDlls}, {newTarget}");
                return;
            }
            
            if (coreDlls == LudeoSDKCoreDll.Release)
                Debug.Log($"{nameof(UpdateLudeoCoreDllReference)} to: {dllName}");
            else
                Debug.LogWarning($"{nameof(UpdateLudeoCoreDllReference)} to: {dllName}");
        }

        private static int GetCoreLibsBitwiseKey(LudeoSDKCoreDll ludeoSDKCoreDll, BuildTarget buildTarget)
        {
            const int BITS_FOR_ENUM = 16;
            int coreLibsKey = ((int)ludeoSDKCoreDll << BITS_FOR_ENUM) | (int)buildTarget;
            return coreLibsKey;
        }
    }
}
/// @endcond INTERNAL