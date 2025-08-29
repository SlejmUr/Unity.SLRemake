using Mirror;
using UnityEngine;

namespace SLRemake.Network.Interests
{
    public abstract class BaseTickInterest : BaseInterest
    {
        [Tooltip("Rebuild all every 'rebuildInterval' seconds.")]
        public virtual float RebuildInterval { get; } = 1;
        double lastRebuildTime;

        [ServerCallback]
        void LateUpdate()
        {
            // rebuild all spawned NetworkIdentity's observers every interval
            if (NetworkTime.localTime >= lastRebuildTime + RebuildInterval)
            {
                (NetworkServer.aoi as MultipleInterestManager).ServerRebuildAll();
                lastRebuildTime = NetworkTime.localTime;
            }
        }
    }
}