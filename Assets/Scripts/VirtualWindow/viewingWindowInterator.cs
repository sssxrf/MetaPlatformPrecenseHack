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
        if (isVirtual)
        {
            blockingWindow.SetActive(false);
            virtualWindow.SetActive(true);
            isVirtual = false;
        }
        else
        {
            blockingWindow.SetActive(true);
            virtualWindow.SetActive(false);
            isVirtual = true;
        }
        
        
          
    }
    // Start is called before the first frame update
    void Start()
    {
        if (isVirtual)
        {
            blockingWindow.SetActive(false);
            virtualWindow.SetActive(true);
        }
        else
        {
            blockingWindow.SetActive(true);
            virtualWindow.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
