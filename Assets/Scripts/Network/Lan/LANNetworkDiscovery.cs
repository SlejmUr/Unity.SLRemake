using Mirror.Discovery;
using SLRemake.Network;
using System;
using System.Net;
using UnityEngine;

namespace SLRemake.Network.Lan
{
    public class LANNetworkDiscovery : NetworkDiscoveryBase<ServerRequest, ServerResponse>
    {
        public CanvasHUD canvasHUD;

        protected override ServerResponse ProcessRequest(ServerRequest request, IPEndPoint endpoint)
        {
            try
            {
                // this is an example reply message,  return your own
                // to include whatever is relevant for your game
                return new ServerResponse
                {
                    serverId = ServerId,
                    uri = transport.ServerUri()
                };
            }
            catch (NotImplementedException)
            {
                Debug.LogError($"Transport {transport} does not support network discovery");
                throw;
            }
        }

        protected override ServerRequest GetRequest() => new();


        protected override void ProcessResponse(ServerResponse response, IPEndPoint endpoint)
        {
            // we received a message from the remote endpoint
            response.EndPoint = endpoint;

            // although we got a supposedly valid url, we may not be able to resolve
            // the provided host
            // However we know the real ip address of the server because we just
            // received a packet from it,  so use that as host.
            UriBuilder realUri = new(response.uri)
            {
                Host = response.EndPoint.Address.ToString()
            };
            response.uri = realUri.Uri;

            //OnServerFound.Invoke(response);
            if (canvasHUD == null)
            {
                canvasHUD = FindAnyObjectByType<CanvasHUD>();
            }
            canvasHUD.OnDiscoveredServer(response);
        }
    }

}