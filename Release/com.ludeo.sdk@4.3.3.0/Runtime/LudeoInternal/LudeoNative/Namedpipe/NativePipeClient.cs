// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
    using LudeoSDK.Internal;
    using System;
    using System.IO.Pipes;

    internal partial class NativePipeClient : INativePipeClient
    {
        private PipeDirection m_direction;
        private string m_pipeName;
        private string m_serverName;
        private PipeOptions m_options;

        public void Init(string serverName, string pipeName, PipeDirection direction, PipeOptions options)
        {
            m_direction = direction;
            m_pipeName = pipeName;
            m_serverName = serverName;
            m_options = options;
        }

        public void NativeConnect(int timeout)
        {
            PLATFORM_Connect(timeout);
        }

        public int NativeRead(byte[] buffer, int offset, int count)
        {
            return PLATFORM_Read(buffer, offset, count);
        }

        public void NativeWrite(byte[] buffer, int offset, int count)
        {
            PLATFORM_Write(buffer, offset, count);
        }

        public bool NativeCloseHandle()
        {
            return PLATFORM_CloseHandle();
        }
    }
}