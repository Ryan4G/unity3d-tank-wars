using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public partial class EventHandler
{
    public static void OnDisconnect(ClientState c)
    {
        Console.WriteLine($"[ Server ] Socket Close {c.socket.RemoteEndPoint}");

        if (c.player != null)
        {
            DBManager.UpdatePlayerData(c.player.id, c.player.data);
            PlayerManager.RemovePlayer(c.player.id);
        }
    }

    public static void OnTimer()
    {
        CheckPing();
    }

    public static void CheckPing()
    {
        long timeNow = NetManager.GetTimeStamp();

        foreach (ClientState s in NetManager.clients.Values)
        {
            if (timeNow - s.lastPingTime > NetManager.pingInterval * 4)
            {
                Console.WriteLine($"[ Server ] Ping Close {s.socket.RemoteEndPoint} -> {timeNow}");

                NetManager.Close(s);

                // close a socket each time
                return;
            }
        }
    }
}
