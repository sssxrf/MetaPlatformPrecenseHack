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
    private GameObject Body;
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
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Body = gameObject.transform.GetChild(0).gameObject;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Body.SetActive(false);
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

    // Message Only
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

    // Room Info
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendRoomInfo(float roomlength, float roomwidth, RpcInfo info = default)
    {
        RPC_RelayRoomInfo(roomlength, roomwidth, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayRoomInfo(float roomlength, float roomwidth, PlayerRef messageSource)
    {

#if UNITY_STANDALONE_WIN
        RoomManager.Instance.SpawnedCenteredRoom(roomlength, roomwidth);
        RoomManager.Instance.StoreRoomInfo(roomlength, roomwidth);
        PcUIManager.Instance.UpdateMessages( "Room set!");

#endif

#if UNITY_ANDROID
        
        HeadSetUIManager.Instance.UpdateMessages("RoomInfo sent");
#endif

#if UNITY_IOS
        RoomManager.Instance.SpawnedCenteredRoom(roomlength, roomwidth);
        RoomManager.Instance.StoreRoomInfo(roomlength, roomwidth);
        MobileUIManager.Instance.UpdateMessages( "Room set!");
#endif
    }


    // Food Info
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendFoodInfo(string foodtype, RpcInfo info = default)
    {
        RPC_RelayFoodInfo(foodtype, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayFoodInfo(string foodtype, PlayerRef messageSource)
    {

#if UNITY_STANDALONE_WIN
        
            PcUIManager.Instance.UpdateMessages(foodtype);


#endif

#if UNITY_ANDROID
            Vector3 spawnPosCenter = new Vector3(MRSceneManager.Instance.RoomCenter.x + 1, MRSceneManager.Instance.calibrationHeight, MRSceneManager.Instance.RoomCenter.z); 
            FoodManager.Instance.SpawnfoodByName(foodtype, spawnPosCenter);
            HeadSetUIManager.Instance.UpdateMessages("food info Sent!");
#endif

#if UNITY_IOS
            Debug.Log("RPC Food info called");
            MobileUIManager.Instance.UpdateMessages("food info Sent!");
#endif
    }

    // New Guests Info
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendNewGuestInfo(int guestID, int guestType, int urgentState, Vector2 PosRelativeToWindow, RpcInfo info = default)
    {
        RPC_RelayNewGuestInfo(guestID, guestType, urgentState, PosRelativeToWindow, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayNewGuestInfo(int guestID, int guestType, int urgentState, Vector2 PosRelativeToWindow, PlayerRef messageSource)
    {

#if UNITY_STANDALONE_WIN
        Debug.Log("RPC New guest is called");
          ClientGuestManager.Instance.StoreNewGuestInfos(guestID, guestType, urgentState, PosRelativeToWindow);

#endif

#if UNITY_ANDROID

#endif

#if UNITY_IOS
           ClientGuestManager.Instance.StoreNewGuestInfos(guestID, guestType, urgentState, PosRelativeToWindow);
#endif
    }

    // Guests Info Update
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendUpdateGuestInfo(int guestID, int urgentState, RpcInfo info = default)
    {
        RPC_RelayUpdateGuestInfo(guestID, urgentState, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayUpdateGuestInfo(int guestID, int urgentState, PlayerRef messageSource)
    {

#if UNITY_STANDALONE_WIN
        
        ClientGuestManager.Instance.UpdateGuestInfos(guestID, urgentState);

#endif

#if UNITY_ANDROID

#endif

#if UNITY_IOS
        ClientGuestManager.Instance.UpdateGuestInfos(guestID, urgentState);
#endif

    }

    // Guests Info Update
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendClearAGuestInfO(int guestID, bool isSatisfied, RpcInfo info = default)
    {
        RPC_RelayClearAGuestInfo(guestID, isSatisfied, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayClearAGuestInfo(int guestID, bool isSatisfied, PlayerRef messageSource)
    {

#if UNITY_STANDALONE_WIN
        ClientGuestManager.Instance.ClearAGuestInfos(guestID, isSatisfied);
        

#endif

#if UNITY_ANDROID

#endif

#if UNITY_IOS
        ClientGuestManager.Instance.ClearAGuestInfos(guestID, isSatisfied);
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