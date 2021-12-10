using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel
{
    private Text idText;
    private Text scoreText;
    private Button createBtn;
    private Button refreshBtn;
    private Transform content;

    private GameObject roomObj;

    public override void OnInit()
    {
        skinPath = "RoomListPanel";
        layer = PanelManager.Layer.Panel;
    }

    public override void OnShow(params object[] args)
    {
        idText = skin.transform.Find("InfoPanel/IdText").GetComponent<Text>();
        scoreText = skin.transform.Find("InfoPanel/ScoreText").GetComponent<Text>();
        createBtn = skin.transform.Find("CtrlPanel/CreateBtn").GetComponent<Button>();
        refreshBtn = skin.transform.Find("CtrlPanel/RefreshBtn").GetComponent<Button>();
        content = skin.transform.Find("ListPanel/Scroll View/Viewport/Content");
        roomObj = skin.transform.Find("Room").gameObject;

        createBtn.onClick.AddListener(OnCreateClick);
        refreshBtn.onClick.AddListener(OnRefreshClick);

        roomObj.SetActive(false);
        idText.text = GameMain.id;

        NetManager.AddMsgListener("MsgGetAchieve", OnMsgGetAchieve);
        NetManager.AddMsgListener("MsgGetRoomList", OnMsgGetRoomList);
        NetManager.AddMsgListener("MsgCreateRoom", OnMsgCreateRoom);
        NetManager.AddMsgListener("MsgEnterRoom", OnMsgEnterRoom);

        MsgGetAchieve msgGetAchieve = new MsgGetAchieve();
        NetManager.Send(msgGetAchieve);

        MsgGetRoomList msgGetRoomList = new MsgGetRoomList();
        NetManager.Send(msgGetRoomList);
    }

    private void OnJoinClick(string name)
    {
        MsgEnterRoom msg = new MsgEnterRoom();
        msg.id = int.Parse(name);

        NetManager.Send(msg);
    }

    private void OnRefreshClick()
    {
        MsgGetRoomList msg = new MsgGetRoomList();
        NetManager.Send(msg);
    }

    private void OnCreateClick()
    {
        MsgCreateRoom msg = new MsgCreateRoom();
        NetManager.Send(msg);
    }

    private void OnMsgGetRoomList(MsgBase msgBase)
    {
        MsgGetRoomList msg = msgBase as MsgGetRoomList;

        for (int i = content.childCount - 1; i >= 0; i--)
        {
            GameObject o = content.GetChild(i).gameObject;
            Destroy(o);
        }

        if (msg.rooms == null)
        {
            return;
        }

        for (int i = 0; i < msg.rooms.Length; i++)
        {
            GenerateRoom(msg.rooms[i]);
        }
    }

    private void GenerateRoom(RoomInfo roomInfo)
    {
        GameObject o = Instantiate(roomObj);
        o.transform.SetParent(content);
        o.SetActive(true);
        o.transform.localScale = Vector3.one;

        Transform trans = o.transform;
        Text idText = trans.Find("IdText").GetComponent<Text>();
        Text countText = trans.Find("CountText").GetComponent<Text>();
        Text stateText = trans.Find("StateText").GetComponent<Text>();
        Button btn = trans.Find("JoinButton").GetComponent<Button>();

        idText.text = roomInfo.id.ToString();
        countText.text = roomInfo.count.ToString();

        if (roomInfo.status == 0)
        {
            stateText.text = "准备中";
        }
        else
        {
            stateText.text = "战斗中";
        }

        btn.name = idText.text;

        btn.onClick.AddListener(() =>
        {
            OnJoinClick(btn.name);
        });
    }

    private void OnMsgGetAchieve(MsgBase msgBase)
    {
        MsgGetAchieve msg = (MsgGetAchieve)msgBase;

        scoreText.text = $"{msg.win} 胜 {msg.lost} 负";
    }

    private void OnMsgEnterRoom(MsgBase msgBase)
    {
        MsgEnterRoom msg = msgBase as MsgEnterRoom;

        if (msg.result == 1)
        {
            PanelManager.Open<RoomPanel>();
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("进入房间失败");
        }
    }

    private void OnMsgCreateRoom(MsgBase msgBase)
    {
        MsgCreateRoom msg = msgBase as MsgCreateRoom;

        if (msg.result == 1) {
            PanelManager.Open<TipPanel>("创建成功");
            PanelManager.Open<RoomPanel>();
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("创建房间失败");
        }
    }

    public override void OnClose()
    {
        NetManager.RemoveMsgListener("MsgGetAchieve", OnMsgGetAchieve);
        NetManager.RemoveMsgListener("MsgGetRoomList", OnMsgGetRoomList);
        NetManager.RemoveMsgListener("MsgCreateRoom", OnMsgCreateRoom);
        NetManager.RemoveMsgListener("MsgEnterRoom", OnMsgEnterRoom);
    }
}
