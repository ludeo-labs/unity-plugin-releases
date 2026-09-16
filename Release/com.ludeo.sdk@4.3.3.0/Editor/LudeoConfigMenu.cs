/// @cond INTERNAL
namespace LudeoSDKUnityEditor
{
    using LudeoSDK.UnityScripts;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public static class LudeoConfigMenu
    {
        private const string ERR_FILE_NOT_EXIST_MSG = "Failed to locate LudeoSettings because it does not exist yet";
        private const string CONFIG_FILE_EXISTS_MSG = "LudeoSettings already exists in your project";
        private const string PARENT_FOLDER_PATH = "Assets";
        private static readonly string CONFIG_FOLDER_PATH = Path.Combine("LudeoSDK","Resources");
        private const string CONFIG_FULL_FILE_NAME = "LudeoSettings.asset";
        private const string DOCUMENTATION_URL = "https://studio.ludeo.com/";
        internal static readonly string FULL_CONFIG_PATH = Path.Combine(PARENT_FOLDER_PATH, CONFIG_FOLDER_PATH, CONFIG_FULL_FILE_NAME);

        [MenuItem("Ludeo/Setup Ludeo SDK")]
        public static void SetupLudeoSDK()
        {
            // Use the in-hand asset instead of Resources.Load: a freshly created asset isn't
            // import-ready inside a StartAssetEditing batch, so a re-load would return null.
            LudeoSettings ludeoSettings = CreateLudeoSettingsFileIfNotExists();
            LudeoPlatformSettings.UpdateLudeoCoreDllReference(ludeoSettings.coreDllReference);
        }

        private static LudeoSettings CreateLudeoSettingsFileIfNotExists()
        {
            if (!IsLudeoSettingsExists())
            {
                TryCreateConfigFolder();
                return CreateLudeoSettings();
            }

            Debug.LogWarning($"[Ludeo SDK] {CONFIG_FULL_FILE_NAME} already exists. Keeping existing one.");
            LocateConfigFile();
            return AssetDatabase.LoadAssetAtPath<LudeoSettings>(FULL_CONFIG_PATH);
        }

        [MenuItem("Ludeo/Open Documentation")]
        private static void OpenDocumentationURL()
        {
            Application.OpenURL(DOCUMENTATION_URL);
        }

        [MenuItem("Ludeo/Locate Config In Assets")]
        private static void LocateConfigFile()
        {
            LudeoSettings asset = AssetDatabase.LoadAssetAtPath<LudeoSettings>(FULL_CONFIG_PATH);
            if (asset != null)
            {
                // Highlight the asset in the Project window
                EditorUtility.FocusProjectWindow();
                EditorGUIUtility.PingObject(asset);
                Selection.activeObject = asset;
            }
            else
            {
                Debug.LogError(ERR_FILE_NOT_EXIST_MSG);
            }
        }

        private static void TryCreateConfigFolder()
        {
            if (!AssetDatabase.IsValidFolder(CONFIG_FOLDER_PATH))
            {
                // Create the parent directories if they don't exist yet
                string[] folders = CONFIG_FOLDER_PATH.Split(Path.DirectorySeparatorChar);
                string parentFolderPath = PARENT_FOLDER_PATH;
                int size = folders.Length;
                for (int i = 0; i < size; ++i)
                {
                    string folderPathInParent = Path.Combine(parentFolderPath, folders[i]);
                    if (!AssetDatabase.IsValidFolder(folderPathInParent))
                    {
                        AssetDatabase.CreateFolder(parentFolderPath, folders[i]);
                    }
                    parentFolderPath = folderPathInParent;
                }
            }
            else
                Debug.Log(CONFIG_FILE_EXISTS_MSG);
        }

        private static bool IsLudeoSettingsExists()
        { 
            LudeoSettings ldkSettings = Resources.Load<LudeoSettings>("LudeoSettings");
            return ldkSettings != null;
        }

        private static LudeoSettings CreateLudeoSettings()
        {
            LudeoSettings asset = ScriptableObject.CreateInstance<LudeoSettings>();

            asset.gameName = Application.productName;
            asset.gameVersion = Application.version;

            AssetDatabase.CreateAsset(asset, FULL_CONFIG_PATH);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;

            return asset;
        }
    }
}
/// @endcond INTERNAL
