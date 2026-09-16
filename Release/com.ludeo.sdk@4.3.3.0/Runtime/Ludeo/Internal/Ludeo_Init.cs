/// @cond INTERNAL
namespace LudeoSDK.Internal
{
    using System;
    using UnityEngine;

    internal static partial class LudeoInit
    {
        private const string LOG_PREFIX = "[INIT_WRPR]";

        internal static LudeoResult Initialize(ILudeoInitWrapper initWrapper, LudeoInitializeParams initializeParams)
        {
            LudeoResult ludeoResult = LudeoResult.Unknown;
            LudeoResult logInitResult = LudeoResult.Unknown;

            try
            {
                ludeoResult = initWrapper.LudeoInitialize(ref initializeParams);

                logInitResult = LudeoLogManager.InitNativeHook();
            }
            catch(DllNotFoundException e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} {LudeoManager.CoreDllName} was not found! {e}");
                throw new DllNotFoundException($"{LOG_PREFIX} {LudeoManager.CoreDllName} was not found! {e}");
            }
            catch(Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} {e}");
                throw new Exception($"{LOG_PREFIX} {e}");
            }
            finally
            {
                LudeoLogManager.LogInfo($"{LOG_PREFIX} logInitResult = {logInitResult}");
            }

            return ludeoResult;
        }

        internal static void Tick(ILudeoInitWrapper initWrapper)
        {
            initWrapper.LudeoTick(); // Are there any exception firing in Core for this call?
        }

        unsafe public static void RemoveNotifications(ILudeoInitWrapper initWrapper, ulong* notificationsIds, int bufferSize)
        {
            for(int i=0; i < bufferSize; ++i)
                initWrapper.RemoveNotification(notificationsIds[i]);
        }

        internal static void LudeoCommand(ILudeoInitWrapper initWrapper, string command, string value)
        {
            initWrapper.LudeoCommand(command, value);
        }

        internal static LudeoResult Shutdown(ILudeoInitWrapper initWrapper)
        {
            LudeoResult ludeoResult = LudeoResult.Unknown;

            LudeoLogManager.LogVerbose($"{LOG_PREFIX} Calling LudeoShutdown!");

            try
            {
                ludeoResult = initWrapper.LudeoShutdown();
            }
            catch (DllNotFoundException e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} {LudeoManager.CoreDllName} was not found! {e}");
                throw new DllNotFoundException($"{LOG_PREFIX} {LudeoManager.CoreDllName} was not found! {e}");
            }
            catch (Exception e)
            {
                LudeoLogManager.LogError($"{LOG_PREFIX} {e}");
                throw new Exception($"{LOG_PREFIX} {e}");
            }

            return ludeoResult;
        }

        //unsafe internal static void RemoveNotifications(ILudeoInitWrapper initWrapper, ulong* notificationIds, int bufferSize)
        //{
        //    try
        //    {
        //        for (int i = 0; i < bufferSize; ++i)
        //            initWrapper.RemoveNotification(notificationIds[i]);
        //    }
        //    catch (DllNotFoundException e)
        //    {
        //        Debug.LogError($"{LOG_PREFIX} {LudeoDefinitions.DLL_NAME} was not found!");
        //        throw new DllNotFoundException($"{LOG_PREFIX} {LudeoDefinitions.DLL_NAME} was not found!");
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError($"{LOG_PREFIX} {e}");
        //        throw new Exception($"{LOG_PREFIX} {e}");
        //    }            
        //}
    }
}
/// @endcond INTERNAL