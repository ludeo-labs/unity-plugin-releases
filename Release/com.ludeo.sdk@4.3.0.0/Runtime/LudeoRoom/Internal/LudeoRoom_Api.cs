/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using AOT;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class LudeoRoom_Api
    {
        private const string LOG_PREFIX = "[LudeoRoom]";

        private static LudeoRoom m_roomSession;
        private static string m_playerId;

        private static List<string> m_unreleasedPlayer = new List<string>();

        private static Action<LudeoCloseRoomCallbackData> m_onCloseRoomDone;
        private static Action<LudeoRoomAddPlayerCallbackData> m_addPlayerCallback;
        private static Action<LudeoRoomRemovePlayerCallbackData> m_removePlayerCallback;


        public void CloseRoom(ILudeoRoomWrapper ludeoLudeoWrapper, LudeoHRoom roomHandle, Action<LudeoCloseRoomCallbackData> closeRoomCallback)
        {
            LudeoRoomCloseParams ludeoSessionCloseRoomParams = LudeoRoomCloseParams.Default;

            m_onCloseRoomDone = closeRoomCallback;

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} calling CloseRoom");

            ludeoLudeoWrapper.CloseRoom(roomHandle, ref ludeoSessionCloseRoomParams, IntPtr.Zero, HandleLudeoCloseRoomCallback);

            LudeoRoom.ActiveRoom.Dispose();
        }


        public void AddPlayer(LudeoRoom roomSession, ILudeoRoomWrapper ludeoRoomWrapper,
                                        LudeoHRoom roomHandle, string playerId, Action<LudeoRoomAddPlayerCallbackData> addPlayerCallback)
        {
            IntPtr playerIdAsIntPtr = Marshal.StringToHGlobalAnsi(playerId);

            LudeoRoomAddPlayerParams ludeoRoomAddPlayersParams = new LudeoRoomAddPlayerParams(playerIdAsIntPtr);

            m_roomSession = roomSession;
            m_playerId = playerId;
            m_addPlayerCallback = addPlayerCallback;

            try
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} Calling AddPlayer = {playerId}");

                ludeoRoomWrapper.AddPlayer(roomHandle, ref ludeoRoomAddPlayersParams, IntPtr.Zero, HandleAddPlayerCallback);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} @AddPlayer, exception: {e}");
            }
            finally
            {
                Marshal.FreeHGlobal(playerIdAsIntPtr);
            }
        }

        public void RemovePlayer(LudeoRoom ludeoRoom, ILudeoRoomWrapper ludeoRoomWrapper,
                                            LudeoHRoom roomHandle, string playerId, Action<LudeoRoomRemovePlayerCallbackData> removePlayerCallback)
        {
            IntPtr playerIdAsIntPtr = Marshal.StringToHGlobalAnsi(playerId);

            LudeoRoomRemovePlayerParams ludeoRoomRemovePlayerParams = new LudeoRoomRemovePlayerParams(playerIdAsIntPtr);

            m_roomSession = ludeoRoom;
            m_playerId = playerId;
            m_removePlayerCallback = removePlayerCallback;

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} Calling RemovePlayer = {playerId}");

            ludeoRoomWrapper.RemovePlayer(roomHandle, ref ludeoRoomRemovePlayerParams, IntPtr.Zero, HandleRemovePlayerCallback);

            Marshal.FreeHGlobal(playerIdAsIntPtr);
        }

        public void ReleaseAllPlayers(ILudeoRoomWrapper ludeoRoomWrapper)
        {
            m_unreleasedPlayer.Clear();
        }

        [MonoPInvokeCallback(typeof(LudeoSessionCloseRoomCallback))]
        private static void HandleLudeoCloseRoomCallback(ref LudeoRoomCloseCallbackParams nativeOutParams)
        {
            try
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} CloseRoom callback.");

                nativeOutParams.ConvertToManaged(out LudeoCloseRoomCallbackData managedData);

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} CloseRoom result: {managedData.resultCode}");

                m_onCloseRoomDone(managedData);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} @HandleLudeoCloseRoomCallback, exception: {e}");
            }
            finally
            {
                m_onCloseRoomDone = null;
            }
        }


        [MonoPInvokeCallback(typeof(LudeoRoomAddPlayerCallback))]
        private static void HandleAddPlayerCallback(ref LudeoRoomAddPlayerCallbackParams nativeOutParams)
        {
            try
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} AddPlayer result: {nativeOutParams.resultCode}. Adding: {m_playerId}");


                if (nativeOutParams.ConvertToManaged(m_playerId, out LudeoRoomAddPlayerCallbackData managedData))
                {
                    m_unreleasedPlayer.Add(m_playerId);
                    m_roomSession.HandleAddPlayerSuccess(m_addPlayerCallback, managedData);
                }
                else
                    m_roomSession.HandleAddPlayerFail(m_addPlayerCallback, managedData);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX}@HandleAddPlayerCallback, exception: {e}, {e.StackTrace}");
            }
            finally
            {
                m_addPlayerCallback = null;
                m_roomSession = null;
                m_playerId = null;
            }
        }

        [MonoPInvokeCallback(typeof(LudeoRoomRemovePlayerCallback))]
        private static void HandleRemovePlayerCallback(ref LudeoRoomRemovePlayerCallbackParams nativeOutParams)
        {
            try
            {
                if (nativeOutParams.ConvertToManaged(out LudeoRoomRemovePlayerCallbackData managedData))
                    m_roomSession.HandleRemovePlayerSuccess(m_playerId, m_removePlayerCallback, managedData);
                else
                    m_roomSession.HandleRemovePlayerFail(m_removePlayerCallback, managedData);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX}@HandleRemovePlayerCallback, exception: {e}, {e.StackTrace}");
            }
            finally
            {
                m_unreleasedPlayer.Remove(m_playerId);

                m_removePlayerCallback = null;
                m_roomSession = null;
                m_playerId = null;
            }
        }

        [MonoPInvokeCallback(typeof(LudeoRoomRemovePlayerCallback))]
        private static void HandleRemovePlayerCallbackNoAction(ref LudeoRoomRemovePlayerCallbackParams nativeOutParams)
        {

        }
    }
}
/// @endcond INTERNAL
