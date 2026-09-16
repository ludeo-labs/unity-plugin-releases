/// @cond INTERNAL
using LudeoSDK.Internal;
using System.Threading;

namespace LudeoSDK.MediaCapture
{
    public interface INativePipeServer
    {
        void Initialize(string pipeName, CancellationToken token, uint maxNumberOfInstances, uint bufferOutSize, uint bufferInSize);

        void WaitForConnection();

        void Write(byte[] buffer, int count);

        void Disconnect();

        void Dispose();
    }

    internal class DummyNativePipeServer : INativePipeServer
    {
        private const string LOG_PREFIX = "DummyNativePipeServer";

        public void Disconnect()
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} Disconnect");
        }

        public void Dispose()
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} Dispose");
        }

        public void Initialize(string pipeName, CancellationToken token, uint maxNumberOfInstances, uint bufferOutSize, uint bufferInSize)
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} Initialize");
        }

        public void WaitForConnection()
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} WaitForConnection");
        }

        public void Write(byte[] buffer, int count)
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} Write");
        }
    }
}
/// @endcond INTERNAL