/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using System.Diagnostics;
    using UnityEngine;

    internal class LudeoSystemInfo
    {
        private readonly static string SDK_PROJECT_NAME = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        private readonly string SDK_VERSION = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private static LudeoSystemInfo m_instance;
        private SystemData m_systemData;

        internal LudeoSystemInfo(SystemData systemData)
        {
            m_systemData = systemData;
            m_instance = this;
            m_systemData.processInstanceId = Process.GetCurrentProcess().Id;

            LudeoLogManager.LogInfo($"{SDK_PROJECT_NAME} version is: {GetSDKVersion()}");
        }

        ~LudeoSystemInfo()
        {
            m_instance = null;
        }

        public static LudeoSystemInfo Instance
        {
            get
            {
                return m_instance;
            }
        }

        internal static string GetSDKProjectName()
        {
            return SDK_PROJECT_NAME;
        }

        internal string GetSDKVersion()
        {
            return SDK_VERSION;
        }

        internal string GetOs()
        {
            return SystemInfo.operatingSystem.ToString();
        }

        internal string GetOsVersion()
        {
            string osVersion = "Unknown";
            if (!string.IsNullOrEmpty(SystemInfo.operatingSystem))
            {
                string[] osParts = SystemInfo.operatingSystem.Split(' ');
                if (osParts.Length > 1)
                {
                    osVersion = osParts[1];
                }
            }
            return osVersion;
        }

        internal string GetResolution()
        {
            return Screen.currentResolution.ToString();
        }

        internal string GetRefreshRate()
        {
            return Screen.currentResolution.refreshRate.ToString();
        }

        internal string GetSystemLanguage()
        {
            return Application.systemLanguage.ToString();
        }

        internal string GetUniqueId()
        {
            return SystemInfo.deviceUniqueIdentifier.ToString();
        }

        internal string GetDeviceType()
        {
            return SystemInfo.deviceType.ToString();
        }

        internal string GetPlatform()
        {
            return Application.platform.ToString();
        }

        internal string GetMemory()
        {
            return SystemInfo.systemMemorySize.ToString();
        }

        internal string GetProcessorType()
        {
            return SystemInfo.processorType.ToString();
        }

        internal string GetGameId()
        {
            return m_systemData.gameData.apiKey;
        }

        internal string GetGameName()
        {
            return m_systemData.localConfigData.gameName;
        }

        internal string GetGameVersion()
        {
            return m_systemData.localConfigData.gameVersion;
        }

        internal string GetUnityVersion()
        {
            return Application.unityVersion;
        }
    }
}
/// @endcond INTERNAL