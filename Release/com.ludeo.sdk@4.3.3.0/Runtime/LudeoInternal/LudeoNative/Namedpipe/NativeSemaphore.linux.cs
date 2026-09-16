// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
#if UNITY_STANDALONE_LINUX
    public partial class NativeSemaphore
    {
        public void PLATFORM_OpenCreateSemaphore(int initialCount, int maximumCount, string name)
        {
        }

        public void PLATFORM_CreateSemaphore(string name, int initialCount, int maximumCount)
        {
        }

        public void PLATFORM_OpenSemaphore(string name)
        {
        }

        public bool PLATFORM_WaitOne()
        {
            return false;
        }

        public bool PLATFORM_WaitOne(int millisecondsTimeout)
        {
            return false;
        }

        public void PLATFORM_ReleaseSemaphore()
        {
        }

        public void PLATFORM_CloseSemaphore()
        {
        }
    }
#endif
}