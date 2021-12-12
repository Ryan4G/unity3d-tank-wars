using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TankInfo
{
    public string id = "";
    public int camp = 0;
    public int hp = 0;

    // position
    public float x = 0;
    public float y = 0;
    public float z = 0;

    // rotation
    public float ex = 0;
    public float ey = 0;
    public float ez = 0;

}

public class MsgEnterBattle : MsgBase
{
    public MsgEnterBattle()
    {
        protoName = nameof(MsgEnterBattle);
    }

    public TankInfo[] tanks;
    public int mapId = 1;
}

public class MsgBattleResult : MsgBase
{
    public MsgBattleResult()
    {
        protoName = nameof(MsgBattleResult);
    }

    public int winCamp = 0;
}

public class MsgLeaveBattle : MsgBase
{
    public MsgLeaveBattle()
    {
        protoName = nameof(MsgLeaveBattle);
    }

    public string id = "";
}

