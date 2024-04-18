using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{

    #region SerializeField
    private NetworkRunner _runner;
    private NetworkRunner _runnerInstance;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private NetworkPrefabRef _playerPrefabHost;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    #endregion

    //async void StartGame(GameMode mode)
    //{
    //    // Create the Fusion runner and let it know that we will be providing user input
    //    _runner = gameObject.AddComponent<NetworkRunner>();
    //    _runner.ProvideInput = true;

    //    // Create the NetworkSceneInfo from the current scene
    //    var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
    //    var sceneInfo = new NetworkSceneInfo();
    //    if (scene.IsValid)
    //    {
    //        sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
    //    }

    //    // Start or join (depends on gamemode) a session with a specific name
    //    await _runner.StartGame(new StartGameArgs()
    //    {
    //        GameMode = mode,
    //        SessionName = "TestRoom",
    //        Scene = scene,
    //        SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
    //    });
    //}

    #region UnityMethods
    private void Start()
    {
#if UNITY_ANDROID
        StartGame(GameMode.Host, "Headset_user");
        HeadSetUIManager.Instance.SetVisibleofUI(true);
        PcUIManager.Instance.SetVisibleofUI(false);
        MobileUIManager.Instance.SetVisibleofUI(false);
#endif

#if UNITY_STANDALONE_WIN
        StartGame(GameMode.Client, "Pc_user");
        HeadSetUIManager.Instance.SetVisibleofUI(false);
        PcUIManager.Instance.SetVisibleofUI(true);
        MobileUIManager.Instance.SetVisibleofUI(false);
#endif
#if UNITY_IOS
        StartGame(GameMode.Client, "Mobile_user");
        HeadSetUIManager.Instance.SetVisibleofUI(false);
        PcUIManager.Instance.SetVisibleofUI(false);
        MobileUIManager.Instance.SetVisibleofUI(true);
#endif

    }


    #endregion



    #region NetworkCallbacks
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            //Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 2, 1, 0);
            Debug.Log("PlayerIndex:" + player.AsIndex);
            if (player.AsIndex == 1)
            {
                
                // host spawn host prefab
                Vector3 spawnPosition = new Vector3(0, 1, 0);
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefabHost, spawnPosition, Quaternion.identity, player);
                // Keep track of the player avatars for easy access
                _spawnedCharacters.Add(player, networkPlayerObject);
            }
            else
            {
                Vector3 spawnPosition = new Vector3(0.5f, 1, 0);
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
                // Keep track of the player avatars for easy access
                _spawnedCharacters.Add(player, networkPlayerObject);
            }



        }

    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }

    }
    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        var data = new NetworkInputData();
#if UNITY_STANDALONE_WIN
        if (Input.GetKey(KeyCode.W))
            data.direction += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            data.direction += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            data.direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.direction += Vector3.right;
#endif

#if UNITY_IOS
        VirtualJoystick joystick = FindObjectOfType<VirtualJoystick>(); 

        Vector3 moveDirection = joystick.GetInputDirection(); // Get the direction from the joystick
        data.direction += moveDirection;
#endif

#if UNITY_ANDROID
        if(MRSceneManager.Instance != null)
        {

            data.headsetPosition2D = MRSceneManager.Instance.PlayerRelativePos;
        }
#endif

        input.Set(data);
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

#endregion

    private async void StartGame(GameMode mode, string sceneName)
    {
        _runnerInstance = gameObject.AddComponent<NetworkRunner>();


        // Let the Fusion Runner know that we will be providing user input
        _runnerInstance.ProvideInput = true;

        var startGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "testRoom",
            //ObjectProvider = _runnerInstance.GetComponent<NetworkObjectPoolDefault>(),
        };

        // GameMode.Host = Start a session with a specific name
        // GameMode.Client = Join a session with a specific name
        await _runnerInstance.StartGame(startGameArgs);

        if (_runnerInstance.IsServer)
        {
            _runnerInstance.LoadScene(sceneName);
        }
    }

    //private void OnGUI()
    //{
    //    if (_runner == null)
    //    {
    //        if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
    //        {
    //            StartGame(GameMode.Host, "Pc_user");
    //        }
    //        if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
    //        {
    //            StartGame(GameMode.Client, "Mobile_user");
    //        }
    //    }
    //}
}
