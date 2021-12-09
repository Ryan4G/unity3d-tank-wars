using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    private InputField idInput;
    private InputField pwInput;
    private InputField repInput;
    private Button registerBtn;
    private Button closeBtn;

    public override void OnInit()
    {
        skinPath = "RegisterPanel";
        layer = PanelManager.Layer.Panel;
    }

    public override void OnShow(params object[] args)
    {
        idInput = skin.transform.Find("IdInput").GetComponent<InputField>();
        pwInput = skin.transform.Find("PwInput").GetComponent<InputField>();
        repInput = skin.transform.Find("RepInput").GetComponent<InputField>();
        closeBtn = skin.transform.Find("CloseBtn").GetComponent<Button>();
        registerBtn = skin.transform.Find("RegisterBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(OnCloseClick);
        registerBtn.onClick.AddListener(OnRegisterClick);

        NetManager.AddMsgListener("MsgRegister", OnMsgRegister);
    }

    private void OnMsgRegister(MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister)msgBase;

        if (msg.result == 1)
        {
            PanelManager.Open<TipPanel>("注册成功");

            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("注册失败");
        }
    }

    private void OnRegisterClick()
    {
        if (string.IsNullOrEmpty(idInput.text) || string.IsNullOrEmpty(pwInput.text))
        {
            PanelManager.Open<TipPanel>("用户名和密码不能为空");
            return;
        }

        if (string.Compare(pwInput.text, repInput.text) != 0)
        {
            PanelManager.Open<TipPanel>("两次输入的密码不同");
            return;
        }

        MsgRegister msg = new MsgRegister();
        msg.id = idInput.text.Trim();
        msg.pw = pwInput.text.Trim();

        NetManager.Send(msg);
    }

    private void OnCloseClick()
    {
        this.Close();
    }

    public override void OnClose()
    {
        NetManager.RemoveMsgListener("MsgRegister", OnMsgRegister);
    }
}
