// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
#if !UNITY_STANDALONE_WIN && !UNITY_ANDROID
    using System;
    using System.Runtime.InteropServices;

    internal partial class LudeoSessionWrapper : ILudeoSessionWrapper
    {
        private static extern LudeoResult ludeo_Session_Create(ref LudeoSessionCreateParams createParams,
            out LudeoHSession outHandle);


        private static extern void ludeo_Session_Release(LudeoHSession sessionHandle,
            ref LudeoSessionReleaseParams releaseParams, IntPtr clientData, LudeoSessionReleaseCallback callback);


        private static extern void ludeo_Session_Activate(LudeoHSession sessionHandle,
            ref LudeoSessionActivateParams activateParams, IntPtr clientData, LudeoSessionActivateCallback callback);


        private static extern void ludeo_Session_GetLudeo(LudeoHSession sessionHandle,
            ref LudeoSessionGetLudeoParams getParams, IntPtr clientData, LudeoSessionGetLudeoCallback callback);


        private static extern ulong ludeo_Session_AddNotifyLudeoSelected(LudeoHSession sessionHandle,
            ref LudeoSessionAddNotifyLudeoSelectedParams notificationParams, IntPtr clientData,
            LudeoSessionLudeoSelectedCallback callback);


        private static extern ulong ludeo_Session_AddNotifyPauseGameRequest(LudeoHSession sessionHandle,
            ref LudeoSessionAddNotifyPauseGameRequestParams notificationParams, IntPtr clientData,
            LudeoSessionPauseGameRequestCallback callback);


        private static extern ulong ludeo_Session_AddNotifyResumeGameRequest(LudeoHSession sessionHandle,
            ref LudeoSessionAddNotifyResumeGameRequestParams notificationParams, IntPtr clientData,
            LudeoSessionResumeGameRequestCallback callback);


        private static extern ulong ludeo_Session_AddNotifyBackToMenuRequest(LudeoHSession sessionHandle,
            ref LudeoSessionAddNotifyBackToMenuRequestParams notificationParams, IntPtr clientData,
            LudeoSessionBackToMenuRequestCallback callback);


        private static extern ulong ludeo_Session_AddNotifyRoomReady(LudeoHSession sessionHandle,
            ref LudeoSessionAddNotifyRoomReadyParams notificationParams, IntPtr clientData,
            LudeoSessionRoomReadyCallback roomReadyCallback);

        private static extern ulong ludeo_Session_AddNotifyHighlight(LudeoHSession sessionHandle,
            ref LudeoSessionAddNotifyHighlightParams notificationParams, IntPtr clientData,
            LudeoSessionHighlightCallback callback);

        private static extern ulong ludeo_Session_AddNotifyConsentUpdated(LudeoHSession sessionHandle, 
            ref LudeoSessionAddNotifyConsentUpdatedParams notificationParams, IntPtr clientData, 
            LudeoSessionConsentUpdatedCallback callback);

        private static extern ulong ludeo_Session_AddNotifyVideoState(LudeoHSession sessionHandle, 
            ref LudeoSessionAddNotifyVideoStateParam notificationParams, IntPtr clientData, 
            LudeoSessionVideoStateCallback callback);

        private static extern ulong ludeo_Session_AddNotifyLocalizationChanged(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyLocalizationChangedParams notificationParams, IntPtr clientData, LudeoSessionLocalizationChangedCallback callback);

        private static extern LudeoResult ludeo_Session_TEMP_AckVideoStateChange(LudeoHSession sessionHandle, IntPtr contextId);
        
        private static extern void ludeo_Session_OpenRoom(LudeoHSession sessionHandle,
            ref LudeoSessionOpenRoomParams ludeoSessionOpenRoomParams, IntPtr clientData,
            LudeoSessionOpenRoomCallback onRoomOpenedCallback);


        private static extern LudeoResult ludeo_Session_OpenGallery(LudeoHSession sessionHandle,
            ref LudeoSessionOpenGalleryParams ludeoSessionOpenGalleryParams);
        
        private static extern LudeoResult ludeo_Session_SetLocalization(LudeoHSession sessionHandle, 
            ref LudeoSessionSetLocalizationParams ludeoSessionSetLocalizationParams);
    }
#endif
}
