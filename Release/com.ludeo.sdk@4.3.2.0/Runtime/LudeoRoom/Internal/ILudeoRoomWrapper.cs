//@cond INTERNAL
namespace LudeoSDK.Internal
{
    using System;

    public interface ILudeoRoomWrapper
    {
        void CloseRoom(LudeoHRoom roomHandle, ref LudeoRoomCloseParams ludeoSessionCloseRoomParams, IntPtr clientData, LudeoSessionCloseRoomCallback ludeoSessionCloseRoomCallback);

        void AddPlayer(LudeoHRoom roomHandle, ref LudeoRoomAddPlayerParams ludeoRoomAddPlayerParams, IntPtr clientData, LudeoRoomAddPlayerCallback ludeoRoomAddPlayerCallback);

        void RemovePlayer(LudeoHRoom roomHandle, ref LudeoRoomRemovePlayerParams ludeoRoomRemovePlayerParams, IntPtr clientData, LudeoRoomRemovePlayerCallback ludeoRoomRemovePlayerCallback);
    }
}
//@endcond