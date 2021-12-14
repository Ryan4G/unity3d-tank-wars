﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTank : BaseTank
{
    private Vector3 lastPos;
    private Vector3 lastRot;
    private Vector3 forecastPos;
    private Vector3 forecastRot;
    private float forecastTime;

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        ForecastUpdate();
    }

    public override void Init(string skinPath)
    {
        base.Init(skinPath);

        _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        _rigidBody.useGravity = false;

        lastPos = transform.position;
        lastRot = transform.eulerAngles;
        forecastPos = transform.position;
        forecastRot = transform.eulerAngles;
        forecastTime = Time.time;
    }
    private void ForecastUpdate()
    {
        float t = (Time.time - forecastTime) / CtrlTank.syncInterval;
        t = Mathf.Clamp(t, 0f, 1f);

        Vector3 pos = transform.position;
        pos = Vector3.Lerp(pos, forecastPos, t);
        transform.position = pos;

        Quaternion quat = transform.rotation;
        Quaternion forecastQuat = Quaternion.Euler(forecastRot);
        quat = Quaternion.Lerp(quat, forecastQuat, t);
        transform.rotation = quat;
    }

    public void SyncFire(MsgFire msg)
    {
        Bullet bullet = Fire();

        Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
        Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);

        bullet.transform.position = pos;
        bullet.transform.eulerAngles = rot;
    }

    public void SyncPos(MsgSyncTank msg)
    {
        Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
        Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);

        forecastPos = pos + 2 * (lastPos - pos);
        forecastRot = pos + 2 * (lastRot - rot);

        lastPos = pos;
        lastRot = rot;
        forecastTime = Time.time;

        Vector3 le = turret.localEulerAngles;
        le.y = msg.turretY;
        turret.localEulerAngles = le;

    }
}
