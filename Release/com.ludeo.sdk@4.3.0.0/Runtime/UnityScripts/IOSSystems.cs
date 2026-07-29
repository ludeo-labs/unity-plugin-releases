/// @cond INTERNAL
using System;

namespace LudeoSDK.Internal
{
    public interface IOSSystems
    {
        void NativeOutputDebugString(string message);

        void Log(string message);

        void LogWarning(string message);

        void LogError(string message);

        void SetLoggerCallbacks(Action<string> log, Action<string> logWarning, Action<string> logError);

        string GetRuntimeVideoServiceExePath();

        void BringAppToFront(bool isSupportBringToFront);

        void Clear();

        IntPtr GetWindowHandle();
    }

    internal class DummyOSSystems : IOSSystems
    {
        private const string LOG_PREFIX = "DummyOSSystems";
        public void BringAppToFront(bool isSupportBringToFront)
        {
            LudeoLogManager.LogWarning($"{LOG_PREFIX} BringAppToFront (called too early?)");
        }

        public void Clear()
        {
            LudeoLogManager.LogWarning($"{LOG_PREFIX} Clear");
        }

        public string GetRuntimeVideoServiceExePath()
        {
            LudeoLogManager.LogWarning($"{LOG_PREFIX} GetRuntimeVideoServiceExePath (called too early?)");
            return null;
        }

        public void Log(string message)
        {
        }

        public void LogError(string message)
        {
        }

        public void LogWarning(string message)
        {
        }

        public void NativeOutputDebugString(string message)
        {
        }

        public void SetLoggerCallbacks(Action<string> log, Action<string> logWarning, Action<string> logError)
        {
            LudeoLogManager.LogWarning($"{LOG_PREFIX} SetLoggerCallbacks (called too early?)");
        }

        public IntPtr GetWindowHandle()
        {
            return IntPtr.Zero;
        }
    }
}
/// @endcond INTERNAL