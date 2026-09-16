/// @cond INTERNAL
using UnityEditor;
using UnityEditor.Build;

namespace LudeoSDKUnityEditor
{
    internal class LudeoActiveBuildTargetProcessing : IActiveBuildTargetChanged
    {
        public int callbackOrder => 0;

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            LudeoPlatformSettings.UpdateLudeoCoreDllReference(newTarget);
        }
    }
}
/// @endcond INTERNAL