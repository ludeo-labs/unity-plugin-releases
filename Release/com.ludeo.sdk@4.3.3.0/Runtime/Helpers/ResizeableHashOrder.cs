/// @cond INTERNAL
using System;
using System.Runtime.InteropServices;

namespace LudeoSDK.Internal
{
    unsafe internal class ResizeableHashOrder : IDisposable
    {
        private int* m_hashes;
        private int m_itemsCount;
        private bool disposed = false;

        private static readonly int sizeOfInt = sizeof(int);

        private int m_currentBufferSize;

        public ResizeableHashOrder(int initialSize=16)
        {
            m_currentBufferSize = initialSize;
            m_itemsCount = 0;
            m_hashes = (int*)Marshal.AllocHGlobal(sizeOfInt * m_currentBufferSize).ToPointer();
        }

        public void AddOrGetHashIndex(string key, out int indexOfHash)
        {
            int hash = Helpers.GetHashCode(key);

            if (m_itemsCount == m_currentBufferSize)
                ResizeIfNecessary(m_currentBufferSize + (int)(m_currentBufferSize*2));

            indexOfHash = Helpers.AddOrGetItemInArray(hash, m_hashes, m_currentBufferSize, m_itemsCount, out m_itemsCount);
        }


        public void Remove(string key)
        {
            int hash = Helpers.GetHashCode(key);

            m_itemsCount = Helpers.RemoveItemFromArray(hash, m_hashes, m_currentBufferSize, m_itemsCount, out int _);
        }

        private void ResizeIfNecessary(int newSize)
        {
            int* newHashes = (int*)Marshal.AllocHGlobal(sizeof(int) * newSize).ToPointer();

            // Copy values to new array
            for (int i = 0; (i < newSize) && (i < m_itemsCount); i++)
                newHashes[i] = m_hashes[i];

            // Free old array
            Marshal.FreeHGlobal((IntPtr)m_hashes);

            m_hashes = newHashes;
            m_currentBufferSize = newSize;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                Marshal.FreeHGlobal((IntPtr)m_hashes);
                m_hashes = null;
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ResizeableHashOrder()
        {
            Dispose(false);
        }
    }

}
/// @endcond INTERNAL