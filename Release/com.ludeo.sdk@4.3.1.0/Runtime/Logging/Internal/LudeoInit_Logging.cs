/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    internal static partial class LudeoInit
    {
        public static LudeoResult SetLoggingCallback(ILudeoLogWrapper ludeoLogWrapper, LudeoLogCallback callback)
        {
            return ludeoLogWrapper.SetLogCallback(callback);
        }

        internal static LudeoResult SetLoggingFile(ILudeoLogWrapper m_logWrapper, bool logToFile, string logDirectory)
        {
            LudeoSetLoggingToFileParams logToFileParams = new LudeoSetLoggingToFileParams
            {
                apiVersion = 0,
                enable = logToFile ? 1 : 0,
                directory = logDirectory
            };
            return m_logWrapper.SetLoggingFile(ref logToFileParams);
        }

        internal static LudeoResult SetLogLevel(ILudeoLogWrapper m_logWrapper, LudeoLogCategory category, LudeoLogLevel logLevel)
        {
            return m_logWrapper.SetLogLevel(category, logLevel);
        }
    }
}
/// @endcond INTERNAL