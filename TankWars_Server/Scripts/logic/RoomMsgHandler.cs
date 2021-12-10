using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class MsgHandler
{
    public static void MsgGetAchieve(ClientState c, MsgBase msgBase)
    {
        MsgGetAchieve msg = msgBase as MsgGetAchieve;
    }

    public static void MsgGetRoomList(ClientState c, MsgBase msgBase)
    {
        MsgGetRoomList msg = msgBase as MsgGetRoomList;
    }

    public static void MsgCreateRoom(ClientState c, MsgBase msgBase)
    {
        MsgCreateRoom msg = msgBase as MsgCreateRoom;
    }

    public static void MsgEnterRoom(ClientState c, MsgBase msgBase)
    {
        MsgEnterRoom msg = msgBase as MsgEnterRoom;
    }

    public static void MsgGetRoomInfo(ClientState c, MsgBase msgBase)
    {
        MsgGetRoomInfo msg = msgBase as MsgGetRoomInfo;
    }

    public static void MsgLeaveRoom(ClientState c, MsgBase msgBase)
    {
        MsgLeaveRoom msg = msgBase as MsgLeaveRoom;
    }

    public static void MsgStartBattle(ClientState c, MsgBase msgBase)
    {
        MsgStartBattle msg = msgBase as MsgStartBattle;
    }
}
