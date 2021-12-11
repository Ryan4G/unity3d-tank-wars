using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Room
{
    public int id = 0;

    public int maxPlayer = 6;

    public Dictionary<string, bool> playerIds = new Dictionary<string, bool>();

    public string ownerId = "";

    public enum Status
    {
        PREPARE = 0,
        FIGHT = 1
    }

    public Status status = Status.PREPARE;

    public bool AddPlayer(string id)
    {
        Player player = PlayerManager.GetPlayer(id);

        if (player == null)
        {
            Console.WriteLine("[ Server ] Room Addplayer fail , player is null");
            return false;
        }

        if (playerIds.Count >= maxPlayer)
        {
            Console.WriteLine("[ Server ] Room Addplayer fail , reach the max of player");
            return false;
        }

        if (status != Status.PREPARE)
        {
            Console.WriteLine("[ Server ] Room Addplayer fail , the room is not prepare");
            return false;
        }

        if (playerIds.ContainsKey(id))
        {
            Console.WriteLine("[ Server ] Room Addplayer fail , already in the room");
            return false;
        }

        playerIds.Add(id, true);

        player.camp = SwitchCamp();
        player.roomId = this.id;

        if (ownerId == "")
        {
            ownerId = player.id;
        }

        Broadcast(ToMsg());

        return true;
    }

    public MsgBase ToMsg()
    {
        MsgGetRoomInfo msg = new MsgGetRoomInfo();
        int count = playerIds.Count;

        msg.players = new PlayerInfo[count];

        int i = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.id = player.id;
            playerInfo.camp = player.camp;
            playerInfo.win = player.data.win;
            playerInfo.lost = player.data.lost;
            playerInfo.isOwner = 0;

            if (isOwner(player))
            {
                playerInfo.isOwner = 1;
            }

            msg.players[i] = playerInfo;
            i++;
        }

        return msg;
    }

    private void Broadcast(MsgBase msg)
    {
        foreach(string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.Send(msg);
        }
    }

    private int SwitchCamp()
    {
        int count1 = 0;
        int count2 = 0;

        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == 1)
            {
                count1++;
            }

            if (player.camp == 2)
            {
                count2++;
            }
        }

        return count1 <= count2 ? 1 : 2;
    }

    public bool RemovePlayer(string id)
    {
        Player player = PlayerManager.GetPlayer(id);

        if (player == null)
        {
            Console.WriteLine("[ Server ] Room Removeplayer fail , player is null");
            return false;
        }

        if (!playerIds.ContainsKey(id))
        {
            Console.WriteLine("[ Server ] Room Removeplayer fail , player is not in this room");
            return false;
        }

        playerIds.Remove(id);

        player.camp = 0;
        player.roomId = -1;

        if (isOwner(player))
        {
            ownerId = SwitchOwner();
        }

        if (playerIds.Count == 0)
        {
            RoomManager.RemoveRoom(this.id);
        }

        Broadcast(ToMsg());
        return true;
    }

    private string SwitchOwner()
    {
        foreach(string id in playerIds.Keys)
        {
            return id;
        }

        return "";
    }

    private bool isOwner(Player player)
    {
        return player.id == ownerId;
    }
}
