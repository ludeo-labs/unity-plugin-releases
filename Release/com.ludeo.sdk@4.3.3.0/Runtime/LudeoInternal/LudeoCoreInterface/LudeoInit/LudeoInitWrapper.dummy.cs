// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
#if !UNITY_STANDALONE_WIN && !UNITY_ANDROID
    using System.Runtime.InteropServices;

    internal partial class LudeoInitWrapper : ILudeoInitWrapper
    {
        private static extern LudeoResult ludeo_Initialize(ref LudeoInitializeParams parameters);

        private static extern void ludeo_Tick();
        
        private static extern LudeoResult ludeo_Shutdown();

        private static extern void ludeo_RemoveNotification(ulong notificationId);

        private static extern LudeoResult ludeo_Command(string name, string value);
    }
#endif
}