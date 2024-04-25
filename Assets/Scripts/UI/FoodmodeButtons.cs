using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodmodeButtons : MonoBehaviour
{
    public Button switchToMapButton;
    

   

    private void OnEnable()
    {
        if (switchToMapButton != null)
        {

            switchToMapButton.onClick.AddListener(OnSwitchToMapButtonClick);
        }
    }

    private void OnDisable()
    {
        if (switchToMapButton != null)
        {
            switchToMapButton.onClick.RemoveListener(OnSwitchToMapButtonClick);
        }

    }

    void OnSwitchToMapButtonClick()
    {
        
        MobileUIManager.Instance.Foodmode.SetActive(false);
        MobileUIManager.Instance.Mapmode.SetActive(true);
    }

}
