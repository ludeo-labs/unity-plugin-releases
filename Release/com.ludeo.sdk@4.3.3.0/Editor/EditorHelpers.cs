/// @cond INTERNAL
namespace LudeoSDKUnityEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    internal static class EditorHelpers
    {
        public const int SEARCH_NOT_FOUND = -1;
        public const int SEARCH_ABORT = -2;

        public static T DrawEnumPopup<T>(SerializedProperty enumProp, float guiWidth) where T : Enum
        {
            int enumIdx = enumProp.enumValueIndex;

            Array enumArray = Enum.GetValues(typeof(T));
            T enumValue = (T)enumArray.GetValue(enumIdx);

            enumValue = (T)EditorGUILayout.EnumPopup(enumValue, GUILayout.Width(guiWidth));

            enumProp.enumValueIndex = Array.IndexOf(enumArray, enumValue);

            return enumValue;
        }

        public static int FindIndexOfKey(string key, int[] array, int itemCount)
        {
            int keyCode = GetHash(key);

            if (itemCount > array.Length)
            {
                //Debug.LogError("Avoiding overflow. Item count larger then array size.");
                return SEARCH_ABORT;
            }

            for (int i = 0; i < itemCount; i++)
            {
                if (keyCode == array[i])
                    return i;
            }

            return EditorHelpers.SEARCH_NOT_FOUND;
        }

        public static int GetHash(string key)
        {
            if (!string.IsNullOrEmpty(key))
                return key.GetHashCode();
            else
                return 0;
        }       

        public static void ReadStringsFromSerializedProperty(SerializedProperty serializedPropertyStringArray, ref string[] strings, params string[] relativeElements)
        {
            int itemsCount = serializedPropertyStringArray != null ? serializedPropertyStringArray.arraySize : 0;

            if ((strings == null) || (strings.Length != itemsCount))
                strings = new string[itemsCount];

            for (int i = 0; i < itemsCount; ++i)
            {
                SerializedProperty element = serializedPropertyStringArray.GetArrayElementAtIndex(i);
                if (relativeElements != null)
                {
                    for (int j = 0; j < relativeElements.Length; ++j)
                        element = element.FindPropertyRelative(relativeElements[j]);
                }
                strings[i] = element.stringValue;
            }
        }

        public static void ReadEnumsFromSerializedProperty<T>(SerializedProperty serializedPropertyEnumArray, ref T[] enumArray) where T : Enum
        {
            Array enumValues = Enum.GetValues(typeof(T));

            Debug.Log("ReadEnumsFromSerializedProperty, size = " + enumValues.Length);
            Debug.Log("ReadEnumsFromSerializedProperty, serializedPropertyEnumArray.arraySize = " + serializedPropertyEnumArray.arraySize);
            int itemsCount = serializedPropertyEnumArray != null ? serializedPropertyEnumArray.arraySize : 0;

            if ((enumArray == null) || (enumArray.Length != itemsCount))
                enumArray = new T[itemsCount];

            Debug.Log("ReadEnumsFromSerializedProperty, itemsCount = " + itemsCount);
            Debug.Log("ReadEnumsFromSerializedProperty, enumArray.Length = " + enumArray.Length);
            for (int i = 0; i < itemsCount; ++i)
            {
                SerializedProperty element = serializedPropertyEnumArray.GetArrayElementAtIndex(i);
                int idx = element.enumValueIndex;

                enumArray[i] = (T)enumValues.GetValue(idx);
                Debug.Log("Got value type = " + enumArray[i]);
            }
        }

        public static void BuildOldToNewIndicesLookup(string[] previousContexts, string[] newContexts, ref Dictionary<int, int> previousToNewContextsIds)
        {
            int previousContextsCount = previousContexts != null ? previousContexts.Length : 0;
            int newContextsCount = newContexts != null ? newContexts.Length : 0;

            previousToNewContextsIds.Clear();

            for (int i = 0; i < previousContextsCount; ++i)
            {
                string existing = previousContexts[i];
                bool wasFoundInNew = false;

                for (int s = 0; s < newContextsCount; ++s)
                {
                    if (string.Compare(existing, newContexts[s]) == 0)
                    {
                        previousToNewContextsIds.Add(i, s);
                        wasFoundInNew = true;
                        s = newContextsCount;
                    }
                }

                if (!wasFoundInNew)
                    previousToNewContextsIds.Add(i, SEARCH_NOT_FOUND);
            }

            for (int i = previousContextsCount; i < newContextsCount; ++i)
                previousToNewContextsIds.Add(i, SEARCH_NOT_FOUND);
        }
    }
}
/// @endcond INTERNAL