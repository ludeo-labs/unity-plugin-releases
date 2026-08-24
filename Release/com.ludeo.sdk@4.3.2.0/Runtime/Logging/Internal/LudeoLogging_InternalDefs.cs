/// @cond INTERNAL
namespace LudeoSDK
{
	using System;
    using System.Runtime.InteropServices;

    #region NATIVE TYPES

	[StructLayout(LayoutKind.Sequential)]
	public struct LudeoSetLoggingToFileParams
	{
		public Int32 apiVersion; // LUDEO_SETLOGGINGTOFILE_API_LATEST
		// Set true to enable or false to disable
		public Int32 enable; // LudeoBool
		// If enable is true, then this specifies in what directory should the log file be created.
		public string directory;
	}


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LudeoLogMessage
    {
        public LudeoLogLevel level;
        public LudeoLogCategory category;
        public IntPtr timestamp;

        // UTF-8 log message
        public IntPtr message;
    }
    #endregion NATIVE TYPES

}
/// @endcond INTERNAL
