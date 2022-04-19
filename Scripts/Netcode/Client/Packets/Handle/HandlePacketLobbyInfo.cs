using Common.Netcode;
using ENet;
using System.Collections.Generic;
using System.Linq;

namespace GodotModules.Netcode.Client
{
    public class HandlePacketLobbyInfo : HandlePacket
    {
        public override void Handle(PacketReader reader)
        {
            var data = new RPacketLobbyInfo(reader);

            Utils.Log($"{GameManager.Options.OnlineUsername} joined lobby with id {data.Id} also other players in lobby are {Utils.StringifyDict(data.Players)}", System.ConsoleColor.Green);

            SceneLobby.AddPlayer(data.Id, GameManager.Options.OnlineUsername);

            foreach (var player in data.Players)
                SceneLobby.AddPlayer(player.Key, player.Value);

            GameManager.ChangeScene("Lobby");
        }
    }
}