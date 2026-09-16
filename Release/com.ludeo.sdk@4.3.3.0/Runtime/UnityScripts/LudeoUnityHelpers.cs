// Copyright 2024 Ludeo. All rights reserved.

/// @cond INTERNAL
namespace LudeoSDK
{
    using LudeoSDK.Internal;
    using LudeoSDK.UnityScripts;
    using System;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class LudeoUnityHelpers
    {
        private const string INTERFACES_ASSET_NAME = "LudeoCoreInterface";
        private const string TEST_INTERFACES_ASSET_NAME = "LudeoCoreTestInterface";

        private static LudeoSettings m_ludeoSettings;
        private static bool m_isTestMode;

        public static void EnableTestMode()
        {
            m_isTestMode = true;
        }

        public static void DisableTestMode()
        {
            m_isTestMode = false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            m_isTestMode = false;
        }

        public static LudeoSettings GetLudeoSettings()
        {
            if (m_ludeoSettings == null)
                m_ludeoSettings = Resources.Load<LudeoSettings>("LudeoSettings");
            return m_ludeoSettings;
        }

        internal static bool IsRunWithoutLauncher()
        {
            GetLudeoSettings();

            return m_ludeoSettings.runWithoutLauncher;
        }

        internal static ILudeoCoreInterface TryGetLudeoCoreInterface()
        {
            string interfacesAssetName = !m_isTestMode ? INTERFACES_ASSET_NAME : TEST_INTERFACES_ASSET_NAME;

            Object unityAsset = Resources.Load(interfacesAssetName);
            if (unityAsset == null)
                throw new NullReferenceException($"Your LudeoSDK package is missing the Resource {interfacesAssetName}. Please download and import the lates LudeoSDK.");

            ILudeoCoreInterface implementedCoreHook = unityAsset as ILudeoCoreInterface;
            if (implementedCoreHook == null)
                throw new NullReferenceException("Your LudeoSDK package is missing the 'ILudeoCoreInterfaceFactory'. Please download and import the lates LudeoSDK.");

            return implementedCoreHook;
        }

        internal static bool InjectInstanceFromUnity<T>(GameObject rootGO, out T iObject) where T : class
        {
            Component[] components = rootGO.GetComponents<Component>();
            Type interfaceType = typeof(T);
            string interfaceName = interfaceType.Name;
            iObject = null;

            if (!interfaceType.IsInterface || !typeof(T).IsInterface)
            {
                LudeoLogManager.LogError($"Injection from Unity supports interfaces types only!");
                return false;
            }

            for (int i = 0; i < components.Length; ++i)
            {
                Type myInterface = components[i].GetType().GetInterface(interfaceName);
                if (myInterface != null)
                {
                    iObject = components[i] as T;
                    LudeoLogManager.LogVerbose($"Interface implementation found: {iObject}");
                    break;
                }
            }

            if (iObject != null)
                return true;

            LudeoLogManager.LogError($"Inject issue! LudeoSDK failed to find a component that implements {interfaceName}!");
            return false;
        }  
    }
}
/// @endcond INTERNAL