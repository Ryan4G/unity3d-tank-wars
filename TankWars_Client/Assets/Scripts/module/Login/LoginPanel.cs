using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    private InputField idInput;
    private InputField pwInput;
    private Button loginBtn;
    private Button registerBtn;

    public override void OnInit()
    {
        skinPath = "LoginPanel";
        layer = PanelManager.Layer.Panel;
    }

    public override void OnShow(params object[] args)
    {
        idInput = skin.transform.Find("IdInput").GetComponent<InputField>();
        pwInput = skin.transform.Find("PwInput").GetComponent<InputField>();
        loginBtn = skin.transform.Find("LoginBtn").GetComponent<Button>();
        registerBtn = skin.transform.Find("RegisterBtn").GetComponent<Button>();

        loginBtn.onClick.AddListener(OnLoginClick);
        registerBtn.onClick.AddListener(OnRegisterClick);

        NetManager.AddMsgListener("MsgLogin", OnMsgLogin);

        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);

        NetManager.Connect("127.0.0.1", 8888);
    }

    private void OnRegisterClick()
    {
        PanelManager.Open<RegisterPanel>();
    }

    private void OnLoginClick()
    {
        if (string.IsNullOrEmpty(idInput.text) || string.IsNullOrEmpty(pwInput.text))
        {
            PanelManager.Open<TipPanel>("用户名和密码不能为空");
            return;
        }

        MsgLogin msgLogin = new MsgLogin();
        msgLogin.id = idInput.text.Trim();
        msgLogin.pw = pwInput.text.Trim();
        NetManager.Send(msgLogin);
    }

    public override void OnClose()
    {
        NetManager.RemoveMsgListener("MsgLogin", OnMsgLogin);
    }

    private void OnConnectFail(string err)
    {
        PanelManager.Open<TipPanel>(err);
    }

    private void OnConnectSucc(string err)
    {
        Debug.Log("连接服务器成功...");
    }

    private void OnMsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;

        if (msg.result == 1)
        {
            Debug.Log("登录成功");

            GameObject tankObj = new GameObject("myTank");
            BaseTank baseTank = tankObj.AddComponent<CtrlTank>();
            baseTank.Init("tankPrefab");

            tankObj.AddComponent<CameraFollow>();

            GameMain.id = msg.id;

            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("登录失败");
        }
    }

}
