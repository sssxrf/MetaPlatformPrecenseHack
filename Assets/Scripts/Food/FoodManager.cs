using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;


public class FoodManager : MonoBehaviour
{
    public static FoodManager Instance { get; private set; }

    [SerializeField] private List<string> foodNames;
    [SerializeField] private List<GameObject> foodModels;
    [SerializeField] private SpawnTable _spawnTable;
    private Dictionary<string, GameObject> foodAssets;
    
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

        foodAssets = new Dictionary<string, GameObject>();

        if (foodNames.Count == foodModels.Count)
        {
            // Loop through the lists and add the elements to the dictionary
            for (int i = 0; i < foodNames.Count; i++)
            {
                // Add to dictionary only if the key is not already present to avoid duplicate key error
                if (!foodAssets.ContainsKey(foodNames[i]))
                {
                    foodAssets.Add(foodNames[i], foodModels[i]);
                }
                
            }
        }
        else
        {
            Debug.LogError("foodNames and foodModels lists do not have the same number of elements. Cannot initialize dictionary.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnfoodByName(string foodName, Vector3 spawnposition)
    {
        Debug.Log("SpawnFoodByName is called");
        foreach (var food in foodAssets)
        {
            if (food.Key == foodName)
            {
                _spawnTable.SpawnItem(food.Value);
                break;
            }
        }
    }
    
    [Button] 
    public void SpawnRamen()
    {
        SpawnfoodByName("Ramen", Vector3.zero);
    }
    [Button]
    public void SpawnBoba()
    {
        SpawnfoodByName("Boba", Vector3.zero);
    }
    [Button]
    public void SpawnSushi()
    {
        SpawnfoodByName("Sushi", Vector3.zero);
    }
    
}
