// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    using System;
    using static LudeoDataWriter_InternalDefs;

    internal partial class LudeoDataReaderWrapper : ILudeoDataReaderWrapper
    {
        public LudeoResult GetObjectsInfo (LudeoHDataReader dataReaderHandle, ref LudeoDataReaderGetObjectsInfoParams dataReaderGetObjectsInfoParams, out IntPtr ludeoObjectsInfo)
        {           
            return ludeo_DataReader_GetObjectsInfo(dataReaderHandle, ref dataReaderGetObjectsInfoParams, out ludeoObjectsInfo);
        }

        public void ObjectsInfoRelease(ref LudeoObjectsInfo info)
        {
            ludeo_ObjectsInfo_Release(ref info);
        }

        public LudeoResult SetCurrent(LudeoHDataReader dataReaderHandle)
        {
            return ludeo_DataReader_SetCurrent(dataReaderHandle);
        }

        public LudeoBool EnterObject(uint objectId)
        {
            return ludeo_DataReader_EnterObject(objectId);
        }

        public LudeoBool LeaveObject()
        {
            return ludeo_DataReader_LeaveObject();
        }

        public LudeoBool EnterComponent(string name)
        {
            return ludeo_DataReader_EnterComponent(name);
        }

        public LudeoBool LeaveComponent()
        {
            return ludeo_DataReader_LeaveComponent();
        }

        public LudeoResult DataReaderRelease(LudeoHDataReader dataReaderHandle)
        {
            return ludeo_DataReader_Release(dataReaderHandle);
        }

        public LudeoResult GetInfo(LudeoHDataReader dataReaderHandle, ref LudeoDataReaderGetInfoParams dataReaderVisitCallback, out IntPtr dataReaderInfo)
        {
            return ludeo_DataReader_GetInfo(dataReaderHandle, ref dataReaderVisitCallback, out dataReaderInfo);
        }

        public void ReleaseInfo(ref IntPtr info)
        {
            ludeo_DataReaderInfo_Release(info);
        }
        
        #region -------- Getters --------

        public LudeoBool GetInt(string name, out Int32 value)
        {
            return ludeo_DataReader_GetInt32(name, out value);
        }

        public LudeoBool GetFloat(string name, out float value)
        {
            return ludeo_DataReader_GetFloat(name, out value);
        }

        public LudeoBool GetDouble(string name, out double value)
        {
            return ludeo_DataReader_GetDouble(name, out value);
        }

        public LudeoBool GetBool(string name, out LudeoBool value)
        {
            return ludeo_DataReader_GetBool(name, out value);
        }

        unsafe public LudeoBool GetString(string name, byte* value)
        {
            return ludeo_DataReader_GetString(name, value);
        }

        unsafe public LudeoBool GetVector3(string name, out float[] value)
        {
            value = new float[3];
            return ludeo_DataReader_GetVec3Float(name, value);
        }

        unsafe public LudeoBool GetQuaternion(string name, out float[] value)
        {
            value = new float[4];
            return ludeo_DataReader_GetVec4Float(name, value);
        }

        public LudeoBool GetSize(string name, out uint size)
        {
            return ludeo_DataReader_GetSize(name, out size);
        }
        
        unsafe public LudeoBool GetBlob(string name, out byte[] data)
        {
            fixed(byte* dataPtr = data)
            {
                return ludeo_DataReader_GetBlob(name, dataPtr);               
            }
        }

        #endregion -------- Getters --------

        unsafe public LudeoBool IsAttributeExists(string name, out LudeoDataType typeOfAttribute)
        {
            LudeoDataType* ludeoDataType = stackalloc LudeoDataType[1];
            LudeoBool result = ludeo_DataReader_AttributeExists(name, ludeoDataType);
            typeOfAttribute = *ludeoDataType;
            return result;
        }

        unsafe public LudeoResult GetAttributesOfCurrentObject(ref LudeoDataReaderVisitAttributesParams attributesParams, UInt32* outCount, IntPtr clientData, LudeoDataReaderVisitAttributesCallback callback)
        {
            return ludeo_DataReader_VisitAttributes(ref attributesParams, outCount, clientData, callback);
        }
    }
}