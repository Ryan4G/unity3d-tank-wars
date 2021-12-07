using System.Collections;
using System.Collections.Generic;


public class MsgAttack : MsgBase
{
    public MsgAttack()
    {
        protoName = nameof(MsgAttack);
    }

    public string desc = "127.0.0.1:6543";
}

public class MsgMove : MsgBase
{
    public MsgMove()
    {
        protoName = nameof(MsgMove);
    }

    public int x = 0;
    public int y = 0;
    public int z = 0;
}
