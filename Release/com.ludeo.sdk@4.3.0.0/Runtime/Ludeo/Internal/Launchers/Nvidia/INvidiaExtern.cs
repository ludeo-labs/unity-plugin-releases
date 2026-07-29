/// @cond INTERNAL
using System;
using static LudeoSDK.Ludeo.Internal.Launchers.Nvidia.NvidiaDefinitions;

namespace LudeoSDK.Ludeo.Internal.Launchers.Nvidia
{
    public interface INvidiaExtern
    {
        GfnRuntimeError GfnInitializeRuntimeSdk(GfnDisplayLanguage language);

        GfnRuntimeError GfnGetPartnerSecureData(out IntPtr partnerData);

        GfnRuntimeError GfnShutdownSdk();

        GfnRuntimeError GfnSendMessage(string message, uint length);
    }

    internal class DummyNvidiaLauncher : INvidiaExtern
    {

        public GfnRuntimeError GfnGetPartnerSecureData(out IntPtr partnerData)
        {
            partnerData = IntPtr.Zero;
            return GfnRuntimeError.gfnLudeoDummy;
        }

        public GfnRuntimeError GfnInitializeRuntimeSdk(GfnDisplayLanguage language)
        {
            return GfnRuntimeError.gfnLudeoDummy;
        }

        public GfnRuntimeError GfnSendMessage(string message, uint length)
        {
            return GfnRuntimeError.gfnLudeoDummy;
        }

        public GfnRuntimeError GfnShutdownSdk()
        {
            return GfnRuntimeError.gfnLudeoDummy;
        }
    }
}
/// @endcond INTERNAL
