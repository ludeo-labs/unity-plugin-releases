// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    /// <summary>
    /// Creates Ludeo sessions. Obtain it via <see cref="LudeoManager.SessionManager"/>.
    /// <code>
    /// LudeoManager.Initialize();
    /// LudeoManager.SessionManager.CreateSession(out var session);
    /// </code>
    /// </summary>
    public class LudeoSessionManager
    {
        internal LudeoSessionManager() { }

        /// <summary>
        /// Creates a Ludeo session. <see cref="LudeoManager.Initialize"/> must have been called
        /// successfully first. A session represents a logged-in Ludeo user and their connection to
        /// the backend. After it succeeds, subscribe to the session's events and call
        /// <see cref="LudeoSession.Activate"/> to connect to the backend.
        /// </summary>
        /// <param name="ludeoSession">The newly created session, or <c>null</c> if creation failed.</param>
        /// <returns>One of several <see cref="LudeoResult"/> values.</returns>
        public LudeoResult CreateSession(out LudeoSession ludeoSession)
        {
            return LudeoManager.DoCreateSession(out ludeoSession);
        }
    }
}
