// Copyright 2024 Ludeo. All rights reserved.

/// @cond INTERNAL
namespace LudeoSDK.UnityScripts
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    public class LudeoUnityMainThreadDispatcher : MonoBehaviour
    {
        private static object m_lockObject = new object();
        private static LudeoUnityMainThreadDispatcher m_instance;
        private static readonly Queue<Action> m_executionQueue = new Queue<Action>();

        #region ------------------ UNITY METHODS ------------------
        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            Clear();
        }

        private void Update()
        {
            lock (m_lockObject)
            {
                while (m_executionQueue.Count > 0)
                {
                    m_executionQueue.Dequeue().Invoke();
                }
            }
        }
        #endregion ------------------ UNITY METHODS ------------------

        public static void Enqueue(Action action)
        {
            lock (m_lockObject)
            {
                m_executionQueue.Enqueue(action);
            }
        }

        private void Clear()
        {
            lock (m_lockObject)
            {
                m_executionQueue.Clear();
            }
        }
    }
}
/// @endcond INTERNAL