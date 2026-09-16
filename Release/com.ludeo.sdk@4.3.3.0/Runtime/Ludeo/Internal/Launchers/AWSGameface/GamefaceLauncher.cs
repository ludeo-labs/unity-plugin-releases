/// @cond INTERNAL
namespace LudeoSDK.Ludeo.Internal.Launchers.Gameface
{
    using LudeoSDK.Internal;
    using LudeoSDK.Ludeo.Internal.Launchers;
    using System;

    internal class GamefaceLauncher : ILauncherProvider
    {
        private const string LOG_PREFIX = "[GAMEFACE LNCR]";

        LaunchableProviderType ILauncherProvider.LauncherType => m_launcherType;

        private IGamefaceExtern m_gamefaceExtern;
        private LaunchableProviderType m_launcherType;
        private bool m_isLauncherRunning = false;

        internal GamefaceLauncher(LaunchableProviderType launcherType)
        {
            m_gamefaceExtern = LudeoUnityManager.LudeoInterfaceFactory.CreateIGamefaceExtern();

            if (m_gamefaceExtern == null)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} failed to create!");
                return;
            }

            m_launcherType = launcherType;
            LudeoLogManager.LogVerbose($"{LOG_PREFIX} LauncherProvider constructor: {m_launcherType}");
        }

        public void Deinit()
        {
            LudeoLogManager.LogVerbose($"{LOG_PREFIX} Deinit");

            m_isLauncherRunning = false;
        }

        public bool TryRunningLauncher()
        {
            if (!m_isLauncherRunning)
                m_isLauncherRunning = IsRunningInGameface();

            return m_isLauncherRunning;
        }

        private bool IsRunningInGameface()
        {
            bool isSuccess = false;

            try
            {
                string cloudToken = m_gamefaceExtern.GetCloudToken();

                isSuccess = !string.IsNullOrEmpty(cloudToken);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} @InitGameface, Caught: {e}");
            }

            return isSuccess;
        }
    }
}
/// @endcond INTERNAL