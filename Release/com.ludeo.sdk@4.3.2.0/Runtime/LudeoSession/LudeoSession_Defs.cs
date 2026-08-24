// Copyright 2024 Ludeo. All rights reserved.

using System;

namespace LudeoSDK
{
    /// <summary>
    /// Parameters for opening a Ludeo room via <see cref="LudeoSession.OpenRoom"/>.
    /// </summary>
    public struct LudeoSessionOpenRoomParameters
    {
        internal string RoomId { get; set; }
        internal Guid? LudeoId { get; set; }

        /// <summary>
        /// Opt into a thread-safe DataWriter for this room (Creator flow). When <c>true</c>, the
        /// data-writer object/component enter/leave, all WriteData calls, DestroyObject and SendAction
        /// may be called concurrently from multiple game threads. Default: <c>false</c>.
        /// </summary>
        public bool ThreadSafe { get; set; }

        /// <summary>
        /// Maximum concurrent in-flight object writes before a game thread blocks. Only meaningful when
        /// <see cref="ThreadSafe"/> is <c>true</c>. <c>0</c> uses the SDK default (1024).
        /// </summary>
        public uint MaxConcurrentWrites { get; set; }

        /// <summary>Ludeo flow: set the required LudeoId and your game's roomId.</summary>
        public LudeoSessionOpenRoomParameters(string roomId, Guid ludeoId)
        {
            RoomId = roomId;
            LudeoId = ludeoId;
            ThreadSafe = false;
            MaxConcurrentWrites = 0;
        }

        /// <summary>Creator flow: set your game's roomId.</summary>
        public LudeoSessionOpenRoomParameters(string roomId)
        {
            RoomId = roomId;
            LudeoId = null;
            ThreadSafe = false;
            MaxConcurrentWrites = 0;
        }

        /// <summary>Ludeo flow: set the required LudeoId.</summary>
        public LudeoSessionOpenRoomParameters(Guid ludeoId)
        {
            RoomId = null;
            LudeoId = ludeoId;
            ThreadSafe = false;
            MaxConcurrentWrites = 0;
        }
    }

    /// <summary>
    /// The parameters received when the OpenRoom callback fires.
    /// </summary>
    public struct LudeoOpenRoomCallbackData
    {
        /// <summary>Tells if the operation succeeded or failed.</summary>
        public LudeoResult resultCode;

        /// <summary>The Ludeo room session opened.</summary>
        public LudeoRoom ludeoRoom;

        internal IntPtr ludeoRoomHandler;
    }

    /// <summary>
    /// The parameters received when the CloseRoom callback fires.
    /// </summary>
    public struct LudeoCloseRoomCallbackData
    {
        /// <summary>Tells if the operation succeeded or failed.</summary>
        public LudeoResult resultCode;
    }

    /// <summary>
    /// The parameters received when LudeoGetLudeoCallback is received.
    /// </summary>
    public struct LudeoGetLudeoCallbackData
    {
        /// <summary>Tells if the operation succeeded or failed.</summary>
        public LudeoResult resultCode;

        /// <summary>The LudeoDataReader object used to read the restoration data for this Ludeo</summary>
        public LudeoDataReader ludeoDataReader;
    }
}
