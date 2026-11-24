using UnityEngine.InputSystem;
using UnityEngine;
using Suussonic.Squash;

public class SquashTester : MonoBehaviour {
        
    [SerializeField] private float force = 0.4f; 
    [SerializeField] private int iteration = 2; 
    [SerializeField] private float duration = 0.5f; 
    [SerializeField] private SquashAxis axis = SquashAxis.Y;
    
    private void Update() {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) {
            transform.Squash(force, iteration, duration, axis);
        } 
    }
}

