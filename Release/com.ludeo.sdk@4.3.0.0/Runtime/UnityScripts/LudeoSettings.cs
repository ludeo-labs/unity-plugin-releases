// Copyright 2024 Ludeo. All rights reserved.

/// @cond INTERNAL
namespace LudeoSDK.UnityScripts
{
    using UnityEngine;

    public class LudeoSettings : ScriptableObject
    {
        internal string sdkVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [Tooltip("Set your game name")]
        public string gameName = "YourGameName";

        [Tooltip("Set your game version")]
        public string gameVersion = "YourGameVersion";

        [Tooltip("Set Ludeo's log level")]
        public LudeoLogLevel ludeoLogLevel = LudeoLogLevel.Error;

        [Tooltip("Set Ludeo's log category")]
        public LudeoLogCategory ludeoLogCategory = LudeoLogCategory.All;

        [Tooltip("Set Platform Url")]
        public string platformUrl = "https://services.ludeo.com";

        [Tooltip("Set Game API Key")]
        public string apiKey = "";

        [Tooltip("Which Core dlls to build the game with.")]
        public LudeoSDKCoreDll coreDllReference = LudeoSDKCoreDll.Release;

        [Tooltip("Set Beta Version")]
        public string betaVersion = "";

        [Tooltip("For development: force Ludeo to avoid using a launcher at init")]
        public bool runWithoutLauncher;

        [Tooltip("For development: for no launcher mode - set user id")]
        public string launcherUserId;

        [Tooltip("For development: Force no-launcher run to start with the given Ludeo ('Ludeo To Auto Start'')")]
        public bool autoStartInLudeo;

        [Tooltip("For development: the Ludeo Id to start with if autoStartInLudeo is true")]
        public string ludeoToAutoStart;

        public LudeoSettings()
        {
        }
    }
}
/// @endcond INTERNAL