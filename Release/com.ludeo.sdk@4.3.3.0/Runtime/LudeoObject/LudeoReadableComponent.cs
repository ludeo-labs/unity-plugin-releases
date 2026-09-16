// Copyright 2024 Ludeo. All rights reserved.

using LudeoSDK.Internal;
using UnityEngine;

namespace LudeoSDK
{
    /// <summary>
    /// Used to read attributes of a LudeoWritableComponent of a parent LudeoWritableObject.
    /// Reads go straight to the core data reader - wrap them in a using (componentRestore.EnterComponentScope()) scope,
    /// opened inside the parent object's read scope.
    /// </summary>
    public class LudeoReadableComponent : LudeoRestoreBase
    {
        private uint m_objectId;

        /// <summary>The parent LudeoWritableObject of this LudeoWritableComponent</summary>
        public string ParentObject { get { return m_parentObjectType; } }

        /// <summary>The componentName as given when creating this with LudeoWritableObject.CreateOrGetComponent</summary>
        public string Name { get { return m_compName; } }

        private string m_compName;
        private string m_parentObjectType;

        internal override uint RestoreObjectId { get { return m_objectId; } }

        internal LudeoReadableComponent(string componentName, uint objectId)
        {
            m_compName = componentName;
            m_objectId = objectId;
        }

        /// <summary>
        /// Opens a read scope for this component. It must be opened inside the parent object's read scope:
        /// <code>
        /// using (readableObject.EnterObjectScope())
        /// using (readableComponent.EnterComponentScope())
        /// {
        ///     readableComponent.ReadData("ammo", out int ammo);
        /// }
        /// </code>
        /// </summary>
        /// <returns>A disposable <see cref="LudeoReadScope"/>; dispose it (end the using block) when done.</returns>
        public LudeoReadScope EnterComponentScope()
        {
            bool entered = LudeoDataReader_GetObjects.EnterComponent(m_compName);
            return new LudeoReadScope(isComponent: true, entered: entered);
        }

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterComponentScope).</summary>
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

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterComponentScope).</summary>
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

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterComponentScope).</summary>
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

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterComponentScope).</summary>
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

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterComponentScope).</summary>
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

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterComponentScope).</summary>
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

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterComponentScope).</summary>
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

        /// <summary>Gets the given attribute if it exists. Must be called inside an open read scope (EnterComponentScope).</summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="data">Out parameter for the attribute's data</param>
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
        /// Checks if an attribute exists in this LudeoReadableComponent. Must be called inside an open read scope.
        /// </summary>
        /// <param name="name">Attribute to search for</param>
        /// <param name="typeOfAttribute">On return 'true' it will contain the attribute type.</param>
        /// <returns>True if the attribute was found, in which case outType will have the type on exit</returns>
        public bool IsAttributeExists(string name, out LudeoDataType typeOfAttribute)
        {
            return LudeoDataReader_GetObjects.IsAttributeExists(name, out typeOfAttribute);
        }
    }
}
