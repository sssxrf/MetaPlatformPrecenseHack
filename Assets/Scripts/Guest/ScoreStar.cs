using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class ScoreStar : MonoBehaviour
{
    private GameObject Headset;
    [SerializeField] private float speed = 5f;
    [SerializeField] MeshRenderer starMeshRenderer;
    public UnityEvent onStarCollected;
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<CollectZone>())
        {
            starMeshRenderer.enabled = false;
            onStarCollected.Invoke();
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Headset != null)
        {
            // Calculate the direction towards the target
            Vector3 targetPosition = Headset.transform.position;

            // Smoothly move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            //find game object with collect zone 
            Headset = FindObjectOfType<CollectZone>().GameObject();
            
        }
    }
}

public class UnityEventy
{
}
