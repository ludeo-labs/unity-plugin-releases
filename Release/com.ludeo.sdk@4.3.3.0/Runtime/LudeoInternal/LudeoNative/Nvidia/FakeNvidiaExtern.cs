// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
    using LudeoSDK.Ludeo.Internal.Launchers.Nvidia;
    using System;
    using System.Runtime.InteropServices;
    using static LudeoSDK.Ludeo.Internal.Launchers.Nvidia.NvidiaDefinitions;
    using Debug = UnityEngine.Debug;

    public class FakeNvidiaExtern : INvidiaExtern
    {
        public GfnRuntimeError GfnInitializeRuntimeSdk(GfnDisplayLanguage language)
        {
            Debug.LogWarning($"{nameof(FakeNvidiaExtern)} GfnInitializeRuntimeSdk. Doing nothing!");
            return GfnRuntimeError.gfnSuccess;
        }

        public GfnRuntimeError GfnGetPartnerSecureData(out IntPtr partnerData)
        {
            Debug.LogWarning($"{nameof(FakeNvidiaExtern)} GfnGetPartnerSecureData. Faking app and auth tokens!");

            string commandLine = $"appToken_34132423 authToken_54645645"; // Can we get some usable dummy here>
            partnerData = Marshal.StringToHGlobalAnsi(commandLine);
            return GfnRuntimeError.gfnSuccess;
        }

        public GfnRuntimeError GfnShutdownSdk()
        {
            Debug.LogWarning($"{nameof(FakeNvidiaExtern)} GfnShutdownSdk. Doing nothing!");

            return GfnRuntimeError.gfnSuccess;
        }

        public GfnRuntimeError GfnSendMessage(string message, uint length)
        {
            Debug.LogWarning($"{nameof(FakeNvidiaExtern)} GfnSendMessage. Doing nothing!");

            return GfnRuntimeError.gfnSuccess;
        }
    }
}