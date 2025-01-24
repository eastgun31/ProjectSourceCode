using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 물이 서서히 상승하는 동작과 리셋 로직을 관리하는 클래스
public class WaterRise : MonoBehaviour
{
    float waterX; // 물의 초기 x축 위치
    float waterZ; // 물의 초기 z축 위치
    Vector3 firstPos; // 물의 초기 위치 저장
    GameManager gm; // 게임 매니저 참조

    [SerializeField]
    private bool waterMove; // 물이 상승 중인지 여부를 나타내는 상태
    [SerializeField]
    private PlayerMovement player; // 플레이어 

    void Start()
    {
        // 초기화 작업
        player = GameObject.FindAnyObjectByType<PlayerMovement>(); // 플레이어를 찾아 참조
        waterMove = true; // 물 상승 활성화
        firstPos = transform.position; // 초기 위치 저장
        waterX = transform.position.x; // 초기 x 좌표 저장
        waterZ = transform.position.z; // 초기 z 좌표 저장
        gm = GameManager.instance; 
    }

    void Update()
    {
        // 매 프레임 물 상승 처리
        if (waterMove)
        {
            // Lerp를 사용하여 물을 부드럽게 상승
            transform.position = Vector3.Lerp(transform.position,new Vector3(waterX, 2.5f, waterZ),
                Time.deltaTime * 0.01f // 물 상승 속도
            );
        }

        // 물이 일정 높이 이상 올라갔을 때 멈춤
        if (transform.position.y >= 2f && waterMove)
        {
            waterMove = false; // 물 상승 중지
        }
    }

    public void _Reset()
    {
        // 물의 위치와 플레이어 위치를 초기화
        player.controller.enabled = false; // 플레이어 컨트롤러 비활성화
        player.gameObject.transform.position = gm.playerStartPos4; // 플레이어 위치 초기화
        player.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0); // 플레이어 회전 초기화
        player.controller.enabled = true; // 플레이어 컨트롤러 재활성화
        transform.position = firstPos; // 물 위치 초기화
        waterMove = true; // 물 상승 재시작
    }
}
