using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomInfo
{
    public int id = 0;
    public int count = 0;
    public int status = 0;
}

[System.Serializable]
public class PlayerInfo
{
    public string id = "";
    public int camp = 0;
    public int win = 0;
    public int lost = 0;
    public int isOwner = 0;
}

public class MsgGetAchieve : MsgBase
{
    public MsgGetAchieve()
    {
        protoName = nameof(MsgGetAchieve);
    }

    public int win = 0;

    public int lost = 0;
}

public class MsgGetRoomList : MsgBase
{
    public MsgGetRoomList()
    {
        protoName = nameof(MsgGetRoomList);
    }

    public RoomInfo[] rooms;
}

public class MsgCreateRoom : MsgBase
{
    public MsgCreateRoom()
    {
        protoName = nameof(MsgCreateRoom);
    }

    public int result = 0;
}

public class MsgEnterRoom : MsgBase
{
    public MsgEnterRoom()
    {
        protoName = nameof(MsgEnterRoom);
    }

    public int id = 0;

    public int result = 0;
}

public class MsgGetRoomInfo : MsgBase
{
    public MsgGetRoomInfo()
    {
        protoName = nameof(MsgGetRoomInfo);
    }

    public PlayerInfo[] players;
}

public class MsgLeaveRoom : MsgBase
{
    public MsgLeaveRoom()
    {
        protoName = nameof(MsgLeaveRoom);
    }

    public int result = 0;
}

public class MsgStartBattle : MsgBase
{
    public MsgStartBattle()
    {
        protoName = nameof(MsgStartBattle);
    }

    public int result = 0;
}