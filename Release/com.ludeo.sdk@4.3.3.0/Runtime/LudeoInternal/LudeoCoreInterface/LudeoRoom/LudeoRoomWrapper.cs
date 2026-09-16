// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    using System;

    internal partial class LudeoRoomWrapper : ILudeoRoomWrapper
    {
        public void CloseRoom(LudeoHRoom roomHandle, ref LudeoRoomCloseParams ludeoSessionCloseRoomParams, IntPtr clientData, LudeoSessionCloseRoomCallback ludeoSessionCloseRoomCallback)
        {
            ludeo_Room_Close(roomHandle, ref ludeoSessionCloseRoomParams, clientData, ludeoSessionCloseRoomCallback);
        }

        public void AddPlayer(LudeoHRoom roomHandle, ref LudeoRoomAddPlayerParams ludeoRoomAddPlayerParams, IntPtr clientData, LudeoRoomAddPlayerCallback callback)
        {
            ludeo_Room_AddPlayer(roomHandle, ref ludeoRoomAddPlayerParams, clientData, callback);
        }

        public void RemovePlayer(LudeoHRoom roomHandle, ref LudeoRoomRemovePlayerParams ludeoRoomRemovePlayerParams, IntPtr clientData, LudeoRoomRemovePlayerCallback callback)
        {
            ludeo_Room_RemovePlayer(roomHandle, ref ludeoRoomRemovePlayerParams, clientData, callback);
        }
    }
}
