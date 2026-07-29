// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    using System.Runtime.InteropServices;

    internal partial class LudeoInitWrapper : ILudeoInitWrapper
    {
        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern LudeoResult ludeo_Initialize(ref LudeoInitializeParams parameters);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ludeo_Tick();

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern LudeoResult ludeo_Shutdown();

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ludeo_RemoveNotification(ulong notificationId);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern LudeoResult ludeo_Command(string name, string value);
    }

}