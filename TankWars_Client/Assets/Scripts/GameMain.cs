using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public static string id = "";

    // Start is called before the first frame update
    void Start()
    {
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);

        PanelManager.Init();
        PanelManager.Open<LoginPanel>();
    }

    private void OnConnectClose(string err)
    {
        Debug.Log("断开连接");
    }

    private void OnMsgKick(MsgBase msgBase)
    {
        PanelManager.Open<TipPanel>("您的账号已在别的地方登录");
    }

    // Update is called once per frame
    void Update()
    {
        NetManager.Update();
    }
}
