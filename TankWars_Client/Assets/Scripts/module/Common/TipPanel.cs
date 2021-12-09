using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    private Button confirmBtn;
    private Text noticeText;

    public override void OnInit()
    {
        skinPath = "TipPanel";
        layer = PanelManager.Layer.Tip;
    }

    public override void OnShow(params object[] args)
    {
        confirmBtn = skin.transform.Find("OkBtn").GetComponent<Button>();
        noticeText = skin.transform.Find("NoticeText").GetComponent<Text>();

        confirmBtn.onClick.AddListener(OnOkClick);

        if (args.Length == 1)
        {
            noticeText.text = args[0] as string;
        }
    }

    private void OnOkClick()
    {
        this.Close();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
