/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using AOT;
    using System;

    internal class LudeoGameplaySessionInternal
    {
        private const string LOG_PREFIX = "[LudeoGameplaySessionInternal]";

        private static Action m_onBeginGameplaySDK;
        private static Action<LudeoPlayerBeginGameplayCallbackData> m_onBeginGameplayCallbackGame;

        private static Action m_onEndGameplaySDK;
        private static Action<LudeoPlayerEndGameplayCallbackData> m_onEndGameplayCallbackGame;
        
        private static Action m_onAbortGameplaySDK;
        private static Action<LudeoPlayerAbortGameplayCallbackData> m_onAbortGameplayCallbackGame;

        private static Action<LudeoGameplaySessionMarkHighlightCallbackData> m_onMarkHighlightCallbackGame;

        public void Begin(ILudeoGameplaySessionWrapper ludeoGameplaySessionWrapper, LudeoHGameplaySession ludeoGameplaySessionHandle, Action onBeginGameplay,
                                Action<LudeoPlayerBeginGameplayCallbackData> onBeginGameplayCallback)
        {
            LudeoGameplaySessionBeginParams beginGameplayParams = LudeoGameplaySessionBeginParams.Default;

            m_onBeginGameplaySDK = onBeginGameplay;
            m_onBeginGameplayCallbackGame = onBeginGameplayCallback;

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} calling GameplaySessionBegin");

            ludeoGameplaySessionWrapper.Begin(ludeoGameplaySessionHandle, ref beginGameplayParams, IntPtr.Zero, HandleBeginGameplayCallback);
        }

        public void End(ILudeoGameplaySessionWrapper ludeoGameplaySessionWrapper, LudeoHGameplaySession ludeoGameplaySessionHandle,
                                Action onEndGameplay, Action<LudeoPlayerEndGameplayCallbackData> onEndGameplayCallback)
        {
            LudeoGameplaySessionEndParams endGameplayParams = new LudeoGameplaySessionEndParams(false);

            m_onEndGameplaySDK = onEndGameplay;
            m_onEndGameplayCallbackGame = onEndGameplayCallback;

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} calling GameplaySessionEnd");

            ludeoGameplaySessionWrapper.End(ludeoGameplaySessionHandle, ref endGameplayParams, IntPtr.Zero, HandleEndGameplayCallback);
        }

        public void Abort(ILudeoGameplaySessionWrapper ludeoGameplaySessionWrapper, LudeoHGameplaySession ludeoGameplaySessionHandle,
                                Action onAbortGameplay, Action<LudeoPlayerAbortGameplayCallbackData> onAbortGameplayCallback)
        {
            LudeoGameplaySessionEndParams endGameplayParams = new LudeoGameplaySessionEndParams(true);

            m_onAbortGameplaySDK = onAbortGameplay;
            m_onAbortGameplayCallbackGame = onAbortGameplayCallback;

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} calling GameplaySessionAbort");

            ludeoGameplaySessionWrapper.End(ludeoGameplaySessionHandle, ref endGameplayParams, IntPtr.Zero, HandleAbortGameplayCallback);
        }

        // TODO: WARNING we use here temporarily sessionHandle!!! NOT ludeoGameplaySessionHandle
        public void MarkHighlight(ILudeoGameplaySessionWrapper ludeoGameplaySessionWrapper, LudeoHSession sessionHandle,
                                        Action<LudeoGameplaySessionMarkHighlightCallbackData> onMarkHighlightCallback)
        {
            LudeoSessionMarkHighlightParams ludeoSessionMarkHighlightParams = LudeoSessionMarkHighlightParams.Default;

            m_onMarkHighlightCallbackGame = onMarkHighlightCallback;

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} calling MarkHighlight");

            ludeoGameplaySessionWrapper.MarkHighlight(sessionHandle, ref ludeoSessionMarkHighlightParams, IntPtr.Zero, HandleMarkHighlightCallback);
        }

        [MonoPInvokeCallback(typeof(LudeoGameplaySessionBeginCallback))]
        private static void HandleBeginGameplayCallback(ref LudeoSessionBeginGameplayCallbackParams nativeOutParams)
        {
            try
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} GameplaySessionBegin callback.");                

                CallOrWarn(m_onBeginGameplaySDK, "m_onBeginGameplaySDK");

                nativeOutParams.ConvertToManaged(out LudeoPlayerBeginGameplayCallbackData managedData);

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} GameplaySessionBegin result: {managedData.resultCode}");

                CallOrWarn(m_onBeginGameplayCallbackGame, "m_onBeginGameplayCallbackGame", managedData);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} @HandleBeginGameplayCallback, exception: {e}");
            }
            finally
            {
                m_onBeginGameplaySDK = null;
                m_onBeginGameplayCallbackGame = null;
            }
        }

        [MonoPInvokeCallback(typeof(LudeoGameplaySessionEndCallback))]
        private static void HandleEndGameplayCallback(ref LudeoGameplaySessionEndCallbackParams nativeOutParams)
        {
            try
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} GameplaySessionEnd callback.");

                CallOrWarn(m_onEndGameplaySDK, "m_onEndGameplaySDK");

                nativeOutParams.ConvertToManaged(out LudeoPlayerEndGameplayCallbackData managedData);

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} GameplaySessionEnd result: {managedData.resultCode}");

                CallOrWarn(m_onEndGameplayCallbackGame, "m_onEndGameplayCallbackGame", managedData);
            }
            catch(Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} @HandleEndGameplayCallback, exception: {e}");
            }
            finally
            {
                m_onEndGameplaySDK = null;
                m_onEndGameplayCallbackGame = null;
            }
        }

        [MonoPInvokeCallback(typeof(LudeoGameplaySessionEndCallback))]
        private static void HandleAbortGameplayCallback(ref LudeoGameplaySessionEndCallbackParams nativeOutParams)
        {
            try
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} GameplaySessionAbort callback.");

                CallOrWarn(m_onAbortGameplaySDK, "m_onAbortGameplaySDK");

                nativeOutParams.ConvertToManaged(out LudeoPlayerAbortGameplayCallbackData managedData);

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} GameplaySessionAbort result: {managedData.resultCode}");

                CallOrWarn(m_onAbortGameplayCallbackGame, "m_onAbortGameplayCallbackGame", managedData);
            }
            catch(Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} @HandleAbortGameplayCallback, exception: {e}");
            }
            finally
            {
                m_onAbortGameplaySDK = null;
                m_onAbortGameplayCallbackGame = null;
            }
        }
        
        [MonoPInvokeCallback(typeof(LudeoSessionMarkHighlightCallback))]
        private static void HandleMarkHighlightCallback(ref LudeoSessionMarkHighlightCallbackParams nativeOutParams)
        {
            try
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} MarkHighlight callback.");

                nativeOutParams.ConvertToManaged(out LudeoGameplaySessionMarkHighlightCallbackData managedData);

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} MarkHighlight result: {managedData.resultCode}");

                CallOrWarn(m_onMarkHighlightCallbackGame, "m_onMarkHighlightCallbackGame", managedData);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} @HandleMarkHighlightCallback, exception: {e}");
            }
            finally
            {
                m_onMarkHighlightCallbackGame = null;
            }            
        }

        private static void CallOrWarn(Action callback, string callname)
        {
            if (callback != null)
                callback();
            else
                LudeoLogManager.LogWarning($"{LOG_PREFIX} {callname} already cleared! Can not callback.");
        }

        private static void CallOrWarn<T>(Action<T> callback, string callname, T data = default)
        {
            if (callback != null)
                callback(data);
            else
                LudeoLogManager.LogWarning($"{LOG_PREFIX} {callname} already cleared! Can not callback.");
        }
    }
}
/// @endcond INTERNAL