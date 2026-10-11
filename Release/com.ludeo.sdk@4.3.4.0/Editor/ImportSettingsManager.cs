/// @cond INTERNAL
namespace LudeoSDKUnityEditor
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    internal class ImportSettingsManager
    {
        private const string DEPRECATED_ATTRIBUTE = "System.ObsoleteAttribute";

        private static readonly string PackagesPluginFolder = "Packages/com.ludeosdk.unity/Plugins";
        private static readonly string PackagesPluginWin64Folders = "Packages/com.ludeosdk.unity/Plugins/x86_64";        
        private static readonly string PackagesPluginEditorFolder = "Packages/com.ludeosdk.unity/Editor";

        private static readonly BuildTarget[] BuildsTargets = (BuildTarget[])Enum.GetValues(typeof(BuildTarget));

        public static void SetImportSettingsForAllBuildTargetsAndEditor(params string[] dlls)
        {
            if (dlls == null || (dlls.Length < 1))
            {
                Debug.LogError("[LudeoSDK Editor] No dlls set @SetImportSettingsForAllBuildTargetsAndEditor.");
                return;
            }

            int length = dlls.Length;

            for (int i = 0; i < length; ++i)
            {                
                if (TryGetPluginImporter(PackagesPluginFolder, dlls[i], out PluginImporter importer))
                {
                    importer.SetCompatibleWithEditor(true);
                    
                    for (int j = 0; j < BuildsTargets.Length; ++j)
                    {
                        SetCompatibleWithPlatform(importer, BuildsTargets[j], true);
                    }

                    importer.SaveAndReimport();
                }
            }
        }

        public static void SetImportSettingsForSpecificBuildTargetOnlyAndEditor(BuildTarget buildTargetToSupport, Dictionary<string,string> kvpSettingForPlatform, params string[] dlls)
        {
            if (dlls == null || (dlls.Length < 1))
            {
                Debug.LogError("[LudeoSDK Editor] No dlls set @SetImportSettingsForSpecificBuildTargetOnlyAndEditor.");
                return;
            }

            int length = dlls.Length;


            for (int i = 0; i < length; ++i)
            {
                if (TryGetPluginImporter(PackagesPluginFolder, dlls[i], out PluginImporter importer))
                {
                    importer.SetCompatibleWithEditor(true);
                    
                    for (int j = 0; j < BuildsTargets.Length; ++j)
                    {
                        BuildTarget currentBuildTarget = BuildsTargets[j];
                        if (currentBuildTarget == buildTargetToSupport)
                        {
                            SetCompatibleWithPlatform(importer, currentBuildTarget, true);

                            //Dictionary<string,string>.Enumerator enumerator = kvpSettingForPlatform.GetEnumerator();
                            //while (enumerator.MoveNext())
                            //{
                            //    importer.SetPlatformData(BuildTarget.StandaloneWindows64, enumerator.Current.Key, enumerator.Current.Value);// "CPU", "x86_64");
                            //}
                        }
                        else
                            SetCompatibleWithPlatform(importer, currentBuildTarget, false);
                    }

                    Debug.Log($"asset path = {importer.assetPath}");
                    importer.SaveAndReimport();
                }
            }
        }

        public static void SetImportSettingsForEditorOnly(params string[] dlls)
        {
            if (dlls == null || (dlls.Length < 1))
            {
                Debug.LogError("[LudeoSDK Editor] No dlls set @SetImportSettingsForEditorOnly.");
                return;
            }

            int length = dlls.Length;

            for (int i = 0; i < length; ++i)
            {
                if (TryGetPluginImporter(PackagesPluginEditorFolder, dlls[i], out PluginImporter importer))
                {
                    importer.SetCompatibleWithEditor(true);
                   
                    for (int j = 0; j < BuildsTargets.Length; ++j)
                    {
                        BuildTarget currentBuildTarget = BuildsTargets[j];
                        SetCompatibleWithPlatform(importer, currentBuildTarget, true);
                    }

                    importer.SaveAndReimport();
                }
                else
                    Debug.LogError($"[LudeoSDK Editor] @SetImportSettingsForEditorOnly {dlls[i]} not found!");
            }
        }

        //private static void ImportAssetsPostUpdate(string[] assets)
        //{
        //    for (int i = 0; i < assets.Length; ++i)
        //        AssetDatabase.ImportAsset(assets[i], ImportAssetOptions.ForceSynchronousImport);
        //}

        public static void SetCompatibleWithPlatform(PluginImporter pluginImporter, BuildTarget buildTarget, bool enable)
        {
            if (!IsBuildTargetDeprecated(buildTarget))
                pluginImporter.SetCompatibleWithPlatform(buildTarget, enable);
        }

        private static bool IsBuildTargetDeprecated(BuildTarget buildTarget)
        {
            Type buildTargetType = typeof(UnityEditor.BuildTarget);
            MemberInfo memberInfo = buildTargetType.GetMember(buildTarget.ToString())[0];

            object[] attributes = memberInfo.GetCustomAttributes(true);
            foreach (object attribute in attributes)
            {
                if (attribute.GetType().FullName == DEPRECATED_ATTRIBUTE)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TryGetPluginImporter(string folderToSearch, string dllName, out PluginImporter importer)
        {
            importer = null;

            if (string.IsNullOrEmpty(dllName))
            {
                Debug.LogWarning($"[LudeoSDK Editor] @TryGetPluginImporter Dll is null or empty.");
                return false;
            }

            string dllPath = Path.Combine(folderToSearch, dllName);
            importer = PluginImporter.GetAtPath(dllPath) as PluginImporter;

            return importer != null;
        }
    }
}
/// @endcond INTERNAL
