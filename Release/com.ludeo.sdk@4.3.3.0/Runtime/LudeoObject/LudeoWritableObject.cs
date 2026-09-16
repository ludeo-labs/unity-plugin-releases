// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using UnityEngine;
    using Internal;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// LudeoWritableObject is the context by which Ludeo Gameplay data is tracked.
    /// You may use a LudeoWritableObject per your game's Game object or use a it to collect 
    /// several state data in a context.
    /// </summary>
    public class LudeoWritableObject : IDisposable
    {
        /// <summary>The Object Key as set when using the LudeoRoom.Writer.CreateObject at Creator Flow.</summary>
        public string ObjectType { get { return m_objectType; } }

        /// <summary>An Id created by LudeoSDK for this object</summary>
        public uint ObjectId { get { return m_objectId; } }

        /// <summary>All playerIds binded to this LudeoWritableObject using this LudeoWritableObject.BindPlayer</summary>
        public string[] PlayerIds { get { return m_playerIds; } }

        private string m_objectType;
        private uint m_objectId;
        private string[] m_playerIds;


        internal Dictionary<string, LudeoWritableComponent> m_ludeoStateComponents = new Dictionary<string, LudeoWritableComponent>();
        private bool disposedValue;

        internal LudeoWritableObject(string objectType, uint objectId)
        {
            m_objectType = objectType;
            m_objectId = objectId;
        }


        #region PUBLIC API
        /// <summary>
        /// Mark the state object as destroyed.
        /// After calling this method, the object is no longer valid and should not be used.
        /// </summary>
        /// <returns></returns>
        public LudeoResult DestroyObject()
        {
            m_ludeoStateComponents.Clear();
            return LudeoDataWriter.RemoveObject(ObjectId);
        }

        /// <summary>
        /// Opens a write scope for this object. Wrap your <see cref="WriteData(string, int)"/> calls in a
        /// <c>using</c> block so the scope is closed automatically:
        /// <code>
        /// using (writableObject.EnterObjectScope())
        /// {
        ///     writableObject.WriteData("health", 100);
        ///     writableObject.WriteData("position", transform.position);
        /// }
        /// </code>
        /// Attributes set outside of an open write scope are not guaranteed to target this object.
        /// </summary>
        /// <returns>A disposable <see cref="LudeoWriteScope"/>; dispose it (end the using block) when done.</returns>
        public LudeoWriteScope EnterObjectScope()
        {
            bool entered = LudeoDataWriter.EnterObject(m_objectId);
            return new LudeoWriteScope(isComponent: false, entered: entered);
        }

        /// <summary>
        /// For Ludeo Create: Creates or gets LudeoWritableComponent with the given componentName in this objectType.
        /// </summary>
        /// <param name="componentName">The unique name (in this LudeoWritableObject instance) of LudeoWritableComponent to create</param>
        /// <param name="ludeoStateComponent">The LudeoWritableComponent instance created by Ludeo SDK</param>
        /// <returns>LudeoResult.Success or a fail reason LudeoResult</returns>
        /// <exception cref="ArgumentException">when an empty or null componentName was provided</exception>
        public LudeoResult CreateOrGetComponent(string componentName, out LudeoWritableComponent ludeoStateComponent)
        {
            ValidateStringOrThrow(componentName);
            if (m_ludeoStateComponents.TryGetValue(componentName, out ludeoStateComponent))
            {
                LudeoLogManager.LogWarning($"The LudeoWritableComponent {componentName} already exists. Returning it.");
                return LudeoResult.Success;
            }

            ludeoStateComponent = new LudeoWritableComponent(componentName, m_objectType, m_objectId);
            m_ludeoStateComponents.Add(componentName, ludeoStateComponent);

            return LudeoResult.Success;
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterObjectScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, int value)
        {
            LudeoDataWriter.SetInt32(name, value);
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterObjectScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, float value)
        {
            LudeoDataWriter.SetFloat(name, value);
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterObjectScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, double value)
        {
            LudeoDataWriter.SetDouble(name, value);
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterObjectScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, bool value)
        {
            LudeoDataWriter.SetBool(name, value);
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterObjectScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, Vector3 value)
        {
            LudeoDataWriter.SetVec3Float(name, new FixedVector3(value).Values);
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterObjectScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, Quaternion value)
        {
            LudeoDataWriter.SetVec4Float(name, new FixedVector4(value).Values);
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterObjectScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, string value)
        {
            LudeoDataWriter.SetString(name, value);
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterObjectScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="data">Value for the key</param>
        /// <param name="size">
        /// The size of the data to use. If not specified (default value is <see cref="uint.MaxValue"/>), the size of the <paramref name="data"/> array will be used. </param>
        public void WriteData(string name, byte[] data, uint size = uint.MaxValue)
        {
            uint actualSize = (size == uint.MaxValue) ? (uint)data.Length : size;
            LudeoDataWriter.SetBlob(name, data, actualSize);
        }
        
        /// <summary>
        /// Binds the object to the given playerId.
        /// This is used to associate the object with a player for reading a Ludeo and for setting objectives. 
        /// </summary>
        /// <param name="playerId">The playerId of the player to bind to</param>
        /// <returns>true or false to state the successful or failed binding of the player</returns>
        public bool BindPlayer(string playerId)
        {
            return LudeoDataWriter.SetPlayerBinding(this, playerId);
        }

        #endregion PUBLIC API

        private void ValidateStringOrThrow(string componentName)
        {
            if (string.IsNullOrWhiteSpace(componentName))
                throw new ArgumentNullException("componentName must not be empty!");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                DestroyObject();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LudeoWritableObject()
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