// Copyright 2024 Ludeo. All rights reserved.

using LudeoSDK.Ludeo.Internal.Launchers.Gameface;
using LudeoSDK.Ludeo.Internal.Launchers.Nvidia;
using LudeoSDK.MediaCapture;

/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    public interface IInterfaceFactory
    {
        IOSSystems GetIOSSystems();

        INativeHelperMethods GetNativeHelperMethods();

        INvidiaExtern CreateINvidiaExtern();

        IGamefaceExtern CreateIGamefaceExtern();

        #region Video Interfaces
        INativePipeServer CreateINativePipeServer();

        INativePipeClient CreateINativePipeClient();

        INativeSemaphore CreateINativeSemaphore();

        IProcessRunner CreateIProcessRunner();
        #endregion Video Interfaces
    }

    internal class DummyInterfaceFactory : IInterfaceFactory
    {
        public IOSSystems GetIOSSystems()
        {
            return new DummyOSSystems();
        }

        public INativeHelperMethods GetNativeHelperMethods()
        {
            return new DummyNativeHelperMethods();
        }

        public INvidiaExtern CreateINvidiaExtern()
        {
            return new DummyNvidiaLauncher();
        }

        public IGamefaceExtern CreateIGamefaceExtern()
        {
            return new DummyGamefaceLauncher();
        }

        #region Video Interfaces
        public INativePipeClient CreateINativePipeClient()
        {
            return new DummyNativePipeClient();
        }

        public INativePipeServer CreateINativePipeServer()
        {
            return new DummyNativePipeServer();
        }

        public INativeSemaphore CreateINativeSemaphore()
        {
            return new DummyNativeSemaphore();
        }

        public IProcessRunner CreateIProcessRunner()
        {
            return new DummyProcessRunner();
        }
        #endregion Video Interfaces
    }
}
/// @endcond INTERNAL