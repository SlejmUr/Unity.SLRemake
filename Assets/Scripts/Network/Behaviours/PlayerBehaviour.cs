using Mirror;

namespace SLRemake.Network.Behaviours
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        public Player Player;

        public void Awake()
        {
            if (Player == null)
                Player = GetComponent<Player>();
        }
    }
}