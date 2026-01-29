using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    [Serializable]
    public class Timer
    {
        public event Action OnTimerEnd;

        public bool enabled = true;

        public bool randomInterval;
        
        [ShowIf("randomInterval"), Min(0)] public float minInterval;
        [ShowIf("randomInterval"), MinValue("@minInterval")] public float maxInterval;
        
        [HideIf("randomInterval")] public float interval;
        
        
        public bool repeat = false;
        protected float timeLeft;
        
        public bool HasEnded => timeLeft <= 0;

        public Timer(float interval = 1)
        {
            this.interval = interval;
            Reset();
        }

        public void Reset(bool accumulativeTime = false)
        {
            enabled = true;
            timeLeft = (randomInterval ? Random.Range(minInterval, maxInterval) : interval) 
                       + (accumulativeTime ? timeLeft : 0);
        }

        public void Update(float deltaTime)
        {
            if (!enabled) return;
            
            timeLeft -= deltaTime;
            
            if (HasEnded)
            {
                OnTimerEnd?.Invoke();

                if (repeat)
                    Reset(true);
                else
                    enabled = false;
            }
        }
    }
}
