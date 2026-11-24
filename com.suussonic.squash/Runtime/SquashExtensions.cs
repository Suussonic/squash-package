using UnityEngine;
using System;

namespace Suussonic.Squash
{
    public static class SquashExtensions
    {
        public static void Squash(this Transform target, float force, int iterations, float duration, SquashAxis axis = SquashAxis.Y, bool unscaledTime = false, Action onBounce = null){
            if (target == null)
            {
                return;
            }
            
            if (!target.TryGetComponent(out SquashRunner runner))
            { 
                runner = target.gameObject.AddComponent<SquashRunner>();
            }

            runner.PlaySquash(force, iterations, duration, axis, unscaledTime, onBounce);

        }
        
        public static void StopSquash(this Transform target)
        {
            if (target == null)
            {
                return;
            }
                
            if (target.TryGetComponent(out SquashRunner runner))
            {
                runner.StopSquash();
            }
        }
    }
}
