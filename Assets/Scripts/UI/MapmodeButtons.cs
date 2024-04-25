using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapmodeButtons : MonoBehaviour
{

    public Button switchToFoodButton;
    

   

    private void OnEnable()
    {
        if (switchToFoodButton != null)
        {

            switchToFoodButton.onClick.AddListener(OnSwitchToFoodButtonClick);
        }
    }

    private void OnDisable()
    {
        if (switchToFoodButton != null)
        {
            switchToFoodButton.onClick.RemoveListener(OnSwitchToFoodButtonClick);
        }

    }

    void OnSwitchToFoodButtonClick()
    {
        
        MobileUIManager.Instance.Foodmode.SetActive(true);
        MobileUIManager.Instance.Mapmode.SetActive(false);
    }

}
