using UnityEngine;

public class WindowMaker : MonoBehaviour
{
    public GameObject prefabToPlace;
    public GameObject debugVisual;

    private bool isInitialized;
    private int wallLayerMask;
    private AudioSource audioSource;

    public void Initialized()
    {
        isInitialized = true;
        wallLayerMask = LayerMask.GetMask("Wall");
        debugVisual = Instantiate(debugVisual);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isInitialized) return;

        Vector3 rayOrigin = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Vector3 rayDirection = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, wallLayerMask))
        {
            debugVisual.transform.position = hit.point;

            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                Quaternion rotation = Quaternion.LookRotation(-hit.normal);
                Vector3 placementPosition = hit.point + hit.normal * 0.01f;

                Instantiate(prefabToPlace, placementPosition, rotation);
                audioSource.Play();
            }
        }
    }
}
