using Mirror;
using SLRemake.Extensions;

namespace SLRemake.Network.Behaviours
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        public Player Player { get; private set; }
        public void Start()
        {
            Player = PlayerExtensions.GetPlayer(netId);
            if (!NetworkServer.active)
                return;
            OnStart();
        }

        public virtual void OnStart()
        {

        }
    }

}