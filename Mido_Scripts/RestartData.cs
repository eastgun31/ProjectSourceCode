using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RestartData : MonoBehaviour
{
    //스테이지별 리셋할때 호출할 이벤트들
    public UnityEvent stage1Restart;
    public UnityEvent stage2Restart;
    public UnityEvent stage3Restart;
    public UnityEvent stage4Restart;

    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    //게임매니저에 있는 대리자들 action 
    public void ResetEnemy() //적 초기화 함수
    {
        gm.enemyreset();
    }

    public void ResetItem() //아이템 초기화 함수
    {
        gm.itemreset();
    }

    public void ResetInven() //인벤토리 초기화 함수
    {
        gm.invenreset();
    }

    public void ResetTrap() //함정 초기화 함수
    {
        gm.trapreset();
    }

    //게임 재시작시 각 스테이지별 이벤트호출
    public void Restart()
    {
        switch(gm.stagelevel)
        {
            case 3:
                stage3Restart.Invoke();
                break;
            case 1:
                stage1Restart.Invoke();
                break;
            case 2:
                stage2Restart.Invoke();
                break;
            case 4:
                stage4Restart.Invoke();
                break;
        }

        ScoreManager.instance.LoadData();  
    }
}
