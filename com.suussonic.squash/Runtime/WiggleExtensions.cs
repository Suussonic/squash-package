using UnityEngine;
using System;

namespace Suussonic.Squash
{
    public static class WiggleExtensions
    {
        public static void Wiggle(this Transform target, float force, int iterations, float duration, WiggleAxis axis = WiggleAxis.Y, bool unscaledTime = false, Action onBounce = null){
            if (target == null)
            {
                return;
            }
            
            if (!target.TryGetComponent(out WiggleRunner runner))
            { 
                runner = target.gameObject.AddComponent<WiggleRunner>();
            }
            
            runner.PlayWiggle(force, iterations, duration, axis, unscaledTime, onBounce);
        }
        
        public static void StopWiggle(this Transform target)
        {
            if (target == null)
            {
                return;
            }
                
            if (target.TryGetComponent(out WiggleRunner runner))
            {
                runner.StopWiggle();
            }
        }
    }
}
