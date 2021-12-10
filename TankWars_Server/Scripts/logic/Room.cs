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

    private object ToMsg()
    {
        throw new NotImplementedException();
    }

    private void Broadcast(object p)
    {
        throw new NotImplementedException();
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
}
