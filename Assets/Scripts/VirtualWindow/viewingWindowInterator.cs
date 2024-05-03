using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

public class viewingWindowInterator : MonoBehaviour
{

    [SerializeField] private GameObject blockingWindow;
    [SerializeField] private GameObject virtualWindow;

    private bool isVirtual = true;
    private bool isInitialized = false;
    public void SwitchWindow()
    {
        
        virtualWindow.SetActive(!virtualWindow.activeInHierarchy);

       
          
    }
    // Start is called before the first frame update
    void Start()
    {
        virtualWindow.SetActive(isVirtual);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
