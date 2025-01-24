using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGun_mini : MonoBehaviour
{
    float speed = 2.0f;

    public GameObject bullet;

    public GameObject mainCamera;
    public GameObject playerCamera;
    public GameObject player;   //플레이어 연결
    public GameObject miniG;    //미니게임 이동 오브젝트
    public GameObject dialogCollider10;

    public GameObject wolf1;

    public GameObject apple1;
    public GameObject apple2;
    public GameObject apple3;
    public GameObject apple4;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;
        pos.x += h * Time.deltaTime * speed;

        transform.position = pos;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bulletPrefab = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 1.5f, 0), Quaternion.identity);
            Rigidbody2D bulletShot = bulletPrefab.GetComponent<Rigidbody2D>();
            bulletShot.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
        }

        if (GameManager.Instance.fruitCount == 4)    //미니게임에서 메인게임으로
        {
            player.SetActive(true);
            miniG.SetActive(false);
            mainCamera.SetActive(false);
            playerCamera.SetActive(true);
            apple1.SetActive(true);
            apple2.SetActive(true);
            apple3.SetActive(true);
            apple4.SetActive(true);
            dialogCollider10.SetActive(true);
            wolf1.SetActive(true);
            GameManager.Instance.fruitCount = 0;
            //Camera.main.transform.position = new Vector3(cameraPos.position.x, cameraPos.position.y, -10);

        }

    }
}
