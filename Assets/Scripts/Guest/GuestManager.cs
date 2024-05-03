using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestManager : MonoBehaviour
{
    public static GuestManager Instance { get; private set; }


    // Values shared to mobile
    //public int _spawnedGuestNum { get; set; } = 0;

    public int _guesttype { get; set; } = 0;

    public int _guestID { get; set; } = 0;

    public int _urgentState { get; set; } = 0;

    public bool _isSatisfied { get; set; } = false;

    public Vector2 _posRelativeToWindow { get; set; }

    // Values keep tracking in GuestManager
    private List<int> _guestIDS;
    private List<int> _guestTypes;
    private List<int> _urgentStates;
    private List<Vector2> _posRelativeToWindows;
    private int _spawnedGuestNum;

    private void Awake()
    {

        if (Instance == null)
        {

            Instance = this;

        }
        else
        {
            // If an instance already exists and it's not this one, destroy this one
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // call it when spawning a new guest
    public void SendNewGuestInfo(int guestID, int guestType, int urgentState, Vector2 PosRelativeToWindow)
    {

    }

    // call it when urgentState changes
    public void UpdateGuestInfo(int guestID, int urgentState)
    {

    } 



    // Call it when the guest is cleared (no matter satisfied or not)
    public void ClearAGuest(int guestID, bool isSatisfied)
    {

    }
    
}
