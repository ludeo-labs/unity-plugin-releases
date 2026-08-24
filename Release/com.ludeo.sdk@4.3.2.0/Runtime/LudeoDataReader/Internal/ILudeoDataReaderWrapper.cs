/// @cond INTERNAL

using System;

namespace LudeoSDK.Internal
{
    using static LudeoDataWriter_InternalDefs;

    public interface ILudeoDataReaderWrapper
    {
        LudeoResult GetObjectsInfo(LudeoHDataReader dataReaderHandle, ref LudeoDataReaderGetObjectsInfoParams dataReaderGetObjectsInfoParams, out IntPtr ludeoObjectsInfo);

        void ObjectsInfoRelease(ref LudeoObjectsInfo info);

        LudeoResult SetCurrent(LudeoHDataReader dataReaderHandle);

        LudeoBool EnterObject(uint objectId);

        LudeoBool LeaveObject();

        LudeoBool EnterComponent(string name);

        LudeoBool LeaveComponent();

        LudeoResult DataReaderRelease(LudeoHDataReader dataReaderHandle);

        LudeoResult GetInfo(LudeoHDataReader dataReaderHandle, ref LudeoDataReaderGetInfoParams dataReaderVisitCallback, out IntPtr dataReaderInfo);

        void ReleaseInfo(ref IntPtr info);

        LudeoBool GetInt(string name, out int value);

        LudeoBool GetFloat(string name, out float value);

        LudeoBool GetDouble(string name, out double value);

        LudeoBool GetBool(string name, out LudeoBool value);

        unsafe LudeoBool GetString(string name, byte* value);

        LudeoBool GetVector3(string name, out float[] value);

        LudeoBool GetQuaternion(string name, out float[] value);

        LudeoBool GetSize(string name, out uint size);

        unsafe LudeoBool GetBlob(string name, out byte[] data);

        LudeoBool IsAttributeExists(string name, out LudeoDataType typeOfAttribute);

        unsafe LudeoResult GetAttributesOfCurrentObject(ref LudeoDataReaderVisitAttributesParams attributesParams, UInt32* outCount, IntPtr clientData, LudeoDataReaderVisitAttributesCallback callback);
    }
}
/// @endcond