/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    public interface ILudeoInitWrapper
    {
        LudeoResult LudeoInitialize(ref LudeoInitializeParams parameters);

        void LudeoTick();

        LudeoResult LudeoShutdown();

        void RemoveNotification(ulong notification);

        LudeoResult LudeoCommand(string name, string value);
    }
}
/// @endcond INTERNAL