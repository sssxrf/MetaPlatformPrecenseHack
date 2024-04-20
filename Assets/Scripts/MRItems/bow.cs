using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class bow : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private Transform stringPosition;
    [SerializeField] private LineRenderer _stringRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RenerString();
    }

    private void RenerString()
    {
        Vector3 newStringPosition = stringPosition.localPosition;
        
        _stringRenderer.SetPosition(1,newStringPosition );
            
    }
}
