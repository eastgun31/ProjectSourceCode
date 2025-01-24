using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolfOnOff : MonoBehaviour
{
    public GameObject wolf2;
    public GameObject wolf1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(wolf1 != null)
            {
                Destroy(wolf1);
            }

            GameManager.Instance.mazeCount++;

            if(GameManager.Instance.mazeCount == 1)
            {
                wolf2.SetActive(true);
            }
            else if(GameManager.Instance.mazeCount == 2)
            {
                wolf2.SetActive(false);
            }

        }
    }
}
