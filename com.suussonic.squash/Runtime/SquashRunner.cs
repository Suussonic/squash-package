using UnityEngine;
using System;
using System.Collections;

namespace Suussonic.Squash
{ 
    public enum SquashAxis
    {
      X,
      Y,
      Z,
      All
    }

  [AddComponentMenu("")]
  public class SquashRunner : MonoBehaviour
  {
    private Vector3 _initialScale;
    private Coroutine _activeCoroutine;
    private bool _isPlaying = false;


    public void PlaySquash(float force, int iterations, float duration, SquashAxis axis, bool unscaledTime,
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
        _initialScale = transform.localScale;
        _isPlaying = true;
      }

      _activeCoroutine = StartCoroutine(SquashCoroutine(force, iterations, duration, axis, unscaledTime, onBounce));
    }

    public void StopSquash()
    {
      if (_activeCoroutine != null)
      {
        StopCoroutine(_activeCoroutine);
        _activeCoroutine = null;
      }

      if (_isPlaying)
      {
        transform.localScale = _initialScale;
        _isPlaying = false;
        _activeCoroutine = null;
      }
    }

    private void OnDisable()
    {
      StopSquash();
    }

    private IEnumerator SquashCoroutine(float force, int iterations, float duration, SquashAxis axis, bool unscaledTime,
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
        float facteur = Mathf.Clamp(Mathf.Sin(angle) * force * amortissement, -0.95f, 0.95f);

        int halfcycle = Mathf.FloorToInt(progression * iterations * 2);
        if (halfcycle > LastHalfCycle)
        {
          LastHalfCycle = halfcycle;
          onBounce?.Invoke();
        }

        Vector3 scale = _initialScale;

        switch (axis)
        {
          case SquashAxis.X:
            scale.x = _initialScale.x * (1f - facteur);
            scale.y = _initialScale.y * (1f + facteur * 0.5f);
            scale.z = _initialScale.z * (1f + facteur * 0.5f);
            break;

          case SquashAxis.Y:
            scale.y = _initialScale.y * (1f - facteur);
            scale.x = _initialScale.x * (1f + facteur * 0.5f);
            scale.z = _initialScale.z * (1f + facteur * 0.5f);
            break;

          case SquashAxis.Z:
            scale.z = _initialScale.z * (1f - facteur);
            scale.x = _initialScale.x * (1f + facteur * 0.5f);
            scale.y = _initialScale.y * (1f + facteur * 0.5f);
            break;
          
          case SquashAxis.All:
            scale = _initialScale * (1f - facteur);
            break;
        }

        transform.localScale = scale;

        yield return null;
      }

      transform.localScale = _initialScale;
      _isPlaying = false;
      _activeCoroutine = null;
    }

  }
}
