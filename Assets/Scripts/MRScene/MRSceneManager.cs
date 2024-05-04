using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Meta.XR.MRUtilityKit;
using System;

public class MRSceneManager : MonoBehaviour
{
    public static MRSceneManager Instance { get; private set; }
    public bool testingMode = false;


    #region SerializeField
    [SerializeField] OVRSceneManager sceneManager;
    [SerializeField] GameObject projector;
    [SerializeField] GameObject skybox;
    [SerializeField] GameObject LeftController;
    [SerializeField] GameObject RightController;
    [SerializeField] int numofPotentialPosForGuests = 8;
    [SerializeField] float spawnOffset = 0.5f;
    [SerializeField] float projectedOffset = 0.556f; 
    private static OVRSceneRoom m_SceneRoom;

    //private List<OVRScenePlane> m_SceneWalls = new List<OVRScenePlane>();
    private OVRScenePlane[] m_SceneWalls;
    private OVRScenePlane m_SceneCeiling;
    private OVRScenePlane m_SceneFloor;
    private OVRScenePlane m_PortalWall;



    private Player _player;
    public GameObject _headset;
    private Vector2 _playerRelativePos;
    private float _roomLength;
    private float _roomWidth;
    private float _roomHeight;
    private Vector3 _roomCenter;
    private Transform _floorTrans;
    private float _calibrationHeight = -1;
    private bool _isCalibrationCompleted = false;
    private bool _isRoomInfoSent = false;
    private bool roomSetupTriggered = false;
    private List<Vector3> _potentialSpawnedPositions;
    private bool _isSpawnedPointsCalculated = false;
    private Vector3 _calibratedRoomCenter;


    public float RoomLength => _roomLength;
    public float RoomWidth => _roomWidth;
    public Vector2 PlayerRelativePos => _playerRelativePos;
    public Transform FloorTrans => _floorTrans;
    public Vector3 RoomCenter => _roomCenter;
    public float calibrationHeight => _calibrationHeight;

    public event Action OnRoomSetupComplete;
    // Spawned points generated
    public List<Vector3> PotentialSpawnedPositions => _potentialSpawnedPositions;
    public bool IsSpawnedPointsCalculated => _isSpawnedPointsCalculated;
    public  Vector3 CalibratedRoomCenter => _calibratedRoomCenter;


    #endregion

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

        if (sceneManager == null)
            sceneManager = GameObject.FindObjectOfType<OVRSceneManager>();

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_ANDROID
        OVRManager.eyeFovPremultipliedAlphaModeEnabled = false;
#endif

