using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTank : MonoBehaviour
{
    private GameObject skin;

    public float steer = 20;

    public float speed = 3f;

    public float turretSpeed = 30f;

    public Transform turret;

    public Transform gun;

    public Transform firePoint;

    public float fireCd = .5f;

    public float lastFireTime = 0;

    protected Rigidbody rigidbody;

    public float hp = 100;

    // Start is called before the first frame update
    public void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public virtual void Init(string skinPath)
    {
        GameObject skinRes = ResManager.LoadPrefab(skinPath);
        skin = Instantiate(skinRes);
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;

        rigidbody = gameObject.AddComponent<Rigidbody>();
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.center = new Vector3(0, 2.5f, 1.46f);
        boxCollider.size = new Vector3(7, 5, 12);

        turret = skin.transform.Find("Turret");
        gun = turret.transform.Find("Gun");
        firePoint = gun.transform.Find("FirePoint");
    }

    public Bullet Fire()
    {
        if (IsDie())
        {
            return null;
        }

        GameObject bulletObj = new GameObject("bullet");
        Bullet bullet = bulletObj.AddComponent<Bullet>();
        bullet.Init();
        bullet.tank = this;

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        lastFireTime = Time.time;
        return bullet;
    }

    protected bool IsDie()
    {
        return hp <= 0;
    }
}
