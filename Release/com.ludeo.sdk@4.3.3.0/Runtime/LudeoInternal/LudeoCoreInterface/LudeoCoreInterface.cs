// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    using LudeoSDK.UnityScripts;
    using UnityEngine;

    // [CreateAssetMenu(fileName = "LudeoCoreInterface", menuName = "Ludeo/Create LudeoCoreInterface")]
    internal class LudeoCoreInterface : ScriptableObject, ILudeoCoreInterface
    {
        private readonly UnityWrapperActivationParams ActivateParams = new UnityWrapperActivationParams()
        {
            coreDllName = LudeoCoreDefinitions.DLL_NAME,
            isDestoryUnityObjectImmediate = false,
            mediaCaptureSupport = MediaCapture.MediaCaptureSupport.NewVideo
        };

        private ILudeoInitWrapper ludeoInitWrapper;
        private ILudeoSessionWrapper ludeoSessionWrapper;
        private ILudeoDataReaderWrapper ludeoDataReaderWrapper;
        private ILudeoDataWriterWrapper ludeoDataWriterWrapper;
        private ILudeoGameplaySessionWrapper ludeoGameplaySessionWrapper;
        private ILudeoRoomWrapper ludeoRoomWrapper;
        private ILudeoLogWrapper ludeoLogWrapper;

        public void Reset()
        {
            ludeoInitWrapper = null;
            ludeoSessionWrapper = null;
            ludeoDataReaderWrapper = null;
            ludeoDataWriterWrapper = null;
            ludeoGameplaySessionWrapper = null;
            ludeoRoomWrapper = null;
            ludeoLogWrapper = null;
        }

        public UnityWrapperActivationParams GetWrapperActivationParams()
        {
            return ActivateParams;
        }

        public ILudeoInitWrapper GetLudeoInitWrapper()
        {
            if (ludeoInitWrapper == null)
                ludeoInitWrapper = new LudeoInitWrapper();

            return ludeoInitWrapper;
        }

        public ILudeoSessionWrapper GetLudeoSessionWrapper()
        {
            if (ludeoSessionWrapper == null)
                ludeoSessionWrapper = new LudeoSessionWrapper();
            
            return ludeoSessionWrapper;
        }

        public ILudeoDataReaderWrapper GetLudeoDataReaderWrapper()
        {
            if (ludeoDataReaderWrapper == null)
                ludeoDataReaderWrapper = new LudeoDataReaderWrapper();
            
            return ludeoDataReaderWrapper;
        }

        public ILudeoDataWriterWrapper GetLudeoDataWriterWrapper()
        {
            if (ludeoDataWriterWrapper == null)
                ludeoDataWriterWrapper = new LudeoDataWriterWrapper();

            return ludeoDataWriterWrapper;
        }

        public ILudeoGameplaySessionWrapper GetLudeoGameplaySessionWrapper()
        {
            if (ludeoGameplaySessionWrapper == null)
                ludeoGameplaySessionWrapper = new LudeoGameplaySessionWrapper();

            return ludeoGameplaySessionWrapper;
        }

        public ILudeoRoomWrapper GetLudeoRoomWrapper()
        {
            if (ludeoRoomWrapper == null)
                ludeoRoomWrapper = new LudeoRoomWrapper();

            return ludeoRoomWrapper;
        }

        public ILudeoLogWrapper GetLudeoLogWrapper()
        {
            if (ludeoLogWrapper == null)
                ludeoLogWrapper = new LudeoLogWrapper();

            return ludeoLogWrapper;
        }
    }
}