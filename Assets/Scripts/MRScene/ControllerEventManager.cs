using UnityEngine;
using System;  

public class ControllerEventManager : MonoBehaviour
{
    public static event Action OnButtonAPressed;  //Debug

    void Update()
    {
        CheckForButtonAPress();
    }

    private void CheckForButtonAPress()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            OnButtonAPressed?.Invoke();  // Trigger the event
        }
    }
}