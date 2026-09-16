/// @cond INTERNAL

using System.Collections.Generic;

namespace LudeoSDK.Internal
{
    using System;
    using System.Runtime.InteropServices;
    using static LudeoSDK.Internal.LudeoDataWriter_InternalDefs;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionReleaseCallback(ref LudeoSessionReleaseCallbackParams callbackParams);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionActivateCallback(ref LudeoSessionActivateCallbackParams callbackParams);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionOpenRoomCallback(ref LudeoSessionOpenRoomCallbackParams ludeoSessionOpenRoomParams);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionCloseRoomCallback(ref LudeoRoomCloseCallbackParams ludeoSessionOpenRoomParams);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionLudeoSelectedCallback(ref LudeoSessionLudeoSelectedCallbackParams callbackParams);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionPauseGameRequestCallback(ref LudeoSessionPauseGameRequestCallbackParams callbackParams);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionResumeGameRequestCallback(ref LudeoSessionResumeGameRequestCallbackParams callbackParams);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionBackToMenuRequestCallback(ref LudeoSessionBackToMenuRequestCallbackParams callbackParams);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionRoomReadyCallback(ref LudeoSessionRoomReadyCallbackParams callbackParams);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionMuteRequestCallback(ref LudeoSessionMuteRequestCallbackParams callbackParams);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionLocalizationChangedCallback(ref LudeoSessionLocalizationChangedCallbackParams callbackParams);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionHighlightCallback(ref LudeoSessionHighlightCallbackParams callbackParams);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionConsentUpdatedCallback(ref LudeoSessionConsentUpdatedCallbackParams callbackParams);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionVideoStateCallback(ref LudeoSessionVideoStateCallbackParams callbackParams);
    

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LudeoSessionGetLudeoCallback(ref LudeoSessionGetLudeoCallbackParams callbackParams);

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoHSession
    {
        public static readonly LudeoHSession Uninitialized = new LudeoHSession(IntPtr.Zero);

        private IntPtr m_Handle;

        public LudeoHSession(IntPtr handle) { m_Handle = handle; }

        //  public static implicit operator LudeoHSession(IntPtr handle) { return new LudeoHSession(handle); }

        public static bool operator ==(LudeoHSession value1, LudeoHSession value2) { return value1.m_Handle == value2.m_Handle; }
        public static bool operator !=(LudeoHSession value1, LudeoHSession value2) { return value1.m_Handle != value2.m_Handle; }
    }

    internal class LudeoSession_InternalDefs
    {
        public const Int32 LUDEO_SESSION_CREATE_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_RELEASE_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_ACTIVATE_API_LATEST = 3;
        public const Int32 LUDEO_SESSION_BEGINCREATOR_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_ADDNOTIFYLUDEOSELECTED_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_ADDNOTIFYPAUSEGAME_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_ADDNOTIFYRESUMEGAME_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_ADDNOTIFYBACKTOMENU_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_ADDNOTIFYROOMREADY_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_ADDNOTIFYHIGHLIGHT_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_OPEN_ROOM_API_LATEST = 2;
        public const Int32 LUDEO_SESSION_OPENGALLERY_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_ADDNOTIFYCONSENTUPDATED_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_ADDNOTIFYVIDEOSTATE_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_ADDNOTIFYMUTEREQUEST_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_ADDNOTIFYLOCALIZATION_PARAMS_API_LATEST = 1;
        public const Int32 LUDEO_SESSION_LOCALIZATION_PARAMS_API_LATEST = 1;
    }

