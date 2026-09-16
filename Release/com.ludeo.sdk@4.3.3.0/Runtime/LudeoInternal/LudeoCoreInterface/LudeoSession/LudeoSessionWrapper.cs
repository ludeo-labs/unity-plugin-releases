// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    using System;

    internal partial class LudeoSessionWrapper : ILudeoSessionWrapper
    {
        public LudeoResult LudeoSessionCreate(ref LudeoSessionCreateParams createParams, out LudeoHSession outHandle)
        {
            return ludeo_Session_Create(ref createParams, out outHandle);
        }

        public void LudeoSessionRelease(LudeoHSession sessionHandle, ref LudeoSessionReleaseParams releaseParams, IntPtr clientData, LudeoSessionReleaseCallback callback)
        {
            ludeo_Session_Release(sessionHandle, ref releaseParams, clientData, callback);
        }

        public void LudeoSessionActivate(LudeoHSession sessionHandle, ref LudeoSessionActivateParams activateParams, IntPtr clientData, LudeoSessionActivateCallback callback)
        {
            ludeo_Session_Activate(sessionHandle, ref activateParams, clientData, callback);
        }

        public void GetLudeo(LudeoHSession sessionHandle, ref LudeoSessionGetLudeoParams getParams, IntPtr clientData, LudeoSessionGetLudeoCallback callback)
        {
            ludeo_Session_GetLudeo( sessionHandle, ref getParams, clientData, callback);
        }
        
        public ulong AddNotifyLudeoSelectedRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyLudeoSelectedParams notificationParams, IntPtr clientData, LudeoSessionLudeoSelectedCallback callback)
        {
            return ludeo_Session_AddNotifyLudeoSelected(sessionHandle, ref notificationParams, clientData, callback);
        }

        public ulong AddNotifyPauseGameRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyPauseGameRequestParams notificationParams, IntPtr clientData, LudeoSessionPauseGameRequestCallback pauseGameCallback)
        {
            return ludeo_Session_AddNotifyPauseGameRequest(sessionHandle, ref notificationParams, clientData, pauseGameCallback);
        }

        public ulong AddNotifyResumeGameRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyResumeGameRequestParams notificationParams, IntPtr clientData, LudeoSessionResumeGameRequestCallback resumeGameCallback)
        {
            return ludeo_Session_AddNotifyResumeGameRequest(sessionHandle, ref notificationParams, clientData, resumeGameCallback);
        }

        public ulong AddNotifyReturnToMainMenuRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyBackToMenuRequestParams notificationParams, IntPtr clientData, LudeoSessionBackToMenuRequestCallback returnToMenuCallback)
        {
            return ludeo_Session_AddNotifyBackToMenuRequest(sessionHandle, ref notificationParams, clientData, returnToMenuCallback);
        }

        public ulong AddNotifyRoomReady(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyRoomReadyParams notificationParams, IntPtr clientData, LudeoSessionRoomReadyCallback roomReadyCallback)
        {
            return ludeo_Session_AddNotifyRoomReady(sessionHandle, ref notificationParams, clientData, roomReadyCallback);
        }

        public ulong AddNotifyHighlightRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyHighlightParams notificationParams, IntPtr clientData, LudeoSessionHighlightCallback callback)
        {
            return ludeo_Session_AddNotifyHighlight(sessionHandle, ref notificationParams, clientData, callback);
        }

        public ulong AddNotifyConsentUpdated(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyConsentUpdatedParams notificationParams, IntPtr clientData, LudeoSessionConsentUpdatedCallback callback)
        {
            return ludeo_Session_AddNotifyConsentUpdated(sessionHandle, ref notificationParams, clientData, callback);
        }

        public ulong AddNotifyMuteRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyMuteRequestParams notificationParams,
            IntPtr clientData, LudeoSessionMuteRequestCallback callback)
        {
            return ludeo_Session_AddNotifyMuteRequest(sessionHandle, ref notificationParams, clientData, callback);
        }
        
        public ulong AddNotifyLocalizationChanged(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyLocalizationChangedParams notificationParams, IntPtr clientData, LudeoSessionLocalizationChangedCallback callback)
        {
            return ludeo_Session_AddNotifyLocalizationChanged(sessionHandle, ref notificationParams, clientData, callback);
        }

        #region VIDEO-LEGACY-SUPPORT
        // TODO: remove the other platform defined methods when removed!
        public ulong AddNotifyVideoState(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyVideoStateParam notificationParams, IntPtr clientData, LudeoSessionVideoStateCallback callback)
        {
            return ludeo_Session_AddNotifyVideoState(sessionHandle, ref notificationParams, clientData, callback);
        }

        public LudeoResult AckVideoStateChange(LudeoHSession sessionHandle, IntPtr contextId)
        {
            return ludeo_Session_TEMP_AckVideoStateChange(sessionHandle, contextId);
        }
        #endregion VIDEO-LEGACY-SUPPORT

        public void OpenRoom(LudeoHSession ludeoSessionHandle, ref LudeoSessionOpenRoomParams ludeoSessionOpenRoomParams, IntPtr clientData, LudeoSessionOpenRoomCallback ludeoSessionOpenRoomCallback)
        {
            ludeo_Session_OpenRoom(ludeoSessionHandle, ref ludeoSessionOpenRoomParams, clientData, ludeoSessionOpenRoomCallback);
        }
        
        public LudeoResult OpenGallery(LudeoHSession sessionHandle, LudeoSessionOpenGalleryParams ludeoSessionOpenGalleryParams)
        {
            return ludeo_Session_OpenGallery(sessionHandle, ref ludeoSessionOpenGalleryParams);
        }
        
        public LudeoResult SetLocalization(LudeoHSession sessionHandle, LudeoSessionSetLocalizationParams ludeoSessionSetLocalizationParams)
        {
            return ludeo_Session_SetLocalization(sessionHandle, ref ludeoSessionSetLocalizationParams);
        }
    }
}