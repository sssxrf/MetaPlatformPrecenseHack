using System;
using System.Collections;
using System.Collections.Generic;
using Meta.WitAi.Attributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GuestController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] float waitTime = 10f;
    [SerializeField] private float paneltyTime = 1f;
    
    [Header("Bubble and Progress")]
    [SerializeField] private List<GameObject> foodBubble;
    [SerializeField] private Transform foodBubblePosition;
    public Image fillImage;
    [SerializeField] GameObject completionstar;
    
    [Header("Events")]
    public UnityEvent _onGuestArrived;
    public UnityEvent _onGuestSatisfied;
    public UnityEvent _onGuestUnsatisfied;
    public UnityEvent _onWrongFood;
    
    
    
    
    private Food.FoodType _foodType;
    private float currentTime = 0f;
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
        Destroy(gameObject);
    }
    
    protected void guestUnsatisfied()
    {
        Destroy(gameObject);
    }

    
    protected void sendWrongFood()
    {
        currentTime += paneltyTime;
    }
    
    protected void setUpEvents()
    {
        _onGuestArrived.AddListener(() => Debug.Log("Guest Arrived"));
        _onGuestSatisfied.AddListener(() => Debug.Log("Guest Satisfied"));
        _onGuestUnsatisfied.AddListener(() => Debug.Log("Guest Unsatisfied"));
        _onWrongFood.AddListener(() => Debug.Log("Wrong Food"));
        _onGuestArrived.Invoke();
        _onGuestSatisfied.AddListener(guestSatisfied);
        _onGuestUnsatisfied.AddListener(guestUnsatisfied);
        _onWrongFood.AddListener(sendWrongFood);
    }
    // Start is called before the first frame update
    protected void Start()
    {
        setUpEvents();
        GenerateFoodType();
       
    }
    

    // Update is called once per frame
    protected void Update()
    {
        currentTime += Time.deltaTime;
        fillImage.fillAmount = (waitTime-currentTime) / waitTime;
        if (currentTime >= waitTime)
        {
            _onGuestUnsatisfied.Invoke();
        }
    }
}
