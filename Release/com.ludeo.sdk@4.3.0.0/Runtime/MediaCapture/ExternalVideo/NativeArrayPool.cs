/// @cond INTERNAL
namespace LudeoSDK.MediaCapture
{
    using System;
    using Unity.Collections;
    using System.Collections.Concurrent;

    internal class NativeArrayPool<T> : IDisposable where T : struct
    {
        private ConcurrentQueue<NativeArray<T>> Pool { get; set; }
        private int ArraySize { get; set; }
        private Allocator Allocator { get; set; }
        private bool IsDisposed { get; set; }
        private int MaxCapacity { get; set; }
        private int TotalAllocated { get; set; } // New field to track total allocated arrays

        public NativeArrayPool(int arrayLength,
            Allocator allocator = Allocator.Persistent,
            int initialCapacity = 10,
            int maxCapacity = int.MaxValue)
        {
            ArraySize = arrayLength;
            Allocator = allocator;
            MaxCapacity = maxCapacity;
            TotalAllocated = 0; // Initialize TotalAllocated
            Pool = new ConcurrentQueue<NativeArray<T>>();

            var capacity = Math.Min(initialCapacity, MaxCapacity);
            for (int i = 0; i < capacity; i++)
            {
                Pool.Enqueue(Allocate());
                TotalAllocated++; // Increment for each allocated array
            }
        }

        private NativeArray<T> Allocate()
        {
            return new NativeArray<T>(ArraySize, Allocator, NativeArrayOptions.UninitializedMemory);
        }

        public NativeArray<T>? Rent()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("NativeArrayPool");

            if (Pool.TryDequeue(out var array))
                return array;

            if (TotalAllocated < MaxCapacity)
            {
                var newArray = Allocate();
                TotalAllocated++;
                return newArray;
            }

            return null; // Return null if max capacity is reached
        }

        public void Return(NativeArray<T> array)
        {
            if (array.Length != ArraySize)
            {
                throw new ArgumentException("Returned array has different length from pool arrays.");
            }

            if (IsDisposed)
            {
                DisposeArray(array);
                return;
            }

            Pool.Enqueue(array);
        }

        private void DisposeArray(NativeArray<T> array)
        {
            if (array.IsCreated)
                array.Dispose();
        }

        public void Dispose()
        {
            // Clean the memory allocated
            while (Pool.TryDequeue(out var array))
            {
                DisposeArray(array);
            }

            IsDisposed = true;
        }
    }
}
/// @endcond INTERNAL
