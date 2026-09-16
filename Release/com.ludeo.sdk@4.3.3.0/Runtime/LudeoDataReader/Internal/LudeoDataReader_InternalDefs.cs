/// @cond INTERNAL

using System;
using System.Runtime.InteropServices;
using static LudeoSDK.Internal.LudeoDataWriter_InternalDefs;

namespace LudeoSDK.Internal
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate LudeoBool LudeoDataReaderVisitAttributesCallback(ref LudeoDataReaderVisitAttributesCallbackParams ludeoDataReaderVisitAttributesCallbackParams);

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoHDataReader
    {
        public static readonly LudeoHDataReader Uninitialized = new LudeoHDataReader(IntPtr.Zero);

        private IntPtr m_Handle;

        public LudeoHDataReader(IntPtr handle) { m_Handle = handle; }

        //  public static implicit operator LudeoHGameplaySession(IntPtr handle) { return new LudeoHGameplaySession(handle); }

        public static bool operator ==(LudeoHDataReader value1, LudeoHDataReader value2) { return value1.m_Handle == value2.m_Handle; }
        public static bool operator !=(LudeoHDataReader value1, LudeoHDataReader value2) { return value1.m_Handle != value2.m_Handle; }
    }

    internal static class LudeoDataReader_InternalDefs
    {
        public static int LUDEO_DATAREADER_GETOBJECTSINFO_API_LATEST = 1;
        public static int LUDEO_DATAREADER_VISITATTRIBUTES_API_LATEST = 1;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoDataReaderGetObjectsInfoParams
    {
        public int apiVersion;

        public static LudeoDataReaderGetObjectsInfoParams Default = new LudeoDataReaderGetObjectsInfoParams() { apiVersion = LudeoDataReader_InternalDefs.LUDEO_DATAREADER_GETOBJECTSINFO_API_LATEST };
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoDataReaderGetInfoParams
    {
        public int apiVersion;
        
        public static LudeoDataReaderGetInfoParams Default = new LudeoDataReaderGetInfoParams() { apiVersion = LudeoDataReader_InternalDefs.LUDEO_DATAREADER_GETOBJECTSINFO_API_LATEST };
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct LudeoDataReaderInfoNative
    {
        public Int32 apiVersion;
        public IntPtr ludeoId;
        public IntPtr playerId;
    }
    
    internal struct LudeoDataReaderInfo
    {
        public Int32 apiVersion;
        public string ludeoId;
        public string playerId;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    internal struct LudeoObjectInfo
    {
        public int apiVersion;
        public uint objectId;
        public string objectType;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoObjectsInfo
    {
        public int apiVersion;  //  Set by the SDK when allocating the struct 
        public IntPtr objects;  // LudeoObjectInfo* objects;
        public uint objectCount;

        public void ConvertToManaged(out LudeoReadableObject[] objectsRecords)
        {
            objectsRecords = new LudeoReadableObject[objectCount];
            IntPtr current = objects;
            for (int i = 0; i < objectCount; i++)
            {
                LudeoObjectInfo data = Marshal.PtrToStructure<LudeoObjectInfo>(current);
                
                LudeoReadableObject managedObjectRecord = new LudeoReadableObject(data.objectId, data.objectType);
                objectsRecords[i] = managedObjectRecord;
                
                current = IntPtr.Add(current, Marshal.SizeOf(typeof(LudeoObjectInfo)));
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoDataReaderVisitAttributesParams
    {
        public Int32 apiVersion;

        public static readonly LudeoDataReaderVisitAttributesParams Default = new LudeoDataReaderVisitAttributesParams() { apiVersion = LudeoDataReader_InternalDefs.LUDEO_DATAREADER_VISITATTRIBUTES_API_LATEST };
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LudeoDataReaderVisitAttributesCallbackParams
    {
        public IntPtr clientData;
        public IntPtr name;
        public LudeoDataType type;
        // The attribute's fully-qualified (dotted) name. Present in the native struct (offset 24,
        // making the struct 32 bytes); must be declared here so the managed layout matches what the
        // core writes, otherwise every field the core places after `type` would be misread.
        public IntPtr fullname;

        internal void ConvertToManaged(out LudeoDataReaderVisitAttributesCallbackData managedData)
        {
            managedData.name = Marshal.PtrToStringAnsi(name);
            managedData.type = type;
        }
    }
}
/// @endcond