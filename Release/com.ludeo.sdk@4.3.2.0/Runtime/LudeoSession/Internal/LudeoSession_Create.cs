/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using System;

    internal static partial class LudeoSessionInternal
    {
        unsafe public static LudeoResult Create(ILudeoSessionWrapper ludeoSessionWrapper, out LudeoHSession sessionHandle)
        {
            const string API_PREFIX = "[Create]";

            LudeoResult ludeoResult = LudeoResult.Unknown;

            try
            {
                LudeoSessionCreateParams nativeSessionCreateParams = LudeoSessionCreateParams.Default;

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} calling Create");

                ludeoResult = ludeoSessionWrapper.LudeoSessionCreate(ref nativeSessionCreateParams, out sessionHandle);

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} Create result: {ludeoResult}");
            }
            catch (DllNotFoundException)
            {
                LudeoLogManager.LogError($"{API_PREFIX} {LudeoManager.CoreDllName} was not found!");
                throw new DllNotFoundException($"{API_PREFIX} {LudeoManager.CoreDllName} was not found!");
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{API_PREFIX} {e}");
                throw new Exception($"{API_PREFIX} {e}");
            }

            return ludeoResult;
        }
    }
}
/// @endcond INTERNAL
