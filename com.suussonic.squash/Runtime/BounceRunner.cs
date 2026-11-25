using System;
using System.Collections;
using UnityEngine;

namespace Suussonic.Squash
{
    public enum BounceAxis
    {
        X,
        Y,
        Z,
        All
    }

    [AddComponentMenu("")]
    public class BounceRunner : MonoBehaviour
    {
        private Vector3 _initialPosition;
        private Coroutine _activeCoroutine;
        private bool _isPlaying;
        private int _playId;

        public void PlayBounce(float force, int iterations, float duration, BounceAxis axis, bool unscaledTime,
            Action onBounce)
        {
            if (duration <= 0 || force == 0)
            {
                return;
            }

            if (_isPlaying)
            {
                if (_activeCoroutine != null) StopCoroutine(_activeCoroutine);
            }
            else
            {
                _initialPosition = transform.position;
                _isPlaying = true;
            }

            int playId = ++_playId;
            Coroutine coroutine = StartCoroutine(BounceCoroutine(force, iterations, duration, axis, unscaledTime,
                onBounce, playId));
            if (_playId == playId && _isPlaying)
            {
                _activeCoroutine = coroutine;
            }
        }

        public void StopBounce()
        {
            _playId++;
            if (_activeCoroutine != null)
            {
                StopCoroutine(_activeCoroutine);
                _activeCoroutine = null;
            }

            if (_isPlaying)
            {
                transform.position = _initialPosition;
                _isPlaying = false;
            }
        }

        private void OnDisable()
        {
            StopBounce();
        }

        private IEnumerator BounceCoroutine(float force, int iterations, float duration, BounceAxis axis,
            bool unscaledTime, Action onBounce, int playId)
        {
            float passedTime = 0f;
            int lastHalfCycle = 0;

            while (passedTime < duration)
            {
                passedTime += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                float progression = Mathf.Min(passedTime / duration, 1f);
                float angle = progression * iterations * 2f * Mathf.PI;
                float offset = Mathf.Sin(angle) * force * (1f - progression);

                int halfCycle = Mathf.FloorToInt(progression * iterations * 2);
                if (halfCycle > lastHalfCycle)
                {
                    lastHalfCycle = halfCycle;
                    onBounce?.Invoke();
                    if (_playId != playId) yield break;
                }

                Vector3 positionOffset = Vector3.zero;
                switch (axis)
                {
                    case BounceAxis.X:
                        positionOffset.x = offset;
                        break;
                    case BounceAxis.Y:
                        positionOffset.y = offset;
                        break;
                    case BounceAxis.Z:
                        positionOffset.z = offset;
                        break;
                    case BounceAxis.All:
                        positionOffset = new Vector3(offset, offset, offset);
                        break;
                }

                transform.position = _initialPosition + positionOffset;
                yield return null;
            }

            transform.position = _initialPosition;
            _isPlaying = false;
            _activeCoroutine = null;
        }
    }
}
