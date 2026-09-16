// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
    using LudeoSDK.Ludeo.Internal.Launchers.Nvidia;
    using System;
    using System.Runtime.InteropServices;
    using static LudeoSDK.Ludeo.Internal.Launchers.Nvidia.NvidiaDefinitions;

    public class NvidiaExtern : INvidiaExtern
    {
        private const string NVIDIA_DLL_NAME = "GfnRuntimeSdk.dll";

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        [DllImport(NVIDIA_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern GfnRuntimeError gfnInitializeRuntimeSdk(GfnDisplayLanguage language);

        [DllImport(NVIDIA_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern GfnRuntimeError gfnGetPartnerSecureData(out IntPtr partnerData);

        [DllImport(NVIDIA_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern GfnRuntimeError gfnShutdownSdk();

        [DllImport(NVIDIA_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern GfnRuntimeError gfnSendMessage([MarshalAs(UnmanagedType.LPStr)] string message, uint length);
#else
        internal static GfnRuntimeError gfnInitializeRuntimeSdk(GfnDisplayLanguage language)
        {
            return GfnRuntimeError.gfnInitFailure;
        }

        internal static GfnRuntimeError gfnGetPartnerSecureData(out IntPtr partnerData)
        {
            partnerData = IntPtr.Zero;
            return GfnRuntimeError.gfnAPINotInit;
        }

        internal static GfnRuntimeError gfnShutdownSdk()
        {
            return GfnRuntimeError.gfnAPINotInit;
        }

        internal static GfnRuntimeError gfnSendMessage(string message, uint length)
        {
            return GfnRuntimeError.gfnAPINotInit;
        }
#endif

        public GfnRuntimeError GfnInitializeRuntimeSdk(GfnDisplayLanguage language)
        {
            return gfnInitializeRuntimeSdk(language);
        }

        public GfnRuntimeError GfnGetPartnerSecureData(out IntPtr partnerData)
        {
            return gfnGetPartnerSecureData(out partnerData);
        }

        public GfnRuntimeError GfnShutdownSdk()
        {
            return gfnShutdownSdk();
        }

        public GfnRuntimeError GfnSendMessage(string message, uint length)
        {
            return gfnSendMessage(message, length);
        }
    }
}