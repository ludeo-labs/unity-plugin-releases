/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using System;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr LudeoMemoryAllocFunc();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr LudeoMemoryReallocFunc();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoMemoryFreeFunc();
    
    internal static class Ludeo_InternalDefs
    {
        public const Int32 LUDEO_INITIALIZE_API_LATEST = 1; // LUDEO_INITIALIZE_API_LATEST from InitTypes.h  
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoInitializeParams
    {
        public Int32 apiVersion;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public LudeoMemoryAllocFunc memoryAllocFunc; // NOT PRODUCTION READY. !!!! Add attribute on handler: [MonoPInvokeCallback(typeof(LudeoMemoryAllocFunc))]

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public LudeoMemoryReallocFunc memoryReallocFunc;  // NOT PRODUCTION READY !!!! Add attribute on handler: [MonoPInvokeCallback(typeof(LudeoMemoryReallocFunc))]

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public LudeoMemoryFreeFunc memoryFreeFunc; // NOT PRODUCTION READY !!!! Add attribute on handler: [MonoPInvokeCallback(typeof(LudeoMemoryFreeFunc))]

        // Optional logging callback set at init. Null = ignored by the core (the plugin sets logging
        // via ludeo_SetLoggingCallback separately). Must be here to match the native LudeoInitializeParams
        // layout (InitTypes.h): apiVersion, 3x memory funcs, loggingCallback, loggingToFileParams; 48 bytes.
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public LudeoLogCallback loggingCallback;

        // Optional pointer to a LudeoSetLoggingToFileParams. IntPtr.Zero = ignored by the core.
        public IntPtr loggingToFileParams;

        public LudeoInitializeParams(Int32 apiVersion)
        {
            this.apiVersion = apiVersion;
            this.memoryAllocFunc = null;
            this.memoryReallocFunc = null;
            this.memoryFreeFunc = null;
            this.loggingCallback = null;
            this.loggingToFileParams = IntPtr.Zero;
        }

        public static readonly LudeoInitializeParams Default = new LudeoInitializeParams(Ludeo_InternalDefs.LUDEO_INITIALIZE_API_LATEST);
    }
}
/// @endcond INTERNAL