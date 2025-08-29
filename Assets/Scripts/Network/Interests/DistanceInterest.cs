using Mirror;
using UnityEngine;

namespace SLRemake.Network.Interests
{
    public class DistanceInterest : BaseTickInterest
    {
        [Tooltip("The maximum range that objects will be visible at.")]
        public int visRange = 50;

        private bool CheckDistance(NetworkConnectionToClient newObserver)
        {
            return Vector3.Distance(netIdentity.transform.position, newObserver.identity.transform.position) < visRange;
        }

        public override bool OnCheckObserver(NetworkConnectionToClient newObserver)
        {
            return CheckDistance(newObserver);
        }

        public override bool OnRebuildObserver(NetworkConnectionToClient newObserver)
        {
            return CheckDistance(newObserver);
        }
    }
}