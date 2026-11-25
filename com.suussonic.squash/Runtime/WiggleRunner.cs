using UnityEngine;
using System;
using System.Collections;

namespace Suussonic.Squash
{ 
    public enum WiggleAxis
    {
      X,
      Y,
      Z,
      All
    }

  [AddComponentMenu("")]
  public class WiggleRunner : MonoBehaviour
  {
    private Quaternion _initialRotation;
    private Coroutine _activeCoroutine;
    private bool _isPlaying = false;


    public void PlayWiggle(float force, int iterations, float duration, WiggleAxis axis, bool unscaledTime,
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
        _initialRotation = transform.rotation;
        _isPlaying = true;
      }

      _activeCoroutine = StartCoroutine(WiggleCoroutine(force, iterations, duration, axis, unscaledTime, onBounce));
    }

    public void StopWiggle()
    {
      if (_activeCoroutine != null)
      {
        StopCoroutine(_activeCoroutine);
        _activeCoroutine = null;
      }

      if (_isPlaying)
      {
        transform.rotation = _initialRotation;
        _isPlaying = false;
        _activeCoroutine = null;
      }
    }

    private void OnDisable()
    {
      StopWiggle();
    }

    private IEnumerator WiggleCoroutine(float force, int iterations, float duration, WiggleAxis axis, bool unscaledTime,
      Action onBounce)
    {
      float PassedTime = 0f;
      int LastHalfCycle = 0;

      while (PassedTime < duration)
      {
        float dt = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        PassedTime += dt;
        float progression = Mathf.Min(PassedTime / duration, 1);

        float angle = progression * iterations * 2 * Mathf.PI;
        float amortissement = 1 - progression;
        float angleOffset = Mathf.Sin(angle) * force * amortissement;

        int halfcycle = Mathf.FloorToInt(progression * iterations * 2);
        if (halfcycle > LastHalfCycle)
        {
          LastHalfCycle = halfcycle;
          onBounce?.Invoke();
        }

        Vector3 rotationOffset = Vector3.zero;

        switch (axis)
        {
          case WiggleAxis.X:
            rotationOffset.x = angleOffset;
            break;

          case WiggleAxis.Y:
            rotationOffset.y = angleOffset;
            break;

          case WiggleAxis.Z:
            rotationOffset.z = angleOffset;
            break;
          
          case WiggleAxis.All:
            rotationOffset = new Vector3(angleOffset, angleOffset, angleOffset);
            break;
        }

        transform.rotation = Quaternion.Euler(rotationOffset) * _initialRotation;

        yield return null;
      }

      transform.rotation = _initialRotation;
      _isPlaying = false;
      _activeCoroutine = null;
    }

  }
}
