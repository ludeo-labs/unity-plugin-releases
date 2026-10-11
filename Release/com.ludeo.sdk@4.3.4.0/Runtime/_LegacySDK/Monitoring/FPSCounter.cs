using System;

/// @cond INTERNAL
namespace LudeoSDK
{
    internal class FPSCounter
    {
        const float fpsMeasurePeriod = 1.0f;

        private float m_fpsAccumulator = 0;
        private float m_fpsNextPeriod = 0;
        private float m_currentFps;

        public float Fps
        {
            get
            {
                return m_currentFps;
            }
        }

        private FPSCounter() { }

        public FPSCounter(float timeSinceStartup)
        {
            m_fpsNextPeriod = timeSinceStartup + fpsMeasurePeriod;
        }

        public void Update(float timeSinceStartup)
        {
            m_fpsAccumulator++;

            if (timeSinceStartup > m_fpsNextPeriod)
            {
                m_currentFps = m_fpsAccumulator / fpsMeasurePeriod;
                m_fpsAccumulator = 0;
                m_fpsNextPeriod += fpsMeasurePeriod;
            }
        }
    }
}
/// @endcond INTERNAL