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

        //GameObject explore = ResManager.LoadPrefab("fire");
        //Instantiate(explore, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
