using UnityEngine;
using UnityEngine.UI;

public class DragDropController : MonoBehaviour
{
    public RectTransform targetArea; // Drop target area
    public Image placeholderImage; // Placeholder image to change for food display
    public Sprite DisplayedImage; // Food display

    private Canvas canvas;
    private RectTransform rectTransform;
    private bool isDragging = false;
    private Vector2 originalPosition;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, touch.position, null))
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
                        if (RectTransformUtility.RectangleContainsScreenPoint(targetArea, touch.position, null))
                        {
                            OnDrop();
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
        Vector2 positionDelta = touch.deltaPosition;
        Vector3 worldDelta = canvas.transform.TransformVector(new Vector3(positionDelta.x, positionDelta.y, 0));
        Vector2 localDelta = new Vector2(worldDelta.x, worldDelta.y) / canvas.scaleFactor;
        rectTransform.anchoredPosition += localDelta;
    }

    private void OnDrop()
    {
        // Optionally change an image or do other effects
        if (placeholderImage != null && DisplayedImage != null)
        {
            placeholderImage.sprite = DisplayedImage;
            placeholderImage.enabled = true;
        }
    }
}
