using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleBullet : MonoBehaviour
{
    private bool passthroughWall = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<viewingWindowInterator>())
        {
            passthroughWall = true;
            Debug.Log("passthroughWall = true");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (passthroughWall && collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // ignore collision
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
