/// @cond INTERNAL
using System;

namespace LudeoSDK.MediaCapture
{
    [Serializable]
    internal enum MediaCaptureSupport
    {
        None,
        Null = None,
        Undefined = None,
        Dummy = 1,
        Screenshots = 2,
        Video = 3,
        NewVideo = 4,
    }

    [Serializable]
    internal enum CaptureStatus
    {
        Uninitialized = 0,
        InitSuccess = 1,    // Capture service is ready to StartCapture when Core requires
        ProcessFailed = 2,  // The process that collectes or/and does the capture failed
        GeneralError = 3,
    }
}
/// @endcond INTERNAL