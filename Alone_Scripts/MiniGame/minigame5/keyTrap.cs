using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyTrap : MonoBehaviour
{
    public GameObject explor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            explor.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
