using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }


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

    private void Start()
    {
        // for debug
        //DrawCenteredRoom(4f, 3f);
    }

    #endregion
    public void DrawCenteredRoom(float length, float width)
    {


        GameObject rectangle = new GameObject("Room");
        LineRenderer lineRenderer = rectangle.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineRenderer.endColor = Color.blue;
        lineRenderer.startWidth = lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 5;

        Vector3[] positions = new Vector3[5];
        positions[0] = new Vector3(-length / 2, 0, -width / 2);
        positions[1] = new Vector3(-length / 2, 0, width / 2);
        positions[2] = new Vector3(length / 2, 0, width / 2);
        positions[3] = new Vector3(length / 2, 0, -width / 2);
        positions[4] = positions[0];

        lineRenderer.SetPositions(positions);
    }
}
