using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragDropController : MonoBehaviour
{

    public static DragDropController Instance { get; private set; }


    // public RectTransform targetArea; // Drop target area
    public RectTransform targetArea1;
    public RectTransform targetArea2;
    public Image placeholderImage; // Placeholder image to change for food display
    public Image deliverButton;
    public Sprite deliveredTexture;
    public Sprite greyTexture;

    [SerializeField] private List<GameObject> draggableObjects;
    [SerializeField] private List<Sprite> displayedImages;  // Food displays
    [SerializeField] private List<string> foodNames;
    private Canvas canvas;
    private GameObject pickedGameObject;
    private RectTransform rectTransform;
    private bool isDragging = false;
    private Vector2 originalPosition;
    private CanvasGroup canvasGroup;

    private int uiButtonsLayer;
    private Sprite DisplayedImage; // Food display

    // Food Status var
    public bool _isfoodReady { get; set; } = false;
    public string _currentFood { get; set; }


    
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

        //rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        //originalPosition = rectTransform.anchoredPosition;
        //canvasGroup = GetComponent<CanvasGroup>();
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
                    int index_count = 0;
                    foreach (GameObject draggableobject in draggableObjects)
                    {

                        if (RectTransformUtility.RectangleContainsScreenPoint(draggableobject.GetComponent<RectTransform>(), touch.position, null))
                        {

                            isDragging = true;
                            pickedGameObject = draggableobject;
                            rectTransform = draggableobject.GetComponent<RectTransform>();
                            originalPosition = rectTransform.anchoredPosition;
                            canvasGroup = draggableobject.GetComponent<CanvasGroup>();
                            canvasGroup.alpha = 0.6f;
                            DisplayedImage = displayedImages[index_count];
                            _currentFood = foodNames[index_count];
                            break;
                        }
                        index_count++;
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

    public void SetPlaceholderImageToDefault()
    {
        placeholderImage.sprite = null;
        placeholderImage.enabled = false;
        deliverButton.sprite = greyTexture;
        _isfoodReady = false;
    }

    private void MoveRect(Touch touch)
    {
        // Convert the screen delta to a world space delta
        Vector2 positionDelta = touch.deltaPosition;
        Vector3 worldDelta = new Vector3(positionDelta.x, positionDelta.y, 0);

        // Transform this delta to world space taking into account the canvas' rotation and scale
        worldDelta = canvas.transform.TransformVector(worldDelta);

        // Adjust the delta by the inverse of the parent's rotation to align it with how the parent has been transformed
        worldDelta = Quaternion.Inverse(pickedGameObject.transform.parent.rotation) * worldDelta;
        //worldDelta.x *= pickedGameObject.transform.parent.lossyScale.x;
        //worldDelta.y *= pickedGameObject.transform.parent.lossyScale.y;
        //worldDelta.z *= pickedGameObject.transform.parent.lossyScale.z;

        // Convert the adjusted world delta back to local space relative to the parent, considering the canvas' scale factor
        Vector2 localDelta = new Vector2(worldDelta.x, worldDelta.y) / canvas.scaleFactor;

        // Apply this adjusted local delta to the RectTransform
        rectTransform.anchoredPosition += localDelta;
    }

    //private bool IsTouchOverUI(Vector2 touchPosition)
    //{
    //    PointerEventData pointerEventData = new PointerEventData(EventSystem.current) { position = touchPosition };
    //    List<RaycastResult> raycastResults = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(pointerEventData, raycastResults);
    //    foreach (var result in raycastResults)
    //    {
    //        if (result.gameObject.layer == uiButtonsLayer)
    //        {
    //            return true; // The touch is over a UI button
    //        }
    //    }
    //    return false; // No UI button was touched
    //}
    private void OnDrop(string area)
    {

        if (area == "Area1")
        {
            if (placeholderImage != null && DisplayedImage != null && pickedGameObject.CompareTag("Food"))
            {
                
                // draggableImage.gameObject.SetActive(true);
                placeholderImage.enabled = true;

                // fit displayed image's size
                // float width = DisplayedImage.rect.width;
                // float height = DisplayedImage.rect.height;
                // placeholderImage.rectTransform.sizeDelta = new Vector2(width, height);
                // placeholderImage.sprite = DisplayedImage;
                // deliverButton.sprite = deliveredTexture;

                float screenPercentage = 1f;  // Adjust this value to change the max percentage of the screen the image can occupy
                float screenWidth = Screen.width * screenPercentage;
                float screenHeight = Screen.height * screenPercentage;

                // Determine the scaling factor to maintain aspect ratio
                float widthFactor = screenWidth / DisplayedImage.rect.width;
                float heightFactor = screenHeight / DisplayedImage.rect.height;
                float scaleFactor = Mathf.Min(widthFactor, heightFactor);
                // Debug.Log("Scale Factor: " + scaleFactor);
                // Debug.Log("Screen Width: " + screenWidth);
                // Debug.Log("Screen Height: " + screenHeight);

                // Adjust placeholder image size while maintaining aspect ratio
                float adjustedWidth = DisplayedImage.rect.width / scaleFactor;
                float adjustedHeight = DisplayedImage.rect.height / scaleFactor;
                placeholderImage.rectTransform.sizeDelta = new Vector2(adjustedWidth, adjustedHeight);
                placeholderImage.sprite = DisplayedImage;
                deliverButton.sprite = deliveredTexture;

                // set food state and string
                _isfoodReady = true;
            }
        }

        else if (area == "Area2")
        {
            if (placeholderImage != null && !pickedGameObject.CompareTag("Food"))
            {
                // draggableImage.gameObject.SetActive(false);
                placeholderImage.sprite = null;
                placeholderImage.enabled = false;
                deliverButton.sprite = greyTexture;
                _isfoodReady = false;
            }
        }
    }
}
