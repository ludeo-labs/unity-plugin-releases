// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    public class LudeoCoreDefinitions
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        public const string DLL_NAME = "LudeoSDK-Win64-Release.dll";
#elif UNITY_ANDROID
        public const string DLL_NAME = "libLudeoSDK-Android-arm64-v8a-Release.so";
#endif
    }
}
