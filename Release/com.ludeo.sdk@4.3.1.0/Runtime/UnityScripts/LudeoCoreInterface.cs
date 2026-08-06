/// @cond INTERNAL
using LudeoSDK.Internal;
using LudeoSDK.MediaCapture;

namespace LudeoSDK.UnityScripts
{
    internal struct UnityWrapperActivationParams
    {
        public string coreDllName;
        public bool isDestoryUnityObjectImmediate;
        public MediaCaptureSupport mediaCaptureSupport;
    }

    internal interface ILudeoCoreInterface
    {
        void Reset();

        UnityWrapperActivationParams GetWrapperActivationParams();

        ILudeoLogWrapper GetLudeoLogWrapper();

        ILudeoInitWrapper GetLudeoInitWrapper();

        ILudeoSessionWrapper GetLudeoSessionWrapper();

        ILudeoDataReaderWrapper GetLudeoDataReaderWrapper();

        ILudeoDataWriterWrapper GetLudeoDataWriterWrapper();

        ILudeoGameplaySessionWrapper GetLudeoGameplaySessionWrapper();

        ILudeoRoomWrapper GetLudeoRoomWrapper();
    }
}
/// @endcond INTERNAL