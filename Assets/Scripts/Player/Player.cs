using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class Player : NetworkBehaviour
{

    #region SerializeField
    private NetworkCharacterController _cc;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
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
    }
#endregion



    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);
        }
    }
}