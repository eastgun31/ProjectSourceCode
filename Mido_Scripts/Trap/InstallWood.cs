using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallWood : MonoBehaviour, IInteractive  //IInteractive 인터페이스 상속
{
    BoxCollider holecol;
    GameManager gm;

    void Start()
    {
        holecol = GetComponent<BoxCollider>();
        gm = GameManager.instance;
        gm.trapreset += HoleReset; //action 대리자에 리셋함수 추가
    }

    public void Interact()  //인터페이스 Interact함수
    {
        if(GameManager.instance.getwood)    //나무 아이템 획득시 나무오브젝트 활성화 후 콜라이더 비활성화
        {
            holecol.enabled = false;
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void HoleReset() //리셋 함수
    {
        holecol.enabled = true;
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
