using System;

namespace TankWars_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!DBManager.Connect("tankwar", "127.0.0.1", 3306, "root", "123456"))
            {
                return;
            }

            NetManager.StartLoop(8888);
        }
    }
}
