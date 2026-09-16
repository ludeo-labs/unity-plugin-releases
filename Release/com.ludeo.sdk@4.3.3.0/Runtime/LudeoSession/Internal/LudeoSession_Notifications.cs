/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using AOT;
    using LudeoSDK.MediaCapture;
    using System;
    using System.Runtime.InteropServices;

    internal static partial class LudeoSessionInternal
    {
        private const string NOTIFY_LOG_PREFIX = "[NOTIFY]";
        private static LudeoHSession m_sessionHandle;
        private static ILudeoSessionWrapper m_ludeoSessionWrapper;

        public static void AddNotifyLudeoSelected(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle)
        {
            LudeoSessionAddNotifyLudeoSelectedParams apiParams = LudeoSessionAddNotifyLudeoSelectedParams.Default;
            ulong notificationId = ludeoSessionWrapper.AddNotifyLudeoSelectedRequest(sessionHandle, ref apiParams, IntPtr.Zero, HandleLudeoSelected);
            PluginCallbacks.RegisterNotificationId("AddNotifyLudeoSelected", notificationId);
        }

        public static void AddNotifyPauseGame(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle)
        {
            LudeoSessionAddNotifyPauseGameRequestParams apiParams = LudeoSessionAddNotifyPauseGameRequestParams.Default;
            ulong notificationId = ludeoSessionWrapper.AddNotifyPauseGameRequest(sessionHandle, ref apiParams, IntPtr.Zero, HandlePauseGame);
            PluginCallbacks.RegisterNotificationId("AddNotifyPauseGame", notificationId);
        }

        public static void AddNotifyResumeGame(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle)
        {
            LudeoSessionAddNotifyResumeGameRequestParams apiParams = LudeoSessionAddNotifyResumeGameRequestParams.Default;
            ulong notificationId = ludeoSessionWrapper.AddNotifyResumeGameRequest(sessionHandle, ref apiParams, IntPtr.Zero, HandleResumeGame);
            PluginCallbacks.RegisterNotificationId("AddNotifyResumeGame", notificationId);
        }

        public static void AddNotifyReturnToMainMenu(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle)
        {
            LudeoSessionAddNotifyBackToMenuRequestParams apiParams = LudeoSessionAddNotifyBackToMenuRequestParams.Default;
            ulong notificationId = ludeoSessionWrapper.AddNotifyReturnToMainMenuRequest(sessionHandle, ref apiParams, IntPtr.Zero, HandleReturnToMainMenu);
            PluginCallbacks.RegisterNotificationId("AddNotifyReturnToMainMenu", notificationId);
        }

        public static void AddNotifyRoomReady(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle)
        {
            LudeoSessionAddNotifyRoomReadyParams apiParams = LudeoSessionAddNotifyRoomReadyParams.Default;
            ulong notificationId = ludeoSessionWrapper.AddNotifyRoomReady(sessionHandle, ref apiParams, IntPtr.Zero, HandleRoomReady);
            PluginCallbacks.RegisterNotificationId("AddNotifyRoomReady", notificationId);
        }

        public static void AddNotifyHighlight(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle)
        {
            LudeoSessionAddNotifyHighlightParams apiParams = LudeoSessionAddNotifyHighlightParams.Default;
            ulong notificationId = ludeoSessionWrapper.AddNotifyHighlightRequest(sessionHandle, ref apiParams, IntPtr.Zero, HandleHighlight);
            PluginCallbacks.RegisterNotificationId("AddNotifyHighlight", notificationId);
        }

        public static void AddNotifyConsentUpdated(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle)
        {
            m_ludeoSessionWrapper = ludeoSessionWrapper;
            LudeoSessionAddNotifyConsentUpdatedParams apiParams = LudeoSessionAddNotifyConsentUpdatedParams.Default;
            ulong notificationId = ludeoSessionWrapper.AddNotifyConsentUpdated(sessionHandle, ref apiParams, IntPtr.Zero, HandleConsentUpdate);
            PluginCallbacks.RegisterNotificationId("AddNotifyConsentUpdated", notificationId);
        }

        public static void AddNotifyVideoState(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle)
        {
            m_ludeoSessionWrapper = ludeoSessionWrapper;
            m_sessionHandle = sessionHandle;
            LudeoSessionAddNotifyVideoStateParam apiParams = LudeoSessionAddNotifyVideoStateParam.Default;
            ulong notificationId = ludeoSessionWrapper.AddNotifyVideoState(sessionHandle, ref apiParams, IntPtr.Zero, HandleVideoUpdate);
            PluginCallbacks.RegisterNotificationId("AddNotifyVideoState", notificationId);
        }

        public static void AddNotifyMuteRequest(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle)
        {
            LudeoSessionAddNotifyMuteRequestParams apiParams = LudeoSessionAddNotifyMuteRequestParams.Default;
            ulong notificationId = ludeoSessionWrapper.AddNotifyMuteRequest(sessionHandle, ref apiParams, IntPtr.Zero, HandleMuteRequest);
            PluginCallbacks.RegisterNotificationId("AddNotifyMuteRequest", notificationId);
        }

        public static void AddNotifyLocalizationChanged(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession sessionHandle)
        {
            LudeoSessionAddNotifyLocalizationChangedParams apiParams = LudeoSessionAddNotifyLocalizationChangedParams.Default;
            ulong notificationId = ludeoSessionWrapper.AddNotifyLocalizationChanged(sessionHandle, ref apiParams, IntPtr.Zero, HandleLocalizationChangedRequest);
            PluginCallbacks.RegisterNotificationId("AddNotifyLocalizationChanged", notificationId);
        }

        [MonoPInvokeCallback(typeof(LudeoSessionLudeoSelectedCallback))]
        private static void HandleLudeoSelected(ref LudeoSessionLudeoSelectedCallbackParams nativeParams)
        {
            try
            {
                nativeParams.ConvertToManaged(out LudeoSelectedCallbackData managedData);
                LudeoLogManager.LogInfo($"{NOTIFY_LOG_PREFIX} Ludeo selected! {managedData.ludeoId}");

                BringAppToFront();

                PluginCallbacks.onLudeoSelected(managedData);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{NOTIFY_LOG_PREFIX} Ludeo selected callback failed: {e}");
            }
        }

        [MonoPInvokeCallback(typeof(LudeoSessionPauseGameRequestCallback))]
        private static void HandlePauseGame(ref LudeoSessionPauseGameRequestCallbackParams nativeParams)
        {
            try
            {
                LudeoLogManager.LogInfo($"{NOTIFY_LOG_PREFIX} Ludeo paused!");
                PluginCallbacks.pauseGameCallback();
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{NOTIFY_LOG_PREFIX} Pause game callback failed: {e}");
            }
        }

        [MonoPInvokeCallback(typeof(LudeoSessionResumeGameRequestCallback))]
        private static void HandleResumeGame(ref LudeoSessionResumeGameRequestCallbackParams nativeParams)
        {
            try
            {
                LudeoLogManager.LogInfo($"{NOTIFY_LOG_PREFIX} Ludeo resumed!");
                PluginCallbacks.resumeGameCallback();
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{NOTIFY_LOG_PREFIX} Resume game callback failed: {e}");
            }
        }

        [MonoPInvokeCallback(typeof(LudeoSessionBackToMenuRequestCallback))]
        private static void HandleReturnToMainMenu(ref LudeoSessionBackToMenuRequestCallbackParams nativeParams)
        {
            try
            {
                LudeoLogManager.LogInfo($"{NOTIFY_LOG_PREFIX} Ludeo return to Main Menu!");
                PluginCallbacks.returnToMainMenuCallback();
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{NOTIFY_LOG_PREFIX} Return to Main Menu callback failed: {e}");
            }
        }
        
        [MonoPInvokeCallback(typeof(LudeoSessionMuteRequestCallback))]
        private static void HandleMuteRequest(ref LudeoSessionMuteRequestCallbackParams nativeParams)
        {
            try
            {
                LudeoLogManager.LogInfo($"{NOTIFY_LOG_PREFIX} Mute request with value - {nativeParams.mute}!");
                nativeParams.ConvertToManaged(out LudeoSessionMuteRequestCallbackData managedData);
                PluginCallbacks.onMuteRequestCallback(managedData);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{NOTIFY_LOG_PREFIX} Mute request callback failed: {e}");
            }
        }

        [MonoPInvokeCallback(typeof(LudeoSessionLocalizationChangedCallback))]
        private static void HandleLocalizationChangedRequest(
            ref LudeoSessionLocalizationChangedCallbackParams nativeParams)
        {
            try
            {
                nativeParams.ConvertToManaged(out LudeoSessionLocalizationChangedCallbackData managedData);
                LudeoLogManager.LogInfo($"{NOTIFY_LOG_PREFIX} Localization changed! {managedData}");
                PluginCallbacks.onLocalizationChanged(managedData);
         }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{NOTIFY_LOG_PREFIX} Localization changed callback failed: {e}");
            }
        }

        [MonoPInvokeCallback(typeof(LudeoSessionRoomReadyCallback))]
        private static void HandleRoomReady(ref LudeoSessionRoomReadyCallbackParams nativeParams)
        {
            try
            {
                nativeParams.ConvertToManaged(out LudeoSessionRoomReadyCallbackData managedData);
                LudeoLogManager.LogInfo($"{NOTIFY_LOG_PREFIX} Ludeo room ready!");

                if (LudeoRoom.ActiveRoom.Handle != nativeParams.room)
                    LudeoLogManager.LogError($"{NOTIFY_LOG_PREFIX} The received room handle does not match the active room's handle!");

                PluginCallbacks.onRoomReadyCallback(managedData);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{NOTIFY_LOG_PREFIX} Room ready callback failed: {e}");
            }
        }
      
        [MonoPInvokeCallback(typeof(LudeoSessionHighlightCallback))]
        private static void HandleHighlight(ref LudeoSessionHighlightCallbackParams nativeParams)
        {
            try
            {
                nativeParams.ConvertToManaged(out LudeoSessionHighlightCallbackData managedData);
                LudeoLogManager.LogInfo($"{NOTIFY_LOG_PREFIX} Ludeo highlight taken!");

                if (PluginCallbacks.onHighlightTakenCallback != null)
                    PluginCallbacks.onHighlightTakenCallback(managedData);
                else
                    LudeoLogManager.LogError($"{API_PREFIX_RELEASE} onHighlightTakenCallback callback already cleared. Will not be able to Upload highlightId: {managedData.highlightId}");
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{NOTIFY_LOG_PREFIX} Highlight callback failed: {e}");
            }
        }

        [MonoPInvokeCallback(typeof(LudeoSessionConsentUpdatedCallback))]
        private static void HandleConsentUpdate(ref LudeoSessionConsentUpdatedCallbackParams nativeParams)
        {
            try
            {
                nativeParams.ConvertToManaged(out LudeoSessionConsentUpdatedCallbackData managedData);
                LudeoLogManager.LogInfo($"{NOTIFY_LOG_PREFIX} Ludeo consent updated! {managedData}");

                if (PluginCallbacks.onConsentUpdateCallback != null)
                    PluginCallbacks.onConsentUpdateCallback(managedData);
                else
                    LudeoLogManager.LogError($"{API_PREFIX_RELEASE} onConsentUpdateCallback callback already cleared.");
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{NOTIFY_LOG_PREFIX} Consent callback failed: {e}");
            }
        }

        [MonoPInvokeCallback(typeof(LudeoSessionVideoStateCallback))]
        private static void HandleVideoUpdate(ref LudeoSessionVideoStateCallbackParams nativeParams)
        {
            try
            {
                nativeParams.ConvertToManaged(out LudeoSessionVideoStateCallbackData managedData);
                LudeoLogManager.LogInfo($"{NOTIFY_LOG_PREFIX} Video state updated! {managedData}");

                if (SystemData.Instance.isCoreVideoEnabled)
                    HandleMediaCaptureUpdateDone(CaptureStatus.InitSuccess, managedData.videoEnabled);
                else
                    LudeoUnityManager.Instance.TEMP_UpdateMediaCaptureService(SystemData.Instance, managedData, HandleMediaCaptureUpdateDone);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{NOTIFY_LOG_PREFIX} Video state callback failed: {e}");
            }
        }

        private static void HandleMediaCaptureUpdateDone(CaptureStatus captureStatus, bool isOn)
        {
            string strContextId = SystemData.Instance.ludeoSessionVideoData.contextId;

            LudeoLogManager.LogInfo($"{LOG_PREFIX} HandleMediaCaptureUpdateDone: {captureStatus}, capture is set to: {isOn}, strContextId: {strContextId}");
            
            IntPtr contextId = !string.IsNullOrEmpty(strContextId) ? Marshal.StringToHGlobalAnsi(strContextId) : IntPtr.Zero;

            if ((m_ludeoSessionWrapper != null) && (m_sessionHandle != LudeoHSession.Uninitialized))
            {
                if (contextId != IntPtr.Zero)
                {
                    try
                    {
                        m_ludeoSessionWrapper.AckVideoStateChange(m_sessionHandle, contextId);
                    }
                    catch(Exception e) 
                    {
                        LudeoLogManager.LogError($"{LOG_PREFIX} AckVideoStateChange error: " + e);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(contextId);
                    }
                }
                else
                    LudeoLogManager.LogVerbose($"{LOG_PREFIX} HandleMediaCaptureUpdateDone got no contextId. No need to AckVideoStateChange.");
            }
            else
                LudeoLogManager.LogError($"{LOG_PREFIX} HandleMediaCaptureUpdateDone missing session wrapper or session and can not AckVideoStateChange.");


            // Temporary fix for LC-8977: the ludeo force lunch should be handled by the session activation flow,
            // but as the plugin doesn't know the auth token in time, we call ludeo selected on the first video state change, as before
            if (LudeoManager.Instance.IsActivationCallbackWaiting && LudeoManager.Instance.IsForceLudeoLaunch(out Guid ludeoIdToInject))
                PluginCallbacks.onLudeoSelected(new LudeoSelectedCallbackData() { ludeoId = ludeoIdToInject });
        }


        private static void BringAppToFront()
        {
            IOSSystems osSys = LudeoUnityManager.LudeoInterfaceFactory.GetIOSSystems();
            osSys.BringAppToFront(isSupportBringToFront: true); // Need to get from config if we want to enable disabling this remotely
        }
    }
    
}
/// @endcond INTERNAL