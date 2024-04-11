using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MobileUIManager : MonoBehaviour
{
    public static MobileUIManager Instance { get; private set; }

    #region SerializeField
    [SerializeField] private TMP_Text _messagesOnMobile;
    [SerializeField] private GameObject _wholeUI;

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

    public void UpdateMessages(string message)
    {
        _messagesOnMobile.text += message;
    }

    public void SetVisibleofUI(bool value)
    {
        _wholeUI.SetActive(value);
    }
}
