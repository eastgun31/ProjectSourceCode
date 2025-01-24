using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//시야 클래스
public class SightController : MonoBehaviour
{
    [Header("시야 범위")]
    [Range(0.1f, 0.5f)]
    public float fogDensity;    // 현재 시야값

    public float fogNextValue;  // 다음 목표 시야값
    [SerializeField]
    private float time;         // 경과 시간
    float sightTimer = 30f;     // 시야 감소 타이머

    bool scriptsonce;           // 플레이어에게 메시지를 한 번만 출력하도록 제어
    GameManager gm;             

    void Start()
    {
        // 초기화
        time = 0f;
        gm = GameManager.instance;

        // 스테이지에 따라 초기 시야값 설정
        if (gm.stagelevel == 3)
            fogDensity = 0.01f;
        else
            fogDensity = 0.1f;

        fogNextValue = fogDensity;
        RenderSettings.fogDensity = fogDensity;

        // GameManager에서 이벤트 연결
        gm.stressAnimEvent += RestSight;

        scriptsonce = false; // 초기화 시 스크립트 메시지를 출력하지 않음
    }

    void Update()
    {
        // 시야 감소가 시작되었는지 확인
        if (gm.veiwStart)
            ChangeView();
    }

    void ChangeView()
    {
        // 시야가 최대로 좁아졌을 때 처리
        if (fogDensity >= 0.4f && scriptsonce)
            return;
        else if (fogDensity >= 0.4f && !scriptsonce)
        {
            // 플레이어에게 경고 메시지 출력
            ExcellTest.instance.Player_text(63);
            scriptsonce = true;
            return;
        }
        // 타이머 증가
        time += Time.deltaTime;
        // 일정 시간이 지나면 시야를 줄임
        if (time > sightTimer)
        {
            time = 0f;
            fogNextValue += 0.1f; // 목표 시야값 증가
            StartCoroutine(DownView());
        }
    }

    IEnumerator DownView()
    {
        // 목표 시야값에 도달했으면 코루틴 종료
        if (fogDensity == fogNextValue)
        {
            StopCoroutine(DownView());
            yield break;
        }

        // 시야 감소 처리, 시야 감소시 lerp로 부드럽게 시야를 감소시킴
        RenderSettings.fogDensity = fogDensity;
        fogDensity = Mathf.Lerp(fogDensity, fogNextValue, Time.deltaTime * 0.5f);
        yield return null;  

        // 반복 실행
        StartCoroutine(DownView());
    }

    public void RestSight()
    {
        // 시야를 초기값으로 복구
        time = 0f;
        scriptsonce = false;
        fogDensity = 0.1f;
        fogNextValue = fogDensity;
        RenderSettings.fogDensity = fogDensity;
    }
}
