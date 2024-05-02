using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodmodeButtons : MonoBehaviour
{
    public Button switchToMapButton;
    public Button Deliver;

    

    [SerializeField] private List<Sprite> displayedImages;


    private void OnEnable()
    {
        if (switchToMapButton != null)
        {

            switchToMapButton.onClick.AddListener(OnSwitchToMapButtonClick);
        }

        if (switchToMapButton != null)
        {
            Deliver.onClick.AddListener(OnDeliverButtonClick);
        }

    }

    private void OnDisable()
    {
        if (switchToMapButton != null)
        {
            switchToMapButton.onClick.RemoveListener(OnSwitchToMapButtonClick);
        }

        if (switchToMapButton != null)
        {
            Deliver.onClick.RemoveListener(OnDeliverButtonClick);
        }
    }

    void OnSwitchToMapButtonClick()
    {

        MobileUIManager.Instance.Foodmode.SetActive(false);
        MobileUIManager.Instance.Mapmode.SetActive(true);
    }

    void OnDeliverButtonClick()
    {
        // if the food is ready
        if (DragDropController.Instance._isfoodReady && DragDropController.Instance._currentFood != null)
        {

        }
    }

}
