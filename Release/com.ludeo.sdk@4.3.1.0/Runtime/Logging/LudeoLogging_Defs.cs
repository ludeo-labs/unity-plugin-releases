// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using System;
    using System.Runtime.InteropServices;

	[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	public delegate void LudeoLogCallback(ref LudeoLogMessage data);

    #region NATIVE TYPES
    /// <summary>
    /// Ludeo Log Level
    /// </summary>
    /// 
    public enum LudeoLogLevel
	{
		/// <summary>Value used to turn off logging</summary>
		Off = 0,

		/// <summary>
		///  Some fatal error or assert has happened. A log of this level means something critical has failed or an SDK assert has
		///  triggered. This should not occur at all. If it does, it's most likely a bug in the SDK.
		/// </summary>
		Fatal = 1,

		/// <summary>
		/// A non-fatal error has occurred.
		/// Logs of this level normally indicate why an API call has failed, or why some other non-fatal internal code has failed.
		/// </summary>
		Error = 2,

		/// <summary>Warning log level</summary>
		Warning = 3,

		/// <summary>Normal log level</summary>
		Log = 4,

		/// <summary>Detailed log level</summary>
		Verbose = 5, // TODO: - Verbose log summary

		/// <summary>All logs level. Requires development dll.</summary>
		VeryVerbose = 6, 
	}

	/// <summary>
	/// Ludeo Log Category
	/// </summary>
	public enum LudeoLogCategory : Int32
    {
		/// <summary>Log category used for any logging not related to a specific interface or service</summary>
		Core = 0,
		/// <summary>Log category used for any logging related to the session interface</summary>
		Session = 1,
		/// <summary>Log category for Http communications</summary>
		Http = 2,
		/// <summary>Log category for writing and reading gameplaydata</summary>
		Data = 3,
		/// <summary> Log category for any logging related to rooms</summary>
		Room = 4,
		/// <summary> Log category for any logging related to the overlay </summary>
		Overlay = 5,

		/// <summary> Log category for the logging coming from Coherent's gameface </summary>
		Coherent = 6,

        /// <summary> Log category for the logging related to video encoding </summary>
        VideoEncoding = 7,

		/// <summary>Not a real category. This is a special value that can be passed to Ludeo.SetLoggingLevel</summary>
		All = 0x7FFFFFFF,
    }

	#endregion NATIVE TYPES
	
}
