// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    using static LudeoDataWriter_InternalDefs;

    internal partial class LudeoDataWriterWrapper : ILudeoDataWriterWrapper
    {
        public LudeoResult SetSendSettings(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterSetSendSettingsParams ludeoDataWriterSetSendSettingsParams)
        {
            return ludeo_DataWriter_SetSendSettings(dataWriterHandle, ref ludeoDataWriterSetSendSettingsParams);
        }

        public LudeoResult CreateObject(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterCreateObjectParams ludeoDataWriterCreateObjectParams, ref uint objectId)
        {
            return ludeo_DataWriter_CreateObject(dataWriterHandle, ref ludeoDataWriterCreateObjectParams, ref objectId);
        }

        public LudeoResult SetCurrent(LudeoHDataWriter dataWriterHandle)
        {
            return ludeo_DataWriter_SetCurrent(dataWriterHandle);
        }

        public LudeoResult DestroyObject(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterDestroyObjectParams ludeoDataWriterDestroyObjectParams)
        {
            return ludeo_DataWriter_DestroyObject(dataWriterHandle, ref ludeoDataWriterDestroyObjectParams);
        }

        public LudeoBool EnterObject(uint objectId)
        {
            return ludeo_DataWriter_EnterObject(objectId);
        }

        public LudeoBool LeaveObject()
        {
            return ludeo_DataWriter_LeaveObject();
        }

        public LudeoBool EnterComponent(string attributeName)
        {
            return ludeo_DataWriter_EnterComponent(attributeName);
        }

        public LudeoBool LeaveComponent()
        {
            return ludeo_DataWriter_LeaveComponent();
        }
        
        public LudeoBool SetPlayerBinding(string playerId)
        {
            return ludeo_DataWriter_SetPlayerBinding(playerId);
        }

        public LudeoResult SendAction(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterSendActionParams sendActionParams)
        {
            return ludeo_DataWriter_SendAction(dataWriterHandle, ref sendActionParams);
        }

        public LudeoBool SetInt8(string attributeName, sbyte value)
        {
            return ludeo_DataWriter_SetInt8(attributeName, value);
        }

        public LudeoBool SetInt16(string attributeName, short value)
        {
            return ludeo_DataWriter_SetInt16(attributeName, value);
        }

        public LudeoBool SetInt32(string attributeName, int value)
        {
            return ludeo_DataWriter_SetInt32(attributeName, value);
        }

        public LudeoBool SetInt64(string attributeName, long value)
        {
            return ludeo_DataWriter_SetInt64(attributeName, value);
        }

        public LudeoBool SetBool(string attributeName, LudeoBool value)
        {
            return ludeo_DataWriter_SetBool(attributeName, value);
        }

        public LudeoBool SetFloat(string attributeName, float value)
        {
            return ludeo_DataWriter_SetFloat(attributeName, value);
        }

        public LudeoBool SetDouble(string attributeName, double value)
        {
            return ludeo_DataWriter_SetDouble(attributeName, value);
        }

        unsafe public LudeoBool SetVec3Float(string attributeName, float[] value)
        {
            return ludeo_DataWriter_SetVec3Float(attributeName, value);
        }

        unsafe public LudeoBool SetVec4Float(string attributeName, float[] value)
        {
            return ludeo_DataWriter_SetVec4Float(attributeName, value);
        }

        public LudeoBool SetString(string attributeName, string value)
        {
            return ludeo_DataWriter_SetString(attributeName, value);
        }

        unsafe public LudeoBool SetBlob(string attributeName, byte[] data, uint size)
        {
            fixed (byte* dataPtr = data)
            {
                return ludeo_DataWriter_SetBlob(attributeName, dataPtr, size);
            }
        }
    }
}