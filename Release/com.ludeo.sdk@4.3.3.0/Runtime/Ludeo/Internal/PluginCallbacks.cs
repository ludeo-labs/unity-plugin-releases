/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using System;
    using System.Collections.Generic;

    internal static class PluginCallbacks
    {
        private const ulong INVALID_NOTIFICATION_ID = 0;

        private const string LOG_PREFIX = "PluginCallbacks";
        public static Action<LudeoSelectedCallbackData> onLudeoSelected;
        public static Action<LudeoSessionRoomReadyCallbackData> onRoomReadyCallback;
        public static Action<LudeoSessionConsentUpdatedCallbackData> onConsentUpdateCallback;
        public static Action pauseGameCallback;
        public static Action resumeGameCallback;
        public static Action returnToMainMenuCallback;
        public static Action<LudeoSessionMuteRequestCallbackData> onMuteRequestCallback;
        
        public static Action<LudeoSessionHighlightCallbackData> onHighlightTakenCallback;
        public static Action<LudeoSessionReleaseCallbackData> onLudeoSessionRelease;
        public static Action<LudeoSessionLocalizationChangedCallbackData> onLocalizationChanged;

        private static HashSet<ulong> registeredNotifications = new HashSet<ulong>();
        private static ILudeoInitWrapper m_ludeoInitWrapper;

        public static void InjectImplementation(ILudeoInitWrapper ludeoInitWrapper)
        {
            m_ludeoInitWrapper = ludeoInitWrapper;
        }
        

        public static void RegisterInternalCallbacks(LudeoManager.LudeoSessionInternalCallbacks gameSessionInternalCallbacks)
        {
            onLudeoSessionRelease = gameSessionInternalCallbacks.onLudeoSessionRelease ?? throw new ArgumentNullException($"{LOG_PREFIX} onLudeoSessionRelease can not be null!");
        }

        public static void DeinitCallbacks()
        {
            // Core calls all notifications at shutdown and clears the registered one
            RemoveAllNotifications(registeredNotifications);
            
            onLudeoSelected = null;
            pauseGameCallback = null;
            resumeGameCallback = null;
            returnToMainMenuCallback = null;
            onRoomReadyCallback = null;
            onHighlightTakenCallback = null;
            onLudeoSessionRelease = null;
            onConsentUpdateCallback = null;
            onMuteRequestCallback = null;
            onLocalizationChanged = null;

            registeredNotifications.Clear();
        }

        public static void RegisterNotificationId(string notificationName, ulong notificationId)
        {
            LudeoLogManager.LogVerbose($"RegisterNotificationId for {notificationName}, got Id: {notificationId}");

            if (notificationId == INVALID_NOTIFICATION_ID)
                throw new Exception($"{LOG_PREFIX} Failed to register notification: {notificationName}");

            if (registeredNotifications.Contains(notificationId))
            {
                LudeoLogManager.LogWarning($"{LOG_PREFIX} notificationId {notificationId} was already registered.");
                return;
            }

            registeredNotifications.Add(notificationId);
        }

        internal static bool CheckAllNotificationRemoved()
        {
            if (registeredNotifications.Count > 0)
                return false;

            if ( onLudeoSelected != null
                || pauseGameCallback != null
                || resumeGameCallback != null
                || returnToMainMenuCallback != null
                || onRoomReadyCallback != null
                || onLudeoSessionRelease != null
                || onHighlightTakenCallback != null
                || onConsentUpdateCallback != null
                || onMuteRequestCallback != null
                || onLocalizationChanged != null)
                return false;

            return true;
        }

        private static unsafe void RemoveAllNotifications(HashSet<ulong> notifications)
        {
            ulong* ptrNotifications = stackalloc ulong[notifications.Count];
            HashSet<ulong>.Enumerator enumerator = notifications.GetEnumerator();
            int bufferSize = 0;
            while (enumerator.MoveNext())
            {
                ptrNotifications[bufferSize++] = enumerator.Current;
            }

            LudeoInit.RemoveNotifications(m_ludeoInitWrapper, ptrNotifications, bufferSize);
        }
    }
}
/// @endcond INTERNAL