// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Task-returning overloads of the callback-based session APIs. The awaited continuation resumes on
    /// Unity's main-thread synchronization context (as long as it is awaited from the main thread),
    /// so it is safe to touch Unity objects after the await.
    /// </summary>
    public partial class LudeoSession
    {
        /// <summary>Awaitable version of <see cref="Activate"/>.</summary>
        public Task<LudeoSessionActivateCallbackData> ActivateAsync(LudeoSessionSetLocalizationParameters ludeoSessionLocalizationData = default)
        {
            var tcs = new TaskCompletionSource<LudeoSessionActivateCallbackData>();
            Activate(data => tcs.TrySetResult(data), ludeoSessionLocalizationData);
            return tcs.Task;
        }

        /// <summary>Awaitable version of <see cref="OpenRoom"/>.</summary>
        public Task<LudeoOpenRoomCallbackData> OpenRoomAsync(LudeoSessionOpenRoomParameters openRoomData)
        {
            var tcs = new TaskCompletionSource<LudeoOpenRoomCallbackData>();
            OpenRoom(openRoomData, data => tcs.TrySetResult(data));
            return tcs.Task;
        }

        /// <summary>Awaitable version of <see cref="GetLudeo"/>.</summary>
        public Task<LudeoGetLudeoCallbackData> GetLudeoAsync(Guid ludeoID)
        {
            var tcs = new TaskCompletionSource<LudeoGetLudeoCallbackData>();
            GetLudeo(ludeoID, data => tcs.TrySetResult(data));
            return tcs.Task;
        }
    }
}
