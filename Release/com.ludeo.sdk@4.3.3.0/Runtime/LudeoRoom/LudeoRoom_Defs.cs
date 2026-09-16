// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    /// <summary>
    /// The parameters received when the AddPlayer callback fires.
    /// </summary>
    public struct LudeoRoomAddPlayerCallbackData
    {
        /// <summary>Tells if the operation succeeded or failed.</summary>
        public LudeoResult resultCode;

        /// <summary>The player context for controlling the Ludeo Track Gameplay flow.</summary>
        public LudeoPlayer player;
    }

    /// <summary>
    /// Holds your game's player data relevant for adding to a LudeoRoom.
    /// </summary>
    public struct LudeoRoomAddPlayerParameters
    {
        internal string playerId;

        public LudeoRoomAddPlayerParameters(string playerId)
        {
            this.playerId = playerId;
        }
    }

    /// <summary>
    /// Holds your game's player data relevant for removing from a LudeoRoom.
    /// </summary>
    public struct LudeoRoomRemovePlayerParameters
    {
        internal string playerId;

        public LudeoRoomRemovePlayerParameters(string playerId)
        {
            this.playerId = playerId;
        }
    }

    /// <summary>
    /// The parameters received when the RemovePlayer callback fires.
    /// </summary>
    public struct LudeoRoomRemovePlayerCallbackData
    {
        /// <summary>Tells if the operation succeeded or failed.</summary>
        public LudeoResult resultCode;
    }
}
