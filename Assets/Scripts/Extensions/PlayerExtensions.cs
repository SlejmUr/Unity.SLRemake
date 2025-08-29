using Mirror;
using SLRemake.Network;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

namespace SLRemake.Extensions
{
    public static class PlayerExtensions
    {
        internal static readonly Dictionary<uint, Player> PlayerByIds = new();

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

        public static Player GetPlayer(uint netId)
        {
            if (!TryGetPlayer(netId, out var hub))
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
            return gameObject.TryGetComponent(out player);
        }

        public static bool TryGetPlayer(NetworkConnection connection, out Player player)
        {
            NetworkIdentity identity = connection.identity;
            if (!connection.isReady || identity == null)
            {
                player = null;
                return false;
            }
            if (!PlayerByIds.TryGetValue(identity.netId, out player))
            {
                return identity.TryGetComponent(out player);
            }
            return true;
        }

        public static bool TryGetPlayer(NetworkIdentity identity, out Player player)
        {
            if (identity == null)
            {
                player = null;
                return false;
            }
            if (!PlayerByIds.TryGetValue(identity.netId, out player))
            {
                return identity.TryGetComponent(out player);
            }
            return true;
        }

        public static bool TryGetPlayer(uint netId, out Player player)
        {
            return PlayerByIds.TryGetValue(netId, out player);
        }
    }

}