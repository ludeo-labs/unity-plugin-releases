/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using System;

    public interface INativeHelperMethods
    {
        string GetAuthToken(LudeoHSession sessionHandle);

        string GetLastHighlightUrl(LudeoHSession sessionHandle);

        string GetLastHighlightId(LudeoHSession sessionHandle);
    }

    internal class DummyNativeHelperMethods : INativeHelperMethods
    {
        public string GetAuthToken(LudeoHSession sessionHandle)
        {
            return "DummyToken";
        }

        public string GetLastHighlightId(LudeoHSession sessionHandle)
        {
            return "DummyHighlightId";
        }

        public string GetLastHighlightUrl(LudeoHSession sessionHandle)
        {
            return "DummyHighlightUrl";
        }
    }
}
/// @endcond INTERNAL