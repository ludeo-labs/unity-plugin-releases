// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
#if UNITY_STANDALONE_WIN
    using System.Runtime.InteropServices;
    using System;
    using LudeoSDK.MediaCapture;

    public partial class NativeSemaphore : IDisposable, INativeSemaphore
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateSemaphore(IntPtr lpSemaphoreAttributes, int lInitialCount, int lMaximumCount, string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReleaseSemaphore(IntPtr hSemaphore, int lReleaseCount, out int lpPreviousCount);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenSemaphore(uint dwDesiredAccess, bool bInheritHandle, string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        // Constants for semaphore access
        private const uint INFINITE = 0xFFFFFFFF;
        private const uint SEMAPHORE_MODIFY_STATE = 0x0002;
        private const uint SEMAPHORE_ALL_ACCESS = 0x1F0003;
        private IntPtr SemaphoreHandle { get; set; }        

        public void PLATFORM_OpenCreateSemaphore(int initialCount, int maximumCount, string name)
        {
            // Try to open an existing named semaphore
            SemaphoreHandle = OpenSemaphore(SEMAPHORE_ALL_ACCESS, false, name);

            if (SemaphoreHandle == IntPtr.Zero)
            {
                // If the semaphore does not exist, create a new one
                SemaphoreHandle = CreateSemaphore(IntPtr.Zero, initialCount, maximumCount, name);

                if (SemaphoreHandle == IntPtr.Zero)
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }

        public void PLATFORM_CreateSemaphore(string name, int initialCount, int maximumCount)
        {
            SemaphoreHandle = CreateSemaphore(IntPtr.Zero, initialCount, maximumCount, name);
            if (SemaphoreHandle == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public void PLATFORM_OpenSemaphore(string name)
        {
            SemaphoreHandle = OpenSemaphore(SEMAPHORE_MODIFY_STATE, false, name);
            if (SemaphoreHandle == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public bool PLATFORM_WaitOne()
        {
            // Wait indefinitely for the semaphore to be signaled
            return WaitForSingleObject(SemaphoreHandle, INFINITE) == 0;
        }

        public bool PLATFORM_WaitOne(int millisecondsTimeout)
        {
            // Wait with a timeout for the semaphore to be signaled
            return WaitForSingleObject(SemaphoreHandle, (uint)millisecondsTimeout) == 0;
        }

        public void PLATFORM_ReleaseSemaphore()
        {
            if (SemaphoreHandle != IntPtr.Zero)
                if (!ReleaseSemaphore(SemaphoreHandle, 1, out int _))
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }
        }


        public void PLATFORM_CloseSemaphore()
        {
            if (SemaphoreHandle != IntPtr.Zero)
            {
                if (!CloseHandle(SemaphoreHandle))
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }
                SemaphoreHandle = IntPtr.Zero;
            }
        }
    }
#endif
}