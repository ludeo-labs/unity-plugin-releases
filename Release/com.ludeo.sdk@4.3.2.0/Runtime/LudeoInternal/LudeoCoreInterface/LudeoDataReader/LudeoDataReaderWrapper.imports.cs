// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
using System;
using System.Runtime.InteropServices;
using static LudeoDataWriter_InternalDefs;

internal partial class LudeoDataReaderWrapper : ILudeoDataReaderWrapper
{
    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern LudeoResult ludeo_DataReader_Release(LudeoHDataReader dataReaderHandle);
        
    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        
    private static extern LudeoResult ludeo_DataReader_GetObjectsInfo(LudeoHDataReader handle, ref LudeoDataReaderGetObjectsInfoParams @params, out IntPtr @out);

	[DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]//, CharSet = CharSet.Ansi)]
    private static extern LudeoResult ludeo_DataReader_GetInfo(LudeoHDataReader dataReaderHandle, ref LudeoDataReaderGetInfoParams dataReaderVisitCallback, out IntPtr dataReaderInfo);
        
    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern void ludeo_DataReaderInfo_Release(IntPtr DatReaderInfoPtr);
        
    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern void ludeo_ObjectsInfo_Release([In] ref LudeoObjectsInfo info);

    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern LudeoResult ludeo_DataReader_SetCurrent(LudeoHDataReader dataReaderHandle);
        
    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern LudeoBool ludeo_DataReader_EnterObject(uint objectId);
        
    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern LudeoBool ludeo_DataReader_LeaveObject();


    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern LudeoBool ludeo_DataReader_EnterComponent([In] string name);

    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern LudeoBool ludeo_DataReader_LeaveComponent();


    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern LudeoBool ludeo_DataReader_GetInt32([In] string name, out Int32 value);
        
    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern LudeoBool ludeo_DataReader_GetFloat(string name, out float value);

    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern LudeoBool ludeo_DataReader_GetDouble([In] string name, out double value);

    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern LudeoBool ludeo_DataReader_GetBool([In] string name, out LudeoBool value);
        
    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    unsafe private static extern LudeoBool ludeo_DataReader_GetString([In] string name, [Out] byte* value);
        
    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern LudeoBool ludeo_DataReader_GetSize([In] string name, out UInt32 value);
        
    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    unsafe private static extern LudeoBool ludeo_DataReader_GetVec3Float([In] string name, [Out] float[] value);
        
    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    unsafe private static extern LudeoBool ludeo_DataReader_GetVec4Float([In] string name, [Out] float[] value);

    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    unsafe private static extern LudeoBool ludeo_DataReader_AttributeExists([In] string attributeName, [Out] LudeoDataType* outType);

    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    unsafe private static extern LudeoResult ludeo_DataReader_VisitAttributes(ref LudeoDataReaderVisitAttributesParams @params, UInt32* outCount, IntPtr clientData, LudeoDataReaderVisitAttributesCallback callback);
       
    [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    unsafe public static extern LudeoBool ludeo_DataReader_GetBlob(string attributeName, [Out] byte* data);
}
}