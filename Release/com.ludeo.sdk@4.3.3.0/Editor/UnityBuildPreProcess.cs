
/// @cond INTERNAL
namespace LudeoSDKUnityEditor
{
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;

    internal class UnityBuildPreProcess : IPreprocessBuildWithReport
    {
        private static bool isOperationComplete = false;

        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            BuildSummary summary = report.summary;
        }
    }
}
/// @endcond INTERNAL