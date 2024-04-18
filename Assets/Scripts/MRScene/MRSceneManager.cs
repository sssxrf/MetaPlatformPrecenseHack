using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MRSceneManager : MonoBehaviour
{
    public static MRSceneManager Instance { get; private set; }

    [Header("OVR Field")]
    [SerializeField] OVRSceneManager sceneManager;
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
    public float RoomLength => _roomLength;
    public float RoomWidth => _roomWidth;

    public Vector2 PlayerRelativePos => _playerRelativePos;

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

            _playerRelativePos = CalculateLocalPosition(_headset.transform.position, m_SceneFloor.transform);
        }
    }
    private void OnSceneLoaded()
    {
        m_SceneRoom = GameObject.FindObjectOfType<OVRSceneRoom>();
        // m_SceneRoom.gameObject.SetLayerRecursive("Room");

        m_SceneCeiling = m_SceneRoom.Ceiling;
        m_SceneFloor = m_SceneRoom.Floor;

        m_SceneWalls = m_SceneRoom.Walls;
       
        GetRoomSizeSquare();

        //Debug.Log("length:"+ _roomLength + "width:" + _roomWidth);
    }

    public void SendRoomInfo()
    {
        Debug.Log("Trigger Pressed");
        _player.RPC_SendRoomInfo(MRSceneManager.Instance.RoomLength, MRSceneManager.Instance.RoomWidth);
    }

    private void GetRoomSizeSquare()
    {
        _roomLength = m_SceneFloor.Width;
        _roomWidth = m_SceneFloor.Height;
    }

    private Vector2 CalculateLocalPosition(Vector3 worldPosition, Transform referenceTransform)
    {
        

        // This converts the world position of objectA to the local coordinate system of objectB
        Vector3 localPosition3D = referenceTransform.InverseTransformPoint(worldPosition);

        
        Vector2 localPosition2D = new Vector2(localPosition3D.x, localPosition3D.y);


        return localPosition2D;
    }
}
