/// @cond INTERNAL
namespace LudeoSDK.Internal
{
	using System;
	using System.Runtime.InteropServices;

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoRoomAddPlayerCallback(ref LudeoRoomAddPlayerCallbackParams ludeoRoomAddPlayerCallbackParams);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoRoomRemovePlayerCallback(ref LudeoRoomRemovePlayerCallbackParams ludeoRoomRemovePlayerCallbackParams);

	[StructLayout(LayoutKind.Sequential)]
    public struct LudeoHRoom
	{
		public static readonly LudeoHRoom Uninitialized = new LudeoHRoom(IntPtr.Zero);

		private IntPtr m_Handle;

		public LudeoHRoom(IntPtr handle) { m_Handle = handle; }

		//    public static implicit operator LudeoHRoom(IntPtr handle) { return new LudeoHRoom(handle); }

		public static bool operator ==(LudeoHRoom value1, LudeoHRoom value2) { return value1.m_Handle == value2.m_Handle; }
		public static bool operator !=(LudeoHRoom value1, LudeoHRoom value2) { return value1.m_Handle != value2.m_Handle; }
	}

	internal class LudeoRoom_InternalDefs
	{
		public const Int32 LUDEO_INITIALIZE_API_LATEST = 1; // LUDEO_INITIALIZE_API_LATEST from InitTypes.h  
		public const Int32 LUDEO_DATAWRITER_SETVALUES_API_LATEST = 1; // LUDEO_DATAWRITER_SETVALUES_API_LATEST from DataWriterTypes.h
		public const Int32 LUDEO_ROOM_ADDPLAYER_API_LATEST  = 1; // LUDEO_ROOMSESSION_ADDPLAYERS_API_LATEST from RoomTypes.h
		public const Int32 LUDEO_ROOM_REMOVEPLAYER_API_LATEST  = 1; // LUDEO_ROOMSESSION_REMOVEPLAYER_API_LATEST from RoomTypes.h
		public const Int32 LUDEO_ROOM_CLOSE_API_LATEST = 1;
	}


	[StructLayout(LayoutKind.Sequential)]
	internal struct LudeoDataWriterSetValuesParams
	{
		public Int32 apiVersion; // LUDEO_DATAWRITER_SETVALUES_API_LATEST
		public IntPtr entries; // const LudeoDataEntry*
		public Int32 count;
	}


	[StructLayout(LayoutKind.Sequential)]
    public struct LudeoRoomAddPlayerParams
    {
		public int apiVersion;
		public IntPtr playerId;

		public LudeoRoomAddPlayerParams(IntPtr playerId)
		{
			this.playerId = playerId;
			apiVersion = LudeoRoom_InternalDefs.LUDEO_ROOM_ADDPLAYER_API_LATEST;
		}
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoRoomAddPlayerCallbackParams
	{
		public LudeoResult resultCode;		
		public IntPtr clientData;
		public LudeoHGameplaySession gameplaySession;

		public bool ConvertToManaged(string playerId, out LudeoRoomAddPlayerCallbackData managedData)
        {
			managedData = new LudeoRoomAddPlayerCallbackData()
			{
				resultCode = resultCode
			};

			if (resultCode == LudeoResult.Success)
			{
				managedData.player = new LudeoPlayer(gameplaySession, playerId);
				return true;
			}

			return false;
        }
	}

	[StructLayout(LayoutKind.Sequential)]
    public struct LudeoRoomRemovePlayerParams
	{
		public int apiVersion;

		public IntPtr playerId;

		public LudeoRoomRemovePlayerParams(IntPtr playerId)
		{
			this.playerId = playerId;
			apiVersion = LudeoRoom_InternalDefs.LUDEO_ROOM_REMOVEPLAYER_API_LATEST;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
    public struct LudeoRoomRemovePlayerCallbackParams
	{
		public LudeoResult resultCode;
		public IntPtr clientData;

		public bool ConvertToManaged(out LudeoRoomRemovePlayerCallbackData managedData)
		{
			managedData = new LudeoRoomRemovePlayerCallbackData()
			{
				resultCode = resultCode
			};

			return resultCode == LudeoResult.Success;
		}
	}

}
/// @endcond INTERNAL