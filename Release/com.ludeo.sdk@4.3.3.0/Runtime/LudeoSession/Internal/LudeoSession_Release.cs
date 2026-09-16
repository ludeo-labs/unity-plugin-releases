/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using AOT;
    using System;

    internal static partial class LudeoSessionInternal
    {
        private const string API_PREFIX_RELEASE = "[Release]";

        public static void Release(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle)
        {
            try
            {
                LudeoSessionReleaseParams nativeInParams = LudeoSessionReleaseParams.Default;

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} calling ReleaseSession");

                ludeoSessionWrapper.LudeoSessionRelease(sessionHandle, ref nativeInParams, IntPtr.Zero, HandleLudeoSessionReleaseDone);
            }
            catch (DllNotFoundException)
            {
                LudeoLogManager.LogError($"{API_PREFIX_RELEASE} {LudeoManager.CoreDllName} was not found!");
                throw new DllNotFoundException($"{API_PREFIX_RELEASE} {LudeoManager.CoreDllName} was not found!");
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{API_PREFIX_RELEASE} {e}");
                throw new Exception($"{API_PREFIX_RELEASE} {e}");
            }
        }

        [MonoPInvokeCallback(typeof(LudeoSessionReleaseCallback))]
        private static void HandleLudeoSessionReleaseDone(ref LudeoSessionReleaseCallbackParams nativeOutParams)
        {
            LudeoLogManager.LogVerbose($"{LOG_PREFIX} ReleaseSession callback.");

            nativeOutParams.ConvertToManaged(out LudeoSessionReleaseCallbackData managedData);

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} ReleaseSession result: {managedData.resultCode}");

            if (PluginCallbacks.onLudeoSessionRelease != null)
                PluginCallbacks.onLudeoSessionRelease(managedData);
            else
                LudeoLogManager.LogWarning($"{API_PREFIX_RELEASE} onLudeoSessionRelease callback already cleared.");
        }
    }
}
/// @endcond INTERNAL
