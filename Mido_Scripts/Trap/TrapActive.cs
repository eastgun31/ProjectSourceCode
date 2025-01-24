using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActive : MonoBehaviour 
{
    [SerializeField]
    private int trapIndex = 0;  //함정 종류 인덱스
    [SerializeField]
    private GameObject trap;    //함정 오브젝트
    [SerializeField]
    private ParticleSystem[] trapParticle;  //함정 파티클
    [SerializeField]
    private bool inRange;   //함정범위
    string player = "Player";
    string trapactive = "TrapActive";
    WaitForSeconds delay = new WaitForSeconds(1.5f);    //대기시간
    GameManager gm; SoundManager sm; Animator anim;

    private void Start()
    {
        // trapIndex가 0 또는 1일 경우 애니메이터를 초기화
        if (trapIndex == 0 || trapIndex == 1)
            anim = GetComponent<Animator>();
        gm = GameManager.instance;
        sm = SoundManager.instance;
        gm.trapreset += _Reset; // 트랩 리셋 이벤트에 _Reset 함수 등록
        inRange = false;
    }
    void FireOn() // 함정 활성화 시 파티클 효과와 사운드를 재생하는 함수
    {
        sm.Effect_paly(10, true); // 사운드 재생
        foreach (var item in trapParticle)
            item.Play(); // 파티클 재생
    }
    void FireOff() // 함정 비활성화 시 파티클 효과와 사운드를 초기화 함수
    {
        sm.Effectsource.Stop(); // 사운드 정지
        foreach (var item in trapParticle)
            item.Stop(); // 파티클 정지
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(player)) // 플레이어인지 확인
        {
            if (trapIndex == 0) // 함정 애니메이션 재생
                anim.SetBool(trapactive, true);
            else if (trapIndex == 1) // 함정 오브젝트 활성화 
                trap.SetActive(true);
            else if (trapIndex == 2) // 파티클 함정
            {
                trap.SetActive(true);
                FireOn();
            }
            inRange = true; // 플레이어가 범위 안에 있다고 설정
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어인지 확인
        if (other.CompareTag(player))
        {
            if (trapIndex == 0) // 애니메이션 비활성화
            {
                anim.SetBool(trapactive, false);
            }
            else if (trapIndex == 1) // 오브젝트 비활성화 
            {
                StartCoroutine(TrapOff());
            }
            else if (trapIndex == 2) // 파티클 비활성화
            {
                trap.SetActive(false);
                FireOff();
            }
            // 플레이어가 범위 밖에 있다고 설정
            inRange = false;
        }
    }
    // 함정을 비활성화하기 위해 지연 실행되는 코루틴
    IEnumerator TrapOff()
    {
        yield return delay;
        // 플레이어가 범위 안에 없을 경우 함정 비활성화
        if (!inRange)
            trap.SetActive(false);
    }

    // 함정 상태를 리셋하는 함수
    public void _Reset()
    {
        if (trapIndex == 0) 
        {
            anim.SetBool(trapactive, false);
        }
        else if (trapIndex == 1) 
        {
            StartCoroutine(TrapOff());
        }
        else if (trapIndex == 2) 
        {
            trap.SetActive(false);
            FireOff();
        }
        inRange = false;
    }
}
