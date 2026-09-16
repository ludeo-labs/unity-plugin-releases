// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
#if !UNITY_STANDALONE_WIN && !UNITY_ANDROID
    using System.Runtime.InteropServices;
    using System;

    internal partial class LudeoRoomWrapper : ILudeoRoomWrapper
    {
        private static extern void ludeo_Room_Close(LudeoHRoom ludeoRoomHandle,
            ref LudeoRoomCloseParams ludeoSessionCloseRoomParams, IntPtr clientData,
            LudeoSessionCloseRoomCallback onRoomClosedCallback);


        static extern void ludeo_Room_AddPlayer(LudeoHRoom ludeoRoomHandle,
            ref LudeoRoomAddPlayerParams ludeoRoomAddPlayerParams, IntPtr clientData,
            LudeoRoomAddPlayerCallback callback);


        static extern void ludeo_Room_RemovePlayer(LudeoHRoom ludeoRoomHandle,
            ref LudeoRoomRemovePlayerParams ludeoRoomRemovePlayerParams, IntPtr clientData,
            LudeoRoomRemovePlayerCallback callback);
    }
#endif
}