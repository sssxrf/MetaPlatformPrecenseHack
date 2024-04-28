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
    [SerializeField] private UnityEngine.Object _attachTobowActiveState;
    
  
    private IActiveState _attachTobow { get; set; }
    
    protected virtual void Awake()
    {
        _attachTobow = _attachTobowActiveState as IActiveState;
        this.AssertField(_attachTobow, nameof(_attachTobow));
    }
    void Start()
    {
    }

   
    // Update is called once per frame
    void Update()
    {
        
    }
}
