/// @cond INTERNAL
using System;

namespace LudeoSDK.Ludeo.Internal.Launchers.Gameface
{
    public interface IGamefaceExtern
    {
        string GetCloudToken();
    }

    internal class DummyGamefaceLauncher : IGamefaceExtern
    {
        public string GetCloudToken()
        {
            return string.Empty;
        }
    }
}
/// @endcond INTERNAL
