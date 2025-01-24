using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    private string playerattack = "PlayerAttack";
    private string playerattack2 = "PlayerAttack2";

    public GameObject enemy;
    IDamage edamage;

    void Start()
    {
        edamage = enemy.gameObject.GetComponent<IDamage>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag(playerattack))
        {
            edamage.TakeEDamage(1);
        }
        else if (other.gameObject.CompareTag(playerattack2))
        {
            edamage.TakeEDamage(2);
        }
    }
}
