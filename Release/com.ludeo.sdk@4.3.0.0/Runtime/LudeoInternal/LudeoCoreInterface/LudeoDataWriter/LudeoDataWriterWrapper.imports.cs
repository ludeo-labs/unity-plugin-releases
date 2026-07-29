// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    using System.Runtime.InteropServices;
    using static LudeoDataWriter_InternalDefs;

    internal partial class LudeoDataWriterWrapper : ILudeoDataWriterWrapper
    {
        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoResult ludeo_DataWriter_SetSendSettings(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterSetSendSettingsParams ludeoDataWriterSetSendSettingsParams);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern LudeoResult ludeo_DataWriter_CreateObject(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterCreateObjectParams ludeoDataWriterCreateObjectParams, ref uint outObjectId);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoResult ludeo_DataWriter_DestroyObject(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterDestroyObjectParams ludeoDataWriterDestroyObjectParams);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoResult ludeo_DataWriter_SetCurrent(LudeoHDataWriter dataWriterHandle);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoResult ludeo_DataWriter_SendAction(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterSendActionParams @params);


        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_EnterObject(uint objectId);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_LeaveObject();

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_EnterComponent(string name);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_LeaveComponent();
        
        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_SetPlayerBinding(string playerId);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_SetInt8(string attributeName, [In] sbyte value);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_SetInt16(string attributeName, [In] short value);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_SetInt32(string attributeName, [In] int value);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_SetInt64(string attributeName, [In] long value);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_SetBool(string attributeName, [In] LudeoBool value);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_SetFloat(string attributeName, [In] float value);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_SetDouble(string attributeName, [In] double value);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_SetVec3Float(string attributeName, [In] float[] value); // Prefer only [In] - we do not need to copy back the memory

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LudeoBool ludeo_DataWriter_SetVec4Float(string attributeName, [In] float[] value); // Prefer only [In] - we do not need to copy back the memory

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern LudeoBool ludeo_DataWriter_SetString(string attributeName, [In] string value);

        [DllImport(LudeoCoreDefinitions.DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern LudeoBool ludeo_DataWriter_SetBlob(string attributeName, [In] byte* data, [In] uint size);
    }
}