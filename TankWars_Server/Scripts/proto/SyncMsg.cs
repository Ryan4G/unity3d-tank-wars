using System.Collections;
using System.Collections.Generic;

public class MsgSyncTank : MsgBase
{
    public MsgSyncTank()
    {
        protoName = nameof(MsgSyncTank);
    }

    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float ex = 0f;
    public float ey = 0f;
    public float ez = 0f;
    public float turretY = 0f;
    // which tank
    public string id = "";
}

public class MsgFire : MsgBase
{
    public MsgFire()
    {
        protoName = nameof(MsgFire);
    }

    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float ex = 0f;
    public float ey = 0f;
    public float ez = 0f;
    // which tank
    public string id = "";
}

public class MsgHit : MsgBase
{
    public MsgHit()
    {
        protoName = nameof(MsgHit);
    }

    // hit which tank
    public string targetId = "";

    public float x = 0f;
    public float y = 0f;
    public float z = 0f;

    // which tank
    public string id = "";
    public int hp = 0;
    public int damage = 0;
}