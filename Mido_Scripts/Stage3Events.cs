using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

//스테이지3 연출 클래스
public class Stage3Events : MonoBehaviour
{   
    public Camera eventcam;             //연출 카메라
    public GameObject eventcinemachine; //시네머신카메라
    public GameObject SightControll;    
    public PlayableDirector timeline;   //타임라인
    public GameObject bombhole;
    public PlayerMovement player;
    public MouseLook mouseLook;

    [SerializeField]
    private ExcellTest script;
    [SerializeField]
    private GameObject player_ui;
    [SerializeField]
    private GameObject arm;

    WaitForSeconds delay = new WaitForSeconds(14f);
    string uioff = "UiOff";

    public void EvnetCinemachine()  //타임라인 실행함수
    {
        script.NPC_text(57);
        SoundManager.instance.Effect_paly(25);
        timeline.Play();
        StartCoroutine(EventEnd());
        Invoke(uioff, 9f);
        arm.SetActive(false);
    }

    void UiOff() //1인칭에서 3인칭으로 바꾸기위해 플레이어 유아이 비활성화
    {
        player_ui.SetActive(false);
    }

    IEnumerator EventEnd()  //이벤트 사작시 플레이어 정지 후 이벤트 종료시 다시 플레이어 활성화
    {   
        player.controller.enabled = false;
        player.gameObject.transform.position = new Vector3 (-1f, 1f, 1.5f);
        player.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.enabled = false;
        mouseLook.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;

        yield return delay; //대기시간으로 타임라인 싱크에 맞춰 이벤트 오브젝트들 비활성화 및 초기화

        eventcam.gameObject.SetActive(false);
        eventcinemachine.SetActive(false);

        player.enabled = true;
        player.controller.enabled = true;
        mouseLook.enabled = true;
        bombhole.SetActive(true);
        GameManager.instance.isCutScene = false;
        player_ui.SetActive(true);
        script.Player_text(40);
        script.NPC_text(55);
        SoundManager.instance.Effect_paly(28);
        arm.SetActive(true);
    }
}
