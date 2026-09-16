/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoHGameplaySession
    {
        public static readonly LudeoHGameplaySession Uninitialized = new LudeoHGameplaySession(IntPtr.Zero);

        private IntPtr m_Handle;

        public LudeoHGameplaySession(IntPtr handle) { m_Handle = handle; }

        //  public static implicit operator LudeoHGameplaySession(IntPtr handle) { return new LudeoHGameplaySession(handle); }

        public static bool operator ==(LudeoHGameplaySession value1, LudeoHGameplaySession value2) { return value1.m_Handle == value2.m_Handle; }
        public static bool operator !=(LudeoHGameplaySession value1, LudeoHGameplaySession value2) { return value1.m_Handle != value2.m_Handle; }
    }

    internal class LudeoGameplaySession_InternalDefs
    {
        public const string LOG_PREFIX = "[LudeoGameplaySession]";
        public const Int32 LUDEO_GAMEPLAYSESSION_BEGIN_API_LATEST = 1; // LUDEO_GAMEPLAYSESSION_BEGIN_API_LATEST from GameplaySessionTypes.h
        public const Int32 LUDEO_GAMEPLAYSESSION_END_API_LATEST = 1; // LUDEO_GAMEPLAYSESSION_END_API_LATEST from GameplaySessionTypes.h
        public const Int32 LUDEO_GAMEPLAYSESSION_MARKHIGHLIGHT_API_LATEST = 1; // LUDEO_GAMEPLAYSESSION_MARKHIGHLIGHT_API_LATEST from GameplaySessionTypes.h
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoGameplaySessionBeginCallback(ref LudeoSessionBeginGameplayCallbackParams ludeoSessionBeginGameplayCallbackParams);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoGameplaySessionEndCallback(ref LudeoGameplaySessionEndCallbackParams ludeoSessionEndGameplayCallbackParams);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionMarkHighlightCallback(ref LudeoSessionMarkHighlightCallbackParams ludeoSessionMarkHighlightCallbackParams);

    
    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoGameplaySessionBeginParams
    {
        public int apiVersion;

        public static LudeoGameplaySessionBeginParams Default = new LudeoGameplaySessionBeginParams { apiVersion = LudeoGameplaySession_InternalDefs.LUDEO_GAMEPLAYSESSION_BEGIN_API_LATEST };
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoGameplaySessionEndParams
    {
        public int apiVersion;
        public LudeoDataWriter_InternalDefs.LudeoBool isAborted;

        public LudeoGameplaySessionEndParams(bool isAborted)
        {
            apiVersion = LudeoGameplaySession_InternalDefs.LUDEO_GAMEPLAYSESSION_END_API_LATEST;
            this.isAborted = isAborted;
        }
    }
    

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionMarkHighlightParams
    {
        public int apiVersion;

        public static LudeoSessionMarkHighlightParams Default = new LudeoSessionMarkHighlightParams { apiVersion = LudeoGameplaySession_InternalDefs.LUDEO_GAMEPLAYSESSION_MARKHIGHLIGHT_API_LATEST };
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionMarkHighlightCallbackParams
    {
        public LudeoResult resultCode;
        public IntPtr clientData;

        internal void ConvertToManaged(out LudeoGameplaySessionMarkHighlightCallbackData managedData)
        {
            managedData.resultCode = resultCode;
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionBeginGameplayCallbackParams
    {
        public LudeoResult resultCode;
        public IntPtr clientData;

        internal void ConvertToManaged(out LudeoPlayerBeginGameplayCallbackData managedData)
        {
            managedData.resultCode = resultCode;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoGameplaySessionEndCallbackParams
    {
        public LudeoResult resultCode;
        public IntPtr clientData;

        internal void ConvertToManaged(out LudeoPlayerEndGameplayCallbackData managedData)
        {
            managedData.resultCode = resultCode;
        }

        internal void ConvertToManaged(out LudeoPlayerAbortGameplayCallbackData managedData)
        {
            managedData.resultCode = resultCode;
        }
    }
}
/// @endcond INTERNAL