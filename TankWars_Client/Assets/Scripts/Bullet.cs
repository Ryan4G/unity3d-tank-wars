using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;

    public BaseTank tank;

    private GameObject skin;

    Rigidbody rigidbody;

    public void Init()
    {
        GameObject skinRes = ResManager.LoadPrefab("bulletPrefab");
        skin = Instantiate(skinRes);
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;

        rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;

        BaseTank hitTank = go.GetComponent<BaseTank>();

        // hit player self
        if (hitTank == tank)
        {
            return;
        }
        
        if (hitTank)
        {
            SendMsgHit(tank, hitTank);

            //// near more damage
            //var distance = Vector3.Distance(transform.position, hitTank.transform.position);
            //var damage = 35f * Mathf.Clamp(50f / distance, 1f, 3f);

            //hitTank.Attacked(damage);
        }

        GameObject exploreEffect = ResManager.LoadPrefab(@"Effects/Fire & Explosion Effects/Prefabs/TinyExplosion");
        var exploreObj = Instantiate(exploreEffect, transform.position, transform.rotation);
        DelayToDestory dtd = exploreObj.AddComponent<DelayToDestory>();
        dtd.Init(2.0f);

        Destroy(gameObject);
    }

    private void SendMsgHit(BaseTank tank, BaseTank hitTank)
    {
        if (tank == null || hitTank == null)
        {
            return;
        }

        if (tank.id != GameMain.id)
        {
            return;
        }

        MsgHit msg = new MsgHit();
        msg.targetId = hitTank.id;
        msg.id = tank.id;
        msg.x = transform.position.x;
        msg.y = transform.position.y;
        msg.z = transform.position.z;

        NetManager.Send(msg);
    }
}
