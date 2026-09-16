// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using LudeoSDK.Internal;
    using LudeoSDK.MediaCapture;
    using LudeoSDK.UnityScripts;
    using System;


    /// <summary>
    /// The LudeoPlayer encapsulates the context of a player in a Room.
    /// Receive it by calling LudeoRoom.AddPlayer.
    /// This context has 3 lifetime stages:
    /// 1. Initial state (as created by the Room)
    /// 2. Active gameplay. This is triggered with LudeoPlayer.BeginGameplay
    /// 3. Ended. This is triggered with LudeoPlayer.EndGameplay
    /// The lifetime of these are tied to the owning Room. This Object
    /// is invalidated when calling LudeoRoom.RemovePlayer, or LudeoRoom.CloseRoom.
    /// </summary>
    public partial class LudeoPlayer
    {
        private static ILudeoGameplaySessionWrapper m_ludeoGameplaySessionWrapper;
        private LudeoHGameplaySession m_gameplaySessionHandle;

        private INativeHelperMethods m_nativeHelperMethods;
        private MediaCaptureService m_mediaCaptureService;
        private LudeoGameplaySessionInternal m_gameplayInternalInterface;

        /// <summary>The player id this player was added to the room with.</summary>
        public string PlayerId { get; }

        private void InitCoreInterface()
        {
            ILudeoCoreInterface coreHook = LudeoUnityHelpers.TryGetLudeoCoreInterface();
            m_ludeoGameplaySessionWrapper = coreHook.GetLudeoGameplaySessionWrapper();
        }

        internal LudeoPlayer(LudeoHGameplaySession gameplaySessionHandle, string playerId)
        {
            InitCoreInterface();

            PluginCallbacks.onHighlightTakenCallback = HandleMarkHighlight;
            m_gameplaySessionHandle = gameplaySessionHandle;
            this.PlayerId = playerId;

            m_nativeHelperMethods = LudeoUnityManager.LudeoInterfaceFactory.GetNativeHelperMethods();
            m_mediaCaptureService = MediaCaptureService.Instance;
            m_gameplayInternalInterface = new LudeoGameplaySessionInternal();
        }

        #region BeginGameplay
        /// <summary>
        /// Signals the SDK to start actual gameplay.
        /// </summary>
        /// <param name="onBeginGameplayCallback">Callback called once the SDK and backend are expecting the client to start actual gameplay.</param>
        /// <exception cref="ArgumentNullException">Thrown in case no callback was provided.</exception>
        public void BeginGameplay(Action<LudeoPlayerBeginGameplayCallbackData> onBeginGameplayCallback)
        {
            if (onBeginGameplayCallback == null)
                throw new ArgumentNullException(nameof(onBeginGameplayCallback));

            m_gameplayInternalInterface.Begin(m_ludeoGameplaySessionWrapper, m_gameplaySessionHandle, HandleBeginGameplay, onBeginGameplayCallback);
        }
        #endregion BeginGameplay

        #region EndGameplay
        /// <summary>
        /// Signals the end of the gameplay
        /// </summary>
        /// <param name="onEndGameplayCallback">Called once the operation completes</param>
        /// <exception cref="ArgumentNullException">Thrown in case no callback was provided.</exception>
        public void EndGameplay(Action<LudeoPlayerEndGameplayCallbackData> onEndGameplayCallback)
        {
            if (onEndGameplayCallback == null)
                throw new ArgumentNullException(nameof(onEndGameplayCallback));

            m_gameplayInternalInterface.End(m_ludeoGameplaySessionWrapper, m_gameplaySessionHandle, HandleEndGameplay, onEndGameplayCallback);
        }

        /// <summary>
        /// Signals the gameplay was aborted.
        /// </summary>
        /// <param name="onAbortGameplayCallback">Called once the operation completes</param>
        /// <exception cref="ArgumentNullException">Thrown in case no callback was provided.</exception>
        public void AbortGameplay(Action<LudeoPlayerAbortGameplayCallbackData> onAbortGameplayCallback)
        {
            if (onAbortGameplayCallback == null)
                throw new ArgumentNullException(nameof(onAbortGameplayCallback));
            m_gameplayInternalInterface.Abort(m_ludeoGameplaySessionWrapper, m_gameplaySessionHandle, HandleEndGameplay, onAbortGameplayCallback);
        }
        #endregion EndGameplay


        /// <summary>
        /// Sends a game action to the backend.
        /// Actions have no parameters. They represent a single thing such as "Kill", "HeadShot", etc.
        /// The action sent is binded to the playerId added to the current room.
        /// </summary>
        /// <param name="action">The action to report</param>
        public void SendAction(string action)
        {
            LudeoDataWriter.SendAction(PlayerId, action);
        }

        [Obsolete("This method is obsolete. LudeoSDK handles Marking Highlights internally.")]
        public void MarkHighlight(Action<LudeoGameplaySessionMarkHighlightCallbackData> onMarkHighlightCallback)
        {
            if (onMarkHighlightCallback == null)
                throw new ArgumentNullException(nameof(onMarkHighlightCallback));

            LudeoHSession sessionHandle = LudeoManager.Instance.LudeoSession.SessionHandle;
            m_gameplayInternalInterface.MarkHighlight(m_ludeoGameplaySessionWrapper, sessionHandle, onMarkHighlightCallback);
        }

        private void HandleMarkHighlight(LudeoSessionHighlightCallbackData data)
        {
            m_mediaCaptureService.TagVideoWithUploadData(highlightId: data.highlightId, uploadUrl: data.highlightUrl);
        }

        private void HandleBeginGameplay()
        {
            if (!LudeoRoom.ActiveRoom.IsLudeo)
                m_mediaCaptureService.BeginCapture();
        }

        private void HandleEndGameplay()
        {
            if (!LudeoRoom.ActiveRoom.IsLudeo)
                m_mediaCaptureService?.EndCapture();
        }
    }
}
