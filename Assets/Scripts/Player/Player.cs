using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
#if UNITY_ANDROID
        using static OVRInput;
#endif

public class Player : NetworkBehaviour
{

    #region SerializeField
    private NetworkCharacterController _cc;
    private NetworkTransform _networktransform;
    #endregion

    #region Unity Methods
    private void Awake()
    {
#if UNITY_STANDALONE_WIN || UNITY_IOS
        _cc = GetComponent<NetworkCharacterController>();

#endif
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
        gameObject.transform.GetChild(0).transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
#endif
    }
    private void Update()
    {
#if UNITY_STANDALONE_WIN
        if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
        {
            RPC_SendMessage("Hey Mate!");
        }

#endif

#if UNITY_ANDROID
        //if ( OVRInput.Get(OVRInput.RawButton.A))
        //{
        //    RPC_SendMessage("Length"+ MRSceneManager.Instance.RoomLength + "width:" + MRSceneManager.Instance.RoomWidth);
        //}
#endif
    }
    #endregion

    #region RPC Messages
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        RPC_RelayMessage(message, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayMessage(string message, PlayerRef messageSource)
    {
        

        if (messageSource == Runner.LocalPlayer)
        {
            
            message = $"You said: {message}\n";
        }
        else
        {
            message = $"Some other player said: {message}\n";
        }

#if UNITY_STANDALONE_WIN
        PcUIManager.Instance.UpdateMessages( message);
#endif

#if UNITY_ANDROID
        HeadSetUIManager.Instance.UpdateMessages(message);
#endif

#if UNITY_IOS
        MobileUIManager.Instance.UpdateMessages(message);
#endif

    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendRoomInfo(float roomlength, float roomwidth, RpcInfo info = default)
    {
        RPC_RelayRoomInfo(roomlength, roomwidth, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayRoomInfo(float roomlength, float roomwidth, PlayerRef messageSource)
    {

#if UNITY_STANDALONE_WIN
        RoomManager.Instance.DrawCenteredRoom(roomlength, roomwidth);
        PcUIManager.Instance.UpdateMessages( "Room set!");
#endif

#if UNITY_ANDROID

        HeadSetUIManager.Instance.UpdateMessages("RoomInfo sent");
#endif
    }
    #endregion



    public override void FixedUpdateNetwork()
    {
#if UNITY_STANDALONE_WIN || UNITY_IOS

        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);
        }
#endif
#if UNITY_ANDROID

        if (GetInput(out NetworkInputData data))
        {
            transform.position = new Vector3(data.headsetPosition2D.x, 1, data.headsetPosition2D.y);
        }
#endif
    }

    #region Public Methods
    public void RemoveComponent<T>() where T : Component
    {
        T component = GetComponent<T>();
        if (component != null)
        {
            Destroy(component);
        }
        else
        {
            Debug.Log("Component not found on the game object.");
        }
    }

    // Generic method to add a component of type T to this GameObject
    public T AddComponent<T>() where T : Component
    {
        T component = gameObject.AddComponent<T>();
        return component;
    }
#endregion
}