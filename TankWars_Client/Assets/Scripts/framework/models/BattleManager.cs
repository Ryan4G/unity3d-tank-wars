using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleManager
{
    public static Dictionary<string, BaseTank> tanks = new Dictionary<string, BaseTank>();

    public static void Init()
    {
        NetManager.AddMsgListener("MsgEnterBattle", OnMsgEnterBattle);
        NetManager.AddMsgListener("MsgBattleResult", OnMsgBattleResult);
        NetManager.AddMsgListener("MsgLeaveBattle", OnMsgLeaveBattle);
    }

    public static void AddTank(string id, BaseTank tank)
    {
        if (!tanks.ContainsKey(id))
        {
            tanks.Add(id, tank);
        }
        else
        {
            tanks[id] = tank;
        }
    }

    public static void RemoveTank(string id)
    {
        if (tanks.ContainsKey(id))
        {
            tanks.Remove(id);
        }
    }

    public static BaseTank GetTank(string id)
    {
        if (tanks.ContainsKey(id))
        {
            return tanks[id];
        }

        return null;
    }

    public static BaseTank GetCtrlTank()
    {
        return GetTank(GameMain.id);
    }

    public static void Reset()
    {
        foreach(BaseTank tank in tanks.Values)
        {
            MonoBehaviour.Destroy(tank.gameObject);
        }

        tanks.Clear();
    }

    private static void OnMsgLeaveBattle(MsgBase msgBase)
    {
        throw new NotImplementedException();
    }

    private static void OnMsgBattleResult(MsgBase msgBase)
    {
        throw new NotImplementedException();
    }

    private static void OnMsgEnterBattle(MsgBase msgBase)
    {
        MsgEnterBattle msg = msgBase as MsgEnterBattle;
        EnterBattle(msg);
    }

    private static void EnterBattle(MsgEnterBattle msg)
    {
        BattleManager.Reset();

        PanelManager.Close("RoomPanel");
        PanelManager.Close("ResultPanel");

        for(var i = 0; i < msg.tanks.Length; i++)
        {
            GenerateTank(msg.tanks[i]);
        }
    }
}
