using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat Instance { get; private set; }
    #region SerializeField
    [SerializeField] private TMP_Text _comboMeter;
    [SerializeField] private GameObject _displayUI;
    #endregion
    #region private variable

    private int comboMeter = 0;
    #endregion
    void Awake()
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
    
    #region public method

    public void IncreaseCombo()
    {
        Debug.Log("Combo increased");
        comboMeter++;
    }

    public void ClearCombo()
    {
        comboMeter = 0;
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (comboMeter > 0)
        {
            _comboMeter.text = "Combo x " + comboMeter;
        }
        else
        {
            _comboMeter.text = "";
        }
    }
}
