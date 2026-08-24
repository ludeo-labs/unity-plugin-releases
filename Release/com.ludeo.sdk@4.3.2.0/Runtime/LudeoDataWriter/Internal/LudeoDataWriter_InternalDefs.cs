//@cond INTERNAL
namespace LudeoSDK.Internal
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoHDataWriter
    {
        public static readonly LudeoHDataWriter Uninitialized = new LudeoHDataWriter(IntPtr.Zero);

        private IntPtr m_Handle;

        public LudeoHDataWriter(IntPtr handle) { m_Handle = handle; }

        //   public static implicit operator LudeoHDataWriter(IntPtr handle) { return new LudeoHDataWriter(handle); }

        public static bool operator ==(LudeoHDataWriter value1, LudeoHDataWriter value2) { return value1.m_Handle == value2.m_Handle; }
        public static bool operator !=(LudeoHDataWriter value1, LudeoHDataWriter value2) { return value1.m_Handle != value2.m_Handle; }
    }

    public static class LudeoDataWriter_InternalDefs
    {
        internal const int LUDEO_SET_STATE_API_LATEST = 1;
        internal const int LUDEO_CREATE_OBJECT_LATEST = 1;
        internal const int LUDEO_DESTROY_OBJECT_LATEST = 1;
        internal const int LUDEO_DATAWRITER_SENDACTION_API_LATEST = 1;
        internal const int LUDEO_DATAWRITER_SETSENDSETTINGS_API_LATEST = 1;

        //Generics
        public static T GetAs<T>(IntPtr Pointer) where T : unmanaged
        {
            return Marshal.PtrToStructure<T>(Pointer);
        }

        public static void SetAs<T>(T value, out IntPtr Pointer) where T : unmanaged
        {
            Pointer = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
            Marshal.StructureToPtr(value, Pointer, false);
        }
        
        public static void Free(IntPtr Pointer)
        {
            Marshal.FreeHGlobal(Pointer);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LudeoBool
        {
            public static readonly LudeoBool LUDEO_TRUE = new LudeoBool(1);
            public static readonly LudeoBool LUDEO_FALSE = new LudeoBool(0);
            
            private int value;

            private LudeoBool(int value)
            {
                this.value = value;
            }

            public static implicit operator bool(LudeoBool myBool)
            {
                return myBool.value != 0;
            }
            
            public static implicit operator LudeoBool(bool myBool)
            {
                return myBool ? LUDEO_TRUE : LUDEO_FALSE;
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct LudeoDataWriterCreateObjectParams
        {
            public int apiVersion;
            public string objectTypeName;

            public LudeoDataWriterCreateObjectParams(string objectTypeName)
            {
                this.apiVersion = LUDEO_CREATE_OBJECT_LATEST;
                this.objectTypeName = objectTypeName;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LudeoDataWriterDestroyObjectParams
        {
            public int apiVersion;
            public uint objectId;

            public LudeoDataWriterDestroyObjectParams(uint objectId)
            {
                apiVersion = LUDEO_DESTROY_OBJECT_LATEST;
                this.objectId = objectId;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LudeoDataWriterSendActionParams
        {
            public int apiVersion;

            public string player;

            public string action;

            public LudeoDataWriterSendActionParams(string player, string action)
            {
                apiVersion = LUDEO_DATAWRITER_SENDACTION_API_LATEST;
                this.player = player;
                this.action = action;
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct LudeoDataWriterSetSendSettingsParams
        {
            public int apiVersion;
            public uint sendIntervalMs;

            public LudeoDataWriterSetSendSettingsParams(uint sendIntervalMs)
            {
                apiVersion = LUDEO_DATAWRITER_SETSENDSETTINGS_API_LATEST;
                this.sendIntervalMs = sendIntervalMs;
            }
        }
    }

}
//@endcond INTERNAL