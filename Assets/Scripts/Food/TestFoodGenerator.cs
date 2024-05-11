using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class TestFoodGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _SushiPrefab;
    [SerializeField] private GameObject _MilkTeaPrefab;
    [SerializeField] private GameObject _RamenPrefab;
    [SerializeField] private Transform _foodGenPosition;
    [SerializeField] private SpawnTable _spawnTable;
    private void GenerateFood(Food.FoodType type)
    {
        switch (type)
        {

            case Food.FoodType.Sushi:
                _spawnTable.SpawnItem(_SushiPrefab);
                break;
            case Food.FoodType.MilkTea:
                _spawnTable.SpawnItem(_MilkTeaPrefab);
                break;
            case Food.FoodType.Ramen:
                _spawnTable.SpawnItem(_RamenPrefab);
                break;
        }
    }
    [Button]
    public void GenerateSushi()
    {
        GenerateFood(Food.FoodType.Sushi);
    }
    [Button]
    public void GenerateMilkTea()
    {
        GenerateFood(Food.FoodType.MilkTea);
    }
    [Button]
    public void GenerateRamen()
    {
        GenerateFood(Food.FoodType.Ramen);
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
