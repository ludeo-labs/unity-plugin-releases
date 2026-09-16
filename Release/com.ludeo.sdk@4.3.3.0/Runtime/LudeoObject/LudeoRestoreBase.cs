// Copyright 2024 Ludeo. All rights reserved.

using LudeoSDK.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LudeoSDK
{
    /// <summary>
    /// Base class for Ludeo restore objects (<see cref="LudeoReadableObject"/> and
    /// <see cref="LudeoReadableComponent"/>). DO NOT use directly.
    ///
    /// The GetAll* methods read on demand from the core data reader and build a fresh collection each call;
    /// they must be called inside an open read scope (see EnterObjectScope / EnterComponentScope on the derived types). There is no managed
    /// cache - the core data reader is the single source of truth.
    /// </summary>
    public abstract class LudeoRestoreBase
    {
        /// <summary>The object id used to tag any components discovered while visiting the current context.</summary>
        internal abstract uint RestoreObjectId { get; }

        private LudeoAttributesCollection ReadCurrentContext()
        {
            LudeoAttributesCollection collection = new LudeoAttributesCollection();
            LudeoDataReader_GetObjects.ReadCurrentContextInto(collection, RestoreObjectId);
            return collection;
        }

        /// <summary>
        /// Get every attribute and component available in the current read scope in a single native traversal.
        /// Prefer this over calling the individual GetAllAttributes(out Dictionary&lt;...&gt;) overloads more than
        /// once, since each of those re-walks the whole context and discards everything but the requested type.
        /// </summary>
        public void GetAllAttributes(out LudeoAttributesCollection attributes) { attributes = ReadCurrentContext(); }

        /// <summary>Get all the integer attributes available in the current read scope.</summary>
        public void GetAllAttributes(out Dictionary<string, int> attributes) { attributes = ReadCurrentContext().m_ludeoInt32Attributes; }

        /// <summary>Get all the float attributes available in the current read scope.</summary>
        public void GetAllAttributes(out Dictionary<string, float> attributes) { attributes = ReadCurrentContext().m_ludeoFloatAttributes; }

        /// <summary>Get all the double attributes available in the current read scope.</summary>
        public void GetAllAttributes(out Dictionary<string, double> attributes) { attributes = ReadCurrentContext().m_ludeoDoubleAttributes; }

        /// <summary>Get all the bool attributes available in the current read scope.</summary>
        public void GetAllAttributes(out Dictionary<string, bool> attributes) { attributes = ReadCurrentContext().m_ludeoBoolAttributes; }

        /// <summary>Get all the Vector3 attributes available in the current read scope.</summary>
        public void GetAllAttributes(out Dictionary<string, Vector3> attributes) { attributes = ReadCurrentContext().m_ludeoVector3Attributes; }

        /// <summary>Get all the Quaternion attributes available in the current read scope.</summary>
        public void GetAllAttributes(out Dictionary<string, Quaternion> attributes) { attributes = ReadCurrentContext().m_ludeoQuaternionAttributes; }

        /// <summary>Get all the string attributes available in the current read scope.</summary>
        public void GetAllAttributes(out Dictionary<string, string> attributes) { attributes = ReadCurrentContext().m_ludeoStringAttributes; }

        /// <summary>Get all the components available in the current read scope.</summary>
        public void GetAllComponents(out Dictionary<string, LudeoReadableComponent> attributes) { attributes = ReadCurrentContext().m_ludeoStateComponentRestores; }

        /// <summary>Get all the blobs ([size - data] pairs stored in Tuple) available in the current read scope.</summary>
        public void GetAllComponents(out Dictionary<string, Tuple<uint, byte[]>> attributes) { attributes = ReadCurrentContext().m_ludeoBlobAttributes; }
    }
}
