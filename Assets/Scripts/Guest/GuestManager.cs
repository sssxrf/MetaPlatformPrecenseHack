using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class GuestManager : MonoBehaviour
{
    public static GuestManager Instance { get; private set; }
    public int gamelevel = 0;
    public int starterGuestNum = 1;
    public UnityEvent StartGameEvent;
    public UnityEvent LevelChanged;

    public UnityEvent LevelChangeComplete;
    // Values shared to mobile
    //public int _spawnedGuestNum { get; set; } = 0;

    // public int _guesttype { get; set; } = 0;
    //
    // public int _guestID { get; set; } = 0;
    //
    // public int _urgentState { get; set; } = 0;
    //
    // public bool _isSatisfied { get; set; } = false;
    //
    // public Vector2 _posRelativeToWindow { get; set; }

    // Values keep tracking in GuestManager
    private Dictionary<int, GameObject> _guests = new Dictionary<int, GameObject>();
    private int _currentGuestID = 0;
    private bool gameStarted = false;
    private bool levelChanging = false;
    private int currentGuestNum = 0;
    private GameObject _HeadsetPlayerPrefab;
    private Player _player;
    private GuestSpawn _guestSpawner;

    IEnumerator FindPlayerWithDelay(float delay)
    {
        while (_HeadsetPlayerPrefab == null)
        {
            yield return new WaitForSeconds(delay);
            _HeadsetPlayerPrefab = GameObject.Find("PlayerPrefabHost(Clone)");
        }
        Debug.Log("HeadsetPlayer is found!");
        _player = _HeadsetPlayerPrefab.GetComponent<Player>();
    }

    
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

    private void Start()
    {
        StartCoroutine(FindPlayerWithDelay(0.1f));
        StartGameEvent.AddListener(Initialize);
        LevelChanged.AddListener(LevelUpdate);
        LevelChangeComplete.AddListener(() => levelChanging = false);
        _guestSpawner = FindObjectOfType<GuestSpawn>();
    }
    
    private void Update()
    {
        if (gameStarted && !levelChanging)
        {
            if (_guests.Count == 0)
            {
                levelChanging = true;
                StartCoroutine(WaitForLevelStart());
            }
        }
    }

    IEnumerator WaitForLevelChange()
    {
        // wait for 3 seconds
        yield return new WaitForSeconds( 3f);
        levelChanging = false;

    }

    private IEnumerator WaitForLevelStart()
    {
        // wait for 3 seconds
        yield return new WaitForSeconds( 1f);
        LevelChanged.Invoke();
        StartCoroutine(WaitForLevelChange());

    }
    
    // update level and spawn new guest
    private void LevelUpdate()
    {
        Debug.Log("Level Update " );
        gamelevel++;
        currentGuestNum = starterGuestNum + gamelevel;
        // spwan new guests
        _guestSpawner.readyToSpawn(currentGuestNum);
        LevelInfo(gamelevel);

    }
    
    public  void Initialize()
    {
        gameStarted = true;
        GameStartInfo(true, gamelevel);
        
    }

    // generate new guest ID 
    public int GenerateGuestID(GameObject guest)
    {
        _guests[_currentGuestID] = guest;
        return _currentGuestID++;
    }
    
    // remove a Guest from the dictionary
    public void RemoveGuestID(int guestID)
    {
        if (!_guests.ContainsKey(guestID)) return;
        Destroy(_guests[guestID]);
        _guests.Remove(guestID);
    }
    
    // destroy all guests
    [Button("Destroy All Guests")]
    public void destroyAllGuests()
    {
        foreach (var guest in _guests)
        {
            Destroy(guest.Value);
        }
        _guests.Clear();
        _currentGuestID = 0;
    }

   // update level 
   [Button("Update Level")]
   public void ForcelevelIncrement()
   {
       destroyAllGuests();
       LevelChanged.Invoke();
   }

 

    // call it when spawning a new guest
    public void SendNewGuestInfo(int guestID, int guestType, int urgentState, Vector2 PosRelativeToWindow)
    {
        Debug.Log("New Guest Info: " + guestID + " " + guestType + " " + urgentState + " " + PosRelativeToWindow);
        if(_player != null)
        {
            _player.RPC_SendNewGuestInfo(guestID, guestType, urgentState, PosRelativeToWindow);
        }
    }

    // call it when urgentState changes
    public void UpdateGuestInfo(int guestID, int urgentState)
    {

        if (_player != null)
        {
            _player.RPC_SendUpdateGuestInfo(guestID, urgentState);
        }

    } 



    // Call it when the guest is cleared (no matter satisfied or not)
    public void ClearAGuest(int guestID, bool isSatisfied)
    {
        Debug.Log("Guest " + guestID + " is cleared, isSatisfied: " + isSatisfied);
        if (_player != null)
        {
            _player.RPC_SendClearAGuestInfO(guestID, isSatisfied);  
        }
    }
    
    //call it when game started, send to phone 
    public void GameStartInfo(bool gameStart,int level)
    {
        if (_player != null)
        {
            _player.RPC_SendStarttheGame(gameStart);
        }
    }
    
    //call it when new level start 
    
    public void LevelInfo(int level)
    {
        
    }
   
    
}
