using LudeoSDK.Internal;

/// @cond INTERNAL
namespace LudeoSDK.Ludeo.Internal.Launchers
{
    internal class LauncherManager
    {
        private const string LOG_PREFIX = "[LauncherManager]";

        internal static bool TryLauncherSetup(SystemData systemData)
        {
            LaunchableProviderType[] authSources = { LaunchableProviderType.Gameface, LaunchableProviderType.Nvidia};

            for(int i=0; i <authSources.Length; ++i)
            {
                if (TryDetectLauncher(systemData, authSources[i]))
                    return true;
            }

            LudeoLogManager.LogInfo("No launcher used in wrapper.");

            return false;
        }

        internal static void DeinitLauncher(SystemData systemData)
        {
            ILauncherProvider launcher = systemData.launchableProvider;

            if (launcher != null)
                launcher.Deinit();
        }

        private static bool TryDetectLauncher(SystemData systemData, LaunchableProviderType launcerType)
        {
            ILauncherProvider launcherProviderClass = LauncherProviderFactory.CreateLaunchableProvider(launcerType);
            if (launcherProviderClass.TryRunningLauncher())
            {
                systemData.launchableProvider = launcherProviderClass;
                systemData.launchableProviderType = launcerType;

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} Detected: {launcerType}");

                return true;
            }

            return false;
        }
    }
}
/// @endcond INTERNAL