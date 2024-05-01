using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFoodGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _SushiPrefab;
    [SerializeField] private GameObject _MilkTeaPrefab;
    [SerializeField] private GameObject _RamenPrefab;
    [SerializeField] private Transform _foodGenPosition;

    private void GenerateFood(Food.FoodType type)
    {
        switch (type)
        {

            case Food.FoodType.Sushi:
                Instantiate(_SushiPrefab, _foodGenPosition.position, Quaternion.identity);
                break;
            case Food.FoodType.MilkTea:
                Instantiate(_MilkTeaPrefab, _foodGenPosition.position, Quaternion.identity);
                break;
            case Food.FoodType.Ramen:
                Instantiate(_RamenPrefab, _foodGenPosition.position, Quaternion.identity);
                break;
        }
    }
    
    public void GenerateSushi()
    {
        GenerateFood(Food.FoodType.Sushi);
    }
    
    public void GenerateMilkTea()
    {
        GenerateFood(Food.FoodType.MilkTea);
    }
    
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
