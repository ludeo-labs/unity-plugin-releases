/// @cond INTERNAL
namespace LudeoSDK.Ludeo.Internal.Launchers
{
    using LudeoSDK.Ludeo.Internal.Launchers.Gameface;
    using LudeoSDK.Ludeo.Internal.Launchers.Nvidia;
    using System;

    internal enum LaunchableProviderType
    {
        Unity,
        Steam,
        SteamFacepunch,
        Nvidia,
        Gameface,
    }

    internal class LauncherProviderFactory
    {
        internal static string GetLaunchProviderAsString(LaunchableProviderType launchableProviderType)
        {
            switch (launchableProviderType)
            {
                case LaunchableProviderType.Nvidia: return "Nvidia";
                case LaunchableProviderType.Steam: return "Steam";
                case LaunchableProviderType.SteamFacepunch: return "Steam"; // Platform currently return 422 is we use SteamFacepunch - should we update?
                case LaunchableProviderType.Unity: return "Unity";
                case LaunchableProviderType.Gameface: return "Gameface";
                default:
                    return "Unknown";
            }
        }

        internal static ILauncherProvider CreateLaunchableProvider(LaunchableProviderType type)
        {
            switch (type)
            {
                case LaunchableProviderType.Gameface:
                    return new GamefaceLauncher(LaunchableProviderType.Gameface);
                case LaunchableProviderType.Nvidia:
                    return new NvidiaLauncher(launcherType: LaunchableProviderType.Nvidia, authSource: LaunchableProviderType.Nvidia);

                default:
                    throw new ArgumentException("Invalid type to CreateLaunchableProvider", "type");
            }
        }
    }
}
/// @endcond INTERNAL