    // Mirrors the native LudeoAuthType (SessionTypes.h). Kept complete so the marshalling enum matches
    // the core contract even though the plugin currently only ever sends Steam.
    internal enum LudeoAuthType : Int32
    {
        Steam = 0,
        Epic = 1,
        Nvidia = 2,
        GenericCloud = 3
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct LudeoSteamAuthDetails
    {
        // Needs to be set to LudeoAuthType::Steam
        public LudeoAuthType authType;

        // Steam Id
        public string authId;

        public string currentBetaName;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionLudeoSelectedCallbackParams
    {
        public IntPtr clientData; // void* // Not currently supported.

        public string ludeoId; // const char*

        public void ConvertToManaged(out LudeoSelectedCallbackData managedData)
        {
            managedData.ludeoId = new Guid(ludeoId);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionPauseGameRequestCallbackParams
    {
        public IntPtr clientData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionResumeGameRequestCallbackParams
    {
        public IntPtr clientData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionBackToMenuRequestCallbackParams
    {
        public IntPtr clientData;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionMuteRequestCallbackParams
    {
        public IntPtr clientData;
        public LudeoBool mute;
        public void ConvertToManaged(out LudeoSessionMuteRequestCallbackData managedData)
        {
            managedData = new LudeoSessionMuteRequestCallbackData()
            {
                isMuted = mute
            };
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionLocalizationChangedCallbackParams
    {
        public IntPtr clientData;
        public string language;
        
        public void ConvertToManaged(out LudeoSessionLocalizationChangedCallbackData managedData)
        {
            managedData = new LudeoSessionLocalizationChangedCallbackData()
            {
                language = language
            };
        }
    }
    

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionRoomReadyCallbackParams
    {
        public IntPtr clientData;
        public LudeoHRoom room;

        public void ConvertToManaged(out LudeoSessionRoomReadyCallbackData managedData)
        {
            managedData = new LudeoSessionRoomReadyCallbackData()
            {
                ludeoRoom = LudeoRoom.ActiveRoom
            };
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionHighlightCallbackParams
    {
        public IntPtr clientData;
        public string highlightUrl;
        public string highlightId;

        public void ConvertToManaged(out LudeoSessionHighlightCallbackData managedData)
        {
            managedData = new LudeoSessionHighlightCallbackData()
            {
                highlightId = highlightId,
                highlightUrl = highlightUrl,
            };
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionConsentUpdatedCallbackParams
    {
        /** Context that was passed to ludeo_Session_AddNotifyConsentUpdated */
        public IntPtr clientData;

        /** If true, opening a room to create ludeos is allowed, and the client will get a valid DataWriter where applicable. */
        public LudeoBool canCreateLudeo;

        /** If true, opening a room to play a ludeo is allowed, and the client will get a valid DataReader interface where applicable. */
        public LudeoBool canPlayLudeo;

        public void ConvertToManaged(out LudeoSessionConsentUpdatedCallbackData managedData)
        {
            managedData = new LudeoSessionConsentUpdatedCallbackData()
            {
                canCreateLudeo = canCreateLudeo,
                canPlayLudeo = canPlayLudeo,
            };
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LudeoSessionVideoStateCallbackParams
    {
        public IntPtr clientData;

        // Set to 1 if video is enabled. 0 if disabled
        public LudeoBool videoEnabled;

        // Maximum Highlight duration in seconds
        public Int32 maxHighlightDuration;

        public IntPtr apiUpdateUploadUrl;

        // If this is not an empty string, the SDK will be waiting on a call to ludeo_Session_TEMP_VideoStateAck with this value
        public IntPtr contextId;

        public void ConvertToManaged(out LudeoSessionVideoStateCallbackData managedData)
        {
            managedData = new LudeoSessionVideoStateCallbackData()
            {
                videoEnabled = videoEnabled,
                maxHighlightDuration = maxHighlightDuration,
                apiUpdateUploadUrl = Marshal.PtrToStringAnsi(apiUpdateUploadUrl),
                contextId = (contextId != IntPtr.Zero) ? Marshal.PtrToStringAnsi(contextId) : string.Empty
            };
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionCreateParams
    {
        public Int32 apiVersion;

        public static readonly LudeoSessionCreateParams Default = new LudeoSessionCreateParams() { apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_CREATE_API_LATEST };
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionReleaseParams
    {
        public Int32 apiVersion;
        public static readonly LudeoSessionReleaseParams Default = new LudeoSessionReleaseParams() { apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_RELEASE_API_LATEST };
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionReleaseCallbackParams
    {
        public LudeoResult resultCode;
        public IntPtr clientData;

        public void ConvertToManaged(out LudeoSessionReleaseCallbackData managedData)
        {
            managedData = new LudeoSessionReleaseCallbackData()
            {
                resultCode = resultCode,
            };
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionActivateParams
    {
        public Int32 apiVersion;

        public string platformUrl;  // const char*
        
        public IntPtr authDetails;  // void*, Typeof LudeoSessionAuthDetails

        public string apiKey;       // const char*
        
        public string appToken;     // const char*

        // The client's window handle (Native OS handle) that should be used for the SDK's overlays and video recording.
        // If nullptr, the SDK will try to find the foreground window on it's own, which might be wrong. 
        // Therefore it is recommended the client specifies this.
        public IntPtr windowHandle; // void*
        
        //this is placed here on purpose to match the Core struct order
        LudeoBool reset;

        public IntPtr localization; // void*, Typeof LudeoSessionSetLocalizationParams

        public string gameVersion; // const char*
        
        public LudeoSessionActivateParams(string platformUrl, string apiKey, IntPtr authDetails, IntPtr localization, string gameVersion)
        {
            IOSSystems osSys = LudeoUnityManager.LudeoInterfaceFactory.GetIOSSystems();

            this.apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_ACTIVATE_API_LATEST;
            this.platformUrl = platformUrl;
            this.authDetails = authDetails;
            this.apiKey = apiKey;
            this.appToken = null;
            this.windowHandle = osSys.GetWindowHandle();
            this.localization = localization;
            reset = LudeoDataWriter_InternalDefs.LudeoBool.LUDEO_FALSE;
            this.gameVersion = gameVersion;
        }     
    }

    
    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionActivateCallbackParams
    {
        public LudeoResult resultCode;
        public IntPtr clientData; // void*

        // If as part of the session activation the SDK detected that a ludeo should be played, this is set to true.
        // When this is true, the SDK guarantees that soon after this callback, a LudeoSelected notification is received.
        public LudeoDataWriter_InternalDefs.LudeoBool ludeoSelected; // LudeoBool

        public void ConvertToManaged(out LudeoSessionActivateCallbackData managedData)
        {
            if (LudeoManager.Instance == null)
                throw new ArgumentNullException($"Ludeo not yet initialized!");
            if (LudeoManager.Instance.LudeoSession == null)
                throw new ArgumentNullException($"LudeoSession not yet initalized!");

            managedData = new LudeoSessionActivateCallbackData()
            {
                resultCode = resultCode,
                isLudeoSelected = false
            };

            if (resultCode == LudeoResult.Success)
            {
                managedData.isLudeoSelected = ludeoSelected;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionGetLudeoCallbackParams 
    {
        public LudeoResult resultCode;
        public IntPtr clientData;
        public LudeoHDataReader dataReader;

        public void ConvertToManaged(out LudeoGetLudeoCallbackData managedData)
        {
            managedData.resultCode = resultCode;
            managedData.ludeoDataReader = null;
            if (resultCode == LudeoResult.Success)
                managedData.ludeoDataReader = new LudeoDataReader(dataReader);
        }
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionOpenGalleryParams
    {
        public int apiVersion;

        public static LudeoSessionOpenGalleryParams Default = new LudeoSessionOpenGalleryParams() { apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_OPENGALLERY_API_LATEST };
    }
    

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionAddNotifyLudeoSelectedParams
    {
        public int apiVersion;

        public static LudeoSessionAddNotifyLudeoSelectedParams Default = new LudeoSessionAddNotifyLudeoSelectedParams() { apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_ADDNOTIFYLUDEOSELECTED_API_LATEST };
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionAddNotifyPauseGameRequestParams
    {
        public int apiVersion;

        public static LudeoSessionAddNotifyPauseGameRequestParams Default = new LudeoSessionAddNotifyPauseGameRequestParams() { apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_ADDNOTIFYPAUSEGAME_API_LATEST };
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionAddNotifyResumeGameRequestParams
    {
        public int apiVersion;

        public static LudeoSessionAddNotifyResumeGameRequestParams Default = new LudeoSessionAddNotifyResumeGameRequestParams() { apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_ADDNOTIFYRESUMEGAME_API_LATEST };
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionAddNotifyBackToMenuRequestParams
    {
        public int apiVersion;

        public static LudeoSessionAddNotifyBackToMenuRequestParams Default = new LudeoSessionAddNotifyBackToMenuRequestParams() { apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_ADDNOTIFYBACKTOMENU_API_LATEST };
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionAddNotifyRoomReadyParams
    {
        public int apiVersion;

        public static LudeoSessionAddNotifyRoomReadyParams Default = new LudeoSessionAddNotifyRoomReadyParams() { apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_ADDNOTIFYROOMREADY_API_LATEST };
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionAddNotifyHighlightParams
    {
        public int apiVersion;

        public static LudeoSessionAddNotifyHighlightParams Default = new LudeoSessionAddNotifyHighlightParams() { apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_ADDNOTIFYHIGHLIGHT_API_LATEST };
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionAddNotifyConsentUpdatedParams
    {
        public int apiVersion;

        public static LudeoSessionAddNotifyConsentUpdatedParams Default = new LudeoSessionAddNotifyConsentUpdatedParams() { apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_ADDNOTIFYCONSENTUPDATED_API_LATEST };
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionAddNotifyMuteRequestParams
    {
        public int apiVersion;

        public static LudeoSessionAddNotifyMuteRequestParams Default = new LudeoSessionAddNotifyMuteRequestParams() { apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_ADDNOTIFYMUTEREQUEST_API_LATEST };
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionAddNotifyLocalizationChangedParams
    {
        public int apiVersion;

        public static LudeoSessionAddNotifyLocalizationChangedParams Default = new LudeoSessionAddNotifyLocalizationChangedParams() { apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_ADDNOTIFYLOCALIZATION_PARAMS_API_LATEST };
    }
    

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionAddNotifyVideoStateParam
    {
        public int apiVersion;

        public static LudeoSessionAddNotifyVideoStateParam Default = new LudeoSessionAddNotifyVideoStateParam() { apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_ADDNOTIFYVIDEOSTATE_API_LATEST };
    }
    

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionGetLudeoParams
    {
        public int apiVersion;

        public IntPtr ludeoId;
        
        internal LudeoSessionGetLudeoParams(IntPtr ludeoId)
        {
            this.apiVersion = Ludeo_InternalDefs.LUDEO_INITIALIZE_API_LATEST;
            this.ludeoId = ludeoId;
        } 
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionOpenRoomParams
    {
        public int apiVersion;

        public IntPtr roomId;

        public IntPtr ludeoId;

        // threadSafe and maxConcurrentWrites are only read by the core when apiVersion >= 2.
        public LudeoDataWriter_InternalDefs.LudeoBool threadSafe;

        public uint maxConcurrentWrites;

        public LudeoSessionOpenRoomParams(IntPtr ludeoId, IntPtr roomId, bool threadSafe, uint maxConcurrentWrites)
        {
            this.ludeoId = ludeoId;
            this.roomId = roomId;
            this.threadSafe = threadSafe;
            this.maxConcurrentWrites = maxConcurrentWrites;
            this.apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_OPEN_ROOM_API_LATEST;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoRoomCloseParams
    {
        public int apiVersion;

        public static LudeoRoomCloseParams Default = new LudeoRoomCloseParams() { apiVersion = LudeoRoom_InternalDefs.LUDEO_ROOM_CLOSE_API_LATEST };
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionOpenRoomCallbackParams
    {
        public LudeoResult resultCode;
        public IntPtr clientData;
        public LudeoHRoom room;
        public LudeoHDataWriter dataWriter;

        public void ConvertToManaged(bool isLudeo, out LudeoOpenRoomCallbackData managedData)
        {
            managedData = new LudeoOpenRoomCallbackData();
            managedData.resultCode = resultCode;
            managedData.ludeoRoom = null;

            if (resultCode == LudeoResult.Success)
                managedData.ludeoRoom = new LudeoRoom(room, isLudeo, dataWriter);
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct LudeoRoomCloseCallbackParams
    {
        public LudeoResult resultCode;
        public IntPtr clientData;

        public void ConvertToManaged(out LudeoCloseRoomCallbackData managedData)
        {
            managedData.resultCode = resultCode;
        }
    }

        [StructLayout(LayoutKind.Sequential)]
    public struct LudeoSessionSetLocalizationParams
    {
        public int apiVersion;
        public IntPtr language;
        public IntPtr supportedLanguages;
        public uint supportedLanguagesCount;
        
        [NonSerialized]
        private static Dictionary<IntPtr,IntPtr[]> supportedLanguagesPtrsDict = new Dictionary<IntPtr, IntPtr[]>();

        public LudeoSessionSetLocalizationParams(string language, string[] supportedLanguages)
        {
            this.apiVersion = LudeoSession_InternalDefs.LUDEO_SESSION_LOCALIZATION_PARAMS_API_LATEST;
            this.language = IntPtr.Zero;
            this.supportedLanguages = IntPtr.Zero;
            this.supportedLanguagesCount = 0;
            
            try
            {
                if (supportedLanguages != null)
                {
                    IntPtr[] supportedLanguagesPtrs = new IntPtr[supportedLanguages.Length];

                    for (int i = 0; i < supportedLanguages.Length; i++)
                    {
                        supportedLanguagesPtrs[i] = Marshal.StringToHGlobalAnsi(supportedLanguages[i]);
                    }

                    this.supportedLanguages = Marshal.UnsafeAddrOfPinnedArrayElement(supportedLanguagesPtrs, 0);
                    supportedLanguagesCount = Convert.ToUInt32(supportedLanguagesPtrs.Length);
                    supportedLanguagesPtrsDict.Add(this.supportedLanguages, supportedLanguagesPtrs); 
                }
                if (language != null)
                    this.language = Marshal.StringToHGlobalAnsi(language);
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"Failed to set localization! {e}");
            }
        }

        public void Release()
        {
            if (supportedLanguagesPtrsDict.TryGetValue(supportedLanguages, out IntPtr[] supportedLanguagesPtrs))
            {
                for (int i = 0; i < supportedLanguagesPtrs.Length; i++)
                {
                    Marshal.FreeHGlobal(supportedLanguagesPtrs[i]);
                }

                Marshal.FreeHGlobal(supportedLanguages);
                supportedLanguagesPtrsDict.Remove(supportedLanguages);
            }

            Marshal.FreeHGlobal(language);
        }
    }
}
/// @endcond INTERNAL