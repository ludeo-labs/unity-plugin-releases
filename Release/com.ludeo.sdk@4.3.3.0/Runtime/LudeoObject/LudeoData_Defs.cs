// Copyright 2024 Ludeo. All rights reserved.

using System;

namespace LudeoSDK
{
    // From DataTypesCommon.h
    /// <summary>
    /// Data types the backend understands.
    /// Data sent with any of these types can be used to setup the scoring/challenges for a Ludeo
    /// </summary>
    /// <remarks>
    /// The native LudeoDataType is 4 bytes wide (see the C++ STATIC_ASSERT_SIZEOF(LudeoDataType, 4)).
    /// It MUST stay Int32-backed: it is written by the core through a pointer in
    /// ludeo_DataReader_AttributeExists (a narrower backing type would let the core write past a
    /// `stackalloc LudeoDataType[1]`) and appears as a 4-byte field in interop structs such as
    /// LudeoDataReaderVisitAttributesCallbackParams.
    /// </remarks>
    public enum LudeoDataType : Int32
    {
        Bool = 0,
        Int8 = 1,
        UInt8 = 2,
        Int16 = 3,
        UInt16 = 4,
        Int32 = 5,
        Int = Int32,
        UInt32 = 6,
        Uint = UInt32,
        Int64 = 7,
        UInt64 = 8,
        Float = 9,
        Double = 10,
        String = 11,
        Vec3Float = 12,

        Vec4Float = 13,
        QuatFloat = 13,

        Blob = 14,

        Component = 100, // TBD: need to update according to Core!!! Currently suggest to be 100 (https://docs.google.com/document/d/1TfR0i5LFWAvZXkzT4bQCL95x8o_dbGf9P-S3qPxHokI/edit)
    }
}