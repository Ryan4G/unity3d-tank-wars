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
        MsgLeaveBattle msg = msgBase as MsgLeaveBattle;
        BaseTank tank = GetTank(msg.id);

        if (tank == null)
        {
            return;
        }

        RemoveTank(msg.id);

        MonoBehaviour.Destroy(tank.gameObject);
    }

    private static void OnMsgBattleResult(MsgBase msgBase)
    {
        MsgBattleResult msg = msgBase as MsgBattleResult;
        bool isWin = false;

        BaseTank tank = GetCtrlTank();

        if (tank != null && tank.camp == msg.winCamp)
        {
            isWin = true;
        }

        PanelManager.Open<ResultPanel>(isWin);
    }

    private static void OnMsgEnterBattle(MsgBase msgBase)
    {
        Debug.Log("OnMsgEnterBattle");
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

    private static void GenerateTank(TankInfo tankInfo)
    {
        string objName = $"Tank_{tankInfo.id}";

        GameObject tankObj = new GameObject(objName);
        BaseTank tank = null;

        if (tankInfo.id == GameMain.id)
        {
            tank = tankObj.AddComponent<CtrlTank>();

            // camera follow player control tank
            tankObj.AddComponent<CameraFollow>();
        }
        else
        {
            tank = tankObj.AddComponent<SyncTank>();
        }

        tank.camp = tankInfo.camp;
        tank.id = tankInfo.id;
        tank.hp = tankInfo.hp;

        Vector3 pos = new Vector3(tankInfo.x, tankInfo.y, tankInfo.z);
        Vector3 rot = new Vector3(tankInfo.ex, tankInfo.ey, tankInfo.ez);

        tank.transform.position = pos;
        tank.transform.eulerAngles = rot;

        if (tankInfo.camp == 1)
        {
            tank.Init("tankPrefab");
        }
        else
        {
            tank.Init("tankPrefab2");
        }

        AddTank(tankInfo.id, tank);
    }
}
