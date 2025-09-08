using Mirror;
using SLRemake.Network;
using System.Collections.Generic;
using UnityEngine;

namespace SLRemake.Extensions
{
    public static class PlayerExtensions
    {
        internal static readonly Dictionary<GameObject, Player> PlayerByGameObject = new();

        public static Player GetPlayer(GameObject gameObject)
        {
            if (!TryGetPlayer(gameObject, out var hub))
            {
                return null;
            }
            return hub;
        }

        public static Player GetPlayer(MonoBehaviour behaviour)
        {
            if (!TryGetPlayer(behaviour.gameObject, out var hub))
            {
                return null;
            }
            return hub;
        }

        public static bool TryGetPlayer(GameObject gameObject, out Player player)
        {
            if (gameObject == null)
            {
                player = null;
                return false;
            }
            return PlayerByGameObject.TryGetValue(gameObject, out player) || gameObject.TryGetComponent(out player);
        }

        public static bool TryGetPlayer(NetworkConnection connection, out Player player)
        {
            NetworkIdentity identity = connection.identity;
            if (!connection.isReady || identity == null)
            {
                player = null;
                return false;
            }
            return TryGetPlayer(connection.identity.gameObject, out player);
        }
    }

}