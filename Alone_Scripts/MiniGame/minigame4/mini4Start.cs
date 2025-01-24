using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mini4Start : MonoBehaviour
{
    public GameObject mini4;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            mini4.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            mini4.SetActive(false);
        }
    }
}
