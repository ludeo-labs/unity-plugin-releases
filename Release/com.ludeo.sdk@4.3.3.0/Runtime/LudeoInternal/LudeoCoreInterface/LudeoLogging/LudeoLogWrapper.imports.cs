// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    using System.Runtime.InteropServices;

    internal partial class LudeoLogWrapper : ILudeoLogWrapper
    {
        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern LudeoResult ludeo_SetLoggingCallback(LudeoLogCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern LudeoResult ludeo_SetLoggingToFile(ref LudeoSetLoggingToFileParams param);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern LudeoResult ludeo_SetLoggingLevel(LudeoLogCategory category, LudeoLogLevel level);
    }
}