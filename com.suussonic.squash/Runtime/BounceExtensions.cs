using System;
using UnityEngine;

namespace Suussonic.Squash
{
    public static class BounceExtensions
    {
        public static void Bounce(this Transform target, float force, int iterations, float duration,
            BounceAxis axis = BounceAxis.Y, bool unscaledTime = false, Action onBounce = null)
        {
            if (target == null)
            {
                return;
            }

            if (!target.TryGetComponent(out BounceRunner runner))
            {
                runner = target.gameObject.AddComponent<BounceRunner>();
            }

            runner.PlayBounce(force, iterations, duration, axis, unscaledTime, onBounce);
        }

        public static void StopBounce(this Transform target)
        {
            if (target == null)
            {
                return;
            }

            if (target.TryGetComponent(out BounceRunner runner))
            {
                runner.StopBounce();
            }
        }
    }
}
