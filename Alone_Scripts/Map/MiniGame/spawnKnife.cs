using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnKnife : MonoBehaviour
{
    public GameObject knife;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(ShootKnife());
            GameManager.Instance.key = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && GameManager.Instance.key == true)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ShootKnife()
    {
        GameObject knifePrefab1 = Instantiate(knife, new Vector3(9f, 13.5f, 0), Quaternion.identity);
        Rigidbody2D knifeThrow1 = knifePrefab1.GetComponent<Rigidbody2D>();
        knifeThrow1.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
        GameObject knifePrefab2 = Instantiate(knife, new Vector3(9f, 10.5f, 0), Quaternion.identity);
        Rigidbody2D knifeThrow2 = knifePrefab2.GetComponent<Rigidbody2D>();
        knifeThrow2.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
        GameObject knifePrefab3 = Instantiate(knife, new Vector3(-9f, 12.5f, 0), Quaternion.Euler(0, 180.0f, 0));
        Rigidbody2D knifeThrow3 = knifePrefab3.GetComponent<Rigidbody2D>();
        knifeThrow3.AddForce(Vector2.right * 5, ForceMode2D.Impulse);
        GameObject knifePrefab4 = Instantiate(knife, new Vector3(-9f, 9.8f, 0), Quaternion.Euler(0, 180.0f, 0));
        Rigidbody2D knifeThrow4 = knifePrefab4.GetComponent<Rigidbody2D>();
        knifeThrow4.AddForce(Vector2.right * 5, ForceMode2D.Impulse);

        yield return new WaitForSeconds(2f);

        StartCoroutine(ShootKnife());
    }

}
