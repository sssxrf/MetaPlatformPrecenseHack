using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientGuestManager : MonoBehaviour
{

    public static ClientGuestManager Instance { get; private set; }

    #region Serialized Field
    [SerializeField] private List<int> _urgentStates;
    [SerializeField] private List<int> _guesttypes;
    [SerializeField] private List<GameObject> _guestModels;


    private Dictionary<(int, int), GameObject> _preStoreGuestDic = new Dictionary<(int, int), GameObject>();

    private Dictionary<int, GuestInfos> _guestsDic = new Dictionary<int, GuestInfos>();

    private Dictionary<int, GameObject> _spawnedGuests = new Dictionary<int, GameObject>();

    public struct GuestInfos
    {
        public int guestID;
        public int guestType;
        public int urgentState;
        public Vector2 PosRelativeToWindow;
    }

    #endregion

    #region Public Methods
    public void StartGame(int targetScore, int Level, int timeLimit)
    {
        MobileUIManager.Instance.targetScore = targetScore;
        MobileUIManager.Instance.totalTime = timeLimit;
        MobileUIManager.Instance.StartCountDown();
    }

    public void StartGame()
    {
        
        MobileUIManager.Instance.StartCountDown();
    }

    public void StoreNewGuestInfos(int newguestID, int newguestType, int newurgentState, Vector2 newPosRelativeToWindow)
    {
        GuestInfos guestInfo;
        guestInfo.guestID = newguestID;
        guestInfo.guestType = newguestType;
        guestInfo.urgentState = newurgentState;
        guestInfo.PosRelativeToWindow = newPosRelativeToWindow;
        _guestsDic.Add(newguestID, guestInfo);
        SpawnNewGuest(guestInfo);

    }

    public void UpdateGuestInfos(int theguestID, int theurgentState)
    {
        if(_guestsDic.ContainsKey(theguestID) && _spawnedGuests.ContainsKey(theguestID))
        {
            GuestInfos updatedguestInfo;
            updatedguestInfo.guestID = _guestsDic[theguestID].guestID;
            updatedguestInfo.guestType = _guestsDic[theguestID].guestType;
            updatedguestInfo.urgentState = theurgentState;
            updatedguestInfo.PosRelativeToWindow = _guestsDic[theguestID].PosRelativeToWindow;
            _guestsDic[theguestID] = updatedguestInfo;
            UpdateGuestState(updatedguestInfo);
        }
    }

    public void ClearAGuestInfos(int theguestID, bool isSatisfied)
    {
        if (_guestsDic.ContainsKey(theguestID) && _spawnedGuests.ContainsKey(theguestID))
        {
            Destroy(_spawnedGuests[theguestID]);

            Debug.Log("Clear a guest, issatisfied?"+ isSatisfied);
            if (isSatisfied)
            {
                // earn reward
                MobileUIManager.Instance.ChangeScore(1);

            }
            else
            {
                // delete point
                MobileUIManager.Instance.ChangeScore(-1);
            }
        }
    }

    #endregion

    #region Private Methods

    private void InitializeGuestAssetsDictionary()
    {
        if (_urgentStates.Count * _guesttypes.Count != _guestModels.Count)
        {
            Debug.LogError("Mismatch in the count of urgent states, guest types, and guest models.");
            return;
        }

        int modelIndex = 0;
        foreach (int type in _urgentStates)
        {
            foreach (int state in _guesttypes)
            {
                if (modelIndex < _guestModels.Count)
                {
                    _preStoreGuestDic[(type, state)] = _guestModels[modelIndex];
                    modelIndex++;
                }
                else
                {
                    Debug.LogError("Not enough models available for all state-type combinations.");
                    return;
                }
            }
        }
    }

    private void SpawnNewGuest(GuestInfos newguest)
    {
        Debug.Log("spawn new guest");
        var id = (newguest.guestType, newguest.urgentState);

        if (_preStoreGuestDic.ContainsKey(id))
        {
            var theNewguest = Instantiate(_preStoreGuestDic[id], new Vector3(newguest.PosRelativeToWindow.x, 1, newguest.PosRelativeToWindow.y), Quaternion.identity);
            _spawnedGuests.Add(newguest.guestID, theNewguest);
        }
        else
        {
            Debug.Log("we don''t have this guest yet");
        }
    }

    private void UpdateGuestState(GuestInfos updatedguest)
    {
        Debug.Log("update guest");
        var id = (updatedguest.guestType, updatedguest.urgentState);

        Destroy(_spawnedGuests[updatedguest.guestID]);
        if (_preStoreGuestDic.ContainsKey(id))
        {
            
            var theUpdatedguest = Instantiate(_preStoreGuestDic[id], new Vector3(updatedguest.PosRelativeToWindow.x, 1, updatedguest.PosRelativeToWindow.y), Quaternion.identity);
            _spawnedGuests[updatedguest.guestID] = theUpdatedguest;
        }
        else
        {
            Debug.Log("can't find this guest");
        }
    }


    #endregion
    #region Unity Method
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

// Start is called before the first frame update
    void Start()
    {
        InitializeGuestAssetsDictionary();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
}
