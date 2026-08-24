/// @cond INTERNAL
namespace LudeoSDK.Ludeo.Internal.Launchers.Nvidia
{
    public class NvidiaDefinitions
    {
        private const GfnDisplayLanguage DEFAULT_LANGUAGE = GfnDisplayLanguage.gfn_en_US;

        public enum GfnRuntimeError
        {
            gfnSuccess = 0,                     //< Success
            gfnInitSuccessClientOnly = 1,       //< SDK initialized, but only cloud independent functionality available (such as gfnStartStream)
            gfnInitSuccessCloudOnly = 2,        //< SDK initialized, but only cloud functionality is available
            gfnInitFailure = -1,                //< SDK initialization failure for any reason other than memory allocation failure
            gfnDllNotPresent = -2,              //< DLL is not present
            gfnComError = -3,                   //< Geforce NOW SDK internal component communication error
            gfnLibraryCallFailure = -4,         //< Geforce NOW SDK components were reachable, but could not serve the request
            gfnIncompatibleVersion = -5,        //< Incompatible version
            gfnUnableToAllocateMemory = -6,     //< Unable to allocate memory
            gfnInvalidParameter = -7,           //< Invalid parameter
            gfnInternalError = -8,              //< Generic Geforce NOW SDK internal error
            gfnUnsupportedAPICall = -9,         //< API Call is not supported
            gfnInvalidToken = -10,              //< Invalid token
            gfnTimedOut = -11,                  //< Operation timed out
            gfnSetupTitleFailure = -12,         //< Failed to setup title
            gfnClientDownloadFailed = -13,      //< Failed to download the Geforce NOW client
            gfnCallWrongEnvironment = -14,      //< Function limited to specific environment called in wrong environment
            gfnWebApiFailed = -15,              //< A call to a NVIDIA Web API failed to return valid data
            gfnStreamFailure = -16,             //< GeForceNOW Streamer hit a failure while starting a stream
            gfnAPINotFound = -17,               //< Library API call not found
            gfnAPINotInit = -18,                //< API not initialized
            gfnStreamStopFailure = -19,         //< Failed to stop active streaming session
            gfnUnhandledException = -20,        //< Unhandled exceptions
            gfnIPCFailure = -21,                //< Messagebus IPC failures
            gfnCanceled = -22,                  //< Activity was canceled, for example, user canceled the download of GFN client
            gfnElevationRequired = -23,         //< API call required to be run from an elevated process
            gfnThrottled = -24,                 //< API call throttled
            gfnInputExpected = -25,             //< API call was expecting input param to have a value
            gfnBinarySignatureInvalid = -26,    //< An attempt to load a binary failed because the digital signature was found to be invalid
            gfnCloudLibraryNotFound = -27,      //< Necessary GFN cloud-based SDK library cannot be found
            gfnClientLibraryNotFound = -28,     //< Necessary GFN client-based SDK library cannot be found
            gfnNoData = -29,

            gfnLudeoDummy = int.MaxValue / 2
        }

        public enum GfnDisplayLanguage
        {
            gfnDefaultLanguage = 0, //< Uses the default system language

            gfn_bg_BG = 1,  //< Bulgarian (Bulgaria)
            gfn_cs_CZ = 2,  //< Czech (Czech Republic)
            gfn_nl_NL = 3,  //< Dutch (Netherlands)
            gfn_de_DE = 4,  //< German (Germany)
            gfn_el_GR = 5,  //< Greek (Greece)
            gfn_en_US = 6,  //< English (US)
            gfn_en_UK = 7,  //< English (Great Britain)
            gfn_es_ES = 8,  //< Spanish (Spain)
            gfn_es_MX = 9,  //< Spanish (Mexico)
            gfn_fi_FI = 10, //< Finnish (Finland)
            gfn_fr_FR = 11, //< French (France)
            gfn_hu_HU = 12, //< Hungarian (Hungary)
            gfn_it_IT = 13, //< Italian (Italy)
            gfn_ja_JP = 14, //< Japanese (Japan)
            gfn_ko_KR = 15, //< Korean (Korea)
            gfn_nb_NO = 16, //< Norwegian - Bokmål (Norway)
            gfn_po_PO = 17, //< Polish (Poland)
            gfn_pt_BR = 18, //< Portuguese (Brazil)
            gfn_pt_PT = 19, //< Portuguese (Portugal)
            gfn_ro_RO = 20, //< Romanian (Romania)
            gfn_ru_RU = 21, //< Russian (Russia)
            gfn_sv_SE = 22, //< Swedish (Sweden)
            gfn_th_TH = 23, //< Thai (Thailand)
            gfn_tr_TR = 24, //< Turkish (Turkey)
            gfn_uk_UA = 25, //< Ukrainian (Ukraine)
            gfn_zh_CN = 26, //< Chinese (China)
            gfn_zh_TW = 27, //< Chinese (Taiwan)
            gfn_en_GB = 28, //< English (Great Britain)
            gfn_hr_HR = 29, //< Croatian (Croatia)
            gfn_sk_SK = 30, //< Slovak (Slovakia)
            gfn_sl_SI = 31, //< Slovenian (Slovenia)
            gfn_da_DK = 32, //< Danish (Denmark)

            gfnMaxLanguage = gfn_da_DK  //< Last Supported Locale (Sentinel Value)
        }

        public static GfnDisplayLanguage GetLanguage()
        {
            GfnDisplayLanguage gfnDisplayLanguage = DEFAULT_LANGUAGE;

            return gfnDisplayLanguage;
        }
    }
}
/// @endcond INTERNAL