/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    public interface ILudeoLogWrapper
    {
        LudeoResult SetLogLevel(LudeoLogCategory category, LudeoLogLevel level);
        
        LudeoResult SetLogCallback(LudeoLogCallback callback);

        LudeoResult SetLoggingFile(ref LudeoSetLoggingToFileParams logToFileParams);
    }
}
/// @endcond INTERNAL
