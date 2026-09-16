//@cond INTERNAL
namespace LudeoSDK.Internal
{
    using LudeoSDK.UnityScripts;
    using System;
    using System.Collections.Generic;
    using Unity.IL2CPP.CompilerServices;
    using static LudeoDataWriter_InternalDefs;

    internal static class LudeoDataWriter
    {
        private static readonly string LOG_PREFIX = "[LudeoDataWriter]";
        private static ILudeoDataWriterWrapper m_dataWriterWrapper;
        private static LudeoHDataWriter m_dataWriterHandle;

        internal static void InitCoreInterface()
        {
            ILudeoCoreInterface coreHook = LudeoUnityHelpers.TryGetLudeoCoreInterface();
            m_dataWriterWrapper = coreHook.GetLudeoDataWriterWrapper();
        }

        #region Object

        public static LudeoResult CreateObject(string objectType, out LudeoWritableObject ludeoStateObject)
        {
            LudeoDataWriterCreateObjectParams ludeoDataWriterCreateObjectParams = new LudeoDataWriterCreateObjectParams(objectType);

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} calling CreatedObject: {objectType}");

            uint objectId = 0;
            LudeoResult result = m_dataWriterWrapper.CreateObject(m_dataWriterHandle, ref ludeoDataWriterCreateObjectParams, ref objectId);

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} CreatedObject result: {result} with objectId: {objectId}");

            ludeoStateObject = default;
            if (result == LudeoResult.Success)
                ludeoStateObject = new LudeoWritableObject(objectType, objectId);
            else
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to create object {objectType}!");

            return result;
        }

        public static LudeoResult GetObject(string objectType, uint objectId, out LudeoWritableObject ludeoStateObject)
        {
            LudeoDataWriterCreateObjectParams ludeoDataWriterCreateObjectParams = new LudeoDataWriterCreateObjectParams(objectType);

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} calling GetObject: {objectType}, id: {objectId}");

            LudeoResult result = m_dataWriterWrapper.CreateObject(m_dataWriterHandle, ref ludeoDataWriterCreateObjectParams, ref objectId);

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} GetObject/CreateObject result: {result} with objectId: {objectId}");

            ludeoStateObject = default;
            if (result == LudeoResult.Success)
                ludeoStateObject = new LudeoWritableObject(objectType, objectId);
            else
                LudeoLogManager.LogError($"Failed to create object {objectType}!");

            return result;
        }

        public static LudeoResult RemoveObject(uint objectId)
        {
            LudeoDataWriterDestroyObjectParams ludeoDataWriterDestroyObjectParams = new LudeoDataWriterDestroyObjectParams(objectId);
            return m_dataWriterWrapper.DestroyObject(m_dataWriterHandle, ref ludeoDataWriterDestroyObjectParams);
        }

        #endregion
        public static void SetDataWriterHandle(LudeoHDataWriter dataWriterHandle)
        {
            InitCoreInterface();
            m_dataWriterHandle = dataWriterHandle;
            if (m_dataWriterWrapper.SetCurrent(dataWriterHandle) != LudeoResult.Success)
            {
                LudeoLogManager.LogError("Failed to set current data writer!");
            }
        }

        public static LudeoResult SendAction(string playerId, string action)
        {
            LudeoDataWriterSendActionParams sendActionParams = new LudeoDataWriterSendActionParams(playerId, action);
            return m_dataWriterWrapper.SendAction(m_dataWriterHandle, ref sendActionParams);
        }

        public static LudeoResult SetSendSettings(uint sendIntervalMs)
        {
            LudeoDataWriterSetSendSettingsParams sendSettingsParams = new LudeoDataWriterSetSendSettingsParams(sendIntervalMs);
            return m_dataWriterWrapper.SetSendSettings(m_dataWriterHandle, ref sendSettingsParams);
        }

        public static LudeoBool SetPlayerBinding(LudeoWritableObject ludeoStateObject, string playerId)
        {
            if (!m_dataWriterWrapper.EnterObject(ludeoStateObject.ObjectId))
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to enter object {ludeoStateObject.ObjectType}!");
                return LudeoBool.LUDEO_FALSE;
            }

            if (!m_dataWriterWrapper.SetPlayerBinding(playerId))
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to set player binding {playerId}!");
                return LudeoBool.LUDEO_FALSE;
            }

            m_dataWriterWrapper.LeaveObject();
            return LudeoBool.LUDEO_TRUE;
        }

        #region Stack navigation

        // The underlying data writer is a stateful cursor: EnterObject / EnterComponent push a context,
        // the Set* calls write to whatever is on top, and Leave* pops it. These helpers are driven by the
        // LudeoWriteScope guards so that enter/leave is always balanced.

        private static bool s_objectEntered;

        internal static bool EnterObject(uint objectId)
        {
            if (!m_dataWriterWrapper.EnterObject(objectId))
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to enter object id {objectId}!");
                return false;
            }

            s_objectEntered = true;
            return true;
        }

        internal static void LeaveObject()
        {
            s_objectEntered = false;

            if (!m_dataWriterWrapper.LeaveObject())
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to leave object!");
        }

        internal static bool EnterComponent(string componentName)
        {
            if (!m_dataWriterWrapper.EnterComponent(componentName))
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to enter component {componentName}!");
                return false;
            }

            return true;
        }

        internal static void LeaveComponent()
        {
            if (!m_dataWriterWrapper.LeaveComponent())
                LudeoLogManager.LogError($"{LOG_PREFIX} Failed to leave component!");
        }

        #endregion

        #region Attribute writers

        // These write straight through to the data writer. The core SDK caches the values and only sends what
        // changed, so there is no managed-side caching here. Writes are ignored unless an object write scope is
        // open (see LudeoWritableObject.EnterObjectScope / LudeoWritableComponent.EnterComponentScope).

        private static bool CanWrite(string attributeName)
        {
            if (!s_objectEntered)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} Cannot set attribute '{attributeName}' - no open write scope. Wrap WriteData calls in a using (object.EnterObjectScope()) block.");
                return false;
            }

            return true;
        }

        internal static void SetInt32(string name, int value)
        {
            if (CanWrite(name))
                m_dataWriterWrapper.SetInt32(name, value);
        }

        internal static void SetFloat(string name, float value)
        {
            if (CanWrite(name))
                m_dataWriterWrapper.SetFloat(name, value);
        }

        internal static void SetDouble(string name, double value)
        {
            if (CanWrite(name))
                m_dataWriterWrapper.SetDouble(name, value);
        }

        internal static void SetBool(string name, bool value)
        {
            if (CanWrite(name))
                m_dataWriterWrapper.SetBool(name, value);
        }

        internal static void SetVec3Float(string name, float[] value)
        {
            if (CanWrite(name))
                m_dataWriterWrapper.SetVec3Float(name, value);
        }

        internal static void SetVec4Float(string name, float[] value)
        {
            if (CanWrite(name))
                m_dataWriterWrapper.SetVec4Float(name, value);
        }

        internal static void SetString(string name, string value)
        {
            if (CanWrite(name))
                m_dataWriterWrapper.SetString(name, value);
        }

        internal static void SetBlob(string name, byte[] data, uint size)
        {
            if (CanWrite(name))
                m_dataWriterWrapper.SetBlob(name, data, size);
        }

        #endregion
    }
}
//@endcond