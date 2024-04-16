using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerMR : MonoBehaviour
{

    private Player _player;
    [SerializeField] private GameObject _headset;
    // Start is called before the first frame update

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

    public void SendRoomInfo()
    {
        Debug.Log("Trigger Pressed");
        _player.RPC_SendMessage("RoomInfo");
    }
}
