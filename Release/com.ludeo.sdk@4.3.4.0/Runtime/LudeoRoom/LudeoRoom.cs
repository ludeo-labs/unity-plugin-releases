// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using Internal;
    using LudeoSDK.UnityScripts;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A class representing a room session - the lobby in which the gameplay is running.
    /// In a single player game, there will be a single gameplay session and player.
    /// In a multiplayer game, there will be multiple gameplay sessions and players.
    /// </summary>
    public partial class LudeoRoom : IDisposable
    {
        internal static string LOG_PREFIX = "[LudeoRoom] ";

        internal static ILudeoRoomWrapper m_ludeoRoomWrapper;

        private static LudeoRoom _instance;

        public static LudeoRoom ActiveRoom => _instance;

        internal LudeoHRoom Handle => m_roomHandle;

        private LudeoHRoom m_roomHandle;

        private LudeoRoomWriter m_writer;

        /// <summary>
        /// The writer used to record gameplay state and actions into this room (Creator flow).
        /// </summary>
        public LudeoRoomWriter Writer => m_writer ?? (m_writer = new LudeoRoomWriter());

        private Dictionary<string, LudeoPlayer> m_playersGameplaySessionLookup = new Dictionary<string, LudeoPlayer>();

        internal Dictionary<string, LudeoPlayer> playersGameplaySessionLookup => m_playersGameplaySessionLookup;

        internal bool IsLudeo { get; private set; }

        private LudeoRoom_Api m_roomApi;
        private bool disposedValue;

        private LudeoRoom_Api RoomInterface
        {
            get
            {
                if (m_roomApi == null)
                    m_roomApi = new LudeoRoom_Api();

                return m_roomApi;
            }
        }

        private void InitCoreInterface()
        {
            ILudeoCoreInterface coreHook = LudeoUnityHelpers.TryGetLudeoCoreInterface();
            m_ludeoRoomWrapper = coreHook.GetLudeoRoomWrapper();
        }

        internal LudeoRoom(LudeoHRoom roomHandle, bool isLudeo, LudeoHDataWriter dataWriterHandle)
        {
            InitCoreInterface();
            m_roomHandle = roomHandle;
            IsLudeo = isLudeo;
            LudeoDataWriter.SetDataWriterHandle(dataWriterHandle);
            _instance = this;
        }

        internal LudeoRoom(LudeoHRoom roomHandle, bool isLudeo)
        {
            InitCoreInterface();
            m_roomHandle = roomHandle;
            IsLudeo = isLudeo;
            _instance = this;
        }

        #region Public Api
        /// <summary>
        /// Closes this room and disposes it, even if it is no longer <see cref="ActiveRoom"/>.
        /// The callback runs asynchronously on a later SDK tick, including when the close fails
        /// (e.g. the room was already closed). Each call gets its own callback.
        /// If called after the SDK has been shut down (or before it is initialized), the callback never runs.
        /// A call still pending at shutdown may complete during shutdown (typically with <see cref="LudeoResult.Canceled"/>), or not at all.
        /// If the native SDK can't be called, the exception propagates, the room is still disposed and the callback doesn't run.
        /// </summary>
        /// <param name="closeRoomCallback">Called when the close operation completes.</param>
        /// <exception cref="ArgumentNullException">Thrown in case no callback was provided</exception>
        public void CloseRoom(Action<LudeoCloseRoomCallbackData> closeRoomCallback)
        {
            if (closeRoomCallback == null)
                throw new ArgumentNullException(nameof(closeRoomCallback));

            RoomInterface.CloseRoom(this, m_ludeoRoomWrapper, m_roomHandle, closeRoomCallback);
        }

        /// <summary>
        /// Adds a new player to the room.
        /// A playerId is the Id you provide for a player in your game.
        /// </summary>
        /// <param name="addPlayerParameters">struct with playerId to add</param>
        /// <param name="addPlayerCallback">Callback to call once the operation completes, on a later SDK tick.
        /// If the native SDK can't be called, it runs synchronously, before AddPlayer returns, with <see cref="LudeoResult.WrapperException"/>.
        /// Don't call AddPlayer again directly from this callback on WrapperException; defer the retry (e.g. to the next frame).
        /// If called after the SDK has been shut down (or before it is initialized), it never runs.
        /// A call still pending at shutdown may complete during shutdown (typically with <see cref="LudeoResult.Canceled"/>), or not at all.</param>
        /// <exception cref="ArgumentNullException">Thrown if playerId is null or empty, or if addPlayerCallback is null</exception>
        public void AddPlayer(LudeoRoomAddPlayerParameters addPlayerParameters, Action<LudeoRoomAddPlayerCallbackData> addPlayerCallback)
        {
            ValidatePlayerDataOrThrow(addPlayerParameters.playerId, addPlayerCallback);
            RoomInterface.AddPlayer(this, m_ludeoRoomWrapper, m_roomHandle, addPlayerParameters.playerId, addPlayerCallback);
        }

        /// <summary>
        /// Removes a player from the room.
        /// </summary>
        /// <param name="removePlayerParameters">Struct with your game's player data. Needed to remove from the LudeoSession.</param>
        /// <param name="removePlayerCallback">Callback to call once the operation completes, on a later SDK tick.
        /// If called after the SDK has been shut down (or before it is initialized), it never runs.
        /// A call still pending at shutdown may complete during shutdown (typically with <see cref="LudeoResult.Canceled"/>), or not at all.
        /// If the native SDK can't be called, the exception propagates and it doesn't run.</param>
        public void RemovePlayer(LudeoRoomRemovePlayerParameters removePlayerParameters, Action<LudeoRoomRemovePlayerCallbackData> removePlayerCallback)
        {
            ValidatePlayerDataOrThrow(removePlayerParameters.playerId, removePlayerCallback);
            RoomInterface.RemovePlayer(this, m_ludeoRoomWrapper, m_roomHandle, removePlayerParameters.playerId, removePlayerCallback);
        }

        /// <summary>
        /// Returns the player for the provided player id.
        /// </summary>
        /// <param name="playerId">This is the id that you provided to <see cref="AddPlayer"/></param>
        /// <param name="player">The instance of the LudeoPlayer for the given playerId</param>
        /// <returns>Success or GameSessionNotFound.</returns>
        /// <exception cref="ArgumentNullException">Thrown if playerId is null or empty</exception>
        public LudeoResult GetPlayer(string playerId, out LudeoPlayer player)
        {
            ValidatePlayerIdOrThrow(playerId);

            if (m_playersGameplaySessionLookup.TryGetValue(playerId, out player))
                return LudeoResult.Success;

            LudeoLogManager.LogWarning(LOG_PREFIX, " no player found for playerId: " + playerId);

            player = null;

            return LudeoResult.GameSessionNotFound;
        }
        #endregion Public Api

        #region Add Player Internals

        internal void HandleAddPlayerSuccess(Action<LudeoRoomAddPlayerCallbackData> addPlayerCallback, LudeoRoomAddPlayerCallbackData data)
        {
            LudeoLogManager.LogVerbose(LOG_PREFIX, "AddPlayer Successful");
            m_playersGameplaySessionLookup[data.player.PlayerId] = data.player;
            addPlayerCallback(data);
        }

        internal void HandleAddPlayerFail(Action<LudeoRoomAddPlayerCallbackData> addPlayerCallback, LudeoRoomAddPlayerCallbackData data)
        {
            LudeoLogManager.LogError(LOG_PREFIX, "AddPlayer failed with error: " + data.resultCode);
            addPlayerCallback(data);
        }

        private void ValidatePlayerDataOrThrow<T>(string playerId, Action<T> callback)
        {
            ValidatePlayerIdOrThrow(playerId);
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));
        }

        private void ValidatePlayerIdOrThrow(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
                throw new ArgumentNullException("playerId must not be empty!");
        }
        #endregion Add Player


        #region Remove Player Internals
        internal void HandleRemovePlayerSuccess(string playerId, Action<LudeoRoomRemovePlayerCallbackData> removePlayerCallback, LudeoRoomRemovePlayerCallbackData data)
        {
            LudeoLogManager.LogVerbose(LOG_PREFIX, "RemovePlayer Successful");

            if (m_playersGameplaySessionLookup.ContainsKey(playerId))
                m_playersGameplaySessionLookup.Remove(playerId);

            removePlayerCallback(data);
        }

        internal void HandleRemovePlayerFail(Action<LudeoRoomRemovePlayerCallbackData> removePlayerCallback, LudeoRoomRemovePlayerCallbackData data)
        {
            LudeoLogManager.LogError(LOG_PREFIX, "RemovePlayer failed with error: " + data.resultCode);
            removePlayerCallback(data);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    m_playersGameplaySessionLookup.Clear();
                    if (_instance == this)
                        _instance = null;
                }

                RoomInterface.ReleaseAllPlayers(m_ludeoRoomWrapper);

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion Remove Player Internals
    }
}
