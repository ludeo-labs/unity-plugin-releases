/// @cond INTERNAL
namespace LudeoSDKUnityEditor
{
    using LudeoSDK;
    using LudeoSDK.UnityScripts;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(LudeoSettings))]
    public class LudeoSettingsEditor : Editor
    {        
        private const string SDK_VERSION_LABEL = "SDK Version";
        
        public struct HighlightColorSet
        {
            public Color labelTextColor;
            public Color highlightColor;
        }

        public SerializedProperty gameName;
        public SerializedProperty gameVersion;
        public SerializedProperty ludeoLogLevel;
        public SerializedProperty ludeoLogCategory;
        public SerializedProperty platformUrl;
        public SerializedProperty apiKey;

        #region DEVELOPMENT-PARAMS
        public SerializedProperty coreDllReference;
        public SerializedProperty runWithoutLauncher;
        public SerializedProperty launcherUserId;
        public SerializedProperty betaVersion;
        public SerializedProperty autoStartInLudeo;
        public SerializedProperty ludeoToAutoStart;
        #endregion DEVELOPMENT-PARAMS
 
        private GUIContent m_runWithoutLauncherLabel = new GUIContent("Run Without Steam Client");
        private GUIContent m_steamUserIdLabel = new GUIContent("Steam User Id");
        private GUIContent m_betaVersionLabel = new GUIContent("Beta Version Name");
        private GUIContent m_isAutoStartLudeoLabel = new GUIContent("Auto Start Ludeo (Optional)");
        private GUIContent m_ludeoToStartLabel = new GUIContent("Ludeo To Auto Start (Optional)");

        private const string FIRST_TIME_UPDATE_KEY = "LUDEO_SDK_CORE_DLL_REFERENCE";

        private static Color WarningColor;
        private static Color GoodColor;
        private static HighlightColorSet WarningHighlightColors;
        private static HighlightColorSet DefaultHighlightColors;

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            ColorUtility.TryParseHtmlString("#f3bb1b", out WarningColor);
            ColorUtility.TryParseHtmlString("#3dc03c", out GoodColor);

            WarningHighlightColors = new HighlightColorSet { highlightColor = WarningColor, labelTextColor = Color.black };
            DefaultHighlightColors = new HighlightColorSet { highlightColor  = GoodColor, labelTextColor = Color.black };

            EditorApplication.update += FirstTimeCoreDllUpdate;
        }

        private static void FirstTimeCoreDllUpdate()
        {
            EditorApplication.update -= FirstTimeCoreDllUpdate;

            if (EditorPrefs.GetBool(FIRST_TIME_UPDATE_KEY, false))
                return;

            EditorPrefs.SetBool(FIRST_TIME_UPDATE_KEY, true);

            LudeoSettings ludeoSettings = LudeoUnityHelpers.GetLudeoSettings();

            LudeoPlatformSettings.UpdateLudeoCoreDllReference(ludeoSettings.coreDllReference);
        }

        public void OnEnable()
        {
            gameName = serializedObject.FindProperty("gameName");
            gameVersion = serializedObject.FindProperty("gameVersion");
            ludeoLogLevel = serializedObject.FindProperty("ludeoLogLevel");
            ludeoLogCategory = serializedObject.FindProperty("ludeoLogCategory");
            platformUrl = serializedObject.FindProperty("platformUrl");
            apiKey = serializedObject.FindProperty("apiKey");

            // Development params
            coreDllReference = serializedObject.FindProperty("coreDllReference");
            runWithoutLauncher = serializedObject.FindProperty("runWithoutLauncher");
            launcherUserId = serializedObject.FindProperty("launcherUserId");
            betaVersion = serializedObject.FindProperty("betaVersion");
            autoStartInLudeo = serializedObject.FindProperty("autoStartInLudeo");
            ludeoToAutoStart = serializedObject.FindProperty("ludeoToAutoStart");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawSettings();

            EditorGUILayout.Space();

            DrawReadonlyFields();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawReadonlyFields()
        {
            var settings = (LudeoSettings)target;
            EditorGUILayout.LabelField(SDK_VERSION_LABEL, settings.sdkVersion);
        }

        private void DrawSettings()
        {
            EditorGUILayout.PropertyField(gameName);
            EditorGUILayout.PropertyField(gameVersion);
            EditorGUILayout.PropertyField(ludeoLogLevel);
            EditorGUILayout.PropertyField(ludeoLogCategory);

            EditorGUILayout.HelpBox("Set the Ludeo platform Url to communicate with", MessageType.Info);
            EditorGUILayout.PropertyField(platformUrl);

            EditorGUILayout.HelpBox("Set your game's Ludeo Api Key to communicate with", MessageType.Info);
            EditorGUILayout.PropertyField(apiKey);

           // EditorGUILayout.Space();

            EditorGUILayout.HelpBox("Development only params", MessageType.Warning);

            LudeoLogLevel currentLogLevel = (LudeoLogLevel)ludeoLogLevel.enumValueIndex;
            DrawAndHandleCoreDllRefChange(currentLogLevel);

            EditorGUILayout.PropertyField(runWithoutLauncher, m_runWithoutLauncherLabel);
            bool newLauncherChoise = runWithoutLauncher.boolValue;
            UnityEngine.GUI.enabled = newLauncherChoise;
            EditorGUILayout.PropertyField(launcherUserId, m_steamUserIdLabel);
            EditorGUILayout.PropertyField(betaVersion, m_betaVersionLabel);
            EditorGUILayout.PropertyField(autoStartInLudeo, m_isAutoStartLudeoLabel);
            EditorGUILayout.PropertyField(ludeoToAutoStart, m_ludeoToStartLabel);
            UnityEngine.GUI.enabled = true;
        }

        private void DrawAndHandleCoreDllRefChange(LudeoLogLevel currentLogLevel)
        {
            LudeoSDKCoreDll previousCoreDllRef = (LudeoSDKCoreDll)coreDllReference.enumValueIndex;

            HighlightColorSet highlightColorSet = previousCoreDllRef == LudeoSDKCoreDll.Release ? DefaultHighlightColors : WarningHighlightColors;
            DrawHighlightedSerializedProperty(coreDllReference, highlightColorSet, "Core Dll Reference");

            LudeoSDKCoreDll newCoreDllRef = (LudeoSDKCoreDll)coreDllReference.enumValueIndex;

            if (currentLogLevel >= LudeoLogLevel.VeryVerbose)
            {
                // If switched from development to release: set back LudeoLogLevel to the supported one
                if ((previousCoreDllRef == LudeoSDKCoreDll.Development) && (newCoreDllRef == LudeoSDKCoreDll.Release))
                {
                    ludeoLogLevel.enumValueIndex = (int)LudeoLogLevel.Verbose;
                }
                // Otherwise, VeryVerbose is still to be used, and requires Development dll.
                else
                {
                    newCoreDllRef = LudeoSDKCoreDll.Development;
                    coreDllReference.enumValueIndex = (int)LudeoSDKCoreDll.Development;
                }
            }

            if (newCoreDllRef != previousCoreDllRef)
                LudeoPlatformSettings.UpdateLudeoCoreDllReference(newCoreDllRef);
        }

        private static void DrawHighlightedSerializedProperty(SerializedProperty property, HighlightColorSet highlightColorSet, string labelText)
        {
            // Draw the highlight
            Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            EditorGUI.DrawRect(rect, highlightColorSet.highlightColor);
            
            // Draw text color
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.normal.textColor = highlightColorSet.labelTextColor;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, rect.height), labelText, labelStyle);

            // Draw the property field
            EditorGUI.PropertyField(new Rect(rect.x + EditorGUIUtility.labelWidth, rect.y, rect.width - EditorGUIUtility.labelWidth, rect.height), property, GUIContent.none);
        }
    }
}
/// @endcond INTERNAL