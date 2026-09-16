// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
#if UNITY_STANDALONE_WIN
    using Microsoft.Win32.SafeHandles;
    using System.IO;
    using System.Runtime.InteropServices;
    using System;
    using LudeoSDK.MediaCapture;

    using System.Threading;
    using UnityEngine;

    public partial class NativePipeServer : IDisposable, INativePipeServer
    {
        private const uint INFINITE = 0xFFFFFFFF; // This constant represents an infinite timeout

        #region PIPE DEFINITIONS
        private const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        private const uint PIPE_ACCESS_OUTBOUND = 0x00000002;
        private const uint PIPE_TYPE_BYTE = 0x00000000;
        private const uint PIPE_READMODE_BYTE = 0x00000000;
        private const uint PIPE_WAIT = 0x00000000;
        private const uint PIPE_NOWAIT = 0x00000001;
        #endregion PIPE DEFINITIONS

        #region PIPE ERRORS
        private const int ERROR_NO_DATA = 232;          // indicates that the pipe is closing and no client is connected.
        private const int ERROR_PIPE_CONNECTED = 535;   // indicates the pipe is already connected.
        private const int ERROR_IO_PENDING = 997;       // indicates the operation is pending.
        private const int ERROR_OPERATION_ABORTED = 995;// The I/O operation has been aborted because of either a thread exit or an application request.
        #endregion PIPE ERRORS


        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeFileHandle CreateNamedPipe(
            string lpName, uint dwOpenMode, uint dwPipeMode, uint nMaxInstances,
            uint nOutBufferSize, uint nInBufferSize, uint nDefaultTimeOut, IntPtr lpSecurityAttributes);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ConnectNamedPipe(SafeFileHandle hNamedPipe, ref NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CancelIoEx(SafeFileHandle hFile, ref NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool DisconnectNamedPipe(SafeFileHandle hNamedPipe);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteFile(
            SafeFileHandle hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten, ref NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ResetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        private SafeFileHandle pipeHandle;
        private NativeOverlapped overlapped;
        private IntPtr eventHandle;

        private void PLATFORM_CreatePipe(string pipeName, uint maxNumberOfInstances, uint bufferOutSize, uint bufferInSize)
        {
            eventHandle = CreateEvent(IntPtr.Zero, true, false, null);
            pipeHandle = CreateNamedPipe(
                @"\\.\pipe\" + pipeName,
                PIPE_ACCESS_OUTBOUND | FILE_FLAG_OVERLAPPED,
                PIPE_TYPE_BYTE | PIPE_READMODE_BYTE | PIPE_WAIT,
                maxNumberOfInstances, // max instances
                bufferOutSize, // out buffer size
                bufferInSize, // in buffer size
                0, // default timeout
                IntPtr.Zero);

            if (pipeHandle.IsInvalid)
            {
                throw new IOException("Unable to create named pipe.", Marshal.GetLastWin32Error());
            }
        }

        private void PLATFORM_WaitForConnection()
        {
            overlapped = new NativeOverlapped();
            ResetEvent(eventHandle);
            overlapped.EventHandle = eventHandle;

            if (!ConnectNamedPipe(pipeHandle, ref overlapped))
            {
                int err = Marshal.GetLastWin32Error();
                if (err == ERROR_IO_PENDING)
                {
                    // Wait for the connection to be established.
                    WaitForSingleObject(eventHandle, INFINITE);
                    ResetEvent(eventHandle);
                }
                else if (err != ERROR_PIPE_CONNECTED)
                {
                    throw new IOException("Unable to connect named pipe.", err);
                }
            }
        }

        private void PLATFORM_Disconnect()
        {
            if (!DisconnectNamedPipe(pipeHandle))
            {
                int err = Marshal.GetLastWin32Error();
                // Only throw an exception for other error codes then ERROR_NO_DATA
                if (err != ERROR_NO_DATA)
                {
                    throw new IOException("Unable to disconnect named pipe.", err);
                }
            }
        }

        private void PLATFORM_Write(byte[] data, uint count)
        {
            if (!WriteFile(pipeHandle, data, count, out uint bytesWritten, ref overlapped))
            {
                int err = Marshal.GetLastWin32Error();
                if (err == ERROR_IO_PENDING)
                {
                    // Wait for the write operation to complete.
                    WaitForSingleObject(eventHandle, INFINITE);
                    ResetEvent(eventHandle);
                }
                else
                {
                    throw new IOException("Unable to write to named pipe.", err);
                }
            }
        }

        private bool PLATFORM_Cancellation()
        {
            return CancelIoEx(pipeHandle, ref overlapped);
        }

        private void PLATFORM_Dispose()
        {
            if (!Disposed)
            {
                if (pipeHandle != null && !pipeHandle.IsInvalid)
                {
                    pipeHandle.Dispose();
                }

                ReleaseEventHandle();
                Disposed = true;
            }
        }

        private void ReleaseEventHandle()
        {
            if (eventHandle != IntPtr.Zero)
            {
                CloseHandle(eventHandle);
                eventHandle = IntPtr.Zero;
            }
        }
    }
#endif
}