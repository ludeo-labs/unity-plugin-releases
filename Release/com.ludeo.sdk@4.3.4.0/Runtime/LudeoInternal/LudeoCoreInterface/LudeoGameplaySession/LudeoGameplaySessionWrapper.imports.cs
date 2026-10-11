// Copyright 2024 Ludeo. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace LudeoSDK.Internal
{
    internal partial class LudeoGameplaySessionWrapper : ILudeoGameplaySessionWrapper
    {
        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern void ludeo_GameplaySession_Begin(LudeoHGameplaySession ludeoGameplaySessionHandle, ref LudeoGameplaySessionBeginParams @params, IntPtr clientData, LudeoGameplaySessionBeginCallback callback);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern void ludeo_GameplaySession_End(LudeoHGameplaySession ludeoGameplaySessionHandle, ref LudeoGameplaySessionEndParams @params, IntPtr clientData, LudeoGameplaySessionEndCallback callback);       
        
        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern void ludeo_Session_MarkHighlight(LudeoHSession ludeoSessionHandle, ref LudeoSessionMarkHighlightParams ludeoSessionMarkHighlightParams, IntPtr clientData, LudeoSessionMarkHighlightCallback callback);
    }
}