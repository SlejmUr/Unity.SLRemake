using Mirror;
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
            base.OnClientConnect();
            OnClientConnected?.Invoke();
        }

        public override void Awake()
        {
            base.Awake();
            StartCoroutine(WaitThenStartOffline());
        }

        IEnumerator WaitThenStartOffline()
        {
            yield return new WaitForSeconds(1f);
            if (!NetworkServer.active)
                SceneManager.LoadScene("Offline", LoadSceneMode.Additive);
        }


        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            if (conn.identity.gameObject.TryGetComponent(out Player player) && player != null)
            {
                StartCoroutine(WaitThenSetRole(player));
            }    
        }

        IEnumerator WaitThenSetRole(Player player)
        {
            yield return new WaitForSeconds(2f);
            if (player != null)
            {
                var currentrole = player.RoleManager.CurrentRole;
                player.RoleManager.RoleType = (RoleTypeId.ClassD);
            }
                
        }
    }

}