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
    [SerializeField] private MeshRenderer ArrowMeshRenderer;
    
    private IActiveState _attachTobow { get; set; }
    
    protected virtual void Awake()
    {
      
    }
    void Start()
    {
        ArrowMeshRenderer.enabled = false;
        _grabInteractable.WhenSelectingInteractorAdded.Action += ArrowPickedUp;
    }

    private void ArrowPickedUp(GrabInteractor obj)
    {
        // transform.SetParent(null);
        ArrowMeshRenderer.enabled = true;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
