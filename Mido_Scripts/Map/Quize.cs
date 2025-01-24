using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 퀴즈 UI와 로직을 관리하는 클래스
public class Quize : MonoBehaviour 
{

    public Toggle[] toggles; // 퀴즈에서 선택 가능한 Toggle 배열
    public List<bool> answerlist; // 사용자가 선택한 정답을 저장하는 리스트
    public int quizeanswer; // 정답 개수
    public BoxOpen boxopen; // 상자 열림 관련 스크립트 참조
    public int answercount = 0; // 현재 정답 개수
    [SerializeField]
    private MouseLook mouseLook; // 마우스 컨트롤 관련 스크립트
    [SerializeField]
    private GameObject quizeUI; // 퀴즈 UI 오브젝트
    [SerializeField]
    private PlayerMovement player; // 플레이어 
    [SerializeField]
    private ExcellTest script; // 자막출력 스크립트

    private void OnEnable()
    {
        // 퀴즈 UI가 활성화될 때 호출
        Cursor.visible = true; // 커서를 보이도록 설정
        Cursor.lockState = CursorLockMode.None; // 커서 잠금 해제
        mouseLook.enabled = false; // 마우스 이동 비활성화
        GameManager.instance.quizOn = true; // 퀴즈 진행 상태 설정
        player.enabled = false; // 플레이어 이동 비활성화
        // 모든 Toggle 초기화
        for (int i = 0; i < toggles.Length; i++)
            toggles[i].isOn = false; // Toggle 끄기
    }

    private void Start()
    {
        answerlist = new List<bool>(); // 정답 리스트 초기화
    }

    public void ToggleClick(bool isOn)
    {
        // 정답 선택 시 호출
        if (isOn)
            answerlist.Add(true); // true 값 추가
        else
            answerlist.Remove(true); // true 값 제거
    }

    public void ToggleClick2(bool isOn)
    {
        // 오답 선택 시 호출
        if (isOn)
            answerlist.Add(false); // false 값 추가
        else
            answerlist.Remove(false); // false 값 제거
    }
    public void AnswerCheck()
    {
        // 정답 확인
        foreach (var i in answerlist)
        {
            if (i)
                answercount++; // true인 값 카운트
        }

        // 정답 개수와 선택한 답안 개수가 일치하면 정답 처리
        if (answercount == quizeanswer && answerlist.Count == quizeanswer)
        {
            CloseQuize(); // 퀴즈 종료
            GameManager.instance.quizeSolve = true; // 퀴즈 해결 상태 업데이트
            GameManager.instance.quizOn = false; // 퀴즈 진행 상태 해제
            script.q_text(38); // 정답 관련 메시지 출력
        }
        else
            // 오답 처리
            script.q_text(39); // 오답 메시지 출력

        answercount = 0; // 정답 카운트 초기화
    }
    public void CloseQuize()
    {
        // 퀴즈 종료 및 UI 비활성화
        answercount = 0; // 정답 카운트 초기화
        quizeUI.SetActive(false); // 퀴즈 UI 비활성화
        Cursor.visible = false; // 커서 숨기기
        Cursor.lockState = CursorLockMode.Locked; // 커서 잠금
        mouseLook.enabled = true; // 마우스 이동 활성화
        player.enabled = true; // 플레이어 이동 활성화
        GameManager.instance.quizOn = false; // 퀴즈 진행 상태 해제
    }
}
