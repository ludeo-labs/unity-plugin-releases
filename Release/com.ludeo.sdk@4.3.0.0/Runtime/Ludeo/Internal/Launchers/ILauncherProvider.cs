using LudeoSDK.Internal;

/// @cond INTERNAL
namespace LudeoSDK.Ludeo.Internal.Launchers
{

    internal delegate bool OnTryReadTokens(string logPrefix, string commandLine, out LudeoTokens ludeoTokens);

    internal interface ILauncherProvider
    {
        LaunchableProviderType LauncherType { get; }

        bool TryRunningLauncher();

        void Deinit();
    }
}
/// @endcond INTERNAL