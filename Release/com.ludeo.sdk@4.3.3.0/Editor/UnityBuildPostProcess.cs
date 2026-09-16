/// @cond INTERNAL
namespace LudeoSDKUnityEditor
{
    using LudeoSDK;
    using LudeoSDK.UnityScripts;
    using System;
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;
    using UnityEngine;

    internal class UnityBuildPostProcess : IPostprocessBuildWithReport
    {
        private const string LUDEO_PACKAGE_NAME = "com.ludeosdk.unity";

        public int callbackOrder { get { return 0; } }

        public void OnPostprocessBuild(BuildReport report)
        {
            const string NEW_VIDEO_PROCESS_FILE = "LudeoVideoService.exe";
            const string X64_FOLDER = "x86_64";
            const string X86_FOLDER = "x86";
            Debug.Log("LudeoSDK: post build process");

            BuildTarget buildTarget = report.summary.platform;

            if (buildTarget == BuildTarget.StandaloneWindows64)
                WindowsPostProcess(report.summary, X64_FOLDER, NEW_VIDEO_PROCESS_FILE);

            if (buildTarget == BuildTarget.StandaloneWindows)
                WindowsPostProcess(report.summary, X86_FOLDER, NEW_VIDEO_PROCESS_FILE);
        }

        private void HandleCoreDllRedundancy(BuildSummary summary, string winArchFolder)
        {
            BuildTarget buildTarget = summary.platform;
            string buildPathRoot = summary.outputPath.Replace(".exe", "_DATA");
            string targetDirectory = BuildPath(buildPathRoot, "Plugins", winArchFolder);

            LudeoSettings ludeoSettings = LudeoUnityHelpers.GetLudeoSettings();
            LudeoSDKCoreDll referencedCoreDll = ludeoSettings.coreDllReference;
            string ludeoCoreDllFilePattern = LudeoSDKSettings.LUDEO_SDK_CORE_DLL_PATTERN;


            LudeoPlatformSettings.TryGetSupportedPlatformDllName(referencedCoreDll, buildTarget, out string fileToKeep);

            // Find all files that match the pattern
            var matchingFiles = Directory.GetFiles(targetDirectory, ludeoCoreDllFilePattern)
                                        .Where(file => !Path.GetFileName(file).Equals(fileToKeep, StringComparison.OrdinalIgnoreCase));

            Debug.Log($"File to keep: {fileToKeep}");
            // Delete the matching files
            foreach (var file in matchingFiles)
            {
                try
                {
                    File.Delete(file);
                    Debug.Log($"Deleted file: {file}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to delete file: {file}. Exception: {ex.Message}");
                }
            }
        }

        private void WindowsPostProcess(BuildSummary summary, string winArchFolder, string newVideoProcess = null)
        {
            Debug.Log($"LudeoSDK: WindowsPostProcess, for: {summary.platform}");

            // TODO: check if there is a builtin Unity way to identify the correct build folder
            string buildPathRoot = summary.outputPath.Replace(".exe", "_DATA");
            string sourceDirectory = BuildPath("Packages", LUDEO_PACKAGE_NAME, "Plugins", winArchFolder);
            string targetDirectory = BuildPath(buildPathRoot, "Plugins", winArchFolder);

            if (!string.IsNullOrEmpty(newVideoProcess))
            {
                const string FFMPEG = "ffmpeg.exe";

                string newVideoRecorderSourceDirectory = BuildPath(sourceDirectory, "VideoRecorder");
                string newVideoRecorderTargetDirectory = BuildPath(targetDirectory, "VideoRecorder");
                CopyFileToWindowsBuild(newVideoRecorderSourceDirectory,
                    newVideoRecorderTargetDirectory, newVideoProcess);
                CopyFileToWindowsBuild(newVideoRecorderSourceDirectory,
                    newVideoRecorderTargetDirectory, FFMPEG);
            }

            HandleCoreDllRedundancy(summary, winArchFolder);
        }

        private void CopyFileToWindowsBuild(string sourceDirectory, string targetDirectory, string fileName)
        {
            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            string sourceFilePath = BuildPath(sourceDirectory, fileName);
            string targetFilePAth = BuildPath(targetDirectory, fileName);

            File.Copy(sourceFilePath, targetFilePAth, true);

            Debug.Log($"LudeoSDK: {fileName} copied to build: {targetFilePAth}");
        }

        private string BuildPath(params string[] strings)
        {
            string fullPath = string.Empty;
            if ((strings == null) || (strings.Length == 0))
                return fullPath;

            int lastIdx = strings.Length - 1;
            for (int i = 0; i < lastIdx; ++i)
                fullPath += strings[i] + Path.DirectorySeparatorChar;
            fullPath += strings[lastIdx];

            return fullPath;
        }
    }
}
/// @endcond INTERNAL