using Mirror;
using UnityEngine;

namespace SLRemake.Network.Behaviours
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        private Transform cache;

        public Player Player;

        public Transform CachedTransform
        {
            get
            {
                if (cache == null)
                    cache = transform;
                return cache;
            }
        }

        public void Awake()
        {
            if (Player == null)
                Player = GetComponent<Player>();
        }
    }
}