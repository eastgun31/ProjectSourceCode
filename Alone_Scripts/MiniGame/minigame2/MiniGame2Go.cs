using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame2Go : MonoBehaviour
{
    public GameObject mainCamera;
    public Transform cameraPos;
    public GameObject miniGame;
    public GameObject playerCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            miniGame.SetActive(true);
            mainCamera.SetActive(true);
            playerCamera.SetActive(false);

            collision.gameObject.SetActive(false);
            Camera.main.transform.position = new Vector3(cameraPos.position.x, cameraPos.position.y, -10);

            gameObject.SetActive(false);
            //Destroy(gameObject,3f);
        }
    }

}
