// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.NativeCode
{
    using LudeoSDK.Internal;
    using LudeoSDK.Ludeo.Internal.Launchers.Gameface;
    using LudeoSDK.Ludeo.Internal.Launchers.Nvidia;
    using LudeoSDK.MediaCapture;
    using UnityEngine;

    internal class LudeoInterfaceFactory : MonoBehaviour, IInterfaceFactory
    {
        private IOSSystems m_osSystems;
        private INativeHelperMethods m_nativeHelperMethods;

        public INvidiaExtern CreateINvidiaExtern()
        {
            return new NvidiaExtern();
        }

        public IGamefaceExtern CreateIGamefaceExtern() 
        {
            return new GamefaceExtern();
        }
        
        public IOSSystems GetIOSSystems()
        {
            if (m_osSystems == null)
                m_osSystems = new OSSystems();
            return m_osSystems;
        }

        public INativeHelperMethods GetNativeHelperMethods()
        {
            if (m_nativeHelperMethods == null)
                m_nativeHelperMethods = new NativeHelperMethods();
            return m_nativeHelperMethods;
        }

        #region Video Interfaces
        public INativePipeClient CreateINativePipeClient()
        {
            return new NativePipeClient();
        }

        public INativePipeServer CreateINativePipeServer()
        {
            return new NativePipeServer();
        }

        public INativeSemaphore CreateINativeSemaphore()
        {
            return new NativeSemaphore();
        }

        public IProcessRunner CreateIProcessRunner()
        {
            return new ProcessRunner();
        }
        #endregion Video Interfaces
    }
}