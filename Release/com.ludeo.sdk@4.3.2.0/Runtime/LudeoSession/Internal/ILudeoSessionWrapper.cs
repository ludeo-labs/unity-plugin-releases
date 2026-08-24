/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using System;

    public interface ILudeoSessionWrapper
    {
        LudeoResult LudeoSessionCreate(ref LudeoSessionCreateParams createParams, out LudeoHSession outHandle);

        void LudeoSessionRelease(LudeoHSession sessionHandle, ref LudeoSessionReleaseParams releaseParams, IntPtr clientData, LudeoSessionReleaseCallback callback);

        void LudeoSessionActivate(LudeoHSession sessionHandle, ref LudeoSessionActivateParams activateParams, IntPtr clientData, LudeoSessionActivateCallback callback);


        void GetLudeo(LudeoHSession sessionHandle, ref LudeoSessionGetLudeoParams getLudeoParams, IntPtr clientDataPrt, LudeoSessionGetLudeoCallback action);


        ulong AddNotifyLudeoSelectedRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyLudeoSelectedParams notificationParams, IntPtr clientData, LudeoSessionLudeoSelectedCallback callback);

        ulong AddNotifyPauseGameRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyPauseGameRequestParams notificationParams, IntPtr clientData, LudeoSessionPauseGameRequestCallback pauseGameCallback);

        ulong AddNotifyResumeGameRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyResumeGameRequestParams notificationParams, IntPtr clientData, LudeoSessionResumeGameRequestCallback resumeGameCallback);

        ulong AddNotifyReturnToMainMenuRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyBackToMenuRequestParams notificationParams, IntPtr clientData, LudeoSessionBackToMenuRequestCallback returnToMenuCallback);

        ulong AddNotifyRoomReady(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyRoomReadyParams notificationParams, IntPtr clientData, LudeoSessionRoomReadyCallback roomReadyCallback);

        ulong AddNotifyHighlightRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyHighlightParams notificationParams, IntPtr clientData, LudeoSessionHighlightCallback callback);

        ulong AddNotifyConsentUpdated(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyConsentUpdatedParams notificationParams, IntPtr clientData, LudeoSessionConsentUpdatedCallback callback);
        
        ulong AddNotifyMuteRequest(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyMuteRequestParams notificationParams, IntPtr clientData, LudeoSessionMuteRequestCallback callback);

        #region VIDEO=LEGACY-SUPPORT
        ulong AddNotifyVideoState(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyVideoStateParam notificationParams, IntPtr clientData, LudeoSessionVideoStateCallback callback);
        
        LudeoResult AckVideoStateChange(LudeoHSession sessionHandle, IntPtr contextId);
        #endregion VIDEO=LEGACY-SUPPORT
        ulong AddNotifyLocalizationChanged(LudeoHSession sessionHandle, ref LudeoSessionAddNotifyLocalizationChangedParams notificationParams, IntPtr clientData, LudeoSessionLocalizationChangedCallback callback);

        void OpenRoom(LudeoHSession ludeoSessonHandle, ref LudeoSessionOpenRoomParams ludeoSessionOpenRoomParams, IntPtr clientData, LudeoSessionOpenRoomCallback ludeoSessionOpenRoomCallback);
        
        LudeoResult OpenGallery(LudeoHSession sessionHandle, LudeoSessionOpenGalleryParams ludeoSessionOpenGalleryParams);
        
        LudeoResult SetLocalization(LudeoHSession sessionHandle, LudeoSessionSetLocalizationParams ludeoSessionSetLocalizationParams);
    }
}
/// @endcond INTERNAL