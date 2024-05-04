using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestSpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> guestTypes;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int numGuestToSpawn = 4;
    private GameObject currentGuest;
    private List<Vector3> _potentialSpawnPoints;
    private List<int> indexesToSpawn;
    private List<int> indexesGusttype;
    private bool isreadyToSpawn = false;
    private bool isguestSpawned = false;
    private void Awake()
    {
        _potentialSpawnPoints = new List<Vector3>();
        indexesToSpawn = new List<int>();
        indexesGusttype = new List<int>();
    }

    
    public void SpawnGuest()
    {
        Debug.Log("SpawnGuest is called");
        if (isreadyToSpawn)
        {
            //if (currentGuest != null)
            //{
            //    return;
            //}
            
            while (indexesToSpawn.Count < numGuestToSpawn)
            {
                int num = Random.Range(0, _potentialSpawnPoints.Count); // Generate a random number within the range
                if (!indexesToSpawn.Contains(num)) // Check if the list already contains this number
                {
                    indexesToSpawn.Add(num); // Add the unique number to the list
                }
               
            }
            while (indexesGusttype.Count < numGuestToSpawn)
            {
                int randomGuest = Random.Range(0, guestTypes.Count); // Generate a random number within the range
                
                indexesGusttype.Add(randomGuest); // alow repeat guest type!!!!!
                

            }

            for (int i = 0; i < numGuestToSpawn; i++)
            {
                currentGuest = Instantiate(guestTypes[indexesGusttype[i]], _potentialSpawnPoints[indexesToSpawn[i]], Quaternion.identity);
                //currentGuest.transform.SetParent(spawnPoint);
                currentGuest.transform.LookAt(MRSceneManager.Instance.CalibratedRoomCenter);
                currentGuest.transform.rotation *= Quaternion.Euler(0, 90, 0);
                var guestController = currentGuest.GetComponent<GuestController>();
                StartCoroutine(WaitForGuestReady(guestController));

                guestController._onGuestSatisfied.AddListener(PlayerStat.Instance.IncreaseCombo);
                guestController._onGuestUnsatisfied.AddListener(PlayerStat.Instance.ClearCombo);
                guestController._onWrongFood.AddListener(PlayerStat.Instance.ClearCombo);

            }

            Debug.Log("Guests are Spawned");
        }
        
        isguestSpawned = true;
        
    }
  
    void Start()
    {
        StartCoroutine(WaitForSpawnGuests());
    }

    IEnumerator WaitForSpawnGuests()
    {
        // Wait until MRSceneManager reports that spawn points are calculated
        yield return new WaitUntil(() => MRSceneManager.Instance.IsSpawnedPointsCalculated);

        _potentialSpawnPoints = MRSceneManager.Instance.PotentialSpawnedPositions;
        isreadyToSpawn = true;
        SpawnGuest();
    }
    
    IEnumerator WaitForGuestReady(GuestController guest)
    {
        // Wait until MRSceneManager reports that spawn points are calculated
        yield return new WaitUntil(() =>isguestSpawned);
        guest._onGuestArrived.Invoke();
        
    }
    // Update is called once per frame
    void Update()
    {
        //if (currentGuest == null)
        //{
        //    Invoke("SpawnGuest", 3f);
        //}

        //if (!isreadyToSpawn)
        //{
        //    if (MRSceneManager.Instance.IsSpawnedPointsCalculated)
        //    {
        //        _potentialSpawnPoints = MRSceneManager.Instance.PotentialSpawnedPositions;
        //        isreadyToSpawn = true;
        //        SpawnGuest();
        //    }
        //}
    }
}
