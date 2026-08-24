// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    using System;
    using System.Runtime.InteropServices;

    public partial class NativeHelperMethods : INativeHelperMethods
    {
        private static bool m_isUseDummyCode;

        static NativeHelperMethods()
        {
            m_isUseDummyCode = false;
        }

        public static void SwitchDummyCode(bool isOn)
        {
            m_isUseDummyCode = isOn;
        }

        public string GetAuthToken(LudeoHSession sessionHandle)
        {
            IntPtr token = !m_isUseDummyCode ? PLATFORM_GetAuthToken(sessionHandle) : GetDummyToken();

            return GetStringValueToReturn(token);
        }

        public string GetLastHighlightUrl(LudeoHSession sessionHandle)
        {
            IntPtr highlightUrl = !m_isUseDummyCode ? PLATFORM_GetLastHighlightUrl(sessionHandle) : GetDummyToken();

            return GetStringValueToReturn(highlightUrl);
        }

        public string GetLastHighlightId(LudeoHSession sessionHandle)
        {
            IntPtr highlightId = !m_isUseDummyCode ? PLATFORM_GetLastHighlightId(sessionHandle) : GetDummyToken();

            return GetStringValueToReturn(highlightId);
        }

        private string GetStringValueToReturn(IntPtr intPtr)
        {
            if (intPtr != IntPtr.Zero)
                return Marshal.PtrToStringAnsi(intPtr);
            else
                return null;
        }

        private IntPtr GetDummyToken()
        {
            return Marshal.StringToHGlobalAnsi("DummyToken");
        }
    }
}