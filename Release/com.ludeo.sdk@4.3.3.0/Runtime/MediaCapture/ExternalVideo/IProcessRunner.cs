/// @cond INTERNAL
using LudeoSDK.Internal;
using System;

namespace LudeoSDK.MediaCapture
{
    public interface IProcessRunner
    {
        void Initialize(string path, string args, string options, Action<string> errorCallback = null);

        void StartProcess();

        bool IsRunning();

        void StopProcess();
    }

    internal class DummyProcessRunner : IProcessRunner
    {
        private const string LOG_PREFIX = "DummyProcessRunner";

        public void Initialize(string path, string args, string options, Action<string> errorCallback = null) 
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} Initialize");
        }

        public void StartProcess() 
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} StartProcess");
        }

        public bool IsRunning() 
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} IsRunning");
            return false; 
        }

        public void StopProcess() 
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} StopProcess");
        }
    }
}

/// @endcond INTERNAL