using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class MsgHandler
{
    public static void MsgEnterBattle(ClientState c, MsgBase msgBase)
    {
        MsgEnterBattle msg = msgBase as MsgEnterBattle;

    }

    public static void MsgBattleResult(ClientState c, MsgBase msgBase)
    {
        MsgBattleResult msg = msgBase as MsgBattleResult;

    }
    public static void MsgLeaveBattle(ClientState c, MsgBase msgBase)
    {
        MsgLeaveBattle msg = msgBase as MsgLeaveBattle;

    }
}
