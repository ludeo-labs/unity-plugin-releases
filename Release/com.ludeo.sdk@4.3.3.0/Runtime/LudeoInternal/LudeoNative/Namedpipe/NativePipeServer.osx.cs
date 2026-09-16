// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
#if UNITY_STANDALONE_OSX
    public partial class NativePipeServer
    {
        private void PLATFORM_CreatePipe(string pipeName, uint maxNumberOfInstances, uint bufferOutSize, uint bufferInSize)
        {
        }

        private void PLATFORM_WaitForConnection()
        {
        }

        private void PLATFORM_Disconnect()
        {
        }

        private void PLATFORM_Write(byte[] data, uint count)
        {
        }

        private bool PLATFORM_Cancellation()
        {
            return true;
        }

        private void PLATFORM_Dispose()
        {
        }
    }
#endif
}