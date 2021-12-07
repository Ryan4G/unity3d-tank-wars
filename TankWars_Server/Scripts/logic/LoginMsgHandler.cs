using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class MsgHandler
{
    public static void MsgRegister(ClientState c, MsgBase msgBase)
    {
        MsgRegister msgRegister = (MsgRegister)msgBase;

        msgRegister.result = DBManager.Register(msgRegister.id, msgRegister.pw) ? 1 : 0;

        // register succ then create player
        if (msgRegister.result == 1)
        {
            DBManager.CreatePlayer(msgRegister.id);
        }

        NetManager.Send(c, msgRegister);
    }

    public static void MsgLogin(ClientState c, MsgBase msgBase)
    {
        MsgLogin msgLogin = (MsgLogin)msgBase;

        var loginResult = DBManager.CheckPassword(msgLogin.id, msgLogin.pw);

        if (!loginResult)
        {
            // login fail
            msgLogin.result = 0;

            NetManager.Send(c, msgLogin);

            return;
        }

        if (c.player != null)
        {
            // same socket already login
            msgLogin.result = 0;

            NetManager.Send(c, msgLogin);

            return;
        }

        if (PlayerManager.IsOnline(msgLogin.id))
        {
            Player other = PlayerManager.GetPlayer(msgLogin.id);
            
            // another socket already login, kick it out
            MsgKick msgKick = new MsgKick();
            msgKick.reason = 0;
            other.Send(msgKick);

            NetManager.Close(other.state);
        }

        PlayerData playerData = DBManager.GetPlayerData(msgLogin.id);

        if (playerData == null)
        {
            msgLogin.result = 0;
            NetManager.Send(c, msgLogin);
            return;
        }

        Player player = new Player(c);
        player.id = msgLogin.id;
        player.data = playerData;
        PlayerManager.AddPlayer(player.id, player);

        c.player = player;

        msgLogin.result = 1;
        player.Send(msgLogin);

    }
}
