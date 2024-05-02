using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectorManager : MonoBehaviour
{

    public static ProjectorManager Instance { get; private set; }
    public enum ProjectorState
    {
        Idle,
        Grabbed,
        Rotating
    }


    #region SerializedField
    [SerializeField] GameObject ProjectedWindow;
    [SerializeField] private List<Transform> ProjectorDirections;
    [SerializeField] private GrabInteractable grabInteractable;

    private float goalAngle = 0f;
    private int currentDirectionIndex = 0;
    private int wallLayerMask;
    private bool isInitialized;
    private List<viewingWindowInterator> ProjectedWindows = new List<viewingWindowInterator>();
    private ProjectorState state = ProjectorState.Idle;
    private viewingWindowInterator currentviewingWindowInterator;
    private GameObject currentOpeningWindow;

    private Vector2 _windowPos2D;
    private bool _isOpening = false;
    private static float _windowLen = 2f;
    private bool _isWindowHorizontal = false;
    public Vector2 windowPos2D => _windowPos2D;
    public bool isOpening => _isOpening;
    public float windowLen => _windowLen;
    public bool isWindowHorizontal => _isWindowHorizontal;

    #endregion

    public void Initialized()
    {
        isInitialized = true;
        wallLayerMask = LayerMask.GetMask("Wall");
    }
    
    
    public void createProjectedWindow()
    {
        //Debug.Log("Creating Projected Window");
        //MRUKRoom mrukComponent = FindObjectOfType<MRUKRoom>();
        //if (mrukComponent == null)
        //{
        //    Debug.LogError("No MRUKRoom found");
        //    return;
        //}
        // try get the room 
        // ray cast from four directions,swapn window at the hit point
        //foreach (var direction in ProjectorDirections)
        //{
        //Vector3 rayOrigin = direction.position;
        //Vector3 rayDirection = direction.forward;

        // make sure there only exist one opening window
        if (currentOpeningWindow != null) 
        { 
            Destroy(currentOpeningWindow);
            _isOpening = false;
        }

        // Open the window at the direction pointed by the indicator
        Vector3 rayOrigin = ProjectorDirections[0].position;
        Vector3 rayDirection = ProjectorDirections[0].forward;
        Ray ray = new Ray(rayOrigin, rayDirection);
        Debug.DrawRay(rayOrigin, rayDirection, Color.red, 1000);
        wallLayerMask =  LayerMask.GetMask("Wall");
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, wallLayerMask))
        {
            Debug.Log("Found wall hit");
            currentOpeningWindow = Instantiate(ProjectedWindow, hit.point, Quaternion.LookRotation(-hit.normal) );
            // rotate the window to 90 degree on y 
            currentOpeningWindow.transform.Rotate(0,90,0);
            // newWall.transform.position = hit.point- hit.normal * 0.1f;
            currentviewingWindowInterator = currentOpeningWindow.GetComponent<viewingWindowInterator>();
            //ProjectedWindows.Add(currentOpeningWindow.GetComponent<viewingWindowInterator>());


        }
        //currentviewingWindowInterator.SwitchWindow();
        //ProjectedWindows[currentDirectionIndex].SwitchWindow();
        _isOpening = true;

        //Debug.Log("Found wall");
        //}
    }

    public void SwitchWindowBlock()
    {
        
        if (currentOpeningWindow != null && currentviewingWindowInterator != null)
        {
            currentviewingWindowInterator.SwitchWindow();
            _isOpening = !_isOpening;
        }
    }
    #region Unity Methods
    private void Awake()
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
    }
    // Start is called before the first frame update
    void Start()
    {
       
        grabInteractable.WhenSelectingInteractorRemoved.Action += RotationFixed;
        grabInteractable.WhenSelectingInteractorAdded.Action += Grabbed;
    }

    
  
   
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

        //Debug.Log("Opening State:" + _isOpening);
        if (currentOpeningWindow != null)
        {
            if (_isOpening)
            {

                //_windowPos2D = new Vector2(currentOpeningWindow.transform.position.x, currentOpeningWindow.transform.position.z);
                _windowPos2D = CalculateLocalPosition(currentOpeningWindow.transform.position, MRSceneManager.Instance.FloorTrans);
                _isWindowHorizontal = IsClosestEdgeLengthorWidth(MRSceneManager.Instance.RoomLength, MRSceneManager.Instance.RoomWidth, _windowPos2D);
            }
            else
            {
                _windowPos2D = Vector2.zero;
            }

        }
        else
        {
           // Debug.Log("No Opening Window");

            _windowPos2D = Vector2.zero;
        }


    }

    #endregion

    #region Private Methods

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
        //Debug.Log(currentYaw);
        //Debug.Log("closest direction " + minIndex + " " + rotationAngels[minIndex]);
        // rotate to the closes direction
        goalAngle = rotationAngels[minIndex];
        state = ProjectorState.Rotating;

        // Delete current window
        Destroy(currentOpeningWindow);
        _isOpening = false;


    }

    private Vector2 CalculateLocalPosition(Vector3 worldPosition, Transform referenceTransform)
    {
        

        // This converts the world position of objectA to the local coordinate system of objectB
        Vector3 localPosition3D = referenceTransform.InverseTransformPoint(worldPosition);

        
        Vector2 localPosition2D = new Vector2(localPosition3D.x, localPosition3D.y);


        return localPosition2D;
    }
   

    private bool IsClosestEdgeLengthorWidth(float roomLength, float roomWidth, Vector2 windowRelativePos2D)
    {
        float halfLength = roomLength / 2;
        float halfWidth = roomWidth / 2;

        // Calculate distances to each edge
        float distanceToLeft = Mathf.Abs(windowRelativePos2D.x + halfLength);
        float distanceToRight = Mathf.Abs(windowRelativePos2D.x - halfLength);
        float distanceToTop = Mathf.Abs(windowRelativePos2D.y - halfWidth);
        float distanceToBottom = Mathf.Abs(windowRelativePos2D.y + halfWidth);

        // Determine the smallest distance and corresponding edge
        float minDistance = Mathf.Min(distanceToLeft, distanceToRight, distanceToTop, distanceToBottom);

        Debug.Log("mindist:" + minDistance);
        if (minDistance == distanceToLeft || minDistance == distanceToRight)
        {
            return false;   //width, then window is vertical
        }
        else
        {
            return true;    //length, then window is horizontal
        }
    }
    #endregion
}
