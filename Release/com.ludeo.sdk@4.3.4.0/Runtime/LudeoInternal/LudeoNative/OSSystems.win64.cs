// Copyright 2024 Ludeo. All rights reserved.

using System.ComponentModel;
using AOT;

namespace LudeoSDK.NativeCode
{
#if UNITY_STANDALONE_WIN
    using System.IO;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;
    using LudeoSDK.Internal;
    using System.Text;
    using System.Diagnostics;

    public partial class OSSystems : IOSSystems
    {
        internal const string KERNEL_32_DLL = "kernel32.dll";
        internal const string USER_32_DLL = "user32.dll";

        internal const string UNITY_WINDOW_BUILD_CLASS_NAME = "UnityWndClass";
        private const string UNITY_WINDOW_EDITOR_CLASS_NAME = "UnityContainerWndClass";
        private const string UNITY_WINDOW_EDITOR_GAME_VIEW_CLASS_NAME = "UnityEditor.GameView";

        [DllImport(KERNEL_32_DLL, CharSet = CharSet.Auto)]
        public static extern void OutputDebugString(string message);

        [DllImport(USER_32_DLL)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport(USER_32_DLL)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport(USER_32_DLL)]
        private static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport(USER_32_DLL)]
        private static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport(USER_32_DLL, EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string className, string windowName);

        [DllImport(USER_32_DLL, EntryPoint = "FindWindowEx", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        private const int SW_RESTORE = 9;

        public void PLATFORM_NativeOutputDebugString(string message)
        {
            OutputDebugString(message);
        }

        public string PLATFORM_GetRuntimeVideoServiceExePath()
        {
#if UNITY_64
            return Path.Combine("Plugins", "x86_64", "VideoRecorder", "LudeoVideoService.exe");
#else
            return Path.Combine("Plugins", "x86", "VideoRecorder", "LudeoVideoService.exe"); // THIS IS 32 bit
#endif
        }

        public void PLATFORM_BringAppToFront()
        {
            IntPtr appWindowHandle = PLATFORM_GetWindowHandle();

            if (appWindowHandle != IntPtr.Zero)
            {
                ShowWindow(appWindowHandle, SW_RESTORE);
                SetForegroundWindow(appWindowHandle);
                BringWindowToTop(appWindowHandle);
                SetActiveWindow(appWindowHandle);
            }
            else
                m_logError("Could not find the existing application window handle :(");
        }

        public IntPtr PLATFORM_GetWindowHandle()
        {
            IntPtr appWindowHandle = FetchWindowHandle();

            if (appWindowHandle == IntPtr.Zero)
                m_logError($"Could not find appWindowHandle. For window: {Application.productName}!");
            else
                m_log($"Found: appWindowHandle = {appWindowHandle}");

            return appWindowHandle;
        }
       
        public void PLATFORM_Clear()
        {
        }

        private static IntPtr FetchWindowHandle()
        {
            if (!Application.isEditor)
                return FindUnityBuildWindowHandle();
            else
                return FindUnityEditorGameViewChildHandle();
        }

        private static IntPtr FindUnityBuildWindowHandle()
        {
            return WindowClassFinder.GetCurrentUnityWindowHandle();
        }

        private static IntPtr FindUnityEditorGameViewChildHandle()
        {
            IntPtr containerHandle = IntPtr.Zero;

            // Loop through all UNITY_WINDOW_EDITOR_CLASS_NAME windows
            while ((containerHandle = FindWindowEx(IntPtr.Zero, containerHandle, UNITY_WINDOW_EDITOR_CLASS_NAME, null)) != IntPtr.Zero)
            {
                // Attempt to find the UNITY_WINDOW_EDITOR_GAME_VIEW_CLASS_NAME window as a child of the current UNITY_WINDOW_EDITOR_CLASS_NAME window
                IntPtr gameViewHandle = FindWindowEx(containerHandle, IntPtr.Zero, null, UNITY_WINDOW_EDITOR_GAME_VIEW_CLASS_NAME);

                if (gameViewHandle != IntPtr.Zero)
                {
                    m_log($"Found UnityEditor.GameView Handle: {gameViewHandle}");
                    return gameViewHandle;
                }
            }

            return IntPtr.Zero;
        }
    }

    // Helper class to retrieve window handle
    internal static class WindowClassFinder
    {

        [DllImport(OSSystems.USER_32_DLL, SetLastError = true)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport(OSSystems.USER_32_DLL, SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport(OSSystems.KERNEL_32_DLL, SetLastError = true)]
        private static extern uint GetCurrentProcessId();
        [DllImport(OSSystems.USER_32_DLL, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        
        // The callback needs to be stored as a static readonly field
        private static readonly EnumWindowsProc EnumWindowsCallback = EnumWindowsCallbackFunction;
        
        [MonoPInvokeCallback(typeof(EnumWindowsProc))]
        private static bool EnumWindowsCallbackFunction(IntPtr hWnd, IntPtr lParam)
        {
            uint currentProcessId = GetCurrentProcessId();
            GetWindowThreadProcessId(hWnd, out uint processId);
            
            if (processId == currentProcessId)
            {
                StringBuilder className = new StringBuilder(256);
                int classNameLength = GetClassName(hWnd, className, className.Capacity);

                if (classNameLength == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    Win32Exception ex = new Win32Exception(errorCode);
                    LudeoLogManager.LogError($"GetWindowThreadProcessId failed. Error: {ex.Message}");
                    return false;
                }
                
                // this is the name of the class of the game window in Unity 
                if (className.ToString() == OSSystems.UNITY_WINDOW_BUILD_CLASS_NAME)
                {
                    LudeoLogManager.LogInfo($"Found UnityWndClass Handle: {hWnd}");
                    GCHandle gch = GCHandle.FromIntPtr(lParam);
                    gch.Target = hWnd;
                    return false; // Found the main game window, stop enumeration
                }
            }

            return true;
        }

        public static IntPtr GetCurrentUnityWindowHandle()
        {
            IntPtr windowHandle = IntPtr.Zero;

            // Use GCHandle to pin the windowHandle
            GCHandle handle = GCHandle.Alloc(windowHandle, GCHandleType.Pinned);
            try
            {
                EnumWindows(EnumWindowsCallback, GCHandle.ToIntPtr(handle));
                windowHandle = (IntPtr)handle.Target; 
            }
            finally
            {
                handle.Free(); 
            }

            return windowHandle;
        }
    }

#endif
}