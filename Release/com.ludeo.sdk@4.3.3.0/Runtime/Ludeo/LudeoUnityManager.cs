// Copyright 2024 Ludeo. All rights reserved.

/// @cond INTERNAL
namespace LudeoSDK
{
    using LudeoSDK.Internal;
    using LudeoSDK.MediaCapture;
    using LudeoSDK.UnityScripts;
    using System;
    using UnityEngine;

    /// <summary>
    /// Initialized by Ludeo.
    /// Handles data and functinoality passed between Ludeo and the UnityEngine.
    /// </summary>
    public class LudeoUnityManager : MonoBehaviour
    {
        private const string LOG_PREFIX = "[UNITY_HOOK]";

        private static LudeoUnityManager m_instance;

        internal Action<CaptureStatus, bool> onInitialConsentUpdates;

        private bool m_canQuitApp;
        private LudeoManager m_ludeoManager;
        private static IInterfaceFactory m_interfaceFactory = new DummyInterfaceFactory();
        private MediaCaptureService m_mediaCaptureService;

        internal static IInterfaceFactory LudeoInterfaceFactory
        {
            get
            {
                return m_interfaceFactory;
            }
        }

        internal static LudeoUnityManager Instance
        {
            get 
            { 
                return m_instance; 
            }
        }

        #region -------------------------- UNITY METHODS --------------------------
#pragma warning disable IDE0051 // Remove unused private members warning
        private void Awake()
        {
            if (m_instance != null)
            {
                Destroy(gameObject);
                throw new NotSupportedException($"{LOG_PREFIX} May not allocate more then one LudeoUnityManager! Disposing new one.");
            }

            gameObject.AddComponent<LudeoUnityMainThreadDispatcher>();

            m_canQuitApp = false;
            m_instance = this;

            if (!LudeoUnityHelpers.InjectInstanceFromUnity(gameObject, out m_interfaceFactory))
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to inject IInterfaceFactory!");

            Application.wantsToQuit -= OnWantsToQuit;
            Application.wantsToQuit += OnWantsToQuit;
            DontDestroyOnLoad(gameObject);            

            LudeoLogManager.LogInfo($"{LOG_PREFIX} Awake");
        }


        private void LateUpdate()
        {
            m_ludeoManager.Tick();
            m_mediaCaptureService.UpdateFrames(Time.unscaledDeltaTime);
        }

        private void OnDestory()
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} OnDestory");

            DeInit();
        }
#pragma warning restore IDE0051 // End Remove unused private members warning
        #endregion -------------------------- UNITY METHODS --------------------------

        internal void SetLudeoManager(LudeoManager ludeoManager, SystemData systemData)
        {
            m_mediaCaptureService = new MediaCaptureService(systemData);
            m_ludeoManager = ludeoManager;
            enabled = true;
        }

        internal void TEMP_InitializeMediaCapture(SystemData systemData, Action<CaptureStatus> tempOnMediaCaptureInitComplete)
        {
            LudeoLogManager.LogVerbose($"{LOG_PREFIX} TEMP_InitializeMediaCapture");

            if (systemData.IsMediaCaptureSupported())
                m_mediaCaptureService.InitializeCaptureServices(tempOnMediaCaptureInitComplete);
            else
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} TEMP_InitializeMediaCapture abort: Video Legacy not supported in this run.");
                systemData.captureStatus = CaptureStatus.InitSuccess;
                tempOnMediaCaptureInitComplete(systemData.captureStatus);
            }
        }

        internal void TEMP_UpdateMediaCaptureService(SystemData systemData, LudeoSessionVideoStateCallbackData ludeoStateUpdateData, Action<CaptureStatus, bool> onCaptureStatusUpdated)
        {
            LudeoLogManager.LogVerbose($"{LOG_PREFIX} TEMP_UpdateMediaCaptureService");

            systemData.ludeoSessionVideoData = ludeoStateUpdateData;

            if (systemData.IsMediaCaptureSupported())
                m_mediaCaptureService.SwitchService(systemData.ludeoSessionVideoData.videoEnabled, onCaptureStatusUpdated);
            else
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} TEMP_UpdateMediaCaptureService abort: Video Legacy not supported in this run.");
                systemData.captureStatus = CaptureStatus.InitSuccess;
                onCaptureStatusUpdated(systemData.captureStatus, systemData.ludeoSessionVideoData.videoEnabled);
            }
        }


        private bool OnWantsToQuit()
        {
            LudeoLogManager.LogInfo($"{LOG_PREFIX} OnWantsToQuit");
            bool isWantsToQuit = true;
            if (!m_canQuitApp)
            {
                m_canQuitApp = true;
                isWantsToQuit = false;
                if (!Application.isEditor && (this != null))
                {
                    LudeoLogManager.LogInfo($"{LOG_PREFIX} OnWantsToQuit - Quitting after a while.");
                    const int WAIT_MS_BEFORE_QUIT = 60;

                    DeInit();
                    QuitProcess.WaitAndQuitAfterAWhile(m_ludeoManager, WAIT_MS_BEFORE_QUIT);
                }
                else
                {
                    LudeoLogManager.LogInfo($"{LOG_PREFIX} OnWantsToQuit - Quitting immediately.");
                    DeInit();
                    isWantsToQuit = true; // In editor, false behaviour is not supported (as designed) by Unity
                }
            }
            LudeoLogManager.LogInfo($"{LOG_PREFIX} OnWantsToQuit - isWantsToQuit: {isWantsToQuit}");
            return isWantsToQuit; // Postpone quitting until operations are complete (We need to call the quit ourselves)
        }

        internal void DeInit()
        {
            if (m_instance != null)
            {
                try
                {
                    LudeoLogManager.LogInfo($"{LOG_PREFIX} DeInit");

                    m_instance.m_mediaCaptureService?.DisposeIfNeeded();
                    m_instance.m_mediaCaptureService = null;

                    if (m_ludeoManager != null)
                    {
                        m_ludeoManager.Shutdown();
                        m_ludeoManager.Tick();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            else
                Debug.Log($"{LOG_PREFIX} already DeInit().");
        }
    }
}
/// @endcond INTERNAL