        sceneManager.SceneModelLoadedSuccessfully += OnSceneLoaded;
        _potentialSpawnedPositions = new List<Vector3>();
    }

    IEnumerator FindPlayerWithDelay(float delay)
    {
        while (_player == null)
        {
            yield return new WaitForSeconds(delay);
            _player = FindObjectOfType<Player>();
        }
        Debug.Log("Player is found!");
    }

    private void Start()
    {
        StartCoroutine(FindPlayerWithDelay(0.1f));
        //OnRoomSetupComplete += () => StartCoroutine(GeneratePointsAroundCircleCoroutine());
    }

    private void Update()
    {

        if (testingMode)
        {
            if ( _isCalibrationCompleted && !roomSetupTriggered)
            {
                roomSetupTriggered = true;
                StartCoroutine(GeneratePointsAroundCircleCoroutine());
                //OnRoomSetupComplete?.Invoke();
            }
        }
        else
        {
            if (_isRoomInfoSent && _isCalibrationCompleted && !roomSetupTriggered)
            {
                roomSetupTriggered = true;
                StartCoroutine(GeneratePointsAroundCircleCoroutine());
                //OnRoomSetupComplete?.Invoke();
            }
        }

        if (_headset != null && m_SceneFloor != null)
        {
            _floorTrans = m_SceneFloor.transform;
            _playerRelativePos = CalculateLocalPosition(_headset.transform.position, _floorTrans);
            //Debug.Log("length:" + _roomLength + "width:" + _roomWidth);
        }
    }


    #endregion

    #region Public methods
    IEnumerator GeneratePointsAroundCircleCoroutine()
    {
        //yield return null;
        Vector3 center = _calibratedRoomCenter;
        float radius = Mathf.Sqrt(Mathf.Pow(RoomLength / 2, 2) + Mathf.Pow(RoomWidth / 2, 2)) + spawnOffset;
        float angleStep = 360f / numofPotentialPosForGuests;

        for (int i = 0; i < numofPotentialPosForGuests; i++)
        {
            float angleInDegrees = i * angleStep;
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
            Vector3 point = new Vector3(
                center.x + Mathf.Cos(angleInRadians) * radius,
                center.y,
                center.z + Mathf.Sin(angleInRadians) * radius
            );
            _potentialSpawnedPositions.Add(point);

            // Yield control back to Unity after processing a few points
            if (i % 10 == 0)  // Adjust this value based on performance observations
                yield return null;
        }

        _isSpawnedPointsCalculated = true;
        Debug.Log("Generated Points Around Circle, total num: " + _potentialSpawnedPositions.Count);
    }
    public void GeneratePointsAroundCircle()
    {
        
        Vector3 center = _calibratedRoomCenter;
        float radius = Mathf.Sqrt(Mathf.Pow(RoomLength / 2, 2) + Mathf.Pow(RoomWidth / 2, 2)) + spawnOffset;
        float angleStep = 360f / numofPotentialPosForGuests;
        for (int i = 0; i < numofPotentialPosForGuests; i++)
        {
            float angleInDegrees = i * angleStep;
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
            Vector3 point = new Vector3(
                center.x + Mathf.Cos(angleInRadians) * radius,
                center.y,
                center.z + Mathf.Sin(angleInRadians) * radius
            );
            _potentialSpawnedPositions.Add(point);
        }
        _isSpawnedPointsCalculated = true;
        Debug.Log("generate Points areound, total num: " + _potentialSpawnedPositions.Count);
    }

    public void SendRoomInfo()
    {
        
        _player.RPC_SendRoomInfo(MRSceneManager.Instance.RoomLength, MRSceneManager.Instance.RoomWidth);
        _isRoomInfoSent = true;
    }


    public void ConfirmCalibration()
    {
        _calibrationHeight = (RightController.transform.position.y + LeftController.transform.position.y) / 2f;
        _calibratedRoomCenter = new Vector3(_roomCenter.x, _calibrationHeight, _roomCenter.z);
        CalibrateProjectorAndSkybox();
        _isCalibrationCompleted = true;
        Debug.Log("calibrationHeight:" + _calibrationHeight);
    }
    #endregion

    #region Private Methods


    private void OnSceneLoaded()
    {
        m_SceneRoom = GameObject.FindObjectOfType<OVRSceneRoom>();
        // m_SceneRoom.gameObject.SetLayerRecursive("Room");

        // Mesh setup
        //effectMesh.CreateMesh();
        //layerApplier.GetRoomObjectAndApplyLayer();

        m_SceneCeiling = m_SceneRoom.Ceiling;
        m_SceneFloor = m_SceneRoom.Floor;

        m_SceneWalls = m_SceneRoom.Walls;
        ApplyLayerWalls();

        GetRoomSizeSquare();
        _roomHeight = m_SceneWalls[0].Height;
        _roomCenter = GetFloorCenter();


        // set projector's and skybox's position at center
        projector.transform.position = _roomCenter;
        skybox.transform.position = _roomCenter;

        Debug.Log("Room center:" + _roomCenter);
        Debug.Log("length:"+ _roomLength + "width:" + _roomWidth);
    }

    private void CalibrateProjectorAndSkybox()
    {
        projector.transform.position = new Vector3(_calibratedRoomCenter.x, _calibratedRoomCenter.y - projectedOffset, _calibratedRoomCenter.z);
        skybox.transform.position = new Vector3(_calibratedRoomCenter.x, _calibratedRoomCenter.y - projectedOffset, _calibratedRoomCenter.z);
    }

    private void GetRoomSizeSquare()
    {
        _roomLength = m_SceneFloor.Width;
        _roomWidth = m_SceneFloor.Height;
    }

    private Vector3 GetFloorCenter()
    {
        var floorCollider = m_SceneFloor.GetComponentInChildren<Collider>(); ;
        return floorCollider.transform.position;
    }

    private Vector2 CalculateLocalPosition(Vector3 worldPosition, Transform referenceTransform)
    {
        

        // This converts the world position of objectA to the local coordinate system of objectB
        Vector3 localPosition3D = referenceTransform.InverseTransformPoint(worldPosition);

        
        Vector2 localPosition2D = new Vector2(localPosition3D.x, localPosition3D.y);


        return localPosition2D;
    }

    private void ApplyLayer(GameObject obj, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        obj.layer = layer;

        foreach (Transform child in obj.transform) ApplyLayer(child.gameObject, layerName);
    }

    private void ApplyLayerWalls()
    {
        foreach (OVRScenePlane wall in m_SceneWalls)
        {
            ApplyLayer(wall.gameObject, "Wall");
        }
    }

    #endregion
}
