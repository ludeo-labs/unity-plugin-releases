// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
#if !UNITY_STANDALONE_LINUX && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN
    using System;
    using System.Runtime.InteropServices;

    public partial class NativeHelperMethods : INativeHelperMethods
    {
        public IntPtr PLATFORM_GetAuthToken(LudeoHSession sessionHandle)
        {
            return IntPtr.Zero;
        }

        public IntPtr PLATFORM_GetLastHighlightUrl(LudeoHSession sessionHandle)
        {
            return IntPtr.Zero;
        }

        public IntPtr PLATFORM_GetLastHighlightId(LudeoHSession sessionHandle)
        {
            return IntPtr.Zero;
        }
    }
#endif
}