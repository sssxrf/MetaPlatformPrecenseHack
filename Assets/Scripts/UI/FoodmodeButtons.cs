using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodmodeButtons : MonoBehaviour
{
    public Button switchToMapButton;
    

    void Start()
    {
        if (switchToMapButton != null)
        {
            
            switchToMapButton.onClick.AddListener(OnSwitchToMapButtonClick);
        }
    }

    void OnDestroy()
    {
      
        if (switchToMapButton != null)
        {
            switchToMapButton.onClick.RemoveListener(OnSwitchToMapButtonClick);
        }
    }

    void OnSwitchToMapButtonClick()
    {
        gameObject.SetActive(false);
        //MobileUIManager.Instance.Foodmode.SetActive(false);
    }

}
