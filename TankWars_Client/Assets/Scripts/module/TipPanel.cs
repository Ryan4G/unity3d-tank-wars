using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    private Button confirmBtn;

    public override void OnInit()
    {
        skinPath = "TipPanel";
        layer = PanelManager.Layer.Tip;
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
