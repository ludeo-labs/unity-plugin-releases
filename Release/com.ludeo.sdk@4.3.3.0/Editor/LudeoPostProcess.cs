namespace LudeoSDKUnityEditor
{
    using UnityEditor;

    // The Ludeo SDK used to be imported as a precompiled managed DLL, and this ran its one-time
    // project setup (StreamingAssets provisioning, native plugin import settings, settings asset)
    // when that DLL was imported. The SDK is compiled from source now, so there is no managed-DLL
    // import event to hook. Instead we run the same setup once per editor session on load. All
    // setup steps are idempotent (they only create what is missing), so a consumer can also
    // re-run it manually via the "Ludeo/Setup Ludeo SDK" menu item.
    [InitializeOnLoad]
    public static class LudeoPostProcess
    {
        private const string SETUP_DONE_SESSION_KEY = "LudeoSDK.SetupDone";

        static LudeoPostProcess()
        {
            if (SessionState.GetBool(SETUP_DONE_SESSION_KEY, false))
                return;

            // Defer out of the domain-reload callback — AssetDatabase edits are not safe to run
            // directly from a static constructor.
            EditorApplication.delayCall += RunSetupOnce;
        }

        private static void RunSetupOnce()
        {
            EditorApplication.delayCall -= RunSetupOnce;

            if (SessionState.GetBool(SETUP_DONE_SESSION_KEY, false))
                return;

            SessionState.SetBool(SETUP_DONE_SESSION_KEY, true);
            LudeoUnityEditorHelpers.SetupLudeoAssets();
        }
    }
}
