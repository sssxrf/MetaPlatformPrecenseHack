using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Unity.VisualScripting;

public class ArrowInteractionControl : MonoBehaviour
{
    public enum  ArrowState
    {
        Attached,
        Released,
        Inactive
    }
    [SerializeField] private GrabInteractable _grabInteractable;
  
    private IActiveState _attachTobow { get; set; }
    
    protected virtual void Awake()
    {
      
    }
    void Start()
    {
        _grabInteractable.WhenSelectingInteractorAdded.Action += ArrowPickedUp;
    }

    private void ArrowPickedUp(GrabInteractor obj)
    {
        // transform.SetParent(null);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
