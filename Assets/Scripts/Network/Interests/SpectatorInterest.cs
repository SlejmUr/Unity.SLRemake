using Mirror;
using SLRemake.Extensions;

namespace SLRemake.Network.Interests
{
    public class SpectatorInterest : BaseInterest
    {
        public override bool OnCheckObserver(NetworkConnectionToClient newObserver)
        {
            // If the observer is not player we dont care.
            if (!PlayerExtensions.TryGetPlayer(newObserver.identity, out Player observerPlayer))
                return true;

            // If the observer is not player we dont care.
            if (!PlayerExtensions.TryGetPlayer(netId, out Player player))
                return true;

            // If its a Spectator role then we good.
            return observerPlayer.RoleManager.CurrentRole.RoleType == RoleTypeId.Spectator;
        }

        public override bool OnRebuildObserver(NetworkConnectionToClient newObserver)
        {
            // If the observer is not player we dont care.
            if (!PlayerExtensions.TryGetPlayer(newObserver.identity, out Player player))
                return true;

            // If its a Spectator role then we good.
            return player.RoleManager.CurrentRole.RoleType == RoleTypeId.Spectator;
        }
    }
}