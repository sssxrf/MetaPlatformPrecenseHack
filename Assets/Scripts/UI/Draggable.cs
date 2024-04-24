using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Make the icon semi-transparent while dragging
        canvasGroup.blocksRaycasts = false; // Allow events to pass through, needed for drop detection
    }

    public void OnDrag(PointerEventData eventData)
    {
        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        Vector3 worldDelta = new Vector3(eventData.delta.x, eventData.delta.y, 0);
        // Convert screen delta to world delta
        worldDelta = canvas.transform.TransformVector(worldDelta);
        // Apply the inverse rotation of the parent
        worldDelta = Quaternion.Inverse(transform.parent.rotation) * worldDelta;
        // Transform the world delta back to local space
        Vector2 localDelta = new Vector2(worldDelta.x, worldDelta.y) / canvas.scaleFactor;
        // Apply the adjusted local delta to the RectTransform
        rectTransform.anchoredPosition += localDelta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; // Restore full opacity
        canvasGroup.blocksRaycasts = true; // Resume block raycasts
        rectTransform.anchoredPosition = originalPosition; // back to original position
    }
}
