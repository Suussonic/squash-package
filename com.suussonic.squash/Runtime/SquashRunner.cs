using UnityEngine;
using System;
using System.Collections;

namespace Suussonic.Squash
{ 
    public enum SquashAxis
    {
      X,
      Y,
      Z
    }

  [AddComponentMenu("")]
  public class SquashRunner : MonoBehaviour
  {
    private Vector3 initialScale;
    private Coroutine activeCoroutine;
    private bool isPlaying;


    public void PlaySquash(float force, int iterations, float duration, SquashAxis axis, bool unscaledTime,
      Action onBounce)
    {
      if (duration <= 0 || force == 0)
      {
        return;
      }

      if (!isPlaying)
      {
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
      }
      else
      {
        initialScale = transform.localScale;
        isPlaying = true;
      }

      activeCoroutine = StartCoroutine(SquashCoroutine(force, iterations, duration, axis, unscaledTime, onBounce));
    }

    public void StopSquash()
    {
      if (activeCoroutine != null)
      {
        StopCoroutine(activeCoroutine);
        activeCoroutine = null;
      }

      if (isPlaying)
      {
        transform.localScale = initialScale;
        isPlaying = false;
        activeCoroutine = null;
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

        Vector3 scale = initialScale;

        switch (axis)
        {
          case SquashAxis.X:
            scale.x = initialScale.x * (1f - facteur);
            scale.y = initialScale.y * (1f + facteur * 0.5f);
            scale.z = initialScale.z * (1f + facteur * 0.5f);
            break;

          case SquashAxis.Y:
            scale.y = initialScale.y * (1f - facteur);
            scale.x = initialScale.x * (1f + facteur * 0.5f);
            scale.z = initialScale.z * (1f + facteur * 0.5f);
            break;

          case SquashAxis.Z:
            scale.z = initialScale.z * (1f - facteur);
            scale.x = initialScale.x * (1f + facteur * 0.5f);
            scale.y = initialScale.y * (1f + facteur * 0.5f);
            break;
        }

        transform.localScale = scale;

        yield return null;
      }

      transform.localScale = initialScale;
      isPlaying = false;
      activeCoroutine = null;
    }

  }
}
