// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    using System;
    using System.Runtime.InteropServices;

    internal partial class LudeoSessionWrapper : ILudeoSessionWrapper
    {
        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern LudeoResult ludeo_Session_Create(ref LudeoSessionCreateParams createParams, out LudeoHSession outHandle);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ludeo_Session_Release(LudeoHSession sessionHandle, ref LudeoSessionReleaseParams releaseParams, IntPtr clientData, LudeoSessionReleaseCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ludeo_Session_Activate(LudeoHSession sessionHandle, ref LudeoSessionActivateParams activateParams, IntPtr clientData, LudeoSessionActivateCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ludeo_Session_GetLudeo(LudeoHSession sessionHandle, ref LudeoSessionGetLudeoParams getParams, IntPtr clientData, LudeoSessionGetLudeoCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong ludeo_Session_AddNotifyLudeoSelected(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyLudeoSelectedParams notificationParams, IntPtr clientData, LudeoSessionLudeoSelectedCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong ludeo_Session_AddNotifyPauseGameRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyPauseGameRequestParams notificationParams,IntPtr clientData, LudeoSessionPauseGameRequestCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong ludeo_Session_AddNotifyResumeGameRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyResumeGameRequestParams notificationParams, IntPtr clientData, LudeoSessionResumeGameRequestCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong ludeo_Session_AddNotifyBackToMenuRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyBackToMenuRequestParams notificationParams, IntPtr clientData, LudeoSessionBackToMenuRequestCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong ludeo_Session_AddNotifyRoomReady(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyRoomReadyParams notificationParams, IntPtr clientData, LudeoSessionRoomReadyCallback roomReadyCallback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong ludeo_Session_AddNotifyHighlight(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyHighlightParams notificationParams, IntPtr clientData, LudeoSessionHighlightCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong ludeo_Session_AddNotifyConsentUpdated(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyConsentUpdatedParams notificationParams, IntPtr clientData, LudeoSessionConsentUpdatedCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong ludeo_Session_AddNotifyVideoState(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyVideoStateParam notificationParams, IntPtr clientData, LudeoSessionVideoStateCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern LudeoResult ludeo_Session_TEMP_AckVideoStateChange(LudeoHSession handle, IntPtr contextId);
        
        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong ludeo_Session_AddNotifyMuteRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyMuteRequestParams notificationParams, IntPtr clientData, LudeoSessionMuteRequestCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong ludeo_Session_AddNotifyLocalizationChanged(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyLocalizationChangedParams notificationParams, IntPtr clientData, LudeoSessionLocalizationChangedCallback callback);
        
        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ludeo_Session_OpenRoom(LudeoHSession sessionHandle, ref LudeoSessionOpenRoomParams ludeoSessionOpenRoomParams, IntPtr clientData, LudeoSessionOpenRoomCallback onRoomOpenedCallback);
        
        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern LudeoResult ludeo_Session_OpenGallery(LudeoHSession sessionHandle, ref LudeoSessionOpenGalleryParams ludeoSessionOpenGalleryParams);
        
        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern LudeoResult ludeo_Session_SetLocalization(LudeoHSession sessionHandle, ref LudeoSessionSetLocalizationParams ludeoSessionSetLocalizationParams);
    }
}