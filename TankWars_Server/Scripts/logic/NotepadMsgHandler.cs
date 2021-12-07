using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class MsgHandler
{
    public static void MsgGetText(ClientState c, MsgBase msgBase)
    {
        MsgGetText msgGetText = (MsgGetText)msgBase;

        Player player = c.player;

        if (player == null)
        {
            return;
        }

        msgGetText.text = player.data.text;

        player.Send(msgGetText);
    }

    public static void MsgSaveText(ClientState c, MsgBase msgBase)
    {
        MsgSaveText msgGetText = (MsgSaveText)msgBase;
        msgGetText.result = 1;

        Player player = c.player;

        if (player == null)
        {
            return;
        }

        player.data.text = msgGetText.text;

        player.Send(msgGetText);
    }
}
