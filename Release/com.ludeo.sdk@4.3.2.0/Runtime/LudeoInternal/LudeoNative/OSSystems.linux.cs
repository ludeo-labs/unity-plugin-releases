// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
#if UNITY_STANDALONE_LINUX
    using System;
    using LudeoSDK.Internal;


    public partial class OSSystems : IOSSystems
    {

        public void PLATFORM_NativeOutputDebugString(string message)
        {
        }

        public string PLATFORM_GetRuntimeVideoServiceExePath()
        {
            throw new PlatformNotSupportedException();
        }

        public void PLATFORM_BringAppToFront()
        {
            throw new PlatformNotSupportedException();
        }

        public IntPtr PLATFORM_GetWindowHandle()
        {
            throw new PlatformNotSupportedException();
        }

        public void PLATFORM_Clear()
        {
            throw new PlatformNotSupportedException();
        }
    }
#endif
}