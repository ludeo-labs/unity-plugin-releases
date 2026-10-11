// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using System.Threading.Tasks;

    /// <summary>
    /// Task-returning overloads of the callback-based room APIs. See <see cref="LudeoSession"/>'s
    /// async section for the threading contract.
    /// </summary>
    public partial class LudeoRoom
    {
        /// <summary>Awaitable version of <see cref="AddPlayer"/>.</summary>
        public Task<LudeoRoomAddPlayerCallbackData> AddPlayerAsync(LudeoRoomAddPlayerParameters addPlayerParameters)
        {
            var tcs = new TaskCompletionSource<LudeoRoomAddPlayerCallbackData>();
            AddPlayer(addPlayerParameters, data => tcs.TrySetResult(data));
            return tcs.Task;
        }

        /// <summary>Awaitable version of <see cref="RemovePlayer"/>.</summary>
        public Task<LudeoRoomRemovePlayerCallbackData> RemovePlayerAsync(LudeoRoomRemovePlayerParameters removePlayerParameters)
        {
            var tcs = new TaskCompletionSource<LudeoRoomRemovePlayerCallbackData>();
            RemovePlayer(removePlayerParameters, data => tcs.TrySetResult(data));
            return tcs.Task;
        }

        /// <summary>Awaitable version of <see cref="CloseRoom"/>.</summary>
        public Task<LudeoCloseRoomCallbackData> CloseRoomAsync()
        {
            var tcs = new TaskCompletionSource<LudeoCloseRoomCallbackData>();
            CloseRoom(data => tcs.TrySetResult(data));
            return tcs.Task;
        }
    }
}
