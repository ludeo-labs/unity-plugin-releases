/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using System;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    internal class DllNameUpdater
    {
        private const string DLL_NAME_CONST_LINE = "public const string DLL_NAME = ";

        public static bool UpdateDllName(string newValue)
        {
            if (Application.isPlaying || !Application.isEditor)
                throw new InvalidOperationException("Not supported and should not call UpdateDllName at runtime.");

            if (!TryFindPathToDefinitionClass(out string filePath))
                return false;

            string[] lines = File.ReadAllLines(filePath);

            // Update the target line
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(DLL_NAME_CONST_LINE))
                {
                    int startIdxToChange = lines[i].IndexOf("\"") + 1;
                    int endIdxToChange = lines[i].LastIndexOf("\"");
                    string existingVal = string.Empty;
                    string newLine;

                    if (startIdxToChange<=0 || endIdxToChange<=0 || (startIdxToChange > endIdxToChange))
                        throw new InvalidOperationException($"Unproccessable line of code{lines[i]}");

                    if (startIdxToChange < endIdxToChange)
                    {
                        existingVal = lines[i].Substring(startIdxToChange, endIdxToChange - startIdxToChange);
                        newLine = lines[i].Replace(existingVal, newValue);
                    }
                    else
                    {
                        newLine = lines[i].Insert(startIdxToChange, newValue);
                    }

                    lines[i] = newLine;
                    break;
                }
            }

            File.WriteAllLines(filePath, lines);

            return true;
        }
        private static bool TryFindPathToDefinitionClass(out string classFilePath)
        {
            classFilePath = null;

            // The SDK is compiled from source now, so the executing assembly no longer sits
            // next to its .cs files. Locate LudeoCoreDefinitions.cs through the AssetDatabase
            // instead, which resolves whether the package lives under Assets/ or Packages/.
            foreach (string guid in AssetDatabase.FindAssets("LudeoCoreDefinitions t:MonoScript"))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (!assetPath.Replace('\\', '/').EndsWith("LudeoCoreInterface/LudeoCoreDefinitions.cs"))
                    continue;

                classFilePath = Path.GetFullPath(assetPath);
                return File.Exists(classFilePath);
            }

            return false;
        }
    }
}
/// @endcond INTERNAL