/// @cond INTERNAL
using AOT;
using System;
using System.Runtime.InteropServices;


namespace LudeoSDK.Internal
{
    internal class LudeoSession_GetLudeo
    {
        private const string LOG_PREFIX = "SESSION_GET_LUDEO";

        private static Action<LudeoGetLudeoCallbackData> m_onGetLudeoCallbackGame;

        public void GetLudeo(ILudeoSessionWrapper sessionWrapper, LudeoHSession ludeoSessionHandle, Guid ludeoId, Action<LudeoGetLudeoCallbackData> onGetLudeoCallback)
        {
            IntPtr ptrLudeoId;

            if (ludeoId != Guid.Empty)
                ptrLudeoId = Marshal.StringToHGlobalAnsi(ludeoId.ToString());
            else
                ptrLudeoId = IntPtr.Zero;

            try 
            { 
                LudeoSessionGetLudeoParams getLudeoParams = new LudeoSessionGetLudeoParams(ptrLudeoId);

                m_onGetLudeoCallbackGame = onGetLudeoCallback;

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} requesting id: {ludeoId}");
                
                sessionWrapper.GetLudeo(ludeoSessionHandle, ref getLudeoParams, IntPtr.Zero, HandleGetLudeoCallback);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} {e}");
                throw new Exception($"{LOG_PREFIX} {e}");
            }
            finally
            {
                Marshal.FreeHGlobal(ptrLudeoId);
            }
        }

        [MonoPInvokeCallback(typeof(LudeoSessionGetLudeoCallback))]
        private static void HandleGetLudeoCallback(ref LudeoSessionGetLudeoCallbackParams getLudeoCallbackParams)
        {
            try
            {
                getLudeoCallbackParams.ConvertToManaged(out LudeoGetLudeoCallbackData managedData);
                m_onGetLudeoCallbackGame(managedData);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} @HandleGetLudeoCallback, exception: {e}");
            }
            finally
            {
                m_onGetLudeoCallbackGame = null;
            }
        }
    }
}
/// @endcond INTERNAL