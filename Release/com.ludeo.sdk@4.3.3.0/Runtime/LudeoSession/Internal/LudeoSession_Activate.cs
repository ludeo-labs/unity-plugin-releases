/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using AOT;
    using System;
    using System.Runtime.InteropServices;


    internal static partial class LudeoSessionInternal
    {
        private const string LOG_PREFIX = "[LudeoSession]";
        
        private static Action<LudeoSessionActivateCallbackData> m_onActivateLudeoDone;
        private static LudeoSessionActivateCallbackData m_ludeoSessionActivateCallbackData;
        
        public static void Activate(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle, Action<LudeoSessionActivateCallbackData> onActivateLudeoDone,
                                        LocalConfigData configData, LudeoSessionSetLocalizationParameters ludeoSessionLocalizationData)
        {
            const string API_PREFIX = "[Activate]";

            m_onActivateLudeoDone = onActivateLudeoDone;
            IntPtr authDetails = IntPtr.Zero;
            IntPtr localizationPtr = IntPtr.Zero;
            LudeoSessionSetLocalizationParams localizationParams = default;
            try
            {
                if (configData.isRunWithoutLauncher)
                    authDetails = CreateAuthDetails(LudeoAuthType.Steam, configData.launcherUserId, configData.betaVersion);

                if (ludeoSessionLocalizationData.language == null)
                {
                    LudeoLogManager.LogInfo($"{LOG_PREFIX} No language set, skipping localization for Activate.");
                }
                else
                {
                    localizationParams = new LudeoSessionSetLocalizationParams(ludeoSessionLocalizationData.language,
                        ludeoSessionLocalizationData.supportedLanguages);
                    localizationPtr = Marshal.AllocHGlobal(Marshal.SizeOf(localizationParams));
                    Marshal.StructureToPtr(localizationParams, localizationPtr, false);
                }

                LudeoSessionActivateParams nativeInParams = new LudeoSessionActivateParams(configData.platformUrl, configData.apiKey, authDetails, localizationPtr, configData.gameVersion);

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} calling Activate.");

                ludeoSessionWrapper.LudeoSessionActivate(sessionHandle, ref nativeInParams, IntPtr.Zero, HandleLudeoSessionActivateDone);
            }
            catch (DllNotFoundException e)
            {
                LudeoLogManager.LogError($"{API_PREFIX} {LudeoManager.CoreDllName} was not found! {e}");
                throw new DllNotFoundException($"{API_PREFIX} {LudeoManager.CoreDllName} was not found!");
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{API_PREFIX} {e}");
                throw new Exception($"{API_PREFIX} {e}");
            }
            finally
            {
                if (authDetails != IntPtr.Zero)
                {
                    Marshal.DestroyStructure<LudeoSteamAuthDetails>(authDetails); // Only if the struct contains pointers to unmanaged memory
                    Marshal.FreeHGlobal(authDetails);
                }

                if (localizationPtr != IntPtr.Zero)
                {
                    localizationParams.Release();
                    Marshal.DestroyStructure<LudeoSessionSetLocalizationParams>(localizationPtr); // Only if the struct contains pointers to unmanaged memory
                    Marshal.FreeHGlobal(localizationPtr);
                }
            }
        }

        private static IntPtr CreateAuthDetails(LudeoAuthType ludeoAuthType, string authId, string betaName)
        {
            if (ludeoAuthType != LudeoAuthType.Steam)
                throw new NotSupportedException($"{ludeoAuthType} not supported yet!");

            LudeoSteamAuthDetails authDetails = new LudeoSteamAuthDetails()
            {
                authType = ludeoAuthType,
                authId = authId,
                currentBetaName = betaName
            };

            int size = Marshal.SizeOf(typeof(LudeoSteamAuthDetails));
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(authDetails, ptr, false);

            return ptr;
        }

        private static bool TrySetUserToken(SystemData systemData, LudeoHSession sessionHandle)
        {
            INativeHelperMethods nativeHelperMethods = LudeoUnityManager.LudeoInterfaceFactory.GetNativeHelperMethods();
            string userToken = nativeHelperMethods.GetAuthToken(sessionHandle);

            if (string.IsNullOrEmpty(userToken))
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} userToken returned is null!");
                return false;
            }

            systemData.userToken = userToken;
            LudeoLogManager.LogVerbose($"{LOG_PREFIX} User token was fetched.");
            return true;
        }

        [MonoPInvokeCallback(typeof(LudeoSessionActivateCallback))]
        private static void HandleLudeoSessionActivateDone(ref LudeoSessionActivateCallbackParams nativeOutParams)
        {
            try
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} Activate callback.");

                nativeOutParams.ConvertToManaged(out m_ludeoSessionActivateCallbackData);
                
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} Activate result: {m_ludeoSessionActivateCallbackData.resultCode}");

                if (TrySetUserToken(SystemData.Instance, LudeoManager.Instance.LudeoSession.SessionHandle))
                {
                    LudeoLogManager.LogVerbose($"{LOG_PREFIX} SetUserToken Done.");

                    // Temporary fix for LC-8977: the ludeo force lunch should be handled by the session activation flow,
                    // but as the plugin doesn't know the auth token in time, we call ludeo selected on the first video state change, as before
                    LudeoManager.Instance.IsActivationCallbackWaiting = true;
                }
                else
                {
                    LudeoLogManager.LogError($"{LOG_PREFIX} Can not set user token!");
                }

                InvokeActivationCallback();
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} @HandleLudeoSessionActivateDone, exception: {e}");
            }
            finally
            {
                m_onActivateLudeoDone = null;
            }
        }
        
        
        private static void InvokeActivationCallback(){
            if (LudeoManager.Instance.IsForceLudeoLaunch(out Guid _))
            {
                LudeoLogManager.LogWarning($"{LOG_PREFIX} Forcing your set Ludeo Id to fire");
                m_ludeoSessionActivateCallbackData.isLudeoSelected = true;
            }
            m_onActivateLudeoDone(m_ludeoSessionActivateCallbackData);
        }
        
        public static void OpenGallery(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle)
        {
            if (ludeoSessionWrapper.OpenGallery(sessionHandle, LudeoSessionOpenGalleryParams.Default) != LudeoResult.Success)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to open gallery!");
            }
            else
            {
                LudeoLogManager.LogInfo($"{LOG_PREFIX} Opened gallery.");
            }
        }
        
        public static void SetLocalization(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle, LudeoSessionSetLocalizationParameters ludeoSessionLocalizationData)
        {
            LudeoSessionSetLocalizationParams localizationParams = new LudeoSessionSetLocalizationParams(ludeoSessionLocalizationData.language, ludeoSessionLocalizationData.supportedLanguages);
            if (ludeoSessionWrapper.SetLocalization(sessionHandle, localizationParams) != LudeoResult.Success)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to set localization!");
            }
            else
            {
                LudeoLogManager.LogInfo($"{LOG_PREFIX} Set localization finished successfully.");
            }
            localizationParams.Release();
        }
    }
}
/// @endcond INTERNAL
