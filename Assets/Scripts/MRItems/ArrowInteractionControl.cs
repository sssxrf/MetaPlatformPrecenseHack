using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
public class ArrowInteractionControl : MonoBehaviour
{
    public enum  ArrowState
    {
        Attached,
        Released,
        Inactive
    }

    [SerializeField] private SnapInteractor _snapInteractor;
    [SerializeField] private SnapInteractable _bowSnapInteractable;
    [SerializeField] private UnityEngine.Object _attachTobowActiveState;
    [SerializeField] private GameObject pullingPosistion;
    
    [SerializeField] private GameObject GrabableNormal;
    [SerializeField] private GameObject _bow;
    private IActiveState _attachTobow { get; set; }
    
    protected virtual void Awake()
    {
        _attachTobow = _attachTobowActiveState as IActiveState;
        this.AssertField(_attachTobow, nameof(_attachTobow));
    }
    void Start()
    {
        _bowSnapInteractable.WhenSelectingInteractorAdded.Action += HandleArrowAttached;
    }

    private void HandleArrowAttached(SnapInteractor obj)
    {
       transform.SetParent(pullingPosistion.transform);
       
       // release arrow from snao socket in 
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
