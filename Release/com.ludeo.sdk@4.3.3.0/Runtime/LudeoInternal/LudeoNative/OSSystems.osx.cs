// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
#if UNITY_STANDALONE_OSX
    using System.Diagnostics;
    using System.IO;
    using UnityEngine;
    using System;
    using LudeoSDK.Internal;
    using System.Runtime.InteropServices;

    public partial class OSSystems : IOSSystems
    {
        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
        private static extern IntPtr NSApplication_sharedApplication();

        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
        private static extern IntPtr NSApplication_mainWindow(IntPtr app);


        private static string m_usedAppleScript;

        public void PLATFORM_NativeOutputDebugString(string message)
        {

        }

        public string PLATFORM_GetRuntimeVideoServiceExePath()
        {
            throw new PlatformNotSupportedException();
        }

        public void PLATFORM_BringAppToFront()
        {
            string appId = Application.identifier;
            m_log($"app id: {appId}");

            // Open an Apple Script (see Apple Script Editor) that refocuses an app
            string appleScriptCommand = $"tell application id \"{appId}\"\nactivate\nend tell\n";

            m_usedAppleScript = CreateAppleScript(appleScriptCommand);
            string appleScriptPath = m_usedAppleScript;
            m_log($"appleScriptPath = {appleScriptPath}");
            Process process = new Process();
            process.StartInfo.FileName = "/usr/bin/osascript"; // Path to the osascript command-line tool
            process.StartInfo.Arguments = $"-ss {appleScriptPath}";
            process.StartInfo.UseShellExecute = false;
            process.Start();
        }

        public IntPtr PLATFORM_GetWindowHandle()
        {
            IntPtr nsApp = NSApplication_sharedApplication();
            IntPtr mainWindow = NSApplication_mainWindow(nsApp);
            return mainWindow;
        }

        public void PLATFORM_Clear()
        {
            if (!string.IsNullOrEmpty(m_usedAppleScript) && File.Exists(m_usedAppleScript))
            {
                File.Delete(m_usedAppleScript);
                m_logWarning($"Deleting {m_usedAppleScript}");
            }
        }

        private static string CreateAppleScript(string content)
        {
            string filePath = Path.Combine(Application.dataPath, "OpenApp.scpt");

            File.WriteAllText(filePath, content);

            return filePath;
        }
    }
#endif
}