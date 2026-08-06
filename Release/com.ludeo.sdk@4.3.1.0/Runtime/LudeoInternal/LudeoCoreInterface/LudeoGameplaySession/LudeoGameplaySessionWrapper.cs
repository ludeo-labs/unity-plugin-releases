// Copyright 2024 Ludeo. All rights reserved.

using System;

namespace LudeoSDK.Internal
{
    internal partial class LudeoGameplaySessionWrapper : ILudeoGameplaySessionWrapper
    {
        public void Begin(LudeoHGameplaySession ludeoGameplaySessionHandle, ref LudeoGameplaySessionBeginParams @params, IntPtr clientData, LudeoGameplaySessionBeginCallback ludeoGameplaySessionBeginGameplayCallback)
        {
            ludeo_GameplaySession_Begin(ludeoGameplaySessionHandle, ref @params, clientData, ludeoGameplaySessionBeginGameplayCallback);
        }

        public void End(LudeoHGameplaySession ludeoGameplaySessionHandle, ref LudeoGameplaySessionEndParams @params, IntPtr clientData, LudeoGameplaySessionEndCallback ludeoGameplaySessionEndGameplayCallback)
        {
            ludeo_GameplaySession_End(ludeoGameplaySessionHandle, ref @params, clientData, ludeoGameplaySessionEndGameplayCallback);
        }

        public void MarkHighlight(LudeoHSession ludeoSessionHandle, ref LudeoSessionMarkHighlightParams ludeoSessionMarkHighlightParams, IntPtr clientData, LudeoSessionMarkHighlightCallback callback)
        {
            ludeo_Session_MarkHighlight(ludeoSessionHandle, ref ludeoSessionMarkHighlightParams, clientData, callback);
        }
    }
}