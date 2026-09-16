/// @cond INTERNAL
using System;
using System.IO.Pipes;

namespace LudeoSDK.Internal
{
    public interface INativePipeClient
    {
        IntPtr PipeHandle { get; }

        void Init(string serverName, string pipeName, PipeDirection direction, PipeOptions options);

        void NativeConnect(int timeout);

        int NativeRead(byte[] buffer, int offset, int count);

        void NativeWrite(byte[] buffer, int offset, int count);

        bool NativeCloseHandle();
    }

    internal class DummyNativePipeClient : INativePipeClient
    {
        private const string LOG_PREFIX = "DummyNativePipeClient";

        public IntPtr PipeHandle { get; }

        public void Init(string serverName, string pipeName, PipeDirection direction, PipeOptions options)
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} Init");
        }

        public void NativeConnect(int timeout)
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} NativeConnect");
        }

        public int NativeRead(byte[] buffer, int offset, int count)
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} NativeRead");
            return -1;
        }

        public void NativeWrite(byte[] buffer, int offset, int count)
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} NativeWrite");
        }

        public bool NativeCloseHandle()
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} NativeCloseHandle");
            return false;
        }
    }
}
/// @endcond INTERNAL