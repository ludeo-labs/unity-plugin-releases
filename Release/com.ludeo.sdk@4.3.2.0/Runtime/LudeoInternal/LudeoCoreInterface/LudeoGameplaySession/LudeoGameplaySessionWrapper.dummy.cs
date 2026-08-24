// Copyright 2024 Ludeo. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace LudeoSDK.Internal
{
#if !UNITY_STANDALONE_WIN && !UNITY_ANDROID

    internal partial class LudeoGameplaySessionWrapper : ILudeoGameplaySessionWrapper
    {
        static extern void ludeo_GameplaySession_Begin(LudeoHGameplaySession ludeoGameplaySessionHandle, ref LudeoGameplaySessionBeginParams @params, IntPtr clientData, LudeoGameplaySessionBeginCallback callback);

        static extern void ludeo_GameplaySession_End(LudeoHGameplaySession ludeoGameplaySessionHandle, ref LudeoGameplaySessionEndParams @params, IntPtr clientData, LudeoGameplaySessionEndCallback callback);       
        
        static extern void ludeo_Session_MarkHighlight(LudeoHSession ludeoSessionHandle, ref LudeoSessionMarkHighlightParams ludeoSessionMarkHighlightParams, IntPtr clientData, LudeoSessionMarkHighlightCallback callback);
    }
#endif
}