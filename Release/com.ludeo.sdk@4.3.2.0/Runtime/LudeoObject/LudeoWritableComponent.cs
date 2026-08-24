// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK
{
    using UnityEngine;
    using Internal;
    using System;

    /// <summary>
    /// LudeoWritableComponent is a nested context to an existing parent LudeoWritableObject.
    /// You may use multiple LudeoStateComponents to give a nesting context to your data.
    /// </summary>
    public class LudeoWritableComponent
    {
        /// <summary>The parent LudeoWritableObject of this LudeoWritableComponent</summary>
        public string ParentObject { get { return m_parentObjectType; } }

        /// <summary>The componentName as given when creating this with LudeoWritableObject.CreateOrGetComponent</summary>
        public string Name { get { return m_compName; } }

        private string m_compName;
        private string m_parentObjectType;
        private uint m_parentObjectId;

        internal LudeoWritableComponent(string componentName, string parentObjectType, uint parentObjectId)
        {
            m_compName = componentName;
            m_parentObjectType = parentObjectType;
            m_parentObjectId = parentObjectId;
        }

        /// <summary>
        /// Opens a write scope for this component. It must be opened inside the parent object's write scope:
        /// <code>
        /// using (writableObject.EnterObjectScope())
        /// using (writableComponent.EnterComponentScope())
        /// {
        ///     writableComponent.WriteData("ammo", 30);
        /// }
        /// </code>
        /// </summary>
        /// <returns>A disposable <see cref="LudeoWriteScope"/>; dispose it (end the using block) when done.</returns>
        public LudeoWriteScope EnterComponentScope()
        {
            bool entered = LudeoDataWriter.EnterComponent(m_compName);
            return new LudeoWriteScope(isComponent: true, entered: entered);
        }

        #region PUBLIC API

       
        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterComponentScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, int value)
        {
            LudeoDataWriter.SetInt32(name, value);
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterComponentScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, float value)
        {
            LudeoDataWriter.SetFloat(name, value);
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterComponentScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, double value)
        {
            LudeoDataWriter.SetDouble(name, value);
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterComponentScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, bool value)
        {
            LudeoDataWriter.SetBool(name, value);
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterComponentScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, Vector3 value)
        {
            LudeoDataWriter.SetVec3Float(name, new FixedVector3(value).Values);
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterComponentScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, Quaternion value)
        {
            LudeoDataWriter.SetVec4Float(name, new FixedVector4(value).Values);
        }

        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterComponentScope).
        /// The value is written immediately to the data writer; the core SDK caches it and only sends changed values on its next internal tick.
        /// </summary>
        /// <param name="name">Key to set</param>
        /// <param name="value">Value for the key</param>
        public void WriteData(string name, string value)
        {
            LudeoDataWriter.SetString(name, value);
        }


        /// <summary>
        /// Sets the attribute value. Must be called inside an open write scope (see EnterComponentScope).
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
        #endregion PUBLIC API
    }
}