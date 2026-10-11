// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
#if UNITY_STANDALONE_WIN
    using System;
    using System.Runtime.InteropServices;

    public partial class NativeHelperMethods : INativeHelperMethods
    {
        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ludeo_Session_TEMP_GetAuthToken(LudeoHSession sessionHandle);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ludeo_Session_TEMP_GetLastHighlightUrl(LudeoHSession sessionHandle);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ludeo_Session_TEMP_GetLastHighlightId(LudeoHSession sessionHandle);

        public IntPtr PLATFORM_GetAuthToken(LudeoHSession sessionHandle)
        {
            return ludeo_Session_TEMP_GetAuthToken(sessionHandle);
        }

        public IntPtr PLATFORM_GetLastHighlightUrl(LudeoHSession sessionHandle)
        {
            return ludeo_Session_TEMP_GetLastHighlightUrl(sessionHandle);
        }

        public IntPtr PLATFORM_GetLastHighlightId(LudeoHSession sessionHandle)
        {
            return ludeo_Session_TEMP_GetLastHighlightId(sessionHandle);
        }
    }
#endif
}