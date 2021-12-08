using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    private Button confirmBtn;

    public override void OnInit()
    {
        skinPath = "RegisterPanel";
        layer = PanelManager.Layer.Panel;
    }

    public override void OnShow(params object[] para)
    {
        confirmBtn = skin.transform.Find("ConfirmBtn").GetComponent<Button>();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
