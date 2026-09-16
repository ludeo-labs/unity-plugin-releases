// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using System.Threading.Tasks;

    /// <summary>
    /// Task-returning overloads of the callback-based gameplay APIs. See <see cref="LudeoSession"/>'s
    /// async section for the threading contract.
    /// </summary>
    public partial class LudeoPlayer
    {
        /// <summary>Awaitable version of <see cref="BeginGameplay"/>.</summary>
        public Task<LudeoPlayerBeginGameplayCallbackData> BeginGameplayAsync()
        {
            var tcs = new TaskCompletionSource<LudeoPlayerBeginGameplayCallbackData>();
            BeginGameplay(data => tcs.TrySetResult(data));
            return tcs.Task;
        }

        /// <summary>Awaitable version of <see cref="EndGameplay"/>.</summary>
        public Task<LudeoPlayerEndGameplayCallbackData> EndGameplayAsync()
        {
            var tcs = new TaskCompletionSource<LudeoPlayerEndGameplayCallbackData>();
            EndGameplay(data => tcs.TrySetResult(data));
            return tcs.Task;
        }

        /// <summary>Awaitable version of <see cref="AbortGameplay"/>.</summary>
        public Task<LudeoPlayerAbortGameplayCallbackData> AbortGameplayAsync()
        {
            var tcs = new TaskCompletionSource<LudeoPlayerAbortGameplayCallbackData>();
            AbortGameplay(data => tcs.TrySetResult(data));
            return tcs.Task;
        }
    }
}
