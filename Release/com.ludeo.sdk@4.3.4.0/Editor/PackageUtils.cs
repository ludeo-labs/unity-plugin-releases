/// @cond INTERNAL
namespace LudeoSDKUnityEditor
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.PackageManager;
    using UnityEditor.PackageManager.Requests;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class PackageUtils
    {
        private static ListRequest m_packageListRequest;
        private static Action<PackageCollection, bool> m_onListFilled;

        public static void UpdatePackageList(Action<PackageCollection, bool> onListFilled)
        {
            m_onListFilled = onListFilled;
            m_packageListRequest = Client.List();
            EditorApplication.update -= Progress;
            EditorApplication.update += Progress;
        }

        private static void Progress()
        {
            if (m_packageListRequest.IsCompleted)
            {
                bool isSuccess = false;

                if (m_packageListRequest.Status == StatusCode.Success)
                {
                    isSuccess = true;
                    //foreach (var package in m_listRequest.Result)
                    //{
                    //    Debug.Log($"Package {package.name} ({package.version} {package.resolvedPath})");
                    //    // Optionally, check for specific files here
                    //}
                }
                else if (m_packageListRequest.Status >= StatusCode.Failure)
                    Debug.LogError("Failed to find existing Unity packages.");


                EditorApplication.update -= Progress;
                m_onListFilled(m_packageListRequest.Result, isSuccess);
            }
        }
    }

}
/// @endcond INTERNAL