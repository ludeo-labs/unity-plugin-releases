/// @cond INTERNAL
namespace LudeoSDK.Ludeo.Internal.Launchers.Nvidia
{
    using LudeoSDK.Internal;
    using LudeoSDK.Ludeo.Internal.Launchers;
    using System;
    using System.Runtime.InteropServices;
    using static LudeoSDK.Ludeo.Internal.Launchers.Nvidia.NvidiaDefinitions;

    internal class NvidiaLauncher : ILauncherProvider
    {
        private const string LOG_PREFIX = "[NVIDIA LNCR]";

        LaunchableProviderType ILauncherProvider.LauncherType => m_launcherType;

        private INvidiaExtern m_iInvidiaExtern;
        private LaunchableProviderType m_launcherType;
        private LaunchableProviderType m_authSource;
        private bool m_isLauncherRunning = false;

        internal NvidiaLauncher(LaunchableProviderType launcherType, LaunchableProviderType authSource)
        {
            m_iInvidiaExtern = LudeoUnityManager.LudeoInterfaceFactory.CreateINvidiaExtern();

            if (m_iInvidiaExtern == null)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} failed to create INvidiaExtern!");
                return;
            }

            m_launcherType = launcherType;
            m_authSource = authSource;
            LudeoLogManager.LogVerbose($"{LOG_PREFIX} LauncherProvider constructor: {m_launcherType}");
        }

        public void Deinit()
        {
            LudeoLogManager.LogVerbose($"{LOG_PREFIX} Deinit");

            if (m_isLauncherRunning)
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} calling: gfnShutdownSdk");

                try
                {
                    m_iInvidiaExtern.GfnShutdownSdk();
                }
                catch (Exception e)
                {
                    LudeoLogManager.LogError($"{LOG_PREFIX} Caught: {e}");
                }
            }

            m_isLauncherRunning = false;
        }

        public bool TryRunningLauncher()
        {
            if (!m_isLauncherRunning)
                m_isLauncherRunning = InitNvidiaSDK();

            return m_isLauncherRunning;
        }

        private bool InitNvidiaSDK()
        {
            GfnRuntimeError gfnRuntimeError = GfnRuntimeError.gfnInitFailure;
            GfnDisplayLanguage language = GetLanguage();
            bool returnValue = false;

            try
            {
                gfnRuntimeError = m_iInvidiaExtern.GfnInitializeRuntimeSdk(language);
            }
            catch (DllNotFoundException e)
            {
                LudeoLogManager.LogWarning($"{LOG_PREFIX} @InitNvidiaSDK, Exception Caught: {e.Message} was not found");
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} @InitNvidiaSDK, Caught: {e}");
            }
            finally
            {
                if (gfnRuntimeError == GfnRuntimeError.gfnSuccess)
                {
                    returnValue = true;
                    LudeoLogManager.LogVerbose($"{LOG_PREFIX} @InitNvidiaSDK, result: {gfnRuntimeError}");
                }
                else if (gfnRuntimeError == GfnRuntimeError.gfnInitSuccessClientOnly || gfnRuntimeError == GfnRuntimeError.gfnInitSuccessCloudOnly)
                    LudeoLogManager.LogVerbose($"{LOG_PREFIX} @InitNvidiaSDK, result: {gfnRuntimeError}"); // TODO: check what are these 2 and how to handle
                else if (gfnRuntimeError == GfnRuntimeError.gfnLudeoDummy)
                    LudeoLogManager.LogVerbose($"{LOG_PREFIX} @InitNvidiaSDK, result: {gfnRuntimeError}");
                else
                    LudeoLogManager.LogError($"{LOG_PREFIX} @InitNvidiaSDK, result: {gfnRuntimeError}");
            }

            return returnValue;
        }
    }
}
/// @endcond INTERNAL