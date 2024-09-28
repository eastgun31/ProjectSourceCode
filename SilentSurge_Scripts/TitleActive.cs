using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleActive : MonoBehaviour
{
    public GameObject door;
    public GameObject backlight;
    public GameObject cam;
    public GameObject character;
    public GameObject menu;
    Animator anim;

    private void Start()
    {

        if (!SoundManager.instance.playingAction)
        {
            menu.SetActive(false);
            door.transform.rotation = Quaternion.Euler(0, 0, 0);
            cam.transform.position = new Vector3(0, cam.transform.position.y, cam.transform.position.z);
            backlight.SetActive(false);
            anim = character.GetComponent<Animator>();
            StartAction();
            anim.SetTrigger("Play");
            Invoke("CamMove", 0.7f);
            Invoke("TurnOnLightMoveCam", 1f);
            SoundManager.instance.playingAction = true;
        }
        else
        {
            menu.SetActive(true);
            backlight.SetActive(true);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            SoundManager.instance.stage1Clear = true;
            SoundManager.instance.stage2Clear = true;
        }
    }

    void StartAction()
    {
        door.GetComponent<DOTweenAnimation>().DOPlay();
    }
    void CamMove()
    {
        cam.GetComponent<DOTweenAnimation>().DOPlay();
    }
    void TurnOnLightMoveCam()
    {
        backlight.SetActive(true);
        menu.SetActive(true);
    }
}
