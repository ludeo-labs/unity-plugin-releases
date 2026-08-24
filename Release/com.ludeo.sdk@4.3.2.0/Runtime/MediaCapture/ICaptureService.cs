/// @cond INTERNAL
namespace LudeoSDK.MediaCapture
{
    using System;

    interface ICaptureService
    {
        bool IsInitialized { get; }

        void Initialize(SystemData systemData, string localCaptureDirectory, Action<CaptureStatus> onCaptureStatusUpdate);

        int BeginCapture(SystemData systemData);

        int TagCaptureWithHighlightData(string highlightId, string uploadUrl);

        int EndCapture(SystemData systemData);

        int Dispose();

        void UpdateFrames(float unscaledDeltaTime);

        void CleanUp();
    }
}
/// @endcond INTERNAL