/// @cond INTERNAL
namespace LudeoSDK.DataStructures
{
    using System.Text;

    internal class StringBuilderPool
    {
        private readonly int MAX_SIZE;
        private StringBuilder[] m_items;
        private int m_index;

        public StringBuilderPool(int size, int itemSize)
        {
            MAX_SIZE = size;
            m_items = new StringBuilder[size];
            m_index = 0;
            for (int i = 0; i < size; ++i)
                m_items[i] = new StringBuilder(itemSize);
        }

        public void Clear()
        {
            m_index = 0;
        }

        public void GetNextItem(out StringBuilder item)
        {
            item = m_items[m_index];
            item.Length = 0;
            m_index = (m_index + 1) % MAX_SIZE;
        }
    }
}
/// @endcond INTERNAL
