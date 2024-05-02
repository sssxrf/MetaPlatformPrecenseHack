using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ProtectionGuestController : GuestController
{
    
    [Header("Protection Guest Controller")] 
    public UnityEvent onProtectionDestroyed;
    [SerializeField] Food.FoodType _protectionFoodType = Food.FoodType.MilkTea;
    [SerializeField] GameObject ProtectionBubble;
    private bool ProtectionGuest = true;
    
    public void DestroyProtection()
    {
        if (ProtectionGuest)
        {
            Debug.Log("Protection Destroyed");
            Destroy(ProtectionBubble);
            ProtectionGuest = false;
        }
    }
    
    protected void OnCollisionEnter(Collision other)
    {
        if (ProtectionGuest)
        {
            if (other.gameObject.TryGetComponent<ArrowInteractionControl>(out var arrow))
            {
                if (arrow.gameObject.TryGetComponent<Food>(out var food))
                {
                    Debug.Log("food type" + food.foodType);
                    Debug.Log("Protection food type" + _protectionFoodType);
                    if (food.foodType == _protectionFoodType)
                    {
                        onProtectionDestroyed.Invoke();
                    }
                    Destroy(arrow.gameObject);
                }
            }
            return;
        }
        base.OnCollisionEnter(other);
    }
    protected void setUpEvents()
    {
        base.setUpEvents();
        onProtectionDestroyed.AddListener(DestroyProtection);
    }
    void Start()
    {
        setUpEvents();
        
    }

    void Update()
    {
        base.Update();
        if (!ProtectionGuest)
        {
            GenerateFoodType();
        }
    }
}
