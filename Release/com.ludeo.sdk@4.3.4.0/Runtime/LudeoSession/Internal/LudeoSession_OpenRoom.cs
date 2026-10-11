/// @cond INTERNAL
using AOT;
using System;
using System.Runtime.InteropServices;

namespace LudeoSDK.Internal
{
    internal class LudeoSession_OpenRoom
    {
        private const string LOG_PREFIX = "[LudeoSession]";

        private static Action<LudeoOpenRoomCallbackData> m_onOpenRoomDone;
        private static bool m_isRoomOpenedForLudeoSession;

        public void OpenRoom(ILudeoSessionWrapper ludeoSessionWrapper, LudeoHSession ludeoSessonHandle,
                                    LudeoSessionOpenRoomParameters openRoomData, Action<LudeoOpenRoomCallbackData> openRoomCallback)
        {
            IntPtr ptrLudeoId = Marshal.StringToHGlobalAnsi(openRoomData.LudeoId.ToString());
            IntPtr ptrRoomId = Marshal.StringToHGlobalAnsi(openRoomData.RoomId);

            LudeoSessionOpenRoomParams nativeParams = new LudeoSessionOpenRoomParams(ptrLudeoId, ptrRoomId, openRoomData.ThreadSafe, openRoomData.MaxConcurrentWrites);

            m_onOpenRoomDone = openRoomCallback;

            SetIfThisSessionIsLudeoSession(openRoomData.LudeoId);

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} calling OpenRoom");

            ludeoSessionWrapper.OpenRoom(ludeoSessonHandle, ref nativeParams, IntPtr.Zero, HandleLudeoOpenRoomCallback);


            Marshal.FreeHGlobal(ptrLudeoId);
            Marshal.FreeHGlobal(ptrRoomId);
        }


        [MonoPInvokeCallback(typeof(LudeoSessionOpenRoomCallback))]        
        private static void HandleLudeoOpenRoomCallback(ref LudeoSessionOpenRoomCallbackParams nativeOutParams)
        {
            try
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} OpenRoom callback.");

                nativeOutParams.ConvertToManaged(m_isRoomOpenedForLudeoSession, out LudeoOpenRoomCallbackData managedData);

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} OpenRoom result: {managedData.resultCode}");

                m_onOpenRoomDone(managedData);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} @HandleLudeoOpenRoomCallback, exception: {e}");
            }
            finally
            {
                m_onOpenRoomDone = null;
            }
        }

        private static void SetIfThisSessionIsLudeoSession(Guid? ludeoId)
        {
            m_isRoomOpenedForLudeoSession = ludeoId.HasValue && (ludeoId.Value != Guid.Empty);
        }
    }
}
/// @endcond INTERNAL
