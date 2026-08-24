// Copyright 2024 Ludeo. All rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace LudeoSDK
{
    /// <summary>
    /// Base class for Ludeo restore objects.
    /// DO NOT use directly.
    /// </summary>
    public class LudeoAttributesCollection
    {
        private const int INITIAL_DICTIONARY_CAPACITY = 5;

        internal Dictionary<string, int> m_ludeoInt32Attributes = new Dictionary<string, int>(INITIAL_DICTIONARY_CAPACITY);
        internal Dictionary<string, float> m_ludeoFloatAttributes = new Dictionary<string, float>(INITIAL_DICTIONARY_CAPACITY);
        internal Dictionary<string, double> m_ludeoDoubleAttributes = new Dictionary<string, double>(INITIAL_DICTIONARY_CAPACITY);
        internal Dictionary<string, bool> m_ludeoBoolAttributes = new Dictionary<string, bool>(INITIAL_DICTIONARY_CAPACITY);
        internal Dictionary<string, Vector3> m_ludeoVector3Attributes = new Dictionary<string, Vector3>(INITIAL_DICTIONARY_CAPACITY);
        internal Dictionary<string, Quaternion> m_ludeoQuaternionAttributes = new Dictionary<string, Quaternion>(INITIAL_DICTIONARY_CAPACITY);
        internal Dictionary<string, string> m_ludeoStringAttributes = new Dictionary<string, string>(INITIAL_DICTIONARY_CAPACITY);
        internal Dictionary<string, Tuple<uint, byte[]>> m_ludeoBlobAttributes = new Dictionary<string, Tuple<uint, byte[]>>(INITIAL_DICTIONARY_CAPACITY);

        internal Dictionary<string, LudeoReadableComponent> m_ludeoStateComponentRestores = new Dictionary<string, LudeoReadableComponent>();

        /// <summary>
        /// Get all the integer attributes available for restoration from this object
        /// </summary>
        /// <param name="attributes">The collection of string-int key-values</param>
        public void GetAllAttributes(out Dictionary<string, int> attributes) { attributes = m_ludeoInt32Attributes; }

        /// <summary>
        /// Get all the float attributes available for restoration from this object
        /// </summary>
        /// <param name="attributes">The collection of string-float key-values</param>
        public void GetAllAttributes(out Dictionary<string, float> attributes) { attributes = m_ludeoFloatAttributes; }

        /// <summary>
        /// Get all the double attributes available for restoration from this object
        /// </summary>
        /// <param name="attributes">The collection of string-double key-values</param>
        public void GetAllAttributes(out Dictionary<string, double> attributes) { attributes = m_ludeoDoubleAttributes; }

        /// <summary>
        /// Get all the bool attributes available for restoration from this object
        /// </summary>
        /// <param name="attributes">The collection of string-bool key-values</param>
        public void GetAllAttributes(out Dictionary<string, bool> attributes) { attributes = m_ludeoBoolAttributes; }

        /// <summary>
        /// Get all the Vector3 attributes available for restoration from this object
        /// </summary>
        /// <param name="attributes">The collection of string-Vector3 key-values</param>
        public void GetAllAttributes(out Dictionary<string, Vector3> attributes) { attributes = m_ludeoVector3Attributes; }

        /// <summary>
        /// Get all the Quaternion attributes available for restoration from this object
        /// </summary>
        /// <param name="attributes">The collection of string-Quaternion key-values</param>
        public void GetAllAttributes(out Dictionary<string, Quaternion> attributes) { attributes = m_ludeoQuaternionAttributes; }

        /// <summary>
        /// Get all the string attributes available for restoration from this object
        /// </summary>
        /// <param name="attributes">The collection of string-string key-values</param>
        public void GetAllAttributes(out Dictionary<string, string> attributes) { attributes = m_ludeoStringAttributes; }

        /// <summary>
        /// Get all the LudeoStateComponentRestores available for restoration from this object
        /// </summary>
        /// <param name="attributes">The collection of string-LudeoReadableComponent key-values</param>
        public void GetAllComponents(out Dictionary<string, LudeoReadableComponent> attributes) { attributes = m_ludeoStateComponentRestores; }

        /// <summary>
        /// Get all the blobs ([size - data] pairs stored in Tuple) available for restoration from this object
        /// </summary>
        /// <param name="attributes">The collection of string-Tuple key-values</param>
        public void GetAllComponents(out Dictionary<string, Tuple<uint, byte[]>> attributes) { attributes = m_ludeoBlobAttributes; } 

        internal void ClearAttributes()
        {
            m_ludeoInt32Attributes.Clear();
            m_ludeoFloatAttributes.Clear();
            m_ludeoDoubleAttributes.Clear();
            m_ludeoBoolAttributes.Clear();
            m_ludeoVector3Attributes.Clear();
            m_ludeoQuaternionAttributes.Clear();
            m_ludeoStringAttributes.Clear();
            m_ludeoStateComponentRestores.Clear();
            m_ludeoBlobAttributes.Clear();
        }

        internal void AddAttribute(string name, int value)
        {
            m_ludeoInt32Attributes.Add(name, value);
        }
        internal void AddAttribute(string name, float value)
        {
            m_ludeoFloatAttributes.Add(name, value);
        }
        internal void AddAttribute(string name, double value)
        {
            m_ludeoDoubleAttributes.Add(name, value);
        }
        internal void AddAttribute(string name, bool value)
        {
            m_ludeoBoolAttributes.Add(name, value);
        }
        internal void AddAttribute(string name, Vector3 value)
        {
            m_ludeoVector3Attributes.Add(name, value);
        }
        internal void AddAttribute(string name, Quaternion value)
        {
            m_ludeoQuaternionAttributes.Add(name, value);
        }
        internal void AddAttribute(string name, string value)
        {
            m_ludeoStringAttributes.Add(name, value);
        }
        internal void AddAttribute(string name, byte[] data, uint size)
        {
            m_ludeoBlobAttributes.Add(name, new Tuple<uint, byte[]>(size, data));
        }
        internal void AddComponent(string name, LudeoReadableComponent ludeoStateComponentRestore)
        {
            m_ludeoStateComponentRestores.Add(name, ludeoStateComponentRestore);
        }

    }
}
