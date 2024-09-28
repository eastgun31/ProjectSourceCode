using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadGameStart()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadStage1_normal()
    {
        SceneManager.LoadScene(1);
        //StartCoroutine(LoadDelay());
    }    
    public void LoadStage1_hard()
    {
        SceneManager.LoadScene(2);
        //StartCoroutine(LoadDelay());
    }    
    public void LoadStage2_normal()
    {
        SceneManager.LoadScene(3);
    }    
    public void LoadStage2_hard()
    {
        SceneManager.LoadScene(4);
    }
    public void LoadStage3()
    {
        SceneManager.LoadScene(5);
    }
    public void LoadEnding1()
    {
        SceneManager.LoadScene(6);
    }    
    public void LoadEnding2()
    {
        SceneManager.LoadScene(7);
    }
    public void LoadEnding3()
    {
        SceneManager.LoadScene(8);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadDelay()
    {
        yield return new WaitForSeconds(2f);
        DataManager.instance.SaveData();
    }
}
