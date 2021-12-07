using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlTank : BaseTank
{
    // Update is called once per frame
    new void Update()
    {
        base.Update();

        MoveUpdate();

        TurretUpdate();

        FireUpdate();
    }

    private void TurretUpdate()
    {
        if (IsDie())
        {
            return;
        }

        float axis = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            axis = -1;
        }
        else if(Input.GetKey(KeyCode.E))
        {
            axis = 1;
        }

        Vector3 angle = turret.localEulerAngles;
        angle.y += axis * Time.deltaTime * turretSpeed;
        turret.localEulerAngles = angle;

    }

    private void MoveUpdate()
    {
        if (IsDie())
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        transform.Rotate(0, x * steer * Time.deltaTime, 0);

        float y = Input.GetAxis("Vertical");
        Vector3 s = y * transform.forward * speed * Time.deltaTime;
        transform.position += s;
    }

    public void FireUpdate()
    {
        if (IsDie())
        {
            return;
        }

        if (!Input.GetKey(KeyCode.Space))
        {
            return;
        }

        if (Time.time - lastFireTime < fireCd)
        {
            return;
        }

        Fire();
    }
}
