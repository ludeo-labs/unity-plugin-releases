// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
#if !UNITY_STANDALONE_WIN && !UNITY_ANDROID
    using System.Runtime.InteropServices;

    internal partial class LudeoLogWrapper : ILudeoLogWrapper
    {
        static extern LudeoResult ludeo_SetLoggingCallback(LudeoLogCallback callback);

        static extern LudeoResult ludeo_SetLoggingToFile(ref LudeoSetLoggingToFileParams param);

        static extern LudeoResult ludeo_SetLoggingLevel(LudeoLogCategory category, LudeoLogLevel level);
    }
#endif
}