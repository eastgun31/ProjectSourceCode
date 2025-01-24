using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGunBullet_mini : MonoBehaviour
{
    float speed = 1f;
    float fruitfall = 2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x - 0.5f * speed * Time.deltaTime, transform.localScale.x - 0.5f * speed * Time.deltaTime, 0);
        Destroy(gameObject, 3f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fruit")
        {
            GameManager.Instance.fruitCount++;
            collision.gameObject.SetActive(false);
            //Rigidbody2D fruitRigid = collision.GetComponent<Rigidbody2D>();
            //fruitRigid.AddForce(Vector2.down * fruitfall, ForceMode2D.Impulse);
            //if (collision.transform.position.y < -2.5f)
            //{
            //    Debug.Log(1);
            //    GameManager.Instance.fruitCount++;
            //}
            Destroy(gameObject);
        }
    }
}
