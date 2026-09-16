/// @cond INTERNAL
using AOT;
using System;
using System.Runtime.InteropServices;
using System.Text;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace LudeoSDK.Internal
{
    using static LudeoDataWriter_InternalDefs;

    internal struct LudeoDataReaderVisitAttributesCallbackData
    {
        public string name;
        public LudeoDataType type;
    }

    internal static class LudeoDataReader_GetObjects
    {
        private static readonly string LOG_PREFIX = "[LudeoDataReader]";
        private static ILudeoDataReaderWrapper m_ludeoDataReaderWrapper;
        private static LudeoHDataReader m_dataReaderHandle;

        // Reads go straight to the core data reader (no managed cache). A read scope must be open
        // (see LudeoReadableObject.EnterObjectScope / LudeoReadableComponent.EnterComponentScope) before reading.
        private static bool s_objectEntered;

        // Target collection for an on-demand GetAll* visit of the current context.
        private static LudeoAttributesCollection s_visitTarget;
        private static uint s_visitObjectId;

        internal static void InjectImplementation(ILudeoDataReaderWrapper ludeoDataReaderWrapper)
        {
            m_ludeoDataReaderWrapper = ludeoDataReaderWrapper;
            s_objectEntered = false;
        }

        internal static LudeoResult GetObjects(LudeoHDataReader dataReaderHandle, out LudeoReadableObject[] ludeoStateObjectRecords)
        {
            LudeoDataReaderGetObjectsInfoParams dataReaderGetObjectsInfoParams = LudeoDataReaderGetObjectsInfoParams.Default;

            LudeoResult result = m_ludeoDataReaderWrapper.GetObjectsInfo(dataReaderHandle, ref dataReaderGetObjectsInfoParams, out IntPtr ludeoObjectsInfoPtr);

            if (result == LudeoResult.Success)
            {
                LudeoObjectsInfo ludeoObjectsInfo = Marshal.PtrToStructure<LudeoObjectsInfo>(ludeoObjectsInfoPtr);
                ludeoObjectsInfo.ConvertToManaged(out ludeoStateObjectRecords);
            }
            else
                throw new Exception("Failed to get state objects");

            return result;
        }

        #region Read scope navigation

        // The native data reader is a stateful cursor (same model as the writer): EnterObject / EnterComponent
        // push a context, the Get* calls read from the top, and Leave* pops it. These helpers are driven by the
        // LudeoReadScope guards so that enter/leave is always balanced.

        internal static bool EnterObject(uint objectId)
        {
            if (!m_ludeoDataReaderWrapper.EnterObject(objectId))
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to enter objectId {objectId}!");
                return false;
            }

            s_objectEntered = true;
            return true;
        }

        internal static void LeaveObject()
        {
            s_objectEntered = false;

            if (!m_ludeoDataReaderWrapper.LeaveObject())
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to leave object!");
        }

        internal static bool EnterComponent(string componentName)
        {
            if (!m_ludeoDataReaderWrapper.EnterComponent(componentName))
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to enter component {componentName}!");
                return false;
            }

            return true;
        }

        internal static void LeaveComponent()
        {
            if (!m_ludeoDataReaderWrapper.LeaveComponent())
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to leave component!");
        }

        internal static bool CanRead(string attributeName)
        {
            if (!s_objectEntered)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Cannot read attribute '{attributeName}' - no open read scope. Wrap reads in a using (restore.EnterObjectScope()) block.");
                return false;
            }

            return true;
        }

        #endregion

        #region On-demand GetAll

        // Visits every attribute of the CURRENT (already-entered) object or component context and fills 'target'.
        // Components are recorded as empty LudeoReadableComponent handles; their attributes are read separately
        // by entering the component's own read scope.
        internal static void ReadCurrentContextInto(LudeoAttributesCollection target, uint objectId)
        {
            if (!s_objectEntered)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Cannot read attributes - no open read scope.");
                return;
            }

            s_visitTarget = target;
            s_visitObjectId = objectId;

            LudeoDataReaderVisitAttributesParams callParams = LudeoDataReaderVisitAttributesParams.Default;
            unsafe
            {
                uint attributesCount = 0;
                m_ludeoDataReaderWrapper.GetAttributesOfCurrentObject(ref callParams, &attributesCount, IntPtr.Zero, HandleVisitIntoTargetCallback);
            }

            s_visitTarget = null;
        }

        [MonoPInvokeCallback(typeof(LudeoDataReaderVisitAttributesCallback))]
        private static LudeoBool HandleVisitIntoTargetCallback(ref LudeoDataReaderVisitAttributesCallbackParams nativeOutParams)
        {
            nativeOutParams.ConvertToManaged(out LudeoDataReaderVisitAttributesCallbackData managedData);

            if (managedData.type == LudeoDataType.Component)
                s_visitTarget.AddComponent(managedData.name, new LudeoReadableComponent(managedData.name, s_visitObjectId));
            else
                ReadAndAddAttributeToCollection(managedData.name, managedData.type, s_visitTarget);

            return true;
        }

        #endregion

        private static void ReadAndAddAttributeToCollection(string attributeName, LudeoDataType dataType, LudeoAttributesCollection attributesCollection)
        {
            LudeoLogManager.LogVeryVerbose($"{LOG_PREFIX} adding attribute: {attributeName}");

            switch (dataType)
            {
                case LudeoDataType.Int32:
                    GetAttribute(attributeName, out int iVal);
                    attributesCollection.AddAttribute(attributeName, iVal);
                    break;

                case LudeoDataType.Float:
                    GetAttribute(attributeName, out float fVal);
                    attributesCollection.AddAttribute(attributeName, fVal);
                    break;

                case LudeoDataType.Double:
                    GetAttribute(attributeName, out double dVal);
                    attributesCollection.AddAttribute(attributeName, dVal);
                    break;

                case LudeoDataType.Bool:
                    GetAttribute(attributeName, out bool bVal);
                    attributesCollection.AddAttribute(attributeName, bVal);
                    break;

                case LudeoDataType.Vec3Float:
                    GetAttribute(attributeName, out Vector3 v3Val);
                    attributesCollection.AddAttribute(attributeName, v3Val);
                    break;

                case LudeoDataType.QuatFloat:
                    GetAttribute(attributeName, out Quaternion qVal);
                    attributesCollection.AddAttribute(attributeName, qVal);
                    break;

                case LudeoDataType.String:
                    GetAttribute(attributeName, out string sVal);
                    attributesCollection.AddAttribute(attributeName, sVal);
                    break;

                case LudeoDataType.Blob:
                    GetAttribute(attributeName, out byte[] blobVal);
                    attributesCollection.AddAttribute(attributeName, blobVal, (uint)(blobVal.Length));                  
                    break;
            }
        }

        internal static void SetDataReaderHandle(LudeoHDataReader dataReaderHandle)
        {
            m_dataReaderHandle = dataReaderHandle;
            if (m_ludeoDataReaderWrapper.SetCurrent(dataReaderHandle) != LudeoResult.Success)
                LudeoLogManager.LogError("Failed to set current data reader!");
        }

        public static bool GetAttribute(string name, out int value)
        {
            return m_ludeoDataReaderWrapper.GetInt(name, out value);
        }

        public static bool GetAttribute(string name, out float value)
        {
            return m_ludeoDataReaderWrapper.GetFloat(name, out value);
        }

        public static bool GetAttribute(string name, out double value)
        {
            return m_ludeoDataReaderWrapper.GetDouble(name, out value);
        }

        public static bool GetAttribute(string name, out bool value)
        {
            bool result = m_ludeoDataReaderWrapper.GetBool(name, out LudeoBool internalValue);

            value = internalValue;

            return result;
        }

        unsafe public static bool GetAttribute(string name, out byte[] data)
        {
            data = default;
            if (!m_ludeoDataReaderWrapper.GetSize(name, out UInt32 size))
                return LudeoBool.LUDEO_FALSE;

            if (size == 0)
                return LudeoBool.LUDEO_TRUE;

            data = new byte[size];
            return m_ludeoDataReaderWrapper.GetBlob(name, out data);
        }

        unsafe public static bool GetAttribute(string name, out string value)
        {
            value = string.Empty;
            if (!m_ludeoDataReaderWrapper.GetSize(name, out UInt32 size))
                return LudeoBool.LUDEO_FALSE;

            if (size == 0)
                return LudeoBool.LUDEO_TRUE;
            
            int signedSize = (int)size;
            byte* outValue = stackalloc byte[unchecked(signedSize)];
            LudeoBool result = m_ludeoDataReaderWrapper.GetString(name, outValue);
            value = Encoding.ASCII.GetString(outValue, signedSize);
            return result;
        }

        public static bool GetAttribute(string name, out Vector3 value)
        {
            bool isGotAttribute = m_ludeoDataReaderWrapper.GetVector3(name, out float[] vectorAsArray);

            value = isGotAttribute ? new Vector3() { x = vectorAsArray[0], y = vectorAsArray[1], z = vectorAsArray[2] } : Vector3.zero;

            return isGotAttribute;
        }

        public static bool GetAttribute(string name, out Quaternion value)
        {
            bool isGotAttribute = m_ludeoDataReaderWrapper.GetQuaternion(name, out float[] vectorAsArray);

            value = isGotAttribute? new Quaternion() { x = vectorAsArray[0], y = vectorAsArray[1], z = vectorAsArray[2], w = vectorAsArray[3]  } : Quaternion.identity;

            return isGotAttribute;
        }

        public static LudeoResult GetInfo(ref LudeoDataReaderGetInfoParams ludeoDataReaderGetInfoParams, out LudeoDataReaderInfo ludeoDataReaderInfo)
        {
            LudeoResult result = m_ludeoDataReaderWrapper.GetInfo(m_dataReaderHandle, ref ludeoDataReaderGetInfoParams, out IntPtr ptrToStruct);
            if (ptrToStruct != IntPtr.Zero)
            {
                LudeoDataReaderInfoNative myStruct = Marshal.PtrToStructure<LudeoDataReaderInfoNative>(ptrToStruct);
                
                string ludeoIdString = Marshal.PtrToStringAnsi(myStruct.ludeoId);
                string playerIdString = Marshal.PtrToStringAnsi(myStruct.playerId);
                
                ludeoDataReaderInfo = new LudeoDataReaderInfo
                {
                    apiVersion = myStruct.apiVersion,
                    ludeoId = ludeoIdString,
                    playerId = playerIdString
                };
                m_ludeoDataReaderWrapper.ReleaseInfo(ref ptrToStruct);
            }
            else
            {
                ludeoDataReaderInfo = new LudeoDataReaderInfo();
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to get reader info!");
            }
         
            return result;
        }

        /// <summary>
        /// Checks for the existence of an attribute as a specific LudeoDataType
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="expectedDataType">The LudeoDataType we expect this attribute to be</param>
        /// <returns>True if the attribute was found as the expected LudeoDataType</returns>
        public static bool IsAttributeExists(string name, LudeoDataType expectedDataType)
        {
            bool isExists = m_ludeoDataReaderWrapper.IsAttributeExists(name, out LudeoDataType typeOfAttribute);
            if (!isExists || (expectedDataType != typeOfAttribute)) 
            {
                LudeoLogManager.LogInfo($"{LOG_PREFIX} Attribute {name} of type {expectedDataType} was not found in this context!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks for the existence of an attribute as any LudeoDataType
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="typeOfAttribute">True if the attribute was found, in which case outType will have the type on exit</param>
        /// <returns></returns>
        public static bool IsAttributeExists(string name, out LudeoDataType typeOfAttribute)
        {
            bool isExists = m_ludeoDataReaderWrapper.IsAttributeExists(name, out typeOfAttribute);
            if (!isExists)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Attribute {name} was not found in this context!");
                return false;
            }

            return true;
        }

        public static bool GetAttributeSize(string name, out uint size)
        {
            return m_ludeoDataReaderWrapper.GetSize(name, out size);
        }
    }
}
/// @endcond INTERNAL