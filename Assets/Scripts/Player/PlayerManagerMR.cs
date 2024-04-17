using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerMR : MonoBehaviour
{
    public static PlayerManagerMR Instance { get; private set; }

    private Player _player;
    [SerializeField] private GameObject _headset;
    // Start is called before the first frame update


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

    // Update is called once per frame
    void Update()
    {
        if(_player != null && _headset != null)
        {
            _player.transform.position = new Vector3(_headset.transform.position.x,
                                      _player.transform.position.y,
                                      _headset.transform.position.z);
        }
    }

    #endregion

    public void SendRoomInfo()
    {
        Debug.Log("Trigger Pressed");
        _player.RPC_SendRoomInfo(MRSceneManager.Instance.RoomLength, MRSceneManager.Instance.RoomWidth);
    }
}
