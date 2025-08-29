using Mirror;
using SLRemake.Network.Interests;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SLRemake.Network
{
    public class CustomNetworkManager : NetworkManager
    {

        public static event Action OnClientConnected;
        public override void OnClientConnect()
        {
            Debug.Log("OnClientConnect!");
            base.OnClientConnect();
            OnClientConnected?.Invoke();
        }

        public override void Awake()
        {
            base.Awake();
            StartCoroutine(WaitThenStartOffline());
        }

        public override void OnStartServer()
        {
            Debug.Log("OnStartServer!");
            base.OnStartServer();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Debug.Log("OnServerAddPlayer!");
            base.OnServerAddPlayer(conn);
            Debug.Log("Server Add Player!");
        }

        IEnumerator WaitThenStartOffline()
        {
            yield return new WaitForSeconds(2f);
            Debug.Log(NetworkServer.active);
            if (!NetworkServer.active)
                SceneManager.LoadScene("Offline", LoadSceneMode.Additive);
        }
    }

}