using System;
using System.Collections;
using System.Collections.Generic;
using Meta.WitAi.Attributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GuestController : MonoBehaviour
{
    
    public enum GuestType
    {
        Purple = 0,
        Orange = 1
    }
    [Header("Setup")]
    [SerializeField] float waitTime = 200f;
    [SerializeField] private float paneltyTime = 1f;
    [SerializeField] private float _urgentPercent = 0.7f;
    [Header("Bubble and Progress")]
    [SerializeField] private List<GameObject> foodBubble;
    [SerializeField] private Transform foodBubblePosition;
    public Image fillImage;
    [SerializeField] GameObject completionstar;
    
    [Header("Events")]
    [Tooltip("Event when guest arrived")]
    public UnityEvent _onGuestArrived;
    [Tooltip("Event when guest is satisfied (food requirement met)")]
    public UnityEvent _onGuestSatisfied;
    [Tooltip("Event when guest is unsatisfied (time out)")]
    public UnityEvent _onGuestUnsatisfied;
    [Tooltip("Event when guest is given wrong food")]
    public UnityEvent _onWrongFood;
    [Tooltip("Event when guest is in urgent state")]
    public UnityEvent _onGuestUrgent;
    //section for basic data, that shared to guest manager 
    // Values shared to mobile
     public Vector2 _posRelativeToWindow { get; set; }
     public GuestType _guesttype;
     public int _guestID  {get; set;} = -1;
     public bool _isSatisfied  {get; set;} = false;
     public int _urgentState  {get; set;} = 0;
     
     public Animator myAnimator;
     // storage variable 
     private GuestManager _guestManager;
     
     

    
    
    
    
    private Food.FoodType _foodType;
    private float currentTime = 0f;
    
    
  
    
    //call this function to set up posrelative to window
    public void SetPosRelativeToWindow(Vector3 worldPosition, Transform referenceTransform)
    {

        // This converts the world position of objectA to the local coordinate system of objectB
        Vector3 localPosition3D = referenceTransform.InverseTransformPoint(worldPosition);


        Vector2 localPosition2D = new Vector2(localPosition3D.x, localPosition3D.y);


        _posRelativeToWindow = localPosition2D;
        //Debug.Log("Set pos relative to window");
    }
    protected void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision");
        if (other.gameObject.TryGetComponent<ArrowInteractionControl>(out var arrow))
        {
            if (arrow.gameObject.TryGetComponent<Food>(out var food))
            {
               
                if (food.foodType == _foodType)
                {
                    _onGuestSatisfied.Invoke();
                }
                else
                {
                    _onWrongFood.Invoke();
                }
                
                Destroy(arrow.gameObject);
            }
        }
    }
    protected void GenerateFoodType()
    {
        _foodType = (Food.FoodType)UnityEngine.Random.Range(0, 3);
        // generate bubble
        if ((int)_foodType >= foodBubble.Count)
        {
            Debug.LogError("Food type not found");
            return;
        }   
        var bubble = Instantiate(foodBubble[(int)_foodType], foodBubblePosition.position, foodBubblePosition.rotation);
        bubble.transform.SetParent(foodBubblePosition);
    }
    
    protected void guestSatisfied()
    {
        var star = Instantiate(completionstar, transform.position, Quaternion.identity);
        _isSatisfied = true;
        if (_guestManager != null)
        {
            if(myAnimator != null){
            myAnimator.SetBool("isSatisfied", true);
            StartCoroutine(WaitForAnimationToEnd());
            }
            else
            {
                Debug.LogError("Animator not found on the guest.");
                RemoveGuestImmediately();
            }
            
        }
        else{
            Debug.LogError("Guest Manager not found.");
        }
    }
    
    // protected void guestUnsatisfied()
    // {
    //     _isSatisfied = false;
    //     if (_guestManager != null)
    //     {
    //         myAnimator.SetBool("isLeaving", true);
    //         _guestManager.ClearAGuest(_guestID, _isSatisfied);
    //         _guestManager.RemoveGuestID(_guestID);
    //     }
    // }

    protected void guestUnsatisfied()
    {
        _isSatisfied = false;
        if (_guestManager != null)
        {
            if (myAnimator != null)
            {
                myAnimator.SetBool("isLeaving", true);
                StartCoroutine(WaitForAnimationToEnd());
            }
            else
            {
                Debug.LogError("Animator not found on the guest.");
                RemoveGuestImmediately();
            }
        }
        else
        {
            Debug.LogError("Guest Manager not found.");
        }
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        // Assuming the leaving animation is 2 seconds long
        yield return new WaitForSeconds(3.0f); 
        RemoveGuestImmediately();
    }

    private void RemoveGuestImmediately()
    {
        _guestManager.ClearAGuest(_guestID, _isSatisfied);
        _guestManager.RemoveGuestID(_guestID);
    }




    protected void sendWrongFood()
    {
        currentTime += paneltyTime;
        myAnimator.SetBool("isAngry", true);
    }
    // Connect to guest manager to update the guest list, and acquire guest id 
    private void ConnectToManager()
    {
        _guestManager = FindObjectOfType<GuestManager>();
        if (_guestManager == null)
        {
            Debug.LogError("Guest Manager not found");
            return;
        }
        else
        {
            _guestID = _guestManager.GenerateGuestID(gameObject);
            _guestManager.SendNewGuestInfo(_guestID, (int)_guesttype, _urgentState, _posRelativeToWindow);
        }
        
    }

    protected void sendUrgentData()
    {
        _guestManager.UpdateGuestInfo(_guestID,1);
    }
    protected void setUpEvents()
    {
        _onGuestArrived.AddListener(() => Debug.Log("Guest Arrived"));
        _onGuestArrived.AddListener(ConnectToManager);
        _onGuestSatisfied.AddListener(() => Debug.Log("Guest Satisfied"));
        _onGuestSatisfied.AddListener(() => MRSceneManager.Instance.onScoreIncrease.Invoke());
        _onGuestUnsatisfied.AddListener(() => Debug.Log("Guest Unsatisfied"));
        _onGuestUnsatisfied.AddListener(() => MRSceneManager.Instance.onScoreDecrease.Invoke());
        _onWrongFood.AddListener(() => Debug.Log("Wrong Food"));
        _onGuestSatisfied.AddListener(guestSatisfied);
        _onGuestUnsatisfied.AddListener(guestUnsatisfied);
        _onWrongFood.AddListener(sendWrongFood);
        _onGuestUrgent.AddListener(sendUrgentData);
        
        
    }

   

    // Start is called before the first frame update
    protected void Start()
    {
        setUpEvents();
        GenerateFoodType();
        myAnimator = GetComponentInChildren<Animator>();

    }
    
    

    // Update is called once per frame
    protected void Update()
    {
        
        //execute action after guest ID got assigned 
        if (_guestID != -1)
        {
            currentTime += Time.deltaTime;
            fillImage.fillAmount = (waitTime-currentTime) / waitTime;
            if (currentTime >= waitTime)
            {
                _onGuestUnsatisfied.Invoke();
            }else if (currentTime >= waitTime * _urgentPercent)
            {
                if (_urgentState == 0)
                {
                    _onGuestUrgent.Invoke();
                }

                _urgentState = 1;
            }
        }
        
    }
    
    [Sirenix.OdinInspector.Button]
    public void ForceGuestSatisfied()
    {
        _onGuestSatisfied.Invoke();
    }
    [Sirenix.OdinInspector.Button]
    public void ForceGuestUnsatisfied()
    {
        _onGuestUnsatisfied.Invoke();
    }
   
}
