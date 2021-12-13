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

    static float[,,] birthConfig = new float[2, 3, 6]
    {
        {
            { -85.8f, 3.8f, -33.8f, 0f, 24.9f, 0f },
            { -49.9f, 3.8f, -61.4f, 0f, 21.4f, 0f },
            { -6.2f,  3.8f, -70.7f, 0f, 21.9f, 0f },
        },
        {
            { 150f, 3.8f, -33.8f, 0f, -156.8f, 0f },
            { -49.9f, 3.8f, -61.4f, 0f, -156.8f, 0f },
            { -6.2f,  3.8f, -70.7f, 0f, -156.8f, 0f }
        }
    };

    private long lastJudgeTime = 0;

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

            if (IsOwner(player))
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

        if (IsOwner(player))
        {
            ownerId = SwitchOwner();
        }

        if (status == Status.FIGHT)
        {
            player.data.lost++;
            MsgLeaveBattle msg = new MsgLeaveBattle();
            msg.id = player.id;
            Broadcast(msg);
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

    public bool IsOwner(Player player)
    {
        return player.id == ownerId;
    }

    private bool CanStartBattle()
    {
        if (status != Status.PREPARE)
        {
            return false;
        }

        int count1 = 0;
        int count2 = 0;

        foreach(string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == 1)
            {
                count1++;
            }
            else
            {
                count2++;
            }
        }

        if (count1 < 1 || count2 < 1)
        {
            return false;
        }

        return true;
    }

    private void SetBirthPos(Player player, int index)
    {
        int camp = player.camp;

        player.x = birthConfig[camp - 1, index, 0];
        player.y = birthConfig[camp - 1, index, 1];
        player.z = birthConfig[camp - 1, index, 2];
        player.ex = birthConfig[camp - 1, index, 3];
        player.ey = birthConfig[camp - 1, index, 4];
        player.ez = birthConfig[camp - 1, index, 5];
    }

    private void ResetPlayers()
    {
        int count1 = 0;
        int count2 = 0;

        foreach(string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == 1)
            {
                SetBirthPos(player, count1++);
            }
            else
            {
                SetBirthPos(player, count2++);
            }

            player.hp = 100;
        }
    }

    public TankInfo PlayerToTankInfo(Player player)
    {
        TankInfo tankInfo = new TankInfo();
        tankInfo.camp = player.camp;
        tankInfo.id = player.id;
        tankInfo.hp = player.hp;

        tankInfo.x = player.x;
        tankInfo.y = player.y;
        tankInfo.z = player.z;
        tankInfo.ex = player.ex;
        tankInfo.ey = player.ey;
        tankInfo.ez = player.ez;

        return tankInfo;
    }

    public bool StartBattle()
    {
        if (!CanStartBattle())
        {
            return false;
        }

        status = Status.FIGHT;

        ResetPlayers();

        MsgEnterBattle msg = new MsgEnterBattle();
        msg.mapId = 1;
        msg.tanks = new TankInfo[playerIds.Count];

        int i = 0;

        foreach(string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            msg.tanks[i] = PlayerToTankInfo(player);
            i++;
        }

        Broadcast(msg);
        return true;
    }

    public bool IsDie(Player player)
    {
        return player.hp <= 0;
    }

    public int Judgement()
    {
        int count1 = 0;
        int count2 = 0;

        foreach(string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);

            if (!IsDie(player))
            {
                if (player.camp == 1)
                {
                    count1++;
                }

                if (player.camp == 2)
                {
                    count2++;
                }
            }
        }

        if (count1 <= 0)
        {
            return 2;
        }

        if (count2 <= 0)
        {
            return 1;
        }

        return 0;
    }

    public void Update()
    {
        if (status != Status.FIGHT)
        {
            return;
        }

        if (NetManager.GetTimeStamp() - lastJudgeTime < 10)
        {
            return;
        }

        lastJudgeTime = NetManager.GetTimeStamp();

        int winCamp = Judgement();

        if (winCamp == 0)
        {
            return;
        }

        status = Status.PREPARE;

        foreach(string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == winCamp)
            {
                player.data.win++;
            }
            else
            {
                player.data.lost++;
            }
        }

        MsgBattleResult msg = new MsgBattleResult();
        msg.winCamp = winCamp;
        Broadcast(msg);
    }
}
