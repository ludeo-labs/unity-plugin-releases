// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
#if UNITY_STANDALONE_LINUX
    using LudeoSDK.Internal;
    using System;

    internal partial class NativePipeClient : INativePipeClient
    {
        public IntPtr PipeHandle { get { return IntPtr.Zero; } }

        public void PLATFORM_Connect(int timeout)
        {
        }


        public int PLATFORM_Read(byte[] buffer, int offset, int count)
        {
            return -1;
        }

        public void PLATFORM_Write(byte[] buffer, int offset, int count)
        {
        }

        public bool PLATFORM_CloseHandle()
        {
            return false;
        }
    }
#endif
}