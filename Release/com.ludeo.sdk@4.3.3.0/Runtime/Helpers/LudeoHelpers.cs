// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using UnityEngine;

    /// <summary>
    /// Contains support methods for the LudeoSDK integration.  
    /// </summary>
    public class LudeoHelpers
    {
        /// <summary>
        /// Get the Ludeo platform URL towards which the SDK directs.
        /// </summary>
        /// <returns>The Ludeo platform URL.</returns>
        public static string GetServiceUrl()
        {
            if (LudeoManager.Instance != null)
                return LudeoManager.Instance.GetLudeoPlatformUrl();
            else
            {
                Debug.LogError("You must call Init before requesting Ludeo service URL");
                return string.Empty;
            }
        }

        /// <summary>
        /// Set the LogLevel you wish Ludeo to output.
        /// </summary>
        /// <param name="logLevel">Required LogLevel of LudeoSDK.</param>
        /// <returns>True if LogLevel was set successfully.</returns>
        public static LudeoResult SetLogLevel(LudeoLogLevel logLevel)
        {
            return LudeoManager.Instance != null ? LudeoManager.SetLoggingLevel(logLevel) : LudeoResult.LudeoNotYetInit;
        }
    }
}