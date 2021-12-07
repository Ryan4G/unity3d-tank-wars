using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Player
{
    public string id = "";

    public ClientState state;

    public int x;
    public int y;
    public int z;

    public PlayerData data;

    public Player(ClientState state)
    {
        this.state = state;
    }

    public void Send(MsgBase msgBase)
    {
        NetManager.Send(this.state, msgBase);
    }
}
