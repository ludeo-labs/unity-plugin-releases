
using LudeoSDK.Internal;
/// @cond INTERNAL
namespace LudeoSDK.MediaCapture
{
    public interface INativeSemaphore
    {
        void Initialize(int initialCount, int maximumCount, string name);

        void WaitOne(int? millisecondsTimeout = null);

        void Release();

        void Dispose();
    }

    internal class DummyNativeSemaphore : INativeSemaphore
    {
        private const string LOG_PREFIX = "DummyNativeSemaphore";

        public void Dispose()
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} Dispose");
        }

        public void Initialize(int initialCount, int maximumCount, string name)
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} Initialize");
        }

        public void Release()
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} Release");
        }

        public void WaitOne(int? millisecondsTimeout)
        {       
            LudeoLogManager.LogInfo($"{LOG_PREFIX} WaitOne");
        }
    }
}

/// @endcond INTERNAL