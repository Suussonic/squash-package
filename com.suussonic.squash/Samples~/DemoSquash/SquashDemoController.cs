using UnityEngine;
using Suussonic.Squash;

public class SquashDemoController : MonoBehaviour
{
  [SerializeField] Transform cubeX, cubeY, cubeZ;
  [SerializeField] Transform pumpTarget;
  [SerializeField] Transform wiggleTarget, bounceTarget;
  [SerializeField] Transform slimeTarget;
  [SerializeField] MeshRenderer slimeRenderer;
  [SerializeField] AudioSource slimeAudio;
  [SerializeField] Transform uiButtonTarget;

  public void TestSquashAxes()
  {
    cubeX.Squash(force: 0.4f, iterations: 2, duration: 0.5f, axis: SquashAxis.X);
    cubeY.Squash(force: 0.4f, iterations: 2, duration: 0.5f, axis: SquashAxis.Y);
    cubeZ.Squash(force: 0.4f, iterations: 2, duration: 0.5f, axis: SquashAxis.Z);
  }

  public void TestPump()
  {
    pumpTarget.Pump(force: 0.35f, iterations: 2, duration: 0.5f);
  }

  public void TestWiggle()
  {
    wiggleTarget.Wiggle(force: 15f, iterations: 4, duration: 0.5f);
  }

  public void TestBounce()
  {
    bounceTarget.Bounce(force: 1.5f, iterations: 3, duration: 0.8f);
  }

  public void TestComboSlime()
  {
    slimeTarget.Bounce(force: 1.2f, iterations: 3, duration: 0.9f);
    slimeTarget.Squash(
      force: 0.45f,
      iterations: 3,
      duration: 0.9f,
      axis: SquashAxis.Y,
      onBounce: () => AppliquerFeedbackRebond()
    );
  }

  public void BasculerPause()
  {
    if (Time.timeScale == 0)
    {
      Time.timeScale = 1;
    }
    else
    {
      Time.timeScale = 0;
    }
  }

  public void TestUIPause()
  {
    uiButtonTarget.Pump(force: 0.25f, iterations: 2, duration: 0.4f, unscaledTime: true);
  }

  public void ToutArreter()
  {
    cubeX.StopSquash();
    cubeY.StopSquash();
    cubeZ.StopSquash();
    pumpTarget.StopSquash();
    wiggleTarget.StopSquash();
    bounceTarget.StopSquash();
    uiButtonTarget.StopSquash();
    slimeTarget.StopSquash();
  }

  private void AppliquerFeedbackRebond()
  {
    if (slimeAudio != null)
    {
      slimeAudio.Play();
    }

    if (slimeRenderer != null)
    {
      Color randomColor = new Color(Random.value, Random.value, Random.value);
      slimeRenderer.material.color = randomColor;
    }
  }
}