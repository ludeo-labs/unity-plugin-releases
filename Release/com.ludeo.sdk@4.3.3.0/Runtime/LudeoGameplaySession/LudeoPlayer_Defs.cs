// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    /// <summary>
    /// The parameters received when onBeginGameplayCallback is received
    /// </summary>
    public struct LudeoPlayerBeginGameplayCallbackData
    {
        /// <summary>Tells if the operation succeeded or failed.</summary>
        public LudeoResult resultCode;
    }

    /// <summary>
    /// The parameters received when onEndGameplayCallback is received
    /// </summary>
    public struct LudeoPlayerEndGameplayCallbackData
    {
        /// <summary>Tells if the operation succeeded or failed.</summary>
        public LudeoResult resultCode;
    }

    /// <summary>
    /// The parameters received when onAbortGameplayCallback is received
    /// </summary>
    public struct LudeoPlayerAbortGameplayCallbackData
    {
        /// <summary>Tells if the operation succeeded or failed.</summary>
        public LudeoResult resultCode;
    }

    public struct LudeoGameplaySessionMarkHighlightCallbackData
    {
        public LudeoResult resultCode;
    }
}
