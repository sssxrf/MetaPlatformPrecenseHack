using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestSpa : MonoBehaviour
{
    [SerializeField] private List<GameObject> guestTypes;
    [SerializeField] private Transform spawnPoint;
    private GameObject currentGuest;
    public void SpawnGuest()
    {
        if (currentGuest != null)
        {
            return;
        }
        int randomGuest = Random.Range(0, guestTypes.Count);
        currentGuest = Instantiate(guestTypes[randomGuest], spawnPoint.position, spawnPoint.rotation);
        currentGuest.transform.SetParent(spawnPoint);
    }
  
    void Start()
    {
        SpawnGuest();
    }
    // Update is called once per frame
    void Update()
    {
        if (currentGuest == null)
        {
            Invoke("SpawnGuest", 3f);
        }
    }
}
