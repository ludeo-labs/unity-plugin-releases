// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
    using LudeoSDK.MediaCapture;
    using System;

    public partial class ProcessRunner : IProcessRunner
    {
        private Action<string> ErrorCallback { get; set; }
        private string Path { get; set; }
        private string Args { get; set; }
        private string Options { get; set; }

        public void Initialize(string path, string args, string options, Action<string> errorCallback = null)
        {
            Path = path;
            Args = args;
            Options = options;
            ErrorCallback = errorCallback;
        }

        public void StartProcess()
        {
            PLATFORM_StartProcessWithArgs(Path, Args, Options);
        }

        public bool IsRunning()
        {
            return PLATFORM_IsProcessRunning();
        }

        public void StopProcess()
        {
            PLATFORM_CloseProcess();
        }
    }
}