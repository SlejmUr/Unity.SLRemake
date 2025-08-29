using Mirror;

namespace SLRemake.Network.Interests
{
    public abstract class BaseInterest : NetworkBehaviour
    {
        public BaseInterest[] Interests;
        public abstract bool OnCheckObserver(NetworkConnectionToClient newObserver);

        public abstract bool OnRebuildObserver(NetworkConnectionToClient newObserver);
    }
}