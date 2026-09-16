// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
    using LudeoSDK.MediaCapture;
    using System;

    public partial class NativeSemaphore : IDisposable, INativeSemaphore
    {
        public void Initialize(int initialCount, int maximumCount, string name)
        {
            PLATFORM_OpenCreateSemaphore(initialCount, maximumCount, name);
        }

        public void WaitOne(int? millisecondsTimeout = null)
        {
            if(millisecondsTimeout.HasValue)
            {
                PLATFORM_WaitOne(millisecondsTimeout.Value);
            }
            else
            {
                PLATFORM_WaitOne();
            }
        }

        public void Release()
        {
            PLATFORM_ReleaseSemaphore();
        }

        public void Dispose()
        {
            PLATFORM_CloseSemaphore();
        }
    }
}