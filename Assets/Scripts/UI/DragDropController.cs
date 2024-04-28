using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragDropController : MonoBehaviour
{
    // public RectTransform targetArea; // Drop target area
    public RectTransform targetArea1;
    public RectTransform targetArea2;
    public Image placeholderImage; // Placeholder image to change for food display
    public Sprite DisplayedImage; // Food display
    public Image deliverButton;
    public Sprite deliveredTexture;
    public Sprite greyTexture;

    private Canvas canvas;
    private RectTransform rectTransform;
    private bool isDragging = false;
    private Vector2 originalPosition;
    private CanvasGroup canvasGroup;

    private int uiButtonsLayer;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
        uiButtonsLayer = LayerMask.NameToLayer("UIButtons");
    }

   

    void Update()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, touch.position, null) && !IsTouchOverUI(touch.position))
                    {
                        isDragging = true;
                        canvasGroup.alpha = 0.6f;
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        MoveRect(touch);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isDragging)
                    {
                        // if (RectTransformUtility.RectangleContainsScreenPoint(targetArea, touch.position, null))
                        // {
                        //     OnDrop();
                        // }
                        if (RectTransformUtility.RectangleContainsScreenPoint(targetArea1, touch.position, null))
            {
                OnDrop("Area1");
            }
                        else if (RectTransformUtility.RectangleContainsScreenPoint(targetArea2, touch.position, null))
            {
                OnDrop("Area2");
            }
                        canvasGroup.alpha = 1f;
                        rectTransform.anchoredPosition = originalPosition;
                        isDragging = false;
                    }
                    break;
            }
        }
    }

    private void MoveRect(Touch touch)
    {
        // Convert the screen delta to a world space delta
        Vector2 positionDelta = touch.deltaPosition;
        Vector3 worldDelta = new Vector3(positionDelta.x, positionDelta.y, 0);

        // Transform this delta to world space taking into account the canvas' rotation and scale
        worldDelta = canvas.transform.TransformVector(worldDelta);

        // Adjust the delta by the inverse of the parent's rotation to align it with how the parent has been transformed
        worldDelta = Quaternion.Inverse(transform.parent.rotation) * worldDelta;

        // Convert the adjusted world delta back to local space relative to the parent, considering the canvas' scale factor
        Vector2 localDelta = new Vector2(worldDelta.x, worldDelta.y) / canvas.scaleFactor;

        // Apply this adjusted local delta to the RectTransform
        rectTransform.anchoredPosition += localDelta;
    }

    private bool IsTouchOverUI(Vector2 touchPosition)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current) { position = touchPosition };
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        foreach (var result in raycastResults)
        {
            if (result.gameObject.layer == uiButtonsLayer)
            {
                return true; // The touch is over a UI button
            }
        }
        return false; // No UI button was touched
    }
    private void OnDrop(string area)
    {
        // Optionally change an image or do other effects
        // if (placeholderImage != null && DisplayedImage != null)
        // {
        //     placeholderImage.sprite = DisplayedImage;
        //     if(area == "Area1")
        //     {
        //     placeholderImage.enabled = true;

        //     }
        //     else if(area == "Area2")
        //     {
        //     // placeholderImage.enabled = false;
        //     placeholderImage.sprite = null;
        //     }
        // }

    if (area == "Area1")
    {
        if (placeholderImage != null && DisplayedImage != null)
        {
            Debug.Log("Setting displayed image for Area1");
            // draggableImage.gameObject.SetActive(true);
            placeholderImage.enabled = true;
            placeholderImage.sprite = DisplayedImage;
            deliverButton.sprite = deliveredTexture;
        }

    }
    else if (area == "Area2")
    {
        if (placeholderImage != null)
        {
            // draggableImage.gameObject.SetActive(false);
            placeholderImage.enabled = false;
            deliverButton.sprite = greyTexture;

        }
    }
    }
}
