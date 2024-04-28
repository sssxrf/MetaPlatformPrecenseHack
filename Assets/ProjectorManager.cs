using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectorManager : MonoBehaviour
{
    public enum ProjectorState
    {
        Idle,
        Grabbed,
        Rotating
    }
    [SerializeField] GameObject ProjectedWindow;
    [SerializeField] private List<Transform> ProjectorDirections;
    [SerializeField] private GrabInteractable grabInteractable;

    private float goalAngle = 0f;
    private int currentDirectionIndex = 0;
    private int wallLayerMask;
    private bool isInitialized;
    private List<viewingWindowInterator> ProjectedWindows = new List<viewingWindowInterator>();
    private ProjectorState state = ProjectorState.Idle;
    public void Initialized()
    {
        isInitialized = true;
        wallLayerMask = LayerMask.GetMask("Wall");
    }
    
    [Button]
    public void createProjectedWindow()
    {
        Debug.Log("Creating Projected Window");
        MRUKRoom mrukComponent = FindObjectOfType<MRUKRoom>();
        if (mrukComponent == null)
        {
            Debug.LogError("No MRUKRoom found");
            return;
        }
        // try get the room 
        // ray cast from four directions,swapn window at the hit point
        foreach (var direction in ProjectorDirections)
        {
            Vector3 rayOrigin = direction.position;
            Vector3 rayDirection = direction.forward;
            Ray ray = new Ray(rayOrigin, rayDirection);
            Debug.DrawRay(rayOrigin, rayDirection, Color.red, 1000);
            wallLayerMask =  LayerMask.GetMask("Wall");
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, wallLayerMask))
            {
                Debug.Log("Found wall hit");
                var newWall = Instantiate(ProjectedWindow, hit.point, Quaternion.LookRotation(-hit.normal) );
                // rotate the window to 90 degree on y 
                newWall.transform.Rotate(0,90,0);
                // newWall.transform.position = hit.point- hit.normal * 0.1f;
                ProjectedWindows.Add(newWall.GetComponent<viewingWindowInterator>());
               
                
            }
            
            ProjectedWindows[currentDirectionIndex].SwitchWindow();
            Debug.Log("Found wall");
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
       
        grabInteractable.WhenSelectingInteractorRemoved.Action += RotationFixed;
        grabInteractable.WhenSelectingInteractorAdded.Action += Grabbed;
    }

    private void Grabbed(GrabInteractor obj)
    {
        state = ProjectorState.Grabbed;
    }

    private void RotationFixed(GrabInteractor obj)
    {
        // check current yaw, check the closes direction, rotate to that direction
        float currentYaw = transform.rotation.eulerAngles.y;
        float minDiff = 360;
        int minIndex = 0;
        List<float> rotationAngels = new List<float>();
        rotationAngels.Add(0);
        rotationAngels.Add(90);
        rotationAngels.Add(180);
        rotationAngels.Add(270);
        for (int i = 0; i < rotationAngels.Count; i++)
        {
            Debug.Log("ProjectorDirections[i].rotation.eulerAngles.y " + rotationAngels[i]);
            float diff = Quaternion.Angle(transform.rotation, Quaternion.Euler(0, rotationAngels[i], 0));
            if (diff < minDiff)
            {
                minDiff = diff;
                minIndex = i;
            }
        }
        Debug.Log(currentYaw);
        Debug.Log("closest direction " + minIndex + " " + rotationAngels[minIndex]);
        // rotate to the closes direction
        goalAngle = rotationAngels[minIndex];
        state = ProjectorState.Rotating;

    }
    // coroutine to rotate the projector
  
   
    // Update is called once per frame
    void Update()
    {
        if (state == ProjectorState.Rotating)
        {
            // lerp the rotation
            float step = 20 * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, goalAngle, 0), step);
            float diff = Quaternion.Angle(transform.rotation, Quaternion.Euler(0, goalAngle, 0));
            if (diff < 2f)
            {
                state = ProjectorState.Idle;
            }
        }
    }
}
