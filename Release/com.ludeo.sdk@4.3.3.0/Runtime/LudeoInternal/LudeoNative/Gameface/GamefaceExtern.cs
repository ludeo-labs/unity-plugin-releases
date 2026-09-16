// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
    using LudeoSDK.Ludeo.Internal.Launchers.Gameface;
    using System;

    public class GamefaceExtern : IGamefaceExtern
    {
        private const string ENV_VARIABLE_NAME = "LUDEO_CLOUD_TOKEN";

        public string GetCloudToken()
        {
            return Environment.GetEnvironmentVariable(ENV_VARIABLE_NAME);
        }
    }
}