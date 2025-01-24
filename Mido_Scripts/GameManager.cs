using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //게임매니저 싱글턴 패턴으로 구현

    public int stagelevel;  //스테이지 단계 변수
    public Vector3 playerStartPos1;  //stage1 플레이어 시작 위치
    public Vector3 playerStartPos2;  //stage2 플레이어 시작 위치
    public Vector3 playerStartPos3;  //stage3 플레이어 시작 위치
    public Vector3 playerStartPos4;  //stage4 플레이어 시작 위치
    public bool getkey;             //플레이어 열쇠 획득 여부
    public bool getwood;             //플레이어 나무 획득 여부
    public bool playerdie;          //플레이어 die 체크
    public bool quickslotOn;          //플레이어 die 체크
    public bool isCutScene;         //ESC창 체크
    public bool quizeSolve;         //퀴즈 풀었는지 체크여부
    public bool quizOn;         //퀴즈창 열렸는지 여부
    public bool inwaterNow;         //물 접촉 체크
    public bool deepwater;         //물속인지 체크
    public bool veiwStart;         //물속인지 체크
    public float sightValue;        //시야값

    public Action enemyreset;   // 적 리셋 대리자
    public Action trapreset;    // 함정 리셋 대리자
    public Action itemreset;    // 아이템 리셋 대리자
    public Action invenreset;    // 인벤토리 리셋 대리자
    public Action nextStage;    // 인벤토리 리셋 대리자
    public Action healAnimEvent;    //   heal아이템 리셋 대리자
    public Action stressAnimEvent;    // stress아이템 리셋 대리자
    public Action stage3Event;      //스테이지3 연출 대리자

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    //변수 초기화
    void Start()
    {
        getkey = false;
        playerdie = false;
        quickslotOn = false;
        playerStartPos1 = new Vector3(13.5f, 1, 13f);
        playerStartPos2 = new Vector3(18.5f, 1, 18.5f);
        playerStartPos3 = new Vector3(23.5f, 1, 23.5f);
        playerStartPos4 = new Vector3(0, -0.3f, 0);
        quizeSolve = false;
        quizOn = false;
        inwaterNow = false;
        getwood = false;
        veiwStart = false;
    }

}
