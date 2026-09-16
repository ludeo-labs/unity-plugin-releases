namespace LudeoSDK
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using System.Collections.Generic;
  
    /// @cond INTERNAL    
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal struct FixedString
    {
        public const sbyte MAX_CHARS = 32;
        public const char CHAR_0 = '\0';

        public static readonly FixedString Empty = new FixedString(string.Empty);
        public static readonly FixedString[] EmptyArray = { Empty };

        unsafe public fixed char Val[MAX_CHARS];
        public byte Length;

        unsafe public FixedString(sbyte emptySize = MAX_CHARS)
        {
            Length = (byte)emptySize;
            fixed (char* characters = Val)
            {
                for (int i = 0; i < MAX_CHARS; ++i)
                    characters[i] = CHAR_0;
            }
        }

        unsafe public FixedString(string val)
        {
            Length = (byte)(!string.IsNullOrEmpty(val) ? val.Length : 0);
            Debug.Assert(Length <= MAX_CHARS, "FixedString is of length of up to 32 chars");
            fixed (char* characters = Val)
            {
                for (int i = 0; i < Length; ++i)
                    characters[i] = val[i];
                for (int i = Length; i < MAX_CHARS; ++i)
                    characters[i] = CHAR_0;
            }
        }

        unsafe override public string ToString()
        {
            string outStr;
            fixed (char* characters = Val)
            {
                outStr = new string(characters, 0, Length);
            }
            return outStr;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal struct FixedString16
    {
        public const sbyte MAX_CHARS = 16;
        public const char CHAR_0 = '\0';

        public static readonly FixedString16 Empty = new FixedString16(string.Empty);

        unsafe public fixed char Val[MAX_CHARS];
        public byte Length;

        unsafe public FixedString16(string val)
        {
            Length = (byte)(!string.IsNullOrEmpty(val) ? val.Length : 0);
            Debug.Assert(Length <= MAX_CHARS, "FixedString is of length of up to 32 chars");
            fixed (char* characters = Val)
            {
                for (int i = 0; i < Length; ++i)
                    characters[i] = val[i];
                for (int i = Length; i < MAX_CHARS; ++i)
                    characters[i] = CHAR_0;
            }
        }

        unsafe override public string ToString()
        {
            string outStr;
            fixed (char* characters = Val)
            {
                outStr = new string(characters, 0, Length);
            }
            return outStr;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal struct FixedString64
    {
        public const sbyte MAX_CHARS = 64;
        public const char CHAR_0 = '\0';

        public static readonly FixedString64 Empty = new FixedString64(string.Empty);

        unsafe public fixed char Val[MAX_CHARS];
        public byte Length;

        unsafe public FixedString64(string val)
        {
            Length = (byte)(!string.IsNullOrEmpty(val) ? val.Length : 0);
            Debug.Assert(Length <= MAX_CHARS, "FixedString64 is of length of up to 64 chars");
            fixed (char* characters = Val)
            {
                for (int i = 0; i < Length; ++i)
                    characters[i] = val[i];
                for (int i = Length; i < MAX_CHARS; ++i)
                    characters[i] = CHAR_0;
            }
        }

        unsafe override public string ToString()
        {
            string outStr;
            fixed (char* characters = Val)
            {
                outStr = new string(characters, 0, Length);
            }
            return outStr;
        }
    }

    /// @endcond
}
