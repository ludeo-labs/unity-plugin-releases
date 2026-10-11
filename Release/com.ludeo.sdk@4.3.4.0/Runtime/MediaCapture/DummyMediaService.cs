/// @cond INTERNAL
namespace LudeoSDK.MediaCapture
{
    using System;

    internal class DummyMediaService : ICaptureService
    {
        public bool IsInitialized { get; private set; }

        public int BeginCapture(SystemData systemData)
        {
            return -1;
        }

        public int Dispose()
        {
            return -1;
        }

        public int EndCapture(SystemData systemData)
        {
            return -1;
        }

        public void Initialize(SystemData systemData, string localCaptureDirectory, Action<CaptureStatus> onCaptureStatusUpdate)
        {
            IsInitialized = true;
            onCaptureStatusUpdate(CaptureStatus.InitSuccess);
        }

        public int TagCaptureWithHighlightData(string highlightId, string uploadUrl)
        {
            return -1;
        }


        public void CleanUp() { }

        public void UpdateFrames(float unscaledDeltaTime) { }
    }
}
/// @endcond INTERNAL