using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class bow : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;

    [SerializeField] private 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            Debug.Log("primary hand trigger down");
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
        {
            Debug.Log("second hand trigger up");
        }
    }
}
