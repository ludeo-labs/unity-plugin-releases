// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
    using LudeoSDK.MediaCapture;
    using System;
    using System.Threading;

    public partial class NativePipeServer : IDisposable, INativePipeServer
    {
        private CancellationToken CancellationToken { get; set; }
        private CancellationTokenRegistration CancellationTokenRegistration { get; set; }
        private bool Disposed { get; set; }

        public void Initialize(string pipeName, CancellationToken token, uint maxNumberOfInstances, uint bufferOutSize, uint bufferInSize)
        {
            PLATFORM_CreatePipe(pipeName, maxNumberOfInstances, bufferOutSize, bufferInSize);
            CancellationToken = token;
            CancellationTokenRegistration = CancellationToken.Register(() =>
            {
                if (!Disposed)
                {
                    bool cancelationResult = PLATFORM_Cancellation();
                }
            });
        }

        public void WaitForConnection()
        {
            PLATFORM_WaitForConnection();
        }

        public void Write(byte[] buffer, int count)
        {
            PLATFORM_Write(buffer, (uint)count);
        }

        public void Disconnect()
        {
            PLATFORM_Disconnect();
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                CancellationTokenRegistration.Dispose();
                bool cancelationResult = PLATFORM_Cancellation();
                PLATFORM_Dispose();
            }
        }
    }
}