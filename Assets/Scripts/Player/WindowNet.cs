using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
#if UNITY_ANDROID
        using static OVRInput;
#endif

public class WindowNet : NetworkBehaviour
{

    #region SerializeField
    private NetworkCharacterController _cc;
    private NetworkTransform _networktransform;
    private GameObject Body;
    #endregion


    [Networked] public Vector3 Scale { get; set; } = new Vector3(2f, 2f, 2f);


    public void OnSpawn()
    {
        
    }

    #region Unity Methods
    private void Awake()
    {

#if UNITY_ANDROID
//        RemoveComponent<NetworkCharacterController>();
//        RemoveComponent<CharacterController>();
//        AddComponent<NetworkTransform>();
       _networktransform = GetComponent<NetworkTransform>();
#endif
    }

    private void Start()
    {
#if UNITY_ANDROID
        
        Body = gameObject.transform.GetChild(0).gameObject;
        Body.SetActive(false);
#endif
    }
    private void Update()
    {

    }
    #endregion

  


    public override void FixedUpdateNetwork()
    {
//#if UNITY_STANDALONE_WIN || UNITY_IOS || UNITY_EDITOR_WIN
        transform.localScale = Scale;
//        Debug.Log("Scale" + Scale);
//#endif

#if UNITY_ANDROID

        if (GetInput(out NetworkInputData data))
        {
            if (data.isWindowOpening)
            {
                //if (Body != null)
                //{
                //    Body.SetActive(true);

                //}
                //Debug.Log("isWindowHorizontal:" + data.isWindowHorizontal);
                //Debug.Log("WindowPosition:" + data.windowPosition2D);
                if (data.isWindowHorizontal)
                {
                    Scale = new Vector3(1f, 0.1f, 0.1f);
                    transform.localScale = Scale;


                }
                else
                {
                    Scale = new Vector3(0.1f, 0.1f, 1f);
                    transform.localScale = Scale;
                    
                }
                transform.position = new Vector3(data.windowPosition2D.x, 1, data.windowPosition2D.y);
            }
            else
            {
                //if(Body != null)
                //{

                //    Body.SetActive(false);
                
                //}

                // a temporary solution to hide the windows on the other side
                transform.position = new Vector3(0, -2, 0);

            }
        }
#endif
    }

}