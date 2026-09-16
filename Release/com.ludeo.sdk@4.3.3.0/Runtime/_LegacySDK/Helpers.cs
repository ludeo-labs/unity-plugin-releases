/// @cond INTERNAL
namespace LudeoSDK
{
    using LudeoSDK.DataStructures;
    using LudeoSDK.Internal;
    using System;
    using System.Text;
    using Debug = System.Diagnostics.Debug;

    internal static class Helpers
    {
        public const int IDX_NOT_FOUND = -1;

        public const long TIME_TICKS_PER_MILLISECOND = 10000;

        private const long EPOCH_TICKS = 621355968000000000; // ticks from 0001-01-01 00:00:00 to 1970-01-01 00:00:00

        private static readonly int SIZE_OF_INT_32 = sizeof(int);

        private static readonly DateTime EPOCH_TIME = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static void PrintTimestampAndDateTime(string prefix, long milliseconds)
        {
            DateTime dt = ConvertFromUnixTimestampMilliSec(milliseconds);
            LudeoLogManager.LogInfo($"{prefix} {milliseconds} {dt}");
        }

        public static DateTime ConvertFromUnixTimestampMilliSec(long milliseconds)
        {
            long ticks = milliseconds * TIME_TICKS_PER_MILLISECOND + EPOCH_TICKS;
            return new DateTime(ticks, DateTimeKind.Utc);
        }

        public static long GetSecondsSinceEpochTime()
        {
            TimeSpan timeSinceEpoch = DateTime.UtcNow.ToUniversalTime() - EPOCH_TIME;
            return (long)Math.Floor(timeSinceEpoch.TotalSeconds);
        }
        
        //public static long GetTokensSinceEpochTime()
        //{
        //    TimeSpan timeSinceEpoch = DateTime.UtcNow.ToUniversalTime() - EPOCH_TIME;
        //    return timeSinceEpoch.Ticks;
        //}

        public static long GetMillisecondsSinceEpochTime()
        {
            TimeSpan timeSinceEpoch = DateTimeOffset.UtcNow - EPOCH_TIME;
            return (long)timeSinceEpoch.TotalMilliseconds;
        }

        public static float MilliSecondsToSeconds(int timeInMilliseconds)
        {
            return ((float)timeInMilliseconds) * 0.001f;
        }

        public static long GetTokensSinceEpochTime()
        {
            return ToUnixTimeMilliseconds();
        }

        public static long ToUnixTimeMilliseconds()
        {
            TimeSpan timeSinceEpoch = DateTimeOffset.UtcNow - EPOCH_TIME;
            return (long)timeSinceEpoch.TotalMilliseconds;
        }

        public static int GetHashCode(string name)
        {
            //Murmur.Murmur3.MurmurStrToHash(name, 0);
            return name.GetHashCode();
        }

        //public static uint GetHashCode(string name)
        //{
        //    unchecked
        //    {
        //        uint hash = 5381; // Starting with a prime number.

        //        // DJB2 hash algorithm
        //        foreach (char c in name)
        //        {
        //            hash = (hash * 33) ^ c; // hash * 33 + c, but optimized with XOR for unsigned overflow
        //        }

        //        return hash;
        //    }
        //}



        public static void IntArrayToByteArray(int[] arr, ref byte[] byteArr)
        {
            Debug.Assert(arr.Length == byteArr.Length * SIZE_OF_INT_32, "the byte array must be exactly 4 times larger then the intger array");
            Buffer.BlockCopy(arr, 0, byteArr, 0, byteArr.Length);
        }

        unsafe public static bool Contains(int* arr, int bufferSize, int val)
        {
            for(int i=0; i < bufferSize; ++i)
            {
                if (val == arr[i])
                    return true;
            }

            return false;
        }

        unsafe public static int AddItemToArray(long val, long[] arr, int bufferSize, int index)
        {
            Debug.Assert(index < bufferSize, "Index out of bounds");
            Debug.Assert(bufferSize >= 1, "Buffer size must not be less than 1");

            fixed (long* ptr = arr)
                ptr[index++] = val;

            return index;
        }

