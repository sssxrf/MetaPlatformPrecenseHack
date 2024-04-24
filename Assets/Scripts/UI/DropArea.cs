using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IDropHandler
{
    public Image placeholderImage; 
    public Sprite DisplayedFood; // Assign the sprite that it should display

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            placeholderImage.sprite = DisplayedFood; // Change the placeholder's sprite
            placeholderImage.enabled = true; // Make the placeholder visible
        }
    }
}
