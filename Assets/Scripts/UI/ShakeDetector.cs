using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    public float shakeDetectionThreshold = 2.0f; // Sensitivity for detecting a shake
    public float shakeCooldownTime = 2.0f;       // Cooldown time between shakes

    private float timeSinceLastShake = 0.0f;
    private Vector3 acceleration;
    private Vector3 lastAcceleration;

    // Property to check if a shake is detected
    public bool IsShakeDetected { get; private set; }

    void Start()
    {
        // Initialize acceleration with the current device acceleration
        acceleration = Input.acceleration;
        lastAcceleration = acceleration;
        IsShakeDetected = false;
    }

    void Update()
    {
        timeSinceLastShake += Time.deltaTime;

        // Update current and last accelerations
        lastAcceleration = acceleration;
        acceleration = Input.acceleration;

        // Calculate the change in acceleration
        Vector3 deltaAcceleration = acceleration - lastAcceleration;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold && timeSinceLastShake >= shakeCooldownTime)
        {
            IsShakeDetected = true;
            timeSinceLastShake = 0.0f;
        }
        else
        {
            // Optionally, automatically reset shake detection state
            IsShakeDetected = false;
        }
    }

    // Method to manually reset the shake detection
    public void ResetShakeDetection()
    {
        IsShakeDetected = false;
    }
}
