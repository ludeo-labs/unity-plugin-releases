// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
#if UNITY_STANDALONE_WIN
    using LudeoSDK.Internal;
    using System;
    using System.IO.Pipes;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    internal partial class NativePipeClient : INativePipeClient
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        private IntPtr m_pipeHandle = new IntPtr(-1);
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint OPEN_EXISTING = 3;
        private const uint FILE_FLAG_OVERLAPPED = 0x40000000;

        public IntPtr PipeHandle { get { return m_pipeHandle; } }

        public void PLATFORM_Connect(int timeout)
        {
            uint desiredAccess = 0;
            switch (m_direction)
            {
                case PipeDirection.In:      desiredAccess = GENERIC_READ;                   break;
                case PipeDirection.Out:     desiredAccess = GENERIC_WRITE;                  break;
                case PipeDirection.InOut:   desiredAccess = GENERIC_READ | GENERIC_WRITE;   break;

                default:
                    throw new NotSupportedException($"{m_direction} is unknown! NamedPipClientStream aborted.");
            }

            uint flagsAndAttributes = m_options == PipeOptions.Asynchronous ? FILE_FLAG_OVERLAPPED : 0;
            string normalizedPipeName = $@"\\{m_serverName}\pipe\{m_pipeName}";

            Task connectionTask = Task.Run(() =>
            {
                m_pipeHandle = CreateFile(
                    normalizedPipeName,
                    desiredAccess,
                    0,
                    IntPtr.Zero,
                    OPEN_EXISTING,
                    flagsAndAttributes,
                    IntPtr.Zero);

                if (m_pipeHandle.ToInt64() == -1)
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }
            });

            if (!connectionTask.Wait(timeout))
            {
                throw new TimeoutException("Connection to the server timed out.");
            }
        }


        public int PLATFORM_Read(byte[] buffer, int offset, int count)
        {
            IntPtr unmanagedBuffer = Marshal.AllocHGlobal(count);
            try
            {
                uint bytesRead;
                bool result = ReadFile(m_pipeHandle, unmanagedBuffer, (uint)count, out bytesRead, IntPtr.Zero);
                if (!result) 
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                Marshal.Copy(unmanagedBuffer, buffer, offset, (int)bytesRead);
                return (int)bytesRead;
            }
            finally
            {
                Marshal.FreeHGlobal(unmanagedBuffer);
            }
        }

        public void PLATFORM_Write(byte[] buffer, int offset, int count)
        {
            IntPtr unmanagedBuffer = Marshal.AllocHGlobal(count);
            try
            {
                Marshal.Copy(buffer, offset, unmanagedBuffer, count);
                bool result = WriteFile(m_pipeHandle, unmanagedBuffer, (uint)count, out uint bytesWritten, IntPtr.Zero);
                if (!result || bytesWritten != count) throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
            finally
            {
                Marshal.FreeHGlobal(unmanagedBuffer);
            }
        }

        public bool PLATFORM_CloseHandle()
        {
            bool result = CloseHandle(m_pipeHandle);
            m_pipeHandle = IntPtr.Zero;
            if (!result)
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            return result;
        }
    }
#endif
}