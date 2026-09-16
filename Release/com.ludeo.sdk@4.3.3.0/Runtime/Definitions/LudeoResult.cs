// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using System;

    /// <summary>Ludeo operation result codes. You should check for Success when accessing the opertion results.</summary>
    public enum LudeoResult : Int32
    {
        /// <summary>Ludeo operation successful</summary>
        Success = 0,

        /// <summary>An invalid apiVersion was passed to a function call</summary>
        InvalidVersion = 1,

        /// <summary>An invalid parameter was passed to a function</summary>
        InvalidParameters = 2,

        /// <summary>Invalid authentication details. E.g: The wrong API key was specified, or authentication failed (either the one explicitly specified or automatic auth)</summary>
        InvalidAuth = 3,

        /// <summary>A resource was not found. E.g: The client passed an invalid ludeo Id to ludeo_Session_GetLudeo</summary>
        NotFound = 4,

        /// <summary>Operation timed out. E.g, a network operation timed out.</summary>
        TimedOut = 5,

        /// <summary>An error that we cannot identify has occurred. Check the logs for more info.</summary>
        Unknown = 6,

        /// <summary>The operation failed because the relevant instance was not in a state valid to perform the request operation.
        /// This can be for example trying to initialize a system which is already initialized, shutting down when it's not initialized,
        /// or the SDK is not initialized.</summary>
        WrongState = 7,

        /// <summary>The SDK was disabled by the backend
        /// To allow disabling the use of the SDK without shipping a new game, the SDK can be disabled by the backend. This can be useful
        /// if problems are found after a game ships.
        /// Whenever a session creation or ongoing session fails with this error, the client should stop further attempts to create sessions.</summary>
        SDKDisabled = 8,

        /// <summary>An operation failed because something went wrong establishing a network connection</summary>
        NetworkError = 9,

        /// <summary>An operation was canceled.
        /// One example of when this can happen is when pending operation are canceled because the SDK is shutting down</summary>
        Canceled = 10,

        /// <summary>Indicates the SDK received invalid configuration from the backend.</summary>
        InvalidConfiguration = 11,

        /// <summary>Something the client requested or provided is of the wrong type.</summary>
        WrongType = 12,

        /// <summary>The call failed because the SDK encountered invalid data.
        /// A situation like this can happen is if the SDK Ludeo data from the backend that is invalid somehow. E.g, problems related to the
        /// data and object types the client previously registered.</summary>
        InvalidData = 13,
        
        // Wrapper enums
        /// <summary>The native dll of Ludeo is not detected or missing.</summary>
        WrapperDllNotFound = Int32.MaxValue / 2,

        /// <summary>Trying to access functionality that requires LudeoInit before init</summary>
        LudeoNotYetInit,

        /// <summary>LudeoManager tried to re-initialize</summary>
        LudeoManagerAlreadyInitialized,

        /// <summary>LudeoManager is disposed</summary>
        LudeoManagerAlreadyDisposed,

        /// <summary>A general error in the wrapper</summary>
        WrapperException,

        /// <summary>The LudeoPlayer you tried to access does not exist</summary>
        GameSessionNotFound,

        /// <summary>General Media capturing service error</summary>
        CaptureServiceError,

        /// <summary>Failed to initialize the Media capturing service</summary>
        CaptureServiceInitFailed,
    }
}
