// Copyright 2024 Ludeo. All rights reserved.

namespace LudeoSDK.Internal
{
    internal partial class LudeoInitWrapper : ILudeoInitWrapper
    {
        public LudeoResult LudeoInitialize(ref LudeoInitializeParams parameters)
        {
            return ludeo_Initialize(ref parameters);
        }

        public void LudeoTick()
        {
            ludeo_Tick();
        }

        public LudeoResult LudeoShutdown()
        {
            return ludeo_Shutdown();
        }

        public void RemoveNotification(ulong notificationId)
        {
            ludeo_RemoveNotification(notificationId);
        }

        public LudeoResult LudeoCommand(string name, string value)
        {
            return ludeo_Command(name, value);
        }
    }
}