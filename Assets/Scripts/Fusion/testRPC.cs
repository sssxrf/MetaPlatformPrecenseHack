using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRPC : MonoBehaviour
{
    private GameObject _winPlayerPrefab;
    private Player _player;

    IEnumerator FindPlayerWithDelay(float delay)
    {
        while (_winPlayerPrefab == null)
        {
            yield return new WaitForSeconds(delay);
            _winPlayerPrefab = GameObject.Find("PlayerPrefab(Clone)");
        }
        Debug.Log("PCPlayer is found!");
        _player = _winPlayerPrefab.GetComponent<Player>();
        OnDeliverButtonClick();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FindPlayerWithDelay(0.1f));
    }

    public void OnDeliverButtonClick()
    {
        
            Debug.Log("deliever button called");
            _player.RPC_SendFoodInfo("yesyes");

           
        
    }
}
