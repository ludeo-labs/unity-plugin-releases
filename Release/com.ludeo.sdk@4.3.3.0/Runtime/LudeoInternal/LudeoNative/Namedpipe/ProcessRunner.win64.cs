// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
#if UNITY_STANDALONE_WIN
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public partial class ProcessRunner
    {
        [DllImport("kernel32.dll")]
        private static extern bool CreateProcess(string lpApplicationName, StringBuilder lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, [In] ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetExitCodeProcess(IntPtr hProcess, out uint lpExitCode);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        private readonly object _lock = new object();
        private IntPtr processHandle = IntPtr.Zero;

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        private struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        private void PLATFORM_StartProcessWithArgs(string path, string args, string options)
        {
            lock (_lock)
            {
                var commandLine = new StringBuilder("\"" + path + "\" " + args);
                var si = new STARTUPINFO();
                var pi = new PROCESS_INFORMATION();

                uint dwCreationFlags = options.Contains("-hidewindow") ? (uint)0x08000000 : 0;

                bool result = CreateProcess(null, commandLine, IntPtr.Zero, IntPtr.Zero, false, dwCreationFlags, IntPtr.Zero, null, ref si, out pi);
                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    ReportError($"Failed to start process. Error code: {error}");
                    return;
                }

                CloseHandle(pi.hThread);
                processHandle = pi.hProcess;
            }
        }

        private bool PLATFORM_IsProcessRunning()
        {
            lock (_lock)
            {
                if (processHandle == IntPtr.Zero)
                    return false;

                GetExitCodeProcess(processHandle, out uint exitCode);
                return exitCode == 259; // STILL_ACTIVE
            }
        }

        private void PLATFORM_CloseProcess()
        {
            lock (_lock)
            {
                if (processHandle != IntPtr.Zero)
                {
                    TerminateProcess(processHandle, 0);
                    CloseHandle(processHandle);
                    processHandle = IntPtr.Zero;
                }
            }
        }

        private void ReportError(string message)
        {
            ErrorCallback?.Invoke(message);
        }
    }
#endif
}