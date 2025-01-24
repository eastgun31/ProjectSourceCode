using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 다음 스테이지로 이동 및 관련 동작을 담당하는 클래스
public class NextStage : MonoBehaviour, IInteractive //인터페이스 상속
{
    [SerializeField]
    private MazeMake makemaze; // 미로 생성 관리
    [SerializeField]
    private PlayerMovement paint; // 플레이어 페인트 관련 관리
    [SerializeField]
    private SightController sight; // 시야 조정 관리
    [SerializeField]
    private FiddleStick_Excell doorScriptobj; // 문 스크립트 오브젝트
    [SerializeField]
    private ExcellTest excellTest; // 엑셀 데이터를 관리하는 오브젝트

    Animator anim; 
    GameManager gm; 
    string open = "Open"; // 문 열기 애니메이션 트리거 문자열
    string doorscripts = "DoorScripts"; // 문 스크립트 오브젝트 문자열

    private void Start()
    {
        // 초기화
        anim = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
        makemaze = FindObjectOfType<MazeMake>(); // MazeMake 오브젝트 찾기
        paint = FindObjectOfType<PlayerMovement>(); // PlayerMovement 오브젝트 찾기
        sight = FindObjectOfType<SightController>(); // SightController 오브젝트 찾기
        excellTest = FindObjectOfType<ExcellTest>(); // ExcellTest 오브젝트 찾기
        gm = GameManager.instance; // GameManager 싱글톤 인스턴스

        doorScriptobj = 
        GameObject.Find(doorscripts).GetComponent<FiddleStick_Excell>(); 
        // DoorScripts 오브젝트에서 FiddleStick_Excell 스크립트 참조
    }

    public void _NextStage()
    {
        // 다음 스테이지로 이동
        if (gm.getkey) // 열쇠가 있는지 확인
        {
            if (gm.stagelevel == 3) // 스테이지 3일 때
            {
                excellTest.text_M(); // 자막 실행
                SceneManager.LoadScene(2); // 씬 2 로드
            }
            else if (gm.stagelevel == 4) // 스테이지 4일 때
            {
                excellTest.text_M(); // 자막 실행
                SceneManager.LoadScene(3); // 씬 3 로드
            }
            else
            {
                // 스테이지가 1 또는 2일 경우
                if (gm.stagelevel == 1)
                    // 스테이지 1: 문 위치 변경
                    doorScriptobj.gameObject.transform.position = new Vector3(-19, 0, -19);
                else if (gm.stagelevel == 2)
                    // 스테이지 2: 문 오브젝트 비활성화
                    doorScriptobj.gameObject.SetActive(false);

                // 다음 스테이지 준비
                makemaze.ChangeMaze(); // 미로 변경
                paint.ResetPaint(); // 페인트 초기화
                sight.RestSight(); // 시야 초기화
                gm.nextStage(); // 스테이지 레벨 증가
                ScoreManager.instance.SaveData(); // 점수 데이터 저장
                excellTest.text_M(); // 자막 실행
            }
        }
    }

    public void Interact()  //상호작용 함수
    {
        if (gm.getkey) // 열쇠가 있으면
            anim.SetTrigger(open); // 문 열기 애니메이션 트리거 실행
        else
        {
            // 열쇠가 없으면 메시지 표시
            excellTest.DisplayNoKeyText();
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 트리거 충돌 처리
        if (other.CompareTag("Player")) // 플레이어와 충돌했을 때
        {
            if (gm.stagelevel == 3) // 스테이지 3이면
                SceneManager.LoadScene(2); // 씬 2 로드
        }
    }
}
