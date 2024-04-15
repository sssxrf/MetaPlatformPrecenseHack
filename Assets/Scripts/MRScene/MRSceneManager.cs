using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MRSceneManager : MonoBehaviour
{
    public static MRSceneManager Instance { get; private set; }

    [Header("Dependencies")]
    [SerializeField] OVRSceneManager sceneManager;
    private static OVRSceneRoom m_SceneRoom;

    //private List<OVRScenePlane> m_SceneWalls = new List<OVRScenePlane>();
    private OVRScenePlane[] m_SceneWalls;
    private OVRScenePlane m_SceneCeiling;
    private OVRScenePlane m_SceneFloor;
    private OVRScenePlane m_PortalWall;

    private float _roomLength;
    private float _roomWidth;
    public float RoomLength => _roomLength;
    public float RoomWidth => _roomWidth;

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


    private void OnSceneLoaded()
    {
        m_SceneRoom = GameObject.FindObjectOfType<OVRSceneRoom>();
        // m_SceneRoom.gameObject.SetLayerRecursive("Room");

        m_SceneCeiling = m_SceneRoom.Ceiling;
        m_SceneFloor = m_SceneRoom.Floor;

        m_SceneWalls = m_SceneRoom.Walls;
        GetRoomSizeSquare();
    }

    private void GetRoomSizeSquare()
    {
        _roomLength = m_SceneFloor.Width;
        _roomWidth = m_SceneFloor.Height;
    }
}
