using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IDropHandler
{
    public Image placeholderImage; // Assign the child image placeholder in the inspector
    public Sprite newImage; // Assign the sprite that it should display

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            placeholderImage.sprite = newImage; // Change the placeholder's sprite
            placeholderImage.enabled = true; // Make the placeholder visible
        }
    }
}
