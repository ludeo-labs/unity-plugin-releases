// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    internal partial class LudeoLogWrapper : ILudeoLogWrapper
    {
        public LudeoResult SetLogLevel(LudeoLogCategory category, LudeoLogLevel level)
        {
            return ludeo_SetLoggingLevel(category, level);
        }
        
        public LudeoResult SetLogCallback(LudeoLogCallback callback)
        {
            return ludeo_SetLoggingCallback(callback);
        }

        public LudeoResult SetLoggingFile(ref LudeoSetLoggingToFileParams logToFileParams)
        {
            return ludeo_SetLoggingToFile(ref logToFileParams);
        }
    }
}