using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowScaleManager : MonoBehaviour
{

    private GameObject _windowOnHand;
    private GameObject _windowNet;

    private Vector2 _winPos2;
    private bool isWinHorizontal;

    IEnumerator FindWinNetWithDelay(float delay)
    {
        while (_windowNet == null)
        {
            yield return new WaitForSeconds(delay);
            _windowNet = GameObject.Find("WindowNetworkObject(Clone)");
        }
        Debug.Log("WindowNet is found!");
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FindWinNetWithDelay(0.1f));
        _windowOnHand = gameObject;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (_windowOnHand != null && _windowNet != null)
        {

            _windowOnHand.transform.position = _windowNet.transform.position;
            _winPos2 = new Vector2(_windowNet.transform.position.x, _windowNet.transform.position.z);
            isWinHorizontal = IsClosestEdgeLengthorWidth(RoomManager.Instance.roomLengthinMR, RoomManager.Instance.roomwidthinMR, _winPos2);
            if (isWinHorizontal)
            {

                // transform.localScale = new Vector3(1f, 0.1f, 0.3f);
                transform.localScale = new Vector3(0.8f, 0.06f, 0.18f);
            }
            else
            {
   
                // transform.localScale = new Vector3(0.3f, 0.1f, 1f);
                transform.localScale = new Vector3(0.15f, 0.05f, 0.5f);
            }


        }
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

        //Debug.Log("mindist:" + minDistance);
        if (minDistance == distanceToLeft || minDistance == distanceToRight)
        {
            return false;   //width, then window is vertical
        }
        else
        {
            return true;    //length, then window is horizontal
        }
    }
}
