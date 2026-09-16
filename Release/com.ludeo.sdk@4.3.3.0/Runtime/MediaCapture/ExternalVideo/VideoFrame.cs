/// @cond INTERNAL
using Unity.Collections;

namespace LudeoSDK.MediaCapture
{
    internal class VideoFrame
    {
        public int FrameNumber { get; set; }
        public NativeArray<byte> Image { get; set; }
    }
}
/// @endcond INTERNAL