        unsafe public static int AddItemIfNewToArray(int val, int[] arr, int bufferSize, int itemCount)
        {
            Debug.Assert(itemCount <= bufferSize, "You may not have more items then buffer size");
            Debug.Assert(bufferSize <= 3000, "Up to 3000 buffer size would take ~0.1 millisecond in worst case.");

            fixed (int* ptr = arr)
            {
                int i = 0;
                for (; i < itemCount; ++i)
                {
                    if (val == ptr[i])
                        return bufferSize;
                }

                if (i < bufferSize)
                {
                    ptr[i] = val;
                    return itemCount + 1;
                }
                else
                    LudeoLogManager.LogError("Out of buffer to add new items to array!");
            }

            return itemCount;
        }

        unsafe public static int AddOrGetItemInArray(int val, int* arrPtr, int bufferSize, int itemCount, out int newItemsCount)
        {
            Debug.Assert(itemCount <= bufferSize, "You may not have more items then buffer size");
            //Debug.Assert(bufferSize <= 3000, "Up to 3000 buffer size would take ~0.1 millisecond in worst case.");

            int i = 0;
            for (; i < itemCount; ++i)
            {
                if (val == arrPtr[i])
                {
                    newItemsCount = itemCount;
                    return i;
                }
            }

            if (i < bufferSize)
            {
                arrPtr[i] = val;
                newItemsCount = itemCount + 1;
                return i;
            }
            else
                LudeoLogManager.LogError("Out of buffer to add new items to array!");

            newItemsCount = itemCount;
            return -1;
        }

        unsafe public static int RemoveItemFromArrayAtIdx(int[] arr, int idxToRemove, int bufferSize, int itemCount)
        {
            Debug.Assert(itemCount <= bufferSize, "You may not have more items then buffer size");

            fixed (int* ptr = arr)
            {
                if (idxToRemove < itemCount)
                {
                    ptr[idxToRemove] = ptr[itemCount - 1];
                    return itemCount - 1;
                }
            }

            LudeoLogManager.LogError("index " + idxToRemove + " overflow");

            return itemCount;
        }

        unsafe public static int RemoveItemFromArray(int val, int[] arr, int bufferSize, int itemCount, out int idxRemoved)
        {
            Debug.Assert(itemCount <= bufferSize, "You may not have more items then buffer size");

            fixed (int* ptr = arr)
            {
                for (int i = 0; i < itemCount; ++i)
                {
                    if (val == ptr[i])
                    {
                        ptr[i] = ptr[itemCount-1];
                        idxRemoved = i;
                        return itemCount - 1;
                    }
                }                                
            }

            LudeoLogManager.LogInfo("value " + val + "not found in array");
            idxRemoved = -1;
            return itemCount;
        }

        unsafe public static int RemoveItemFromArray(int val, int* arrPtr, int bufferSize, int itemCount, out int idxRemoved)
        {
            Debug.Assert(itemCount <= bufferSize, "You may not have more items then buffer size");

            for (int i = 0; i < itemCount; ++i)
            {
                if (val == arrPtr[i])
                {
                    arrPtr[i] = arrPtr[itemCount - 1];
                    idxRemoved = i;
                    return itemCount - 1;
                }
            }

            LudeoLogManager.LogVerbose("value " + val + "not found in array");
            idxRemoved = -1;
            return itemCount;
        }

        unsafe public static int IndexOf(int val, int* arr, int bufferSize)
        {
            for (int i = 0; i < bufferSize; ++i)
            {
                if (val == arr[i])
                    return i;
            }

            return IDX_NOT_FOUND;
        }

        unsafe public static int IndexOf(uint val, uint* arr, int bufferSize)
        {
            for (int i = 0; i < bufferSize; ++i)
            {
                if (val == arr[i])
                    return i;
            }

            return IDX_NOT_FOUND;
        }

        unsafe public static int IndexOf(int val, int[] arr, int bufferSize)
        {
            fixed (int* ptr = arr)
            {
                for (int i = 0; i < bufferSize; ++i)
                {
                    if (val == ptr[i])
                        return i;
                }
            }

            return IDX_NOT_FOUND;
        }

        unsafe public static int IndexOf(bool val, bool[] arr, int bufferSize)
        {
            fixed (bool* ptr = arr)
            {
                for (int i = 0; i < bufferSize; ++i)
                {
                    if (val == ptr[i])
                        return i;
                }
            }

            return IDX_NOT_FOUND;
        }

