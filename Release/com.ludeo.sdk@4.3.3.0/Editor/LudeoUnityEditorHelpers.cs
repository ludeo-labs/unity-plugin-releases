/// @cond INTERNAL
namespace LudeoSDKUnityEditor
{
    using LudeoSDK.Internal;
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class LudeoUnityEditorHelpers
    {
        private const string LUDEO_DEFAULT_SETTINGS_FILE_NAME = "LudeoSettings.json";
        private const string LUDEO_DEFAULT_RUN_COMMANDS_FILE_NAME = "RunCommands.json";
        private const string LUDEO_DEFAULT_SETTINGS = "{\n\t\"LogLevel\": \"Error\"\n}";


        private static readonly RunCommands DefaultLudeoRunCommands = new RunCommands()
        {
            commands = new RunCommand[]
            {
                new RunCommand() { key = "video-enabled", value= "1"}
            }
        };

        private readonly static Dictionary<string, string> WIN64_KEYVALUES_SETTINGS = new Dictionary<string, string>()
        {
            { "CPU", "x86_64"}
        };

        private static bool m_importStarted = false;

        private static readonly string[] DLLS_TO_INCLUDE_WIN64_ONLY_PLATFORM =
        {
            "GfnRuntimeSdk.dll",
        };

        private static readonly string[] DLLS_TO_INCLUDE_ANY_PLATFORM =
        {
        };


        public static string GetLudeoSDKAssemblyName()
        {
            return LudeoSystemInfo.GetSDKProjectName();
        }

        public static void SetupLudeoAssets()
        {
            UpdateMetaSettings();
            SetupLudeoStreamingAssets();
        }


        private static void UpdateMetaSettings()
        {
            if (!m_importStarted)
            {
                m_importStarted = true;

                AssetDatabase.StartAssetEditing();
                SetupLudeoStreamingAssets();
                SetWin64ImportSettings();
                //SetGeneralImportSettings();
                // The editor tooling is compiled from source now (no LudeoSDKUnityEditor.dll to
                // configure), so there is no editor-only plugin import step anymore.
                LudeoConfigMenu.SetupLudeoSDK();
                AssetDatabase.StopAssetEditing();
                AssetDatabase.Refresh();

                m_importStarted = false;
            }
        }

        private static void SetWin64ImportSettings()
        {
            ImportSettingsManager.SetImportSettingsForSpecificBuildTargetOnlyAndEditor(BuildTarget.StandaloneWindows64, WIN64_KEYVALUES_SETTINGS, DLLS_TO_INCLUDE_WIN64_ONLY_PLATFORM);
        }

        private static void SetGeneralImportSettings()
        {
            ImportSettingsManager.SetImportSettingsForAllBuildTargetsAndEditor(DLLS_TO_INCLUDE_ANY_PLATFORM);
        }

        // In order to support Unity Package Manager distribution channel - we may not supply the LudeoSDK with StreamingAssets.
        // Instead we create what we need after LudeoSDK is imported.
        private static void SetupLudeoStreamingAssets()
        {
            string ludeoFolder = Path.Combine(Application.streamingAssetsPath, GetLudeoSDKAssemblyName());

            CreateFileIfMissing(ludeoFolder, LUDEO_DEFAULT_SETTINGS_FILE_NAME,      LUDEO_DEFAULT_SETTINGS);
            CreateFileIfMissing(ludeoFolder, LUDEO_DEFAULT_RUN_COMMANDS_FILE_NAME, JsonUtility.ToJson(DefaultLudeoRunCommands, prettyPrint: true));
        }

        private static void CreateFileIfMissing(string folder, string file, string fileContent)
        {
            string filePath = Path.Combine(folder, file);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (!File.Exists(filePath))
                File.WriteAllText(filePath, fileContent);
        }
    }
}
/// @endcond INTERNAL