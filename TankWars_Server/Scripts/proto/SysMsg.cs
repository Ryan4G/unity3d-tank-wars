using System.Collections;
using System.Collections.Generic;


public class MsgPing : MsgBase
{
    public MsgPing()
    {
        protoName = nameof(MsgPing);
    }
}
public class MsgPong : MsgBase
{
    public MsgPong()
    {
        protoName = nameof(MsgPong);
    }
}