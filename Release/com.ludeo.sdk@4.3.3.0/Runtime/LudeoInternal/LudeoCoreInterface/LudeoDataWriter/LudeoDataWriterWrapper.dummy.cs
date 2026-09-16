// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
#if !UNITY_STANDALONE_WIN && !UNITY_ANDROID
    using System.Runtime.InteropServices;
    using static LudeoDataWriter_InternalDefs;

    internal partial class LudeoDataWriterWrapper : ILudeoDataWriterWrapper
    {
        public static LudeoResult ludeo_DataWriter_SetSendSettings(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterSetSendSettingsParams ludeoDataWriterSetSendSettingsParams)
        {
            return LudeoResult.SDKDisabled;
        }

        public static LudeoResult ludeo_DataWriter_CreateObject(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterCreateObjectParams ludeoDataWriterCreateObjectParams, ref uint outObjectId)
        {
            return LudeoResult.SDKDisabled;
        }

        public static LudeoResult ludeo_DataWriter_DestroyObject(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterDestroyObjectParams ludeoDataWriterDestroyObjectParams)
        {
            return LudeoResult.SDKDisabled;
        }

        public static LudeoResult ludeo_DataWriter_SetCurrent(LudeoHDataWriter dataWriterHandle)
        {
            return LudeoResult.SDKDisabled;
        }

        public static LudeoResult ludeo_DataWriter_SendAction(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterSendActionParams @params)
        {
            return LudeoResult.SDKDisabled;
        }

        public static LudeoBool ludeo_DataWriter_EnterObject(uint objectId)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_LeaveObject()
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_EnterComponent(string name)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_LeaveComponent()
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_SetPlayerBinding(string playerId)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_SetInt8(string attributeName, [In] sbyte value)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_SetInt16(string attributeName, [In] short value)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_SetInt32(string attributeName, [In] int value)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_SetInt64(string attributeName, [In] long value)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_SetBool(string attributeName, [In] LudeoBool value)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_SetFloat(string attributeName, [In] float value)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_SetDouble(string attributeName, [In] double value)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_SetVec3Float(string attributeName, [In] float[] value)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_SetVec4Float(string attributeName, [In] float[] value)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        public static LudeoBool ludeo_DataWriter_SetString(string attributeName, [In] string value)
        {
            return LudeoBool.LUDEO_FALSE;
        }

        unsafe public static LudeoBool ludeo_DataWriter_SetBlob(string attributeName, [In] byte* data, [In] uint size)
        {
            return LudeoBool.LUDEO_FALSE;
        }
    }
#endif
}