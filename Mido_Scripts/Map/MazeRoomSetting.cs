using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스테이지별 방 세팅 클래스
public class MazeRoomSetting : MonoBehaviour 
{
    // 스테이지별 방 오브젝트 참조
    [SerializeField]
    private GameObject stage1Room;
    [SerializeField]
    private GameObject stage2Room;
    [SerializeField]
    private GameObject stage3Room;
    [SerializeField]
    private GameObject stage3event; // 스테이지 3 이벤트 오브젝트
    // 방 위치 저장 배열
    [SerializeField]
    private Vector3[] roomPos; // 스테이지 2 방 위치 배열
    [SerializeField]
    private Vector3[] roomPos2; // 스테이지 3 방 위치 배열

    private void Start()
    {
        // 방 위치 배열 초기화
        roomPos = new Vector3[4];
        roomPos2 = new Vector3[8];

        // 스테이지 3 이벤트 호출 시 이벤트 설정 메서드 등록
        GameManager.instance.stage3Event += stage3EventSettig;
    }
    // 스테이지 1 방 설정
    public void _1RoomSettig()
    {
        stage3Room.SetActive(false); // 스테이지 3 방 비활성화
        stage2Room.SetActive(false); // 스테이지 2 방 비활성화
        stage1Room.SetActive(true);  // 스테이지 1 방 활성화
    }
    // 스테이지 2 방 설정
    public void _2RoomSettig()
    {
        stage1Room.SetActive(false); // 스테이지 1 방 비활성화
        stage3Room.SetActive(false); // 스테이지 3 방 비활성화
        stage2Room.SetActive(true);  // 스테이지 2 방 활성화
        // 방 위치를 저장
        for (int i = 0; i < roomPos.Length; i++)
        {
            roomPos[i] = stage2Room.transform.GetChild(i).transform.position;
        }
        // 방 위치를 랜덤으로 섞기
        List<Vector3> randroomPos = new List<Vector3>() { roomPos[0], roomPos[1], roomPos[2], roomPos[3] };
        for (int i = 0; i < 4; i++)
        {
            int rand = Random.Range(0, randroomPos.Count); // 랜덤 인덱스 생성
            stage2Room.transform.GetChild(i).transform.position = randroomPos[rand]; // 방 위치 변경
            randroomPos.Remove(randroomPos[rand]); // 사용한 위치는 제거
        }
    }

    // 스테이지 3 방 설정
    public void _3RoomSettig()
    {
        stage1Room.SetActive(false); // 스테이지 1 방 비활성화
        stage2Room.SetActive(false); // 스테이지 2 방 비활성화
        stage3Room.SetActive(true);  // 스테이지 3 방 활성화

        // 방 위치를 저장
        for (int i = 0; i < roomPos2.Length; i++)
        {
            roomPos2[i] = stage3Room.transform.GetChild(i).transform.position;
        }

        // 방 위치를 랜덤으로 섞기
        List<Vector3> randroomPos = new List<Vector3>()
        { roomPos2[0], roomPos2[1], roomPos2[2], roomPos2[3], roomPos2[4], roomPos2[5], roomPos2[6], roomPos2[7]};

        for (int i = 0; i < 8; i++)
        {
            int rand = Random.Range(0, randroomPos.Count); // 랜덤 인덱스 생성

            stage3Room.transform.GetChild(i).transform.position = randroomPos[rand]; // 방 위치 변경
            randroomPos.Remove(randroomPos[rand]); // 사용한 위치는 제거
        }
    }

    // 스테이지 3 이벤트 설정
    public void stage3EventSettig()
    {
        stage3event.SetActive(true); // 스테이지 3 이벤트 활성화
    }
}