        unsafe public static int FindIndexOfEqualsOrSmaller(int valueToLookFor, int[] arr, int bufferSize)
        {
            int currentValueTested = -1;

            if (bufferSize < 1)
                return IDX_NOT_FOUND;

            fixed (int* ptr = arr)
            {
                for (int i = 0; i < bufferSize; ++i)
                {
                    currentValueTested = ptr[i];

                    if (valueToLookFor <= currentValueTested)
                    {
                        if (valueToLookFor < currentValueTested)
                        {
                            if (i > 0)
                                return i - 1;
                            else
                                break;
                        }
                        return i;
                    }
                }

            }

            if (valueToLookFor > currentValueTested)
                return bufferSize - 1;

            return IDX_NOT_FOUND;
        }

        unsafe public static void ClearArrayContent(long[] arr, int bufferSize)
        {
            fixed (long* ptr = arr)
            {
                for (int i = 0; i < bufferSize; ++i)
                    ptr[i] = 0;
            }
        }

        unsafe public static void ClearArrayContent(bool[] arr, int bufferSize)
        {
            fixed (bool* ptr = arr)
            {
                for (int i = 0; i < bufferSize; ++i)
                    ptr[i] = false;
            }
        }

        public static void CopyArrayContent<T>(T[] source, T[] target, int bufferSize) where T : struct
        {
            for (int i = 0; i < bufferSize; ++i)
                source[i] = target[i];
        }

        unsafe public static void CopyArrayContent(float[] source, float[] target, int bufferSize)
        {
            fixed (float* a = source, b = target)
            {
                for (int i = 0; i < bufferSize; ++i)
                    b[i] = a[i];
            }
        }

        unsafe public static void CopyArrayContent(bool[] source, bool[] target, int bufferSize)
        {
            fixed (bool* a = source, b = target)
            {
                for (int i = 0; i < bufferSize; ++i)
                    b[i] = a[i];
            }
        }

        unsafe public static void CopyArrayContent(FixedString64[] source, FixedString64[] target, int bufferSize)
        {
            fixed (FixedString64* a = source, b = target)
            {
                for (int i = 0; i < bufferSize; ++i)
                    b[i] = a[i];
            }
        }

        public static bool IsArraySize<T>(T[] arr, int size)
        {
            return (arr != null) && (arr.Length == size);
        }

        public static string GetTimeDisplay(DateTime dateTime)
        {
            string timeToDisplay;
            string ordinalString;

            int dayOfTheMonth = dateTime.Day;

            switch (dayOfTheMonth)
            {
                case 1:
                case 21:
                case 31:
                    ordinalString = "st";
                    break;

                case 2:
                case 22:
                    ordinalString = "nd";
                    break;

                case 3:
                case 33:
                    ordinalString = "rd";
                    break;

                default:
                    ordinalString = "th";
                    break;
            }

            timeToDisplay = string.Format("{0}{1}, {2}", dateTime.ToString("MMM dd"), ordinalString, dateTime.ToString("HH:mm"));

            return timeToDisplay;
        }

        public static bool IsNullCallbackAndInform(Action callback, string registerName)
        {
            if (callback == null)
            {
                LudeoLogManager.LogInfo("Please register to " + registerName + " LudeoEvent");
                return true;
            }

            return false;
        }

        public static int ReplaceSegment(this StringBuilder sb, int startIndex, string replacement, int replacementLength = 0, bool removeExcessChars = false)
        {
            int existingSbLength = sb.Length;

            if (replacementLength <= 0)
                replacementLength =  replacement.Length;
            int replacableEndIndex = startIndex + replacementLength;
            int existingEndSBIndex = existingSbLength - 1;

      //      int originalLength = replacableEndIndex - startIndex + 1;
            int currentIndex = startIndex;
            int lastReplacableIndex = Math.Min(replacableEndIndex, existingEndSBIndex);
            int replacableCharsCount = lastReplacableIndex - startIndex;

            // Replace characters within the existing range
            for (int i = 0; i < replacableCharsCount; i++)
            {
                sb[currentIndex] = replacement[i];
                currentIndex++;
            }

            // If the replacement string is longer, insert the remaining characters
            if (replacableEndIndex > existingEndSBIndex)
            {
                for (int i = replacableCharsCount; i < replacementLength; i++)
                {
                    sb.Insert(currentIndex, replacement[i]);
                    currentIndex++;
                }
            }
            // If the replacement string is shorter, remove the excess characters
            else if (removeExcessChars && (existingEndSBIndex > replacableEndIndex))
            {
                sb.Remove(currentIndex, existingEndSBIndex - replacableEndIndex + 1);
            }

            return currentIndex;
        }

    }
}
/// @endcond
