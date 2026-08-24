/// @cond INTERNAL
namespace LudeoSDK.MediaCapture
{
    using LudeoSDK.Internal;
    using System;
    using System.IO;
    using System.IO.Pipes;

    internal class CustomNamedPipeClientStream : Stream
    {
        public override bool CanRead => m_customNamedPipeClient.PipeHandle != IntPtr.Zero;

        public override bool CanSeek => false;

        public override bool CanWrite => m_customNamedPipeClient.PipeHandle != IntPtr.Zero;

        public override long Length => throw new NotSupportedException();

        private INativePipeClient m_customNamedPipeClient;


        public CustomNamedPipeClientStream(INativePipeClient customNamedPipeClient, string serverName, string pipeName, PipeDirection direction, PipeOptions options)
        {
            m_customNamedPipeClient = customNamedPipeClient;

            m_customNamedPipeClient.Init(serverName, pipeName, direction, options);
        }

        public void Connect(int timeout)
        {
            m_customNamedPipeClient.NativeConnect(timeout);
        }


        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush()
        {
            // Flush might not be applicable for named pipes, but if needed, implement it using FlushFileBuffers API.
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!CanRead) throw new InvalidOperationException("Pipe not open for reading.");

            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0 || count < 0) throw new ArgumentOutOfRangeException(offset < 0 ? nameof(offset) : nameof(count));
            if (buffer.Length - offset < count) throw new ArgumentException("Invalid offset/count combination.");


            return m_customNamedPipeClient.NativeRead(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!CanWrite) throw new InvalidOperationException("Pipe not open for writing.");

            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0 || count < 0) throw new ArgumentOutOfRangeException(offset < 0 ? nameof(offset) : nameof(count));
            if (buffer.Length - offset < count) throw new ArgumentException("Invalid offset/count combination.");

            m_customNamedPipeClient.NativeWrite(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Managed disposal
            }

            try
            {
                m_customNamedPipeClient.NativeCloseHandle();
            }
            catch(Exception e)
            {
                LudeoLogManager.LogError($"Error closing the NamedPipeClient: {e}");
            }

            base.Dispose(disposing);
        }
    }

}
/// @endcond INTERNAL