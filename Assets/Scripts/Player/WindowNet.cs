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
        Body.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Body.SetActive(false);
#endif
    }
    private void Update()
    {

    }
    #endregion

  


    public override void FixedUpdateNetwork()
    {

#if UNITY_ANDROID

        if (GetInput(out NetworkInputData data))
        {
            if (data.isWindowOpening)
            {
                if (Body != null)
                {
                    Body.SetActive(true);

                }
                transform.position = new Vector3(data.windowPosition2D.x, 1, data.windowPosition2D.y);
            }
            else
            {
                if(Body != null)
                {

                    Body.SetActive(false);
                
                }
            }
        }
#endif
    }

}