using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage2 : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void TitleLoad()
    {
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance.stagelevel == 3)
            {
                ScoreManager.instance.SaveData();
                SceneManager.LoadScene(2);
            }
            else if(GameManager.instance.stagelevel == 4)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                ScoreManager.instance.EndTime();
                SoundManager.instance.BG_paly(0);
                SceneManager.LoadScene(3);
            }
        }
    }
}
