using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    private InputField ipInput;
    private InputField portInput;
    private Button connectBtn;
    private Button closeBtn;

    public override void OnInit()
    {
        skinPath = "SettingPanel";
        layer = PanelManager.Layer.Panel;
    }

    public override void OnShow(params object[] args)
    {
        ipInput = skin.transform.Find("IPInput").GetComponent<InputField>();
        portInput = skin.transform.Find("PortInput").GetComponent<InputField>();
        connectBtn = skin.transform.Find("ConnectBtn").GetComponent<Button>();
        closeBtn = skin.transform.Find("CloseBtn").GetComponent<Button>();

        connectBtn.onClick.AddListener(OnConnectClick);
        closeBtn.onClick.AddListener(OnCloseClick);

        ipInput.text = "127.0.0.1";
        portInput.text = "8888";

        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
    }

    private void OnCloseClick()
    {
        this.Close();
    }

    private void OnConnectClick()
    {
        if (string.IsNullOrEmpty(ipInput.text) || string.IsNullOrEmpty(portInput.text))
        {
            PanelManager.Open<TipPanel>("服务器地址和端口不能为空");
            return;
        }

        var ip = ipInput.text.Trim();
        var port = portInput.text.Trim();

        if (!NetManager.SocketConnected)
        {
            NetManager.Connect(ip, int.Parse(port));
        }
        else
        {
            this.Close();
        }
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    private void OnConnectFail(string err)
    {
        PanelManager.Open<TipPanel>(err);
    }

    private void OnConnectSucc(string err)
    {
        Debug.Log("连接成功...");
    }
}
