using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistObject : MonoBehaviour
{
    void Awake()
    {
        
        DontDestroyOnLoad(gameObject);
    }
}
