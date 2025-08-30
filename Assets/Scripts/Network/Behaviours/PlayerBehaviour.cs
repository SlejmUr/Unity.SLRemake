using Mirror;

namespace SLRemake.Network.Behaviours
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        public Player Player;

        private void Start()
        {
            if (Player = null)
                Player = GetComponent<Player>();
        }
    }

}