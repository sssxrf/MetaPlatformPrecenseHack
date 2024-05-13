using System.Collections;
using System.Collections.Generic;
using Meta.WitAi.Attributes;
using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEngine;

public class bow : MonoBehaviour
{
    [Header("Bow Settings")]
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private Transform stringPosition;
    [SerializeField] private LineRenderer _stringRenderer;

    [Header("Interaction Settings")] [SerializeField]
    private OneGrabTranslateTransformer _stringConstraint;
    [SerializeField] private SnapInteractable _bowSnapInteractable;
    [SerializeField] private GrabInteractable _stringGrabInteractable;
    private GameObject _arrow;
    private SnapInteractor _bowSnapInteractor;
    private bool _isArrowAttached = false;
    
    [Header("Sound effect")]
    [SerializeField] private AudioSource _pullSound;
    [SerializeField] private AudioSource _releaseSound;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Test xxxxxxx");
        _bowSnapInteractable.WhenSelectingInteractorAdded.Action += HandleArrowAttached;
        _bowSnapInteractable.WhenSelectingInteractorRemoved.Action += HandleArrowRemoved;
        _stringGrabInteractable.WhenSelectingInteractorRemoved.Action += HandleArrowReleased;
        _stringGrabInteractable.WhenSelectingInteractorAdded.Action += HandleArrowPulled;
        //Update string constraint 
        FloatConstraint stringConstraint = new FloatConstraint();
        stringConstraint.Constrain = true;
        stringConstraint.Value = endPosition.position.z - startPosition.position.z;
        Debug.Log("string value " + stringConstraint.Value);
        _stringConstraint.Constraints.MaxZ = stringConstraint;
    }

    private void HandleArrowPulled(GrabInteractor obj)
    {
        if (!_isArrowAttached )
        {
            return;
        }
        Debug.Log("Arrow Pulled");
        _pullSound.Play();
    }

    private void HandleArrowRemoved(SnapInteractor obj)
    {
        if (!_isArrowAttached )
        {
            return;
        }
        
        _isArrowAttached = false;
    }

    private void HandleArrowReleased(GrabInteractor obj)
    {
        if (!_isArrowAttached )
        {
            return;
        }
        
        _isArrowAttached = false;
        Debug.Log("Arrow Released");
        
        // release arrow from snap socket in
        var arrowRb =  _arrow.GetComponent<Rigidbody>();
        _bowSnapInteractor.InjectAllSnapInteractor(_stringGrabInteractable.gameObject.GetComponent<Grabbable>(),arrowRb);
        
        // shoot arrow
        _arrow.transform.SetParent(null);
        arrowRb.isKinematic = false;
        arrowRb.useGravity = false;
        arrowRb.AddForce(stringPosition.up * 10, ForceMode.Impulse);
        _releaseSound.Play();
        _pullSound.Stop();
        
        
    }

    private void HandleArrowAttached(SnapInteractor obj)
    {
        Debug.Log("Arrow Attached");
        _isArrowAttached = true;
        // traverse the arrowsocket interactable to get the interactor 
        foreach (var snapInteractor in _bowSnapInteractable.SelectingInteractors)
        {
            Debug.Log("Snap Interactor " + snapInteractor.gameObject);
            _arrow = snapInteractor.gameObject;
            _bowSnapInteractor = snapInteractor;
        }
        _arrow.transform.SetParent(null);
       
    }

    // Update is called once per frame
    void Update()
    {
        float pullingDistance = Vector3.Distance(stringPosition.position, startPosition.position);
        if (pullingDistance > 0.01f)
        {
            ReleaseString();
        }
        RenerString();
    }

    private void RenerString()
    {
        Vector3 newStringPosition = stringPosition.localPosition;
        
        _stringRenderer.SetPosition(1,newStringPosition );
            
    }

    private void ReleaseString()
    {
        stringPosition.position = Vector3.Lerp(stringPosition.position, startPosition.position, Time.deltaTime*5f);
    }
    
}
