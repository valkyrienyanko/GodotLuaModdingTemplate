using GodotModules.Netcode.Server;

namespace GodotModules.Netcode
{
    public class CPacketPing : APacketClient
    {
        public override void Handle(GameServer server, ENet.Peer peer)
        {
            server.Send(ServerPacketOpcode.Pong, peer);
        }
    }
}