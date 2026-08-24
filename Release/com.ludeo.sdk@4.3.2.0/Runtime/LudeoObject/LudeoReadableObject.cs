// Copyright 2024 Ludeo. All rights reserved.

using LudeoSDK.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LudeoSDK
{
    /// <summary>
    /// Used to read attributes that were tracked with a LudeoWritableObject.
    /// Reads go straight to the core data reader - wrap them in a using (restore.EnterObjectScope()) scope.
    /// It is recommended to use GetAllAttributes to get the required attributes of a LudeoReadableObject
    /// instead of the singular ReadData.
    /// </summary>
    public class LudeoReadableObject : LudeoRestoreBase, IDisposable
    {
        private uint m_objectId;
        private string m_objectType;

        /// <summary>An Id created by LudeoSDK for this object</summary>
        public uint ObjectId { get { return m_objectId; } }

        /// <summary>The Object Key as set when using the LudeoRoom.Writer.CreateObject at Creator Flow.</summary>
        public string ObjectType { get { return m_objectType; } }

        internal override uint RestoreObjectId { get { return m_objectId; } }

        // Handle de-dup only (so CreateOrGetComponent returns the same instance); NOT an attribute cache.
        private readonly Dictionary<string, LudeoReadableComponent> m_componentHandles = new Dictionary<string, LudeoReadableComponent>();
        private bool disposedValue;

        internal LudeoReadableObject(uint objectId, string objectType)
        {
            m_objectId = objectId;
            m_objectType = objectType;
        }

        /// <summary>
        /// Opens a read scope for this object. Wrap your reads in a using block so the scope is closed
        /// automatically:
        /// <code>
        /// using (readableObject.EnterObjectScope())
        /// {
        ///     readableObject.ReadData("health", out int health);
        ///     readableObject.GetAllAttributes(out Dictionary&lt;string, float&gt; floats);
        /// }
        /// </code>
        /// Reads outside of an open read scope are ignored.
        /// </summary>
        /// <returns>A disposable <see cref="LudeoReadScope"/>; dispose it (end the using block) when done.</returns>
        public LudeoReadScope EnterObjectScope()
        {
            bool entered = LudeoDataReader_GetObjects.EnterObject(m_objectId);
            return new LudeoReadScope(isComponent: false, entered: entered);
        }

        /// <summary>
        /// For Ludeo Play: Creates or gets LudeoReadableComponent with the given componentName in this objectType.
        /// </summary>
        /// <param name="componentName">The unique name (in this LudeoReadableObject instance) of LudeoReadableComponent to create</param>
        /// <param name="ludeoStateComponentRestore">The LudeoReadableComponent instance created by Ludeo SDK</param>
        /// <returns>LudeoResult.Success or a fail reason LudeoResult</returns>
        /// <exception cref="ArgumentException">when an empty or null componentName was provided</exception>
        public LudeoResult CreateOrGetComponent(string componentName, out LudeoReadableComponent ludeoStateComponentRestore)
        {
            ValidateStringOrThrow(componentName);
            if (m_componentHandles.TryGetValue(componentName, out ludeoStateComponentRestore))
                return LudeoResult.Success;

            ludeoStateComponentRestore = new LudeoReadableComponent(componentName, m_objectId);
            m_componentHandles.Add(componentName, ludeoStateComponentRestore);

            return LudeoResult.Success;
        }

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterObjectScope).</summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">Out parameter for the attribute's value</param>
        /// <returns>true if the value was retrieved successfully, false otherwise.</returns>
        public bool ReadData(string name, out int value)
        {
            value = default;
            if (!LudeoDataReader_GetObjects.CanRead(name))
                return false;

            if (!LudeoDataReader_GetObjects.IsAttributeExists(name, LudeoDataType.Int32))
                return false;

            return LudeoDataReader_GetObjects.GetAttribute(name, out value);
        }

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterObjectScope).</summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">Out parameter for the attribute's value</param>
        /// <returns>true if the value was retrieved successfully, false otherwise.</returns>
        public bool ReadData(string name, out float value)
        {
            value = default;
            if (!LudeoDataReader_GetObjects.CanRead(name))
                return false;

            if (!LudeoDataReader_GetObjects.IsAttributeExists(name, LudeoDataType.Float))
                return false;

            return LudeoDataReader_GetObjects.GetAttribute(name, out value);
        }

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterObjectScope).</summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">Out parameter for the attribute's value</param>
        /// <returns>true if the value was retrieved successfully, false otherwise.</returns>
        public bool ReadData(string name, out double value)
        {
            value = default;
            if (!LudeoDataReader_GetObjects.CanRead(name))
                return false;

            if (!LudeoDataReader_GetObjects.IsAttributeExists(name, LudeoDataType.Double))
                return false;

            return LudeoDataReader_GetObjects.GetAttribute(name, out value);
        }

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterObjectScope).</summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">Out parameter for the attribute's value</param>
        /// <returns>true if the value was retrieved successfully, false otherwise.</returns>
        public bool ReadData(string name, out bool value)
        {
            value = default;
            if (!LudeoDataReader_GetObjects.CanRead(name))
                return false;

            if (!LudeoDataReader_GetObjects.IsAttributeExists(name, LudeoDataType.Bool))
                return false;

            return LudeoDataReader_GetObjects.GetAttribute(name, out value);
        }

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterObjectScope).</summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">Out parameter for the attribute's value</param>
        /// <returns>true if the value was retrieved successfully, false otherwise.</returns>
        public bool ReadData(string name, out string value)
        {
            value = default;
            if (!LudeoDataReader_GetObjects.CanRead(name))
                return false;

            if (!LudeoDataReader_GetObjects.IsAttributeExists(name, LudeoDataType.String))
                return false;

            return LudeoDataReader_GetObjects.GetAttribute(name, out value);
        }

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterObjectScope).</summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">Out parameter for the attribute's value</param>
        /// <returns>true if the value was retrieved successfully, false otherwise.</returns>
        public bool ReadData(string name, out Vector3 value)
        {
            value = default;
            if (!LudeoDataReader_GetObjects.CanRead(name))
                return false;

            if (!LudeoDataReader_GetObjects.IsAttributeExists(name, LudeoDataType.Vec3Float))
                return false;

            return LudeoDataReader_GetObjects.GetAttribute(name, out value);
        }

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterObjectScope).</summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">Out parameter for the attribute's value</param>
        /// <returns>true if the value was retrieved successfully, false otherwise.</returns>
        public bool ReadData(string name, out Quaternion value)
        {
            value = default;
            if (!LudeoDataReader_GetObjects.CanRead(name))
                return false;

            if (!LudeoDataReader_GetObjects.IsAttributeExists(name, LudeoDataType.Vec4Float))
                return false;

            return LudeoDataReader_GetObjects.GetAttribute(name, out value);
        }

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterObjectScope).</summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="data">Out parameter for the attribute's value</param>
        /// <returns>true if the value was retrieved successfully, false otherwise.</returns>
        public bool ReadData(string name, out byte[] data)
        {
            data = default;
            if (!LudeoDataReader_GetObjects.CanRead(name))
                return false;

            if (!LudeoDataReader_GetObjects.IsAttributeExists(name, LudeoDataType.Blob))
                return false;

            return LudeoDataReader_GetObjects.GetAttribute(name, out data);
        }

        /// <summary>
        /// Checks if an attribute exists in this LudeoReadableObject. Must be called inside an open read scope.
        /// </summary>
        /// <param name="name">Attribute to search for</param>
        /// <param name="ludeoDataTypeOfAttribute">On return 'true' it will contain the attribute type. Returned as LudeoDataType.</param>
        /// <returns>True if the attribute was found, in which case outType will have the type on exit</returns>
        public bool IsAttributeExists(string name, out LudeoDataType ludeoDataTypeOfAttribute)
        {
            return LudeoDataReader_GetObjects.IsAttributeExists(name, out ludeoDataTypeOfAttribute);
        }

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
                    m_componentHandles.Clear();
                }

                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
