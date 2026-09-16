// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using LudeoSDK.Internal;
    using LudeoSDK.Ludeo.Internal.Launchers;
    using LudeoSDK.MediaCapture;
    using LudeoSDK.UnityScripts;
    using System;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    /// The LudeoManager serves as your entry point to run Ludeo Services from a Game client.
    /// </summary>
    public class LudeoManager : IDisposable
    {
        public static string CoreDllName {  get; private set; }

        private static LudeoSessionManager m_sessionManager;

        /// <summary>
        /// The session manager used to create Ludeo sessions.
        /// <see cref="Initialize"/> must have succeeded before creating a session.
        /// </summary>
        public static LudeoSessionManager SessionManager => m_sessionManager ?? (m_sessionManager = new LudeoSessionManager());

        // Temporary fix for LC-8977: the ludeo force lunch should be handled by the session activation flow,
        // but as the plugin doesn't know the auth token in time, we call ludeo selected on the first video state change, as before
        internal bool IsActivationCallbackWaiting
        {
            get
            {
                bool isRequired = m_isActivationCallbackWaiting;
                m_isActivationCallbackWaiting = false;
                return isRequired;
            }
            set
            {
                m_isActivationCallbackWaiting = value;
            }
        }

        private bool m_isActivationCallbackWaiting;

        private const string LOG_PREFIX = "[LudeoManager]";

        internal struct LudeoSessionInternalCallbacks
        {
            public Action<LudeoSessionReleaseCallbackData> onLudeoSessionRelease;
            public Action<LudeoSessionHighlightCallbackData> onHighlightTaken;
        }

        internal static LudeoManager Instance { get => m_instance; }

        internal LudeoSession LudeoSession { get; private set; }

        internal LocalConfigData LocalConfigData { get => m_localConfigData; }
        
        private static ILudeoCoreInterface m_coreHook;
        private static bool m_isLudeoInit;
        private static LudeoManager m_instance;
        private static bool m_isDestroyUnityImmediate;
        private static ILudeoInitWrapper m_ludeoInitWrapper;
        private static ILudeoSessionWrapper m_ludeoSessionWrapper;
        private static LudeoInitializeParams m_ludeoInitializeParams;
        private static MediaCaptureSupport m_defaultMediaCaptureSupport;

        private SystemData m_systemData;
        private LocalConfigData m_localConfigData;
        
        private LudeoUnityManager m_ludeoUnityManager;

        private bool disposedValue;

        private void InitCoreInterface()
        {
            m_coreHook = LudeoUnityHelpers.TryGetLudeoCoreInterface();
            UnityWrapperActivationParams activationParams = m_coreHook.GetWrapperActivationParams();

            CoreDllName                     = activationParams.coreDllName;
            m_isDestroyUnityImmediate       = activationParams.isDestoryUnityObjectImmediate;
            m_ludeoInitializeParams         = LudeoInitializeParams.Default;
            m_defaultMediaCaptureSupport    = activationParams.mediaCaptureSupport;

            m_ludeoInitWrapper              = m_coreHook.GetLudeoInitWrapper();
            m_ludeoSessionWrapper           = m_coreHook.GetLudeoSessionWrapper();
        }

        private LudeoManager() 
        {
            InitCoreInterface();
            LudeoLogManager.Init();

            m_instance = this;
            m_systemData = new SystemData(m_defaultMediaCaptureSupport);
                    
            m_instance.SetupLocalConfig();
        }

        //~LudeoManager()
        //{
        //    Dispose();
        //}

        private LudeoResult DoDispose(bool disposing)
        {
            LudeoResult ludeoResult = LudeoResult.LudeoManagerAlreadyDisposed;

            LudeoLogManager.LogInfo($"{LOG_PREFIX} Disposing. LI: {m_instance}, LUMI: {m_ludeoUnityManager}, DI:{m_isDestroyUnityImmediate}");

            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    if (m_instance != null)
                    {
                        if (m_ludeoUnityManager != null)
                        {
                            if (!m_isDestroyUnityImmediate)
                                Object.Destroy(m_ludeoUnityManager.gameObject);
                            else
                                Object.DestroyImmediate(m_ludeoUnityManager.gameObject);
                        }

                        LudeoSession?.Dispose();
                        LudeoSession = null;

                        LauncherManager.DeinitLauncher(m_systemData);
                        
                        m_systemData?.Deinit();
                        m_systemData = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null

                PluginCallbacks.DeinitCallbacks();

                ludeoResult = LudeoInit.Shutdown(m_ludeoInitWrapper);
                
                m_instance = null;
                m_isLudeoInit = false;

                disposedValue = true;
            }

           
            LudeoLogManager.LogInfo($"{LOG_PREFIX} Disposed");

            return ludeoResult;
        }

        
        /// <summary>
        /// Initializes the Ludeo SDK. Must be called before any other SDK function.
        /// Sets up the SDK's core components and prepares the runtime, but does NOT create a session.
        /// After Initialize succeeds, call <see cref="SessionManager"/>.CreateSession to obtain a <see cref="LudeoSession"/>,
        /// subscribe to its events, and then call <see cref="LudeoSession.Activate"/> to connect to the backend.
        /// </summary>
        /// <returns>
        /// <see cref="LudeoResult.Success"/> on first call;
        /// <see cref="LudeoResult.LudeoManagerAlreadyInitialized"/> if the SDK is already initialized;
        /// or one of several other <see cref="LudeoResult"/> values on failure.
        /// </returns>
        public static LudeoResult Initialize()
        {
            if (m_isLudeoInit)
                return LudeoResult.LudeoManagerAlreadyInitialized;

            m_isLudeoInit = true;
            new LudeoManager();
            Instance.InstantiateUnityManager();

            PluginCallbacks.InjectImplementation(m_ludeoInitWrapper);
            PluginCallbacks.RegisterInternalCallbacks(new LudeoSessionInternalCallbacks() { onLudeoSessionRelease = Instance.HandleGameSessionRelease });

            HandleOptionalRunCommands();

            LudeoResult ludeoResult = LudeoInit.Initialize(m_ludeoInitWrapper, m_ludeoInitializeParams);

            Instance.UpdateLogSettings();

            if (ludeoResult == LudeoResult.Success)
            {
                Instance.m_ludeoUnityManager.SetLudeoManager(Instance, Instance.m_systemData);
                Instance.disposedValue = false;
            }
            else
                Instance.DoDispose(true);

            return ludeoResult;
        }

        /// <summary>
        /// Creates a Ludeo session. <see cref="Initialize"/> must have been called successfully first.
        /// A session represents a logged-in Ludeo user and their connection to the backend.
        /// After CreateSession succeeds, subscribe to the session's events, then call
        /// <see cref="LudeoSession.Activate"/> to connect to the backend.
        /// </summary>
        /// <param name="ludeoSession">The newly created session, or <c>null</c> if creation failed.</param>
        /// <returns>One of several <see cref="LudeoResult"/> values.</returns>
        internal static LudeoResult DoCreateSession(out LudeoSession ludeoSession)
        {
            ludeoSession = null;

            if (!m_isLudeoInit || Instance == null)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} CreateSession called before Initialize");
                return LudeoResult.LudeoNotYetInit;
            }

            LudeoResult sessionCreateResult = LudeoSessionInternal.Create(m_ludeoSessionWrapper, out LudeoHSession sessionHandle);

            if (sessionCreateResult == LudeoResult.Success)
            {
                ludeoSession = new LudeoSession(sessionHandle);
                Instance.LudeoSession = ludeoSession;
            }

            return sessionCreateResult;
        }


        /// <summary>
        /// Sets the maximum log level for the a log category
        /// By default all categories will log at the LudeoLogLevel.Log level or bellow, which means Verbose
        /// or VeryVerbose levels will not show up in the logs.
        /// </summary>
        /// <param name="level">One of LudeoSDK's LudeoLogLevel</param>
        /// <param name="category">One of LudeoSDK's LudeoLogCategory</param>
        /// <returns>One of several LudeoResult values</returns>
        public static LudeoResult SetLoggingLevel(LudeoLogLevel level, LudeoLogCategory category = LudeoLogCategory.All)
        {
            return LudeoLogManager.Instance.SetLogLevel(level, category);
        }


        /// <summary>
        /// Sets the logging callback functions.
        /// Any logging done by the SDK will be forwarded to the relevant function.
        /// Avoid expensive operations from the callback, such as directly logging to a file.
        /// </summary>
        /// <param name="ludeoLogInfoCallback">callback for info level logs</param>
        /// <param name="ludeoLogWarningCallback">callback for warning level logs</param>
        /// <param name="ludeoLogErrorCallback">callback for error level logs</param>
        /// <returns>One of several LudeoResult values</returns>
        public static LudeoResult SetLoggingCallback(Action<string> ludeoLogInfoCallback, Action<string> ludeoLogWarningCallback, Action<string> ludeoLogErrorCallback)
        {
            if (ludeoLogInfoCallback == null || ludeoLogWarningCallback == null || ludeoLogErrorCallback == null)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} SetLoggingCallback was called with null callback");
                return LudeoResult.InvalidParameters;
            }
            return LudeoLogManager.Instance.SetLogCallback(ludeoLogInfoCallback, ludeoLogWarningCallback, ludeoLogErrorCallback);
        }

        internal string GetLudeoPlatformUrl() { return m_systemData.localConfigData.platformUrl; }

        internal void Tick()
        {
            LudeoInit.Tick(m_ludeoInitWrapper);
        }

        private void SetupLocalConfig()
        {
            bool isSuccess = LocalConfigSystem.ReadConfig(out m_systemData.localConfigData);
            m_localConfigData = m_systemData.localConfigData;

            Instance.UpdateLogSettings();
        }

        internal LudeoResult Shutdown()
        {
            return DoDispose(true);
        }

        // This is a development only use case (for SDK and Integration developers)
        internal bool IsForceLudeoLaunch(out Guid ludeoIdToInject)
        {
            return (m_localConfigData.IsForceLudeoInjection(out ludeoIdToInject));
        }

        private static void HandleOptionalRunCommands()
        {
            Instance.m_systemData.isCoreVideoEnabled = true;

            if (LocalConfigSystem.TryReadRunCommands(out RunCommands runCommands))
            {
                RunCommand[] commands = runCommands.commands;

                // Core handle
                for (int i = 0; i < commands.Length; ++i)
                {
                    RunCommand command = commands[i];
                    if (!string.IsNullOrWhiteSpace(command.key))
                    {
                        LudeoResult result = m_ludeoInitWrapper.LudeoCommand(command.key, command.value);
                        LudeoLogManager.LogWarning($"{LOG_PREFIX} Ludeo Command called: {command.key} : {command.value}, result = {result}");
                    }
                    else
                        LudeoLogManager.LogError($"{LOG_PREFIX} Can not call empty Ludeo Command key");
                }

                // Wrapper handle
                Instance.m_systemData.isCoreVideoEnabled = runCommands.IsCoreVideoEnabled();
            }

            if (Instance.m_systemData.isCoreVideoEnabled)
            {
                Instance.m_systemData.supportedMediaCapture = MediaCaptureSupport.Dummy;
                LudeoLogManager.LogInfo($"{LOG_PREFIX} Core video enabled. Setting Legacy video to: {Instance.m_systemData.supportedMediaCapture}");
            }
            else
                LudeoLogManager.LogInfo($"{LOG_PREFIX} Core video disabled. Will use Legacy video when on.");
        }

        private void UpdateLogSettings()
        {
            SetLoggingLevel(m_localConfigData.coreLogLevel, m_localConfigData.coreLogCategory);
            SetLoggingCallback(Debug.Log, Debug.LogWarning, Debug.LogError);
        }

        private void InstantiateUnityManager()
        {
            GameObject prefab = Resources.Load("LudeoUnityManager") as GameObject;
            GameObject go = GameObject.Instantiate(prefab);

            m_ludeoUnityManager = go.AddComponent<LudeoUnityManager>();

            if (!LudeoUnityHelpers.IsRunWithoutLauncher())
                LauncherManager.TryLauncherSetup(m_systemData);
            else
                LudeoLogManager.LogInfo("runWithoutLauncher is true. Not setting up one.");
        }

        private void HandleGameSessionRelease(LudeoSessionReleaseCallbackData ludeoSessionReleaseCallbackData)
        {

        }

        protected virtual void Dispose(bool disposing)
        {
            DoDispose(disposing);
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LudeoManager()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
