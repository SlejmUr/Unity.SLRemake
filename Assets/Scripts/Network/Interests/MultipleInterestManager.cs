using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace SLRemake.Network.Interests
{
    public class MultipleInterestManager : InterestManagement
    {
        public override bool OnCheckObserver(NetworkIdentity identity, NetworkConnectionToClient newObserver)
        {
            BaseInterest[] interests = identity.GetComponentsInChildren<BaseInterest>();
            foreach (BaseInterest interest in interests)
            {
                interest.Interests = interests;
                if (!interest.OnCheckObserver(newObserver))
                    return false;
            }
            return true;
        }

        public override void OnRebuildObservers(NetworkIdentity identity, HashSet<NetworkConnectionToClient> newObservers)
        {
            foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
            {
                if (conn == null || !conn.isAuthenticated || conn.identity == null)
                    continue;
                bool addObserver = true;
                BaseInterest[] interests = identity.GetComponentsInChildren<BaseInterest>();
                foreach (BaseInterest interest in interests)
                {
                    interest.Interests = interests;
                    if (!interest.OnRebuildObserver(conn))
                    {
                        addObserver = false;
                        break;
                    }
                }
                if (addObserver)
                    newObservers.Add(conn);
            }
        }


        [Server]
        public void ServerRebuildAll()
        {
            RebuildAll();
        }
    }
}