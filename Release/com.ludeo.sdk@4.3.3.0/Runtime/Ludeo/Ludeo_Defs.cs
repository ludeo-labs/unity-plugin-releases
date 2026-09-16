// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using LudeoSDK.Internal;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;


    public enum LudeoSDKCoreDll
    {
        Release,
        Development,
    }

    /// <summary>
    /// Structure with the parameters passed to the callback for when a ludeo is selected
    /// </summary>
    public struct LudeoSelectedCallbackData
    {
        /// <summary>
        /// Id of the selected ludeo.
        /// If the client wishes to play the ludeo, it should pass this to LudeoSession.GetLudeo to get the actual ludeo data.
        /// </summary>
        public Guid ludeoId;
    }

    /// <summary>
    /// Returned as an argument of the pauseGameCallback callback  .
    /// </summary>
    public struct LudeoPauseGameCallbackData
    {
    }

    /// <summary>
    /// Returned as an argument of the resumeGameCallback callback  .
    /// </summary>
    public struct LudeoResumeGameCallbackData
    {
    }

    /// <summary>
    /// Returned as an argument of the returnToMainMenuCallback callback  .
    /// </summary>
    public struct LudeoReturnToMainMenuCallbackData
    {
    }

    /// <summary>
    /// Returned as an argument of the onRoomReadyCallback callback  .
    /// </summary>
    public struct LudeoSessionRoomReadyCallbackData
    {
        /// <summary>The LudeoRoom created for this session.</summary>
        public LudeoRoom ludeoRoom;
    }

    /// <summary>
    /// Returned as an argument of the onConsentUpdateCallback callback.
    /// </summary>
    public struct LudeoSessionConsentUpdatedCallbackData
    {
        /// <summary>If true, opening a room to create Ludeos is allowed.</summary>
        public bool canCreateLudeo;

        /// <summary>If true, opening a room to play a ludeo is allowed.</summary>
        public bool canPlayLudeo;

        public override string ToString()
        {
            return "ConsentUpdated: canCreateLudeo=" + canCreateLudeo + "; canPlayLudeo=" + canPlayLudeo;
        }
    }

    /// <summary>
    /// Returned as an argument of the <see cref="LudeoSessionMuteRequestCallback"/> callback.
    /// </summary>
    public struct LudeoSessionMuteRequestCallbackData
    {
        /// <summary> If true, the request is to mute the game, otherwise it is to unmute the game. </summary>
        public bool isMuted;
    }


    /// <summary>
    ///  Returned as an argument of the onLocalizationChanged callback.
    /// </summary>
    public struct LudeoSessionLocalizationChangedCallbackData
    {
        /// <summary>
        /// The new language that the SDK is using.
        /// This should be in the format of a 2 character language code, such as "en" for English.
        /// </summary>
        public string language;
    }

    public struct LudeoSessionVideoStateCallbackData
    {
        public bool videoEnabled;
        public int maxHighlightDuration;
        public string apiUpdateUploadUrl;
        public string contextId;

        public override string ToString()
        {
            return $"VideoEnabled:{videoEnabled}; maxVideoDuration:{maxHighlightDuration}; updateUrl:{apiUpdateUploadUrl}; contextId:{contextId}";
        }
    }

    public struct LudeoSessionHighlightCallbackData
    {
        public string highlightId;
        public string highlightUrl;
    }

    
    /// <summary>
    /// Returned as an argument of the LudeoSession.Activate callback.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionActivateCallbackData
    {
        /// <summary>Tells if the operation succeeded or failed.</summary>
        public LudeoResult resultCode;

        /// <summary>
        /// If as part of the session activation the SDK detected that a ludeo should be played, this is set to true.
        /// When this is true, the SDK guarantees that soon after this callback, a LudeoSelected notification is received.
        /// </summary>
        public bool isLudeoSelected;
    }

    /// <summary>
    /// Parameters for setting the SDK localization (preferred + supported languages).
    /// </summary>
    public struct LudeoSessionSetLocalizationParameters
    {
        /// <summary>A 2 characters code for the client's preferred language.</summary>
        public string language;
        /// <summary>Array of strings for all the languages the client supports. (Optional)</summary>
        public string[] supportedLanguages;
    }
    
    public struct LudeoSessionReleaseCallbackData
    {
        public LudeoResult resultCode;
    }
}
