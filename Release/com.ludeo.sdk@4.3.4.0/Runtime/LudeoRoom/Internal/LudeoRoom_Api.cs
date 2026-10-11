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

        private static List<string> m_unreleasedPlayer = new List<string>();

        // Per-call state, passed to the core SDK as clientData (a GCHandle) and handed back in the callback,
        // so overlapping calls can't overwrite each other's callbacks.
        private sealed class PlayerCallState<T>
        {
            public LudeoRoom room;
            public string playerId;
            public Action<T> callback;
        }

        public void CloseRoom(LudeoRoom room, ILudeoRoomWrapper ludeoLudeoWrapper, LudeoHRoom roomHandle, Action<LudeoCloseRoomCallbackData> closeRoomCallback)
        {
            LudeoRoomCloseParams ludeoSessionCloseRoomParams = LudeoRoomCloseParams.Default;

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} calling CloseRoom");

            IntPtr clientData = ToClientData(closeRoomCallback);

            try
            {
                ludeoLudeoWrapper.CloseRoom(roomHandle, ref ludeoSessionCloseRoomParams, clientData, HandleLudeoCloseRoomCallback);
            }
            catch
            {
                FreeClientData(clientData);
                throw;
            }
            finally
            {
                room.Dispose();
            }
        }


        public void AddPlayer(LudeoRoom roomSession, ILudeoRoomWrapper ludeoRoomWrapper,
                                        LudeoHRoom roomHandle, string playerId, Action<LudeoRoomAddPlayerCallbackData> addPlayerCallback)
        {
            IntPtr playerIdAsIntPtr = Marshal.StringToHGlobalAnsi(playerId);

            LudeoRoomAddPlayerParams ludeoRoomAddPlayersParams = new LudeoRoomAddPlayerParams(playerIdAsIntPtr);

            var state = new PlayerCallState<LudeoRoomAddPlayerCallbackData> { room = roomSession, playerId = playerId, callback = addPlayerCallback };
            IntPtr clientData = ToClientData(state);

            try
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} Calling AddPlayer = {playerId}");

                ludeoRoomWrapper.AddPlayer(roomHandle, ref ludeoRoomAddPlayersParams, clientData, HandleAddPlayerCallback);
                return;
            }
            catch (Exception e)
            {
                FreeClientData(clientData);
                LudeoLogManager.LogError($"{LOG_PREFIX} @AddPlayer, exception: {e}");
            }
            finally
            {
                Marshal.FreeHGlobal(playerIdAsIntPtr);
            }

            // The call never reached the core SDK, so no callback is coming. Report the failure here,
            // otherwise the caller (and AddPlayerAsync) waits forever. This runs before AddPlayer returns,
            // so guard it like the native path does: a throwing user callback must not escape AddPlayer.
            try
            {
                roomSession.HandleAddPlayerFail(addPlayerCallback, new LudeoRoomAddPlayerCallbackData { resultCode = LudeoResult.WrapperException });
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} @AddPlayer failure callback, exception: {e}");
            }
        }

        public void RemovePlayer(LudeoRoom ludeoRoom, ILudeoRoomWrapper ludeoRoomWrapper,
                                            LudeoHRoom roomHandle, string playerId, Action<LudeoRoomRemovePlayerCallbackData> removePlayerCallback)
        {
            IntPtr playerIdAsIntPtr = Marshal.StringToHGlobalAnsi(playerId);

            LudeoRoomRemovePlayerParams ludeoRoomRemovePlayerParams = new LudeoRoomRemovePlayerParams(playerIdAsIntPtr);

            var state = new PlayerCallState<LudeoRoomRemovePlayerCallbackData> { room = ludeoRoom, playerId = playerId, callback = removePlayerCallback };
            IntPtr clientData = ToClientData(state);

            try
            {
                LudeoLogManager.LogVerbose($"{LOG_PREFIX} Calling RemovePlayer = {playerId}");

                ludeoRoomWrapper.RemovePlayer(roomHandle, ref ludeoRoomRemovePlayerParams, clientData, HandleRemovePlayerCallback);
            }
            catch
            {
                FreeClientData(clientData);
                throw;
            }
            finally
            {
                Marshal.FreeHGlobal(playerIdAsIntPtr);
            }
        }

        public void ReleaseAllPlayers(ILudeoRoomWrapper ludeoRoomWrapper)
        {
            m_unreleasedPlayer.Clear();
        }

        private static IntPtr ToClientData(object state)
        {
            return GCHandle.ToIntPtr(GCHandle.Alloc(state));
        }

        // Only for a call that threw before reaching the core SDK: its callback will never run to free the handle.
        private static void FreeClientData(IntPtr clientData)
        {
            GCHandle.FromIntPtr(clientData).Free();
        }

        // The core SDK invokes each async callback at most once, so the handle is freed here. It skips the callback
        // when the SDK isn't initialized, or when shutdown drops callbacks still queued; those handles (and the
        // room and user callback they reference) leak. The leak is small and bounded by the calls made then.
        private static T FromClientData<T>(IntPtr clientData) where T : class
        {
            if (clientData == IntPtr.Zero)
                return null;

            GCHandle handle = GCHandle.FromIntPtr(clientData);
            T state = handle.Target as T;
            handle.Free();
            return state;
        }

        [MonoPInvokeCallback(typeof(LudeoSessionCloseRoomCallback))]
        private static void HandleLudeoCloseRoomCallback(ref LudeoRoomCloseCallbackParams nativeOutParams)
        {
            try
            {
                var closeRoomCallback = FromClientData<Action<LudeoCloseRoomCallbackData>>(nativeOutParams.clientData);
                if (closeRoomCallback == null)
                {
                    LudeoLogManager.LogError($"{LOG_PREFIX} CloseRoom callback received without its caller state. Result: {nativeOutParams.resultCode}");
                    return;
                }

                nativeOutParams.ConvertToManaged(out LudeoCloseRoomCallbackData managedData);

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} CloseRoom result: {managedData.resultCode}");

                closeRoomCallback(managedData);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} @HandleLudeoCloseRoomCallback, exception: {e}");
            }
        }


        [MonoPInvokeCallback(typeof(LudeoRoomAddPlayerCallback))]
        private static void HandleAddPlayerCallback(ref LudeoRoomAddPlayerCallbackParams nativeOutParams)
        {
            try
            {
                var state = FromClientData<PlayerCallState<LudeoRoomAddPlayerCallbackData>>(nativeOutParams.clientData);
                if (state == null)
                {
                    LudeoLogManager.LogError($"{LOG_PREFIX} AddPlayer callback received without its caller state. Result: {nativeOutParams.resultCode}");
                    return;
                }

                LudeoLogManager.LogVerbose($"{LOG_PREFIX} AddPlayer result: {nativeOutParams.resultCode}. Adding: {state.playerId}");

                if (nativeOutParams.ConvertToManaged(state.playerId, out LudeoRoomAddPlayerCallbackData managedData))
                {
                    m_unreleasedPlayer.Add(state.playerId);
                    state.room.HandleAddPlayerSuccess(state.callback, managedData);
                }
                else
                    state.room.HandleAddPlayerFail(state.callback, managedData);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX}@HandleAddPlayerCallback, exception: {e}, {e.StackTrace}");
            }
        }

        [MonoPInvokeCallback(typeof(LudeoRoomRemovePlayerCallback))]
        private static void HandleRemovePlayerCallback(ref LudeoRoomRemovePlayerCallbackParams nativeOutParams)
        {
            try
            {
                var state = FromClientData<PlayerCallState<LudeoRoomRemovePlayerCallbackData>>(nativeOutParams.clientData);
                if (state == null)
                {
                    LudeoLogManager.LogError($"{LOG_PREFIX} RemovePlayer callback received without its caller state. Result: {nativeOutParams.resultCode}");
                    return;
                }

                m_unreleasedPlayer.Remove(state.playerId);

                if (nativeOutParams.ConvertToManaged(out LudeoRoomRemovePlayerCallbackData managedData))
                    state.room.HandleRemovePlayerSuccess(state.playerId, state.callback, managedData);
                else
                    state.room.HandleRemovePlayerFail(state.callback, managedData);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX}@HandleRemovePlayerCallback, exception: {e}, {e.StackTrace}");
            }
        }

        [MonoPInvokeCallback(typeof(LudeoRoomRemovePlayerCallback))]
        private static void HandleRemovePlayerCallbackNoAction(ref LudeoRoomRemovePlayerCallbackParams nativeOutParams)
        {

        }
    }
}
/// @endcond INTERNAL
