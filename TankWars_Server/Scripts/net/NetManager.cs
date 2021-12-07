using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
public class NetManager
{
    public static Socket serverSocket;

    public static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();

    static List<Socket> checkRead = new List<Socket>();

    public static long pingInterval = 30;

    public static void StartLoop(int listenPort)
    {
        Console.WriteLine("[ Server ] Started! ");

        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        serverSocket.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"), listenPort));

        serverSocket.Listen(0);

        Console.WriteLine("[ Server ] Launch Successful");

        while (true)
        {
            checkRead.Clear();
            checkRead.Add(serverSocket);

            foreach (ClientState client in clients.Values)
            {
                checkRead.Add(client.socket);
            }

            // Select Enable Read Socket
            Socket.Select(checkRead, null, null, 1000);

            for (var i = checkRead.Count - 1; i >= 0; i--)
            {
                Socket s = checkRead[i];

                if (s == serverSocket)
                {
                    ReadListenServer(s);
                }
                else
                {
                    ReadListenClient(s);
                }
            }

            Timer();
        }
    }

    private static void ReadListenServer(Socket serverSocket)
    {
        try
        {
            Socket socket = serverSocket.Accept();

            Console.WriteLine($"[ Server ] Accept <- {socket.RemoteEndPoint} <- {GetTimeStamp()}");

            ClientState clientState = new ClientState();
            clientState.socket = socket;
            clientState.lastPingTime = GetTimeStamp();
            clients.Add(socket, clientState);
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"[ Server ] Accept Fail {ex}");
        }
    }


    private static void ReadListenClient(Socket s)
    {
        ClientState clientState = clients[s];
        ByteArray readBuff = clientState.readBuff;

        int count = 0;

        if (readBuff.remain <= 0)
        {
            OnReceiveData(clientState);

            readBuff.MoveBytes();
        }

        if (readBuff.remain <= 0)
        {
            Console.WriteLine("[ Server ] Receive Fail, maybe msg length > buff capacity");
            Close(clientState);
            return;
        }

        try
        {
            count = s.Receive(readBuff.bytes, readBuff.writeIdx, readBuff.remain, SocketFlags.None);
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"[ Server ] Receive SocketException :{ex}");
            Close(clientState);
            return;
        }

        if (count <= 0)
        {
            Console.WriteLine($"[ Server ] Socket Close <- {clientState.socket.RemoteEndPoint}");
            Close(clientState);
            return;
        }

        readBuff.writeIdx += count;

        OnReceiveData(clientState);

        readBuff.CheckAndMoveBytes();
    }

    public static void Close(ClientState clientState)
    {
        MethodInfo mei = typeof(EventHandler).GetMethod("OnDisconnect");
        object[] obs = { clientState };
        mei.Invoke(null, obs);

        clientState.socket.Close();
        clients.Remove(clientState.socket);
    }

    private static void OnReceiveData(ClientState clientState)
    {
        ByteArray readBuff = clientState.readBuff;

        // only length bytes
        if (readBuff.length <= 2)
        {
            return;
        }

        short bodyLength = readBuff.ReadInt16();

        // package is not completed
        if (readBuff.length < bodyLength)
        {
            readBuff.readIdx -= 2;
            return;
        }

        int nameCount = 0;
        string protoName = MsgBase.DecodeName(readBuff.bytes, readBuff.readIdx, out nameCount);

        if (string.IsNullOrEmpty(protoName))
        {
            Console.WriteLine($"[ Server ] OnReceiveData MsgBase.DecodeName fail");
            Close(clientState);
            return;
        }

        readBuff.readIdx += nameCount;

        int bodyCount = bodyLength - nameCount;
        MsgBase msgBase = MsgBase.Decode(protoName, readBuff.bytes, readBuff.readIdx, bodyCount);

        readBuff.readIdx += bodyCount;
        readBuff.CheckAndMoveBytes();

        MethodInfo mei = typeof(MsgHandler).GetMethod(protoName);
        object[] obs = { clientState, msgBase };

        Console.WriteLine($"[ Server ] Receive {protoName}");

        if (mei != null)
        {
            mei.Invoke(null, obs);
        }
        else
        {
            Console.WriteLine($"[ Server ] OnReceiveData Invoke fail {protoName}");
        }

        if (readBuff.length > 2)
        {
            // work utill return
            OnReceiveData(clientState);
        }
    }

    static void Timer()
    {
        MethodInfo mei = typeof(EventHandler).GetMethod("OnTimer");
        object[] obs = { };
        mei.Invoke(null, obs);
    }

    public static void Send(ClientState clientState, MsgBase msg)
    {
        Socket socket = clientState.socket;

        if (socket == null || !socket.Connected)
        {
            return;
        }

        byte[] nameBytes = MsgBase.EncodeName(msg);
        byte[] bodyBytes = MsgBase.Encode(msg);

        int len = nameBytes.Length + bodyBytes.Length;
        byte[] sendBytes = new byte[2 + len];

        sendBytes[0] = (byte)(len % 256);
        sendBytes[1] = (byte)(len / 256);

        Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
        Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);

        ByteArray ba = new ByteArray(sendBytes);

        try
        {
            socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, null, socket);
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"[ Server ] Socket Close on BeginSend {ex}");
        }
    }

    public static long GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }
}
