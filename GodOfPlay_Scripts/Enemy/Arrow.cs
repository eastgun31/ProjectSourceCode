using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int value;
    Rigidbody rigid;
    public Vector3 shotdir;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        Destroy(gameObject, 2f);
    }

    private void Update()
    {
        rigid.AddForce(shotdir * 1.5f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(value == 1 && other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        if(value == 2 && other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

}
