// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using Internal;
    using LudeoSDK.UnityScripts;
    using System;

    /// <summary>
    /// The LudeoDataReader encapsulates the concept of Ludeo data. As-in, it contains all the information related to a
    /// specific ludeo, such as the necessary data to restore the game state. 
    /// 
    /// The lifetime of a LudeoDataReader instance is tied to the Session lifetime but not to a specific run. When a session is destroyed,
    /// any LudeoDataReader created as part of that Session are invalidated.
    /// </summary>
    public class LudeoDataReader : IDisposable
    {
        private ILudeoDataReaderWrapper m_ludeoDataReaderWrapper; 
        private LudeoHDataReader m_dataReaderHandle;
        private const string LOG_PREFIX = "[LudeoDataReader]";
        
		private string m_playerId;
        private string m_ludeoId;

        /// <summary>The playerId binded to this Ludeo data when it was created at Creator Flow (if applicable).</summary>
        public string PlayerId => m_playerId;

        /// <summary>The LudeoService's LudeoId this data belongs to.</summary>
        public string LudeoId => m_ludeoId;

        private void InitCoreInterface()
        {
            ILudeoCoreInterface coreHook = LudeoUnityHelpers.TryGetLudeoCoreInterface();
            m_ludeoDataReaderWrapper = coreHook.GetLudeoDataReaderWrapper();
            LudeoDataReader_GetObjects.InjectImplementation(m_ludeoDataReaderWrapper);
        }

        internal LudeoDataReader(LudeoHDataReader dataReaderHandle)
        {
            InitCoreInterface();
            m_dataReaderHandle = dataReaderHandle;
            LudeoDataReaderGetInfoParams ludeoDataReaderGetInfoParams = LudeoDataReaderGetInfoParams.Default;
            LudeoDataReader_GetObjects.SetDataReaderHandle(m_dataReaderHandle);
            if (LudeoDataReader_GetObjects.GetInfo(ref ludeoDataReaderGetInfoParams, out LudeoDataReaderInfo dataReaderInfo) == LudeoResult.Success)
            {
                m_playerId = dataReaderInfo.playerId;
                m_ludeoId = dataReaderInfo.ludeoId;
            }
        }

        /// <summary>
        /// Retrieve information about all the existing objects in the Ludeo data.
        /// The client can use this information to create all the required object instances.
        /// The client is required to maintain the right objectType mappings to its object instances. E.g: if using the SDK from
        /// C#, when playing as a creator, the client will e.g. create instances of several classes, and for every relevant instance
        /// it will call LudeoRoom.Writer.CreateObject to register the object (with the right "objectType"). When restoring the data to
        /// play a Ludeo, the client is expected to create the right amount of instances to the one supplied by this API.
        /// </summary>
        /// <param name="ludeoStateObjectRecords"></param>
        /// <returns>A LudeoResult to indicate success or failure to initialize.</returns>
        public LudeoResult GetObjects(out LudeoReadableObject[] ludeoStateObjectRecords)
        {
            LudeoDataReader_GetObjects.SetDataReaderHandle(m_dataReaderHandle);
            return LudeoDataReader_GetObjects.GetObjects(m_dataReaderHandle, out ludeoStateObjectRecords);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_dataReaderHandle != LudeoHDataReader.Uninitialized)
                {
                    m_ludeoDataReaderWrapper.DataReaderRelease(m_dataReaderHandle);
                    m_dataReaderHandle = LudeoHDataReader.Uninitialized;
                }
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LudeoDataReader()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
