/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using AOT;
    using LudeoSDK.DataStructures;
    using LudeoSDK.UnityScripts;
    using UnityEngine;

    internal class LudeoLogManager
    {
        public const string LOG_PREFIX_INFO     = "LudeoSDK: ";
        public const string LOG_PREFIX_WARNING  = "<color=\"#FFFF00\">LudeoSDK: </color>";
        public const string LOG_PREFIX_ERROR    = "<color=\"#FF0000\">LudeoSDK: </color>";

        private static Action<string> m_logInfoCallback = Debug.Log;
        private static Action<string> m_logWarningCallback = Debug.LogWarning;
        private static Action<string> m_logErrorCallback = Debug.LogError;

        private static ILudeoLogWrapper m_logWrapper;

        private StringBuilder m_concatenateStringBuilder = new StringBuilder();
        
        private StringBuilder m_logBuilder = new StringBuilder(200);
        private LudeoLogLevel m_logLevel = LudeoLogLevel.Off;

        private StringBuilderPool m_stringBuilderPool;

        public static LudeoLogManager Instance { get; private set; }
        
        public static void Init()
        {
            if (Instance != null)
                return;

            Instance = new LudeoLogManager();            
        }

        public static void Deinit()
        {
            Instance = null;
        }

        public static LudeoResult InitNativeHook()
        {
            return LudeoInit.SetLoggingCallback(m_logWrapper, InvokeLogCallbackWithLudeoMessage);
        }

        private LudeoLogManager()
        {
            ILudeoCoreInterface coreHook = LudeoUnityHelpers.TryGetLudeoCoreInterface();
            InjectImplementation(coreHook.GetLudeoLogWrapper());
        }

        ~LudeoLogManager()
        {
            if (Instance != this)
                return;
            Instance = null;
            m_logInfoCallback = null;
            m_logWarningCallback = null;
            m_logErrorCallback = null;
        }

        public LudeoResult SetLogLevel(LudeoLogLevel logLevel, LudeoLogCategory category)
        {
            m_logLevel = logLevel;
            LogInfo($"SetLogLevel to category: '{category}' with level '{logLevel}'");
            return LudeoInit.SetLogLevel(m_logWrapper, category, logLevel);
        }
        
        public LudeoResult SetLogToFile(bool logToFile, string logDirectory = null)
        {
            LogInfo("Set log output to: " + logDirectory);
            return LudeoInit.SetLoggingFile(m_logWrapper, logToFile, logDirectory);
        }
        

        public LudeoResult SetLogCallback(Action<string> infoCallback, Action<string> warningCallback, Action<string> errorCallback)
        {
            m_logInfoCallback = infoCallback;
            m_logWarningCallback = warningCallback;
            m_logErrorCallback = errorCallback;

            IOSSystems osSys = LudeoUnityManager.LudeoInterfaceFactory.GetIOSSystems();
            osSys.SetLoggerCallbacks(infoCallback, warningCallback, errorCallback);

            return LudeoResult.Success;
        }


        #region logging functions

        public static void LogFatal(params string[] msgs)
        {
            WriteStatic(LudeoLogLevel.Fatal, msgs);
        }
        public static void LogError(params string [] msgs)
        {
            WriteStatic(LudeoLogLevel.Error, msgs);
        }

        public static void LogWarning(params string [] msgs)
        {
            WriteStatic(LudeoLogLevel.Warning, msgs);
        }

        public static void LogInfo(params string [] msgs)
        {
            WriteStatic(LudeoLogLevel.Log, msgs);
        }
        public static void LogVerbose(params string[] msgs)
        {
            WriteStatic(LudeoLogLevel.Verbose, msgs);
        }
        public static void LogVeryVerbose(params string[] msgs)
        {
            WriteStatic(LudeoLogLevel.VeryVerbose, msgs);
        }

        // Routes a static log call to the manager instance, or to the default Debug callbacks when the
        // manager hasn't been created yet. Init() only runs inside LudeoManager.Initialize(), so any SDK
        // path that logs before Initialize (e.g. LudeoManager.SessionManager.CreateSession being called
        // too early, which logs "called before Initialize") would otherwise NRE on a null Instance and
        // mask the real error/return code.
        private static void WriteStatic(LudeoLogLevel level, string[] msgs)
        {
            LudeoLogManager instance = Instance;
            if (instance != null)
            {
                instance.WriteLog(level, msgs);
                return;
            }

            string message = LOG_PREFIX_INFO + string.Join(" ", msgs);
            if (level <= LudeoLogLevel.Error)
                m_logErrorCallback?.Invoke(message);
            else if (level == LudeoLogLevel.Warning)
                m_logWarningCallback?.Invoke(message);
            else
                m_logInfoCallback?.Invoke(message);
        }

        #endregion


        #region PRIVATE METHODS
        internal static void InjectImplementation(ILudeoLogWrapper ludeoLogWrapper)
        {
            m_logWrapper = ludeoLogWrapper;            
        }

        [MonoPInvokeCallback(typeof(LudeoLogCallback))]
        private static void InvokeLogCallbackWithLudeoMessage(ref LudeoLogMessage msg)
        {
            string timestamp = Marshal.PtrToStringAnsi(msg.timestamp);
            string message = SafePtrToUtf8String(msg.message);

            string logMsg = $"{timestamp}:{msg.category}:{msg.level} {message}";
            if (msg.level >= LudeoLogLevel.Log)
                m_logInfoCallback(logMsg);
            else if (msg.level <= LudeoLogLevel.Error)
                m_logErrorCallback(logMsg);
            else
                m_logWarningCallback(logMsg);
        }

        private static string SafePtrToUtf8String(IntPtr ptrToMsg)
        {
            string outString = string.Empty;
            if (ptrToMsg == IntPtr.Zero)
                return outString;
            int len = 0;
            unsafe
            {
                byte* ptr = (byte*)ptrToMsg;
                while (ptr[len] != 0) ++len;
                outString = Encoding.UTF8.GetString(ptr, len);
            }
            return outString;
        }
        
        private StringBuilder GetStringBuilderForLogging()
        {
            const int STRING_BUILDERS_COUNT = 10;
            const int INITIAL_ALLOCATION_SIZE = 500;

            if (m_stringBuilderPool == null)
                m_stringBuilderPool = new StringBuilderPool(STRING_BUILDERS_COUNT, INITIAL_ALLOCATION_SIZE);

            StringBuilder stringBuilder;
            m_stringBuilderPool.GetNextItem(out stringBuilder);
            return stringBuilder;
        }

        private void WriteLog(LudeoLogLevel level, params string [] msgs)
        {
            if (IsLogLevelOn(level))
            {
                m_concatenateStringBuilder.Clear();
                for (int i = 0; i < msgs.Length; i++)
                {
                    m_concatenateStringBuilder.Append(" ");
                    m_concatenateStringBuilder.Append(msgs[i]);
                }

                string levelPrefix = GetPrefix(level);
                string finalMsg = m_concatenateStringBuilder.ToString();
                
                m_logBuilder.Length = 0;
                m_logBuilder.Append($"{levelPrefix}{finalMsg}");
                ReportLogMessage(m_logBuilder.ToString(), level);
            }
        }

        private static string GetPrefix(LudeoLogLevel logLevel)
        {
            switch (logLevel)
            {
                case LudeoLogLevel level when level >= LudeoLogLevel.Log:
                    return LOG_PREFIX_INFO;
                case LudeoLogLevel.Warning:
                    return LOG_PREFIX_WARNING;
                case LudeoLogLevel level when level <= LudeoLogLevel.Error:
                    return LOG_PREFIX_ERROR;
                default: return LOG_PREFIX_INFO;
            }
        }

       private bool IsLogLevelOn(LudeoLogLevel level)
        {
            return level <= m_logLevel;
        }


        private void ReportLogMessage(string msg, LudeoLogLevel level)
        {
            switch (level)
            {
                case LudeoLogLevel l when l >= LudeoLogLevel.Log:
                    m_logInfoCallback(msg);
                    break;
                case LudeoLogLevel.Warning:
                    m_logWarningCallback(msg);
                    break;
                case LudeoLogLevel l when l <= LudeoLogLevel.Error:
                    m_logErrorCallback(msg);
                    break;
                default: 
                    Debug.Log(msg);
                    break;
            }
        }

        #endregion PRIVATE METHODS
    }
}
/// @endcond INTERNAL
