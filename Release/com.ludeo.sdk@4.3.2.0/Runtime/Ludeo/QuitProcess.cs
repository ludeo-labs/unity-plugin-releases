
/// @cond INTERNAL
namespace LudeoSDK
{
    using System.Threading.Tasks;
    using UnityEngine;

    internal class QuitProcess
    {
        public static void WaitAndQuitAfterAWhile(LudeoManager ludeoManager, int millisecondsToWait)
        {
            _ = DoWaitAndQuitApplication(ludeoManager, millisecondsToWait);
        }

        private static async Task DoWaitAndQuitApplication(LudeoManager ludeoManager, int millisecondsToWait)
        {
            await Task.Delay(millisecondsToWait);

            Application.Quit();
        }
    }
}
/// @endcond INTERNAL