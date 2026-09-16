// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
    using UnityEngine;
    using LudeoSDK.Internal;
    using System;

    /// <summary>
    /// Internal Ludeo functionality
    /// </summary>
    public partial class OSSystems : IOSSystems
    {
        private static readonly RuntimePlatform m_platform;
        private static Action<string> m_log;
        private static Action<string> m_logWarning;
        private static Action<string> m_logError;

        public void Log(string message)
        {
            m_log(message);
        }

        public void LogWarning(string message)
        {
            m_logWarning(message);
        }

        public void LogError(string message)
        {
            m_logError(message);
        }

        static OSSystems()
        {
            m_platform = Application.platform;
            m_log = Debug.Log;
            m_logWarning = Debug.LogWarning;
            m_logError = Debug.LogError;
        }

        public void NativeOutputDebugString(string message)
        {
            PLATFORM_NativeOutputDebugString(message);
        }

        public void SetLoggerCallbacks(Action<string> log, Action<string> logWarning, Action<string> logError)
        {
            m_log = log;
            m_logWarning = logWarning;
            m_logError = logError;
        }

        public string GetRuntimeVideoServiceExePath()
        {
            return PLATFORM_GetRuntimeVideoServiceExePath();
        }

        public void BringAppToFront(bool isSupportBringToFront)
        {
            if (isSupportBringToFront)
                PLATFORM_BringAppToFront();
        }

        public IntPtr GetWindowHandle()
        {
            return PLATFORM_GetWindowHandle();
        }

        public void Clear()
        {
            PLATFORM_Clear();
        }        
    }
}