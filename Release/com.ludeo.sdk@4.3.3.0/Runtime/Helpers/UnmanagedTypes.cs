/// @cond INTERNAL
using System.Runtime.InteropServices;
using UnityEngine;

namespace LudeoSDK.Internal
{
    internal class FloatPool
    {
        private float[][] m_pool;
        private int m_head;
        private int m_poolSize;

        public FloatPool(int floatsPerItem, int poolSize)
        {
            m_poolSize = poolSize;
            m_head = 0;
            m_pool = new float[poolSize][];
            for(int i = 0; i < poolSize; ++i)
                m_pool[i] = new float[floatsPerItem];
        }

        public float[] Next()
        {
            float[] outVal = m_pool[m_head];
            m_head = (m_head  + 1 ) % m_poolSize;
            return outVal;
        }

        public void Clear()
        {
            m_pool = null;
        }
    }

    internal class MemoryPools
    {
        private FloatPool m_vector3pool;
        private FloatPool m_vector4pool;

        private static MemoryPools m_instance;

        static MemoryPools()
        {
            m_instance = new MemoryPools();
        }

        private MemoryPools()
        {
            m_vector3pool = new FloatPool(3, 100000);
            m_vector4pool = new FloatPool(4, 100000);
        }

        public static void Dispose()
        {
            m_instance.m_vector3pool.Clear();
            m_instance.m_vector4pool.Clear();

            m_instance.m_vector3pool = null;
            m_instance.m_vector4pool = null;
        }

        public static float[] GetMemForVector3()
        {
            return m_instance.m_vector3pool.Next();
        }

        public static float[] GetMemForVector4()
        {
            return m_instance.m_vector4pool.Next();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct FixedVector3
    {
        public float[] Values;

        public FixedVector3(Vector3 vector3)
        {
            Values = MemoryPools.GetMemForVector3();
            Values[0] = vector3.x;
            Values[1] = vector3.y;
            Values[2] = vector3.z;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct FixedVector4
    {
        public float[] Values;

        public FixedVector4(Quaternion quaternion)
        {
            Values = MemoryPools.GetMemForVector4();
            Values[0] = quaternion.x;
            Values[1] = quaternion.y;
            Values[2] = quaternion.z;
            Values[3] = quaternion.w;
        }

        public FixedVector4(Vector4 vector4)
        {
            Values = MemoryPools.GetMemForVector4();
            Values[0] = vector4.x;
            Values[1] = vector4.y;
            Values[2] = vector4.z;
            Values[3] = vector4.w;
        }
    }
}
/// @endcond INTERNAL