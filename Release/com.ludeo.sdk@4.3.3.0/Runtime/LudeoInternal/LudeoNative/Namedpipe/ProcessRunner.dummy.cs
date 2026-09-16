// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
#if !UNITY_STANDALONE_LINUX && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN
    public partial class ProcessRunner
    {
        private void PLATFORM_StartProcessWithArgs(string path, string args, string options)
        {
        }

        private bool PLATFORM_IsProcessRunning()
        {
            return false;
        }

        private void PLATFORM_CloseProcess()
        {
        }

        private void ReportError(string message)
        {
        }
    }
#endif
}