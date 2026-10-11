// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
#if !UNITY_STANDALONE_WIN && !UNITY_ANDROID
    using System;
    using System.Runtime.InteropServices;
    using static LudeoDataWriter_InternalDefs;

    internal partial class LudeoDataReaderWrapper : ILudeoDataReaderWrapper
    {
        private static LudeoResult ludeo_DataReader_Release(LudeoHDataReader dataReaderHandle)
        {
            return LudeoResult.SDKDisabled;
        }
        
        private static LudeoResult ludeo_DataReader_GetObjectsInfo(LudeoHDataReader handle, ref LudeoDataReaderGetObjectsInfoParams @params, out IntPtr @out)
        {
            @out = IntPtr.Zero;
            return LudeoResult.SDKDisabled;
        }
        
        private static  LudeoResult ludeo_DataReader_GetInfo(LudeoHDataReader dataReaderHandle, ref LudeoDataReaderGetInfoParams dataReaderVisitCallback, ref IntPtr dataReaderInfo)
        {
            return LudeoResult.SDKDisabled;
        }

        private static void ludeo_DataReaderInfo_Release(IntPtr DatReaderInfoPtr)
        {
        }

        private static void ludeo_ObjectsInfo_Release([In] ref LudeoObjectsInfo info)
        {
        }

        private static  LudeoResult ludeo_DataReader_SetCurrent(LudeoHDataReader dataReaderHandle)
        {
            return LudeoResult.SDKDisabled;
        }
        
        private static  LudeoBool ludeo_DataReader_EnterObject(uint objectId)
        {
            return LudeoBool.LUDEO_FALSE;
        }
        
        private static  LudeoBool ludeo_DataReader_LeaveObject()
        {
            return LudeoBool.LUDEO_FALSE;
        }

        
        private static  LudeoBool ludeo_DataReader_EnterComponent(string name)
        {
            return LudeoBool.LUDEO_FALSE;
        }
        
        private static  LudeoBool ludeo_DataReader_LeaveComponent()
        {
            return LudeoBool.LUDEO_FALSE;
        }


        private static LudeoBool ludeo_DataReader_GetInt32(string name, out Int32 value)
        {
            value = 0;
            return LudeoBool.LUDEO_FALSE;
        }


        private static LudeoBool ludeo_DataReader_GetFloat(string name, out float value)
        {
            value = 0;
            return LudeoBool.LUDEO_FALSE;
        }

        
        private static  LudeoBool ludeo_DataReader_GetDouble(string name, out double value)
        {
            value = 0;
            return LudeoBool.LUDEO_FALSE;
        }
        
        private static  LudeoBool ludeo_DataReader_GetBool(string name, out LudeoBool value)
        {
            value = LudeoBool.LUDEO_FALSE;
            return LudeoBool.LUDEO_FALSE;
        }
        
        unsafe private static  LudeoBool ludeo_DataReader_GetString(string name, byte* value)
        {
            return LudeoBool.LUDEO_FALSE;
        }
        
        private static  LudeoBool ludeo_DataReader_GetSize(string name, out UInt32 value)
        {
            value = 0;
            return LudeoBool.LUDEO_FALSE;
        }
        
        unsafe private static  LudeoBool ludeo_DataReader_GetVec3Float(string name, [Out] float[] value)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        unsafe private static LudeoBool ludeo_DataReader_GetVec4Float(string name, [Out] float[] value)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        unsafe public static LudeoBool ludeo_DataReader_AttributeExists([In] string attributeName, [Out] LudeoDataType* outType)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        unsafe private static LudeoResult ludeo_DataReader_VisitAttributes(ref LudeoDataReaderVisitAttributesParams @params, UInt32* outCount, IntPtr clientData, LudeoDataReaderVisitAttributesCallback callback)
        {
            return LudeoResult.LudeoNotYetInit;
        }

        unsafe public static LudeoBool ludeo_DataReader_GetBlob(string attributeName, [Out] byte* data)
        {
            return LudeoBool.LUDEO_FALSE;
        }
    }
#endif
}