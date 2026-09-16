/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using System;

    public interface ILudeoGameplaySessionWrapper
    {
        void Begin(LudeoHGameplaySession ludeoGameplaySessionHandle, ref LudeoGameplaySessionBeginParams @params, IntPtr clientData, LudeoGameplaySessionBeginCallback ludeoGameplaySessionBeginGameplayCallback);
        
        void End(LudeoHGameplaySession ludeoGameplaySessionHandle, ref LudeoGameplaySessionEndParams @params, IntPtr clientData, LudeoGameplaySessionEndCallback ludeoGameplaySessionBeginGameplayCallback);

        // TODO: MarkHighlight is supposed to use the Gameplay! This is a needed update in Core (then in Managed level)
        void MarkHighlight(LudeoHSession ludeoSessionHandle, ref LudeoSessionMarkHighlightParams ludeoSessionMarkHighlightParams, IntPtr clientData, LudeoSessionMarkHighlightCallback callback);
    }
}
/// @endcond INTERNAL
