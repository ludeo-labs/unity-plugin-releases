//@cond INTERNAL
namespace LudeoSDK.Internal
{
    using System;
    using static LudeoDataWriter_InternalDefs;

    public interface ILudeoDataWriterWrapper
    {
        LudeoResult SetSendSettings(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterSetSendSettingsParams setSendSettingsParams);

        //TODO - Rui removed support for this completely... 
        // LudeoResult SetState(IntPtr dataWriterHandle, ref LudeoDataWriterSetStateParams ludeoDataWriterSetStateParams);
        LudeoResult CreateObject(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterCreateObjectParams ludeoDataWriterCreateObjectParams, ref uint objectId);
     
        LudeoResult SetCurrent(LudeoHDataWriter dataWriterHandle);

        LudeoResult DestroyObject(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterDestroyObjectParams ludeoDataWriterDestroyObjectParams);

        LudeoBool EnterObject(uint objectId);
        
        LudeoBool LeaveObject();

        LudeoBool SetPlayerBinding(string playerId);

        LudeoBool EnterComponent(string attributeName);
        
        LudeoBool LeaveComponent();

        LudeoResult SendAction(LudeoHDataWriter dataWriterHandle, ref LudeoDataWriterSendActionParams sendActionParams);

        #region ------ Setters ------
        LudeoBool SetInt8(string attributeName, sbyte value);
        
        LudeoBool SetInt16(string attributeName, short value);
        
        LudeoBool SetInt32(string attributeName, int value);
        
        LudeoBool SetInt64(string attributeName, long value);
        
        LudeoBool SetBool(string attributeName, LudeoBool value);
        
        LudeoBool SetFloat(string attributeName, float value);
        
        LudeoBool SetDouble(string attributeName, double value);

        LudeoBool SetVec3Float(string attributeName, float[] value);

        LudeoBool SetVec4Float(string attributeName, float[] value);

        LudeoBool SetString(string attributeName, string value);

        LudeoBool SetBlob(string attributeName, byte[] data, uint size);

        #endregion ------ Setters ------
    }
}
//@endcond INTERNAL