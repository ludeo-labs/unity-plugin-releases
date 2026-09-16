// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using LudeoSDK.Internal;
    using System;

    /// <summary>
    /// Writes gameplay state and actions into a <see cref="LudeoRoom"/> during the Creator flow.
    /// Obtain it from <see cref="LudeoRoom.Writer"/>:
    /// <code>
    /// var writer = room.Writer;
    /// writer.CreateObject("Enemy", out var enemy);
    /// writer.SendAction(player.PlayerId, "Kill");
    /// </code>
    /// </summary>
    public class LudeoRoomWriter
    {
        internal LudeoRoomWriter() { }

        /// <summary>
        /// Create a <see cref="LudeoWritableObject"/> with the given objectType.
        /// </summary>
        /// <param name="objectType">The type of LudeoWritableObject to create</param>
        /// <param name="ludeoStateObject">The LudeoWritableObject instance created by Ludeo SDK</param>
        /// <returns>LudeoResult.Success or a fail reason LudeoResult</returns>
        /// <exception cref="ArgumentException">when an empty or null objectType was provided</exception>
        public LudeoResult CreateObject(string objectType, out LudeoWritableObject ludeoStateObject)
        {
            return LudeoDataWriter.CreateObject(objectType, out ludeoStateObject);
        }

        /// <summary>
        /// Create or retrieve a <see cref="LudeoWritableObject"/> with the given objectType and objectId.
        /// </summary>
        /// <param name="objectType">The type of LudeoWritableObject to create/retrieve</param>
        /// <param name="objectId">The id of the LudeoWritableObject if it already exists. Fetch it via LudeoWritableObject.ObjectId</param>
        /// <param name="ludeoStateObject">The LudeoWritableObject instance created by Ludeo SDK</param>
        /// <returns>LudeoResult.Success or a fail reason LudeoResult</returns>
        /// <exception cref="ArgumentException">when an empty or null objectType or a negative Id were provided</exception>
        public LudeoResult GetObject(string objectType, uint objectId, out LudeoWritableObject ludeoStateObject)
        {
            return LudeoDataWriter.GetObject(objectType, objectId, out ludeoStateObject);
        }

        /// <summary>
        /// Mark the given state object as destroyed. After this call the object is no longer valid.
        /// </summary>
        /// <param name="ludeoStateObject">The object to destroy.</param>
        /// <returns>LudeoResult.Success or a fail reason LudeoResult</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="ludeoStateObject"/> is null.</exception>
        public LudeoResult DestroyObject(LudeoWritableObject ludeoStateObject)
        {
            if (ludeoStateObject == null)
                throw new ArgumentNullException(nameof(ludeoStateObject));

            return ludeoStateObject.DestroyObject();
        }

        /// <summary>
        /// Sends a game action to the backend, bound to the given player.
        /// Actions have no parameters. They represent a single thing such as "Kill", "HeadShot", etc.
        /// </summary>
        /// <param name="playerId">The player the action is bound to.</param>
        /// <param name="action">The action to report.</param>
        /// <returns>LudeoResult.Success or a fail reason LudeoResult</returns>
        public LudeoResult SendAction(string playerId, string action)
        {
            return LudeoDataWriter.SendAction(playerId, action);
        }

        /// <summary>
        /// Tunes how often captured data is flushed to the backend (send batching). The SDK assumes
        /// sensible defaults, so most games never need to call this.
        /// <para>
        /// Data is only sent as a result of the SDK's internal tick, so the true send rate is also
        /// bounded by the tick frequency. The SDK may enforce a lower bound on the interval.
        /// </para>
        /// <para>
        /// Must be called from the main thread, even when the room's DataWriter is thread-safe.
        /// </para>
        /// </summary>
        /// <param name="sendIntervalMs">Target interval between sends, in milliseconds.</param>
        /// <returns>LudeoResult.Success or a fail reason LudeoResult</returns>
        public LudeoResult SetSendSettings(uint sendIntervalMs)
        {
            return LudeoDataWriter.SetSendSettings(sendIntervalMs);
        }
    }
}
