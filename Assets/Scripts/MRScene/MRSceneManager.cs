using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Meta.XR.MRUtilityKit;

public class MRSceneManager : MonoBehaviour
{
    public static MRSceneManager Instance { get; private set; }



    #region SerializeField
    [SerializeField] OVRSceneManager sceneManager;
    [SerializeField] GameObject projector;
    [SerializeField] GameObject skybox;

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
    public float RoomLength => _roomLength;
    public float RoomWidth => _roomWidth;
    public Vector2 PlayerRelativePos => _playerRelativePos;
    public Transform FloorTrans => _floorTrans;
    public Vector3 RoomCenter => _roomCenter;
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
    }

    private void Update()
    {

        if(_headset != null && m_SceneFloor != null)
        {
            _floorTrans = m_SceneFloor.transform;
            _playerRelativePos = CalculateLocalPosition(_headset.transform.position, _floorTrans);
            //Debug.Log("length:" + _roomLength + "width:" + _roomWidth);
        }
    }


    #endregion

    #region Public methods
    public void SendRoomInfo()
    {
        
        _player.RPC_SendRoomInfo(MRSceneManager.Instance.RoomLength, MRSceneManager.Instance.RoomWidth);
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
