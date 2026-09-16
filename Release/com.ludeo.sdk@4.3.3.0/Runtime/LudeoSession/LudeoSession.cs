// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using LudeoSDK.Internal;
    using LudeoSDK.MediaCapture;
    using LudeoSDK.UnityScripts;
    using System;

    /// <summary>
    /// A LudeoSession instance represents a logged in Ludeo user and their connection to the backend.
    /// </summary>
    public partial class LudeoSession : IDisposable
    {
        private const string LOG_PREFIX = "[LudeoSession]";

        internal LudeoHSession SessionHandle { get; private set; }

        private static ILudeoSessionWrapper m_ludeoSessionWrapper;
        private bool disposedValue;

        private LudeoSession_GetLudeo m_getLudeoInterface;
        private LudeoSession_OpenRoom m_roomApi;
        private bool m_notificationsRegistered;

        private LudeoSession_GetLudeo GetLudeoInterface
        {
            get
            {
                if (m_getLudeoInterface == null)
                    m_getLudeoInterface = new LudeoSession_GetLudeo();

                return m_getLudeoInterface;
            }
        }

        private LudeoSession_OpenRoom RoomInterface
        {
            get
            {
                if (m_roomApi == null)
                    m_roomApi = new LudeoSession_OpenRoom();

                return m_roomApi;
            }
        }

        private void InitCoreInterface()
        {
            ILudeoCoreInterface coreHook = LudeoUnityHelpers.TryGetLudeoCoreInterface();
            m_ludeoSessionWrapper = coreHook.GetLudeoSessionWrapper();
        }

        internal LudeoSession(LudeoHSession ludeoSessionHandle)
        {
            InitCoreInterface();

            SessionHandle = ludeoSessionHandle;            
        }
        
        /// <summary>
        /// Activates the Ludeo session, establishing the connection to the backend.
        /// Should be called after subscribing to any of the session's events (e.g. <see cref="LudeoSelected"/>).
        /// Activation registers the required native notifications and (on platforms that require it)
        /// initializes legacy media capture before connecting to the backend.
        /// </summary>
        /// <param name="callback">Callback to be called when the session activation is finished.</param>
        /// <param name="ludeoSessionLocalizationData">Optional struct to provide language preferences. <see cref="SetLocalization"/></param>
        public void Activate(Action<LudeoSessionActivateCallbackData> callback, LudeoSessionSetLocalizationParameters ludeoSessionLocalizationData = default)
        {
            RegisterCallbacksInNative();

#if UNITY_WIN //Legacy video capture is only supported on Windows
            if (!SystemData.Instance.isCoreVideoEnabled)
            {
                LudeoUnityManager.Instance.TEMP_InitializeMediaCapture(SystemData.Instance, captureStatus =>
                {
                    if (captureStatus != CaptureStatus.InitSuccess)
                    {
                        LudeoLogManager.LogError($"{LOG_PREFIX} Legacy media capture init failed: {captureStatus}");
                        callback?.Invoke(new LudeoSessionActivateCallbackData
                        {
                            resultCode = LudeoResult.CaptureServiceError,
                            isLudeoSelected = false,
                        });
                        return;
                    }
                    CallNativeActivate(callback, ludeoSessionLocalizationData);
                });
                return;
            }
#endif
            CallNativeActivate(callback, ludeoSessionLocalizationData);
        }

        private void CallNativeActivate(Action<LudeoSessionActivateCallbackData> callback, LudeoSessionSetLocalizationParameters ludeoSessionLocalizationData)
        {
            LudeoSessionInternal.Activate(m_ludeoSessionWrapper, SessionHandle, callback,
                LudeoManager.Instance.LocalConfigData, ludeoSessionLocalizationData);
        }


        
        /// <summary>
        /// Opens the ludeo gallery.
        /// This allows the client to programmatically open the ludeo gallery.
        /// </summary>
        public void OpenGallery()
        {
            LudeoSessionInternal.OpenGallery(m_ludeoSessionWrapper, SessionHandle);
        } 
        
        /// <summary>
        /// Changes the Overlay's localization.
        /// The change will only occur if the Overlay actually supports the requested language(s).
        /// If a change was made, the LocalizationChanged notification is raised.
        /// </summary>
        /// <param name="ludeoSessionLocalizationData"><see cref="LudeoSessionSetLocalizationParameters"/></param>
        public void SetLocalization(LudeoSessionSetLocalizationParameters ludeoSessionLocalizationData)
        {
            LudeoSessionInternal.SetLocalization(m_ludeoSessionWrapper, SessionHandle, ludeoSessionLocalizationData);
        }

        /// <summary>
        /// Opens a Ludeo room
        /// </summary>
        /// <param name="openRoomCallback">Callback to when the room is opened</param>
        /// <param name="openRoomData">Optional struct to provide:
        ///  LudeoId - ID of the Ludeo which the room was opened for
        ///  RoomId - Your multiplier game room Id</param>
        /// <exception cref="ArgumentNullException">Thrown in case no callback was provided</exception>
        public void OpenRoom(LudeoSessionOpenRoomParameters openRoomData, Action<LudeoOpenRoomCallbackData> openRoomCallback)
        {
            if (openRoomCallback == null)
                throw new ArgumentNullException(nameof(openRoomCallback));
            RoomInterface.OpenRoom(m_ludeoSessionWrapper, SessionHandle, openRoomData, openRoomCallback);
        }

        /// <summary>
        /// Asynchronously retrieves from the backend (or from the local cache if available)
        /// the information needed to start playing a specified Ludeo.
        /// </summary>
        /// <param name="ludeoID">Requested Ludeo ID</param>
        /// <param name="onGetLudeoDone">Called once the ludeo information is available. </param>
        /// <exception cref="ArgumentNullException">If no callback was given</exception>
        public void GetLudeo(Guid ludeoID, Action<LudeoGetLudeoCallbackData> onGetLudeoDone)
        {
            if (onGetLudeoDone == null)
                throw new ArgumentNullException(nameof(onGetLudeoDone));

            GetLudeoInterface.GetLudeo(m_ludeoSessionWrapper, SessionHandle, ludeoID, onGetLudeoDone);
        }

        

        #region Ludeo Notifications

        private void RegisterCallbacksInNative()
        {
            if (m_notificationsRegistered)
                return;

            try
            {
                LudeoSessionInternal.AddNotifyLudeoSelected(m_ludeoSessionWrapper, SessionHandle);
                LudeoSessionInternal.AddNotifyPauseGame(m_ludeoSessionWrapper, SessionHandle);
                LudeoSessionInternal.AddNotifyResumeGame(m_ludeoSessionWrapper, SessionHandle);
                LudeoSessionInternal.AddNotifyReturnToMainMenu(m_ludeoSessionWrapper, SessionHandle);
                LudeoSessionInternal.AddNotifyHighlight(m_ludeoSessionWrapper, SessionHandle);
                LudeoSessionInternal.AddNotifyRoomReady(m_ludeoSessionWrapper, SessionHandle);
                LudeoSessionInternal.AddNotifyConsentUpdated(m_ludeoSessionWrapper, SessionHandle);
                LudeoSessionInternal.AddNotifyVideoState(m_ludeoSessionWrapper, SessionHandle);
                LudeoSessionInternal.AddNotifyMuteRequest(m_ludeoSessionWrapper, SessionHandle);
                LudeoSessionInternal.AddNotifyLocalizationChanged(m_ludeoSessionWrapper, SessionHandle);
                m_notificationsRegistered = true;
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} {e}");
                throw new Exception($"{LOG_PREFIX} {e}");
            }
        }
        
        // --- Events ---

        /// <summary>
        /// Raised when the player selects a Ludeo to play — via "Play Ludeo" from the platform
        /// (once the SDK is initialized and ready), a shared Ludeo link, a discovery panel, or
        /// restarting/replaying a Ludeo.
        /// </summary>
        public event Action<LudeoSelectedCallbackData> LudeoSelected
        {
            add => PluginCallbacks.onLudeoSelected += value;
            remove => PluginCallbacks.onLudeoSelected -= value;
        }

        /// <summary>Raised when the Ludeo requests the game to pause.</summary>
        public event Action PauseGameRequested
        {
            add => PluginCallbacks.pauseGameCallback += value;
            remove => PluginCallbacks.pauseGameCallback -= value;
        }

        /// <summary>Raised when the Ludeo requests the game to resume.</summary>
        public event Action ResumeGameRequested
        {
            add => PluginCallbacks.resumeGameCallback += value;
            remove => PluginCallbacks.resumeGameCallback -= value;
        }

        /// <summary>Raised when the player chooses to exit the Ludeo back to the main menu.</summary>
        public event Action GameBackToMenuRequested
        {
            add => PluginCallbacks.returnToMainMenuCallback += value;
            remove => PluginCallbacks.returnToMainMenuCallback -= value;
        }

        /// <summary>Raised when the opened room is ready for play.</summary>
        public event Action<LudeoSessionRoomReadyCallbackData> RoomReady
        {
            add => PluginCallbacks.onRoomReadyCallback += value;
            remove => PluginCallbacks.onRoomReadyCallback -= value;
        }

        /// <summary>Raised when the player changes their consent.</summary>
        public event Action<LudeoSessionConsentUpdatedCallbackData> PlayerConsentUpdated
        {
            add => PluginCallbacks.onConsentUpdateCallback += value;
            remove => PluginCallbacks.onConsentUpdateCallback -= value;
        }

        /// <summary>Raised when the Ludeo is about to play/stop audio.</summary>
        public event Action<LudeoSessionMuteRequestCallbackData> MuteGameRequested
        {
            add => PluginCallbacks.onMuteRequestCallback += value;
            remove => PluginCallbacks.onMuteRequestCallback -= value;
        }

        /// <summary>Raised when the Ludeo localization changes.</summary>
        public event Action<LudeoSessionLocalizationChangedCallbackData> LocalizationUpdated
        {
            add => PluginCallbacks.onLocalizationChanged += value;
            remove => PluginCallbacks.onLocalizationChanged -= value;
        }

        #endregion Ludeo Notifications

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (m_ludeoSessionWrapper == null)
                    throw new InvalidOperationException($"{LOG_PREFIX} ILudeoSessionWrapper should not be null before sessionHandle is released!");

                if (disposing)
                {
                   
                }

                // Dispose of Gameplay Sessions
                if (LudeoRoom.ActiveRoom != null)
                    LudeoRoom.ActiveRoom.Dispose();
                else
                    LudeoLogManager.LogInfo($"{LOG_PREFIX} No active room to dispose.");

                // Dispose of DataWriters

                // Dispose of Room

                // Dispose of Session

                // TODO: dispose managed state (managed objects)
                if (SessionHandle != LudeoHSession.Uninitialized)
                {
                    LudeoSessionInternal.Release(m_ludeoSessionWrapper, SessionHandle);
                    SessionHandle = LudeoHSession.Uninitialized;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }

            m_ludeoSessionWrapper = null;
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LudeoSession()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}
