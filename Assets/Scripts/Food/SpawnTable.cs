using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTable : MonoBehaviour
{
    // this class spawn an item on the table in order based on availiable place 
    [SerializeField] private List<Transform> _spawnPoints;
    
    
    public void SpawnItem(GameObject item)
  
    {
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if (_spawnPoints[i].childCount == 0)
            {
                Instantiate(item, _spawnPoints[i].position, _spawnPoints[i].rotation, _spawnPoints[i]);
                break;
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
