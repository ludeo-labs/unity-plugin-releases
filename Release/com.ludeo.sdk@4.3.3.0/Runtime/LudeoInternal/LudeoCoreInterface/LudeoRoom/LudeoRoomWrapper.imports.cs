// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    using System.Runtime.InteropServices;
    using System;

    internal partial class LudeoRoomWrapper : ILudeoRoomWrapper
    {
        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ludeo_Room_Close(LudeoHRoom ludeoRoomHandle, ref LudeoRoomCloseParams ludeoSessionCloseRoomParams, IntPtr clientData, LudeoSessionCloseRoomCallback onRoomClosedCallback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern void ludeo_Room_AddPlayer(LudeoHRoom ludeoRoomHandle, ref LudeoRoomAddPlayerParams ludeoRoomAddPlayerParams, IntPtr clientData, LudeoRoomAddPlayerCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern void ludeo_Room_RemovePlayer(LudeoHRoom ludeoRoomHandle, ref LudeoRoomRemovePlayerParams ludeoRoomRemovePlayerParams, IntPtr clientData, LudeoRoomRemovePlayerCallback callback);
    }
}