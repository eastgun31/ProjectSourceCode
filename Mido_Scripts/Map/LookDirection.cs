using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

//타겟 바라보는 클래스
public class LooKClass : MonoBehaviour 
{
    Vector3 velocity; // 타겟과의 방향 벡터
    float rot;        // 회전 각도

    // 타겟 방향으로 회전
    public void LookTarget(GameObject _target)
    {
        // 타겟과 현재 위치의 벡터 차이 계산
        velocity = _target.transform.position - transform.position;

        // 회전 각도 계산
        float turnAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;

        // 현재 오일러 각도를 목표 각도로 선형 보간하여 회전
        rot = Mathf.LerpAngle(transform.eulerAngles.y, turnAngle, Time.deltaTime * 100f);

        // 객체의 y축 회전 설정
        transform.eulerAngles = new Vector3(0, rot, 0);
    }
}

//미니맵의 화살표 클래스
public class LookDirection : LooKClass 
{
    GameManager gm;                      
    bool firsttime = true;               // 최초 실행 여부
    [SerializeField] private GameObject st1Key;      // 스테이지 1 키
    [SerializeField] private GameObject st2Key;      // 스테이지 2 키
    [SerializeField] private GameObject st3Key;      // 스테이지 3 키
    [SerializeField] private GameObject st4Key;      // 스테이지 4 키
    [SerializeField] private GameObject door;        // 문 객체
    [SerializeField] private GameObject st3door;     // 스테이지 3의 문 객체
    [SerializeField] private GameObject st1Items;    // 스테이지 1의 아이템
    [SerializeField] private GameObject st2Items;    // 스테이지 2의 아이템
    [SerializeField] private GameObject st3Items;    // 스테이지 3의 아이템
    [SerializeField] private GameObject st4Items;    // 스테이지 4의 아이템

    private void OnEnable()
    {
        // 최초 실행 시 현재 스테이지의 문 객체를 찾음
        if (firsttime && GameManager.instance.stagelevel == 1)
            door = FindFirstObjectByType<NextStage>().gameObject;
        else if (firsttime && GameManager.instance.stagelevel == 2)
            door = FindFirstObjectByType<NextStage>().gameObject;
        else if (firsttime && GameManager.instance.stagelevel == 3)
            door = st3door;
        // 초기 실행이 아닌 경우 아이템 활성화
        if (!firsttime)
            LookItemsOn();
    }
    private void OnDisable()
    {
        // 스테이지에 따라 아이템 화살표 비활성화
        switch (gm.stagelevel)
        {
            case 1:
                LookItemsOff(st1Items);
                break;
            case 2:
                LookItemsOff(st2Items);
                break;
            case 3:
                LookItemsOff(st3Items);
                break;
            case 4:
                LookItemsOff(st4Items);
                break;
        }
    }
    void Start()
    {
        gm = GameManager.instance; // GameManager 참조
        LookItemsOn();             // 아이템 화살표 활성화
        firsttime = false;         // 초기 실행 상태 설정
    }

    void Update()
    {
        LookPoint(); // 매 프레임 타겟 방향으로 회전
    }
    void LookPoint()
    {
        if (!gm.getkey) // 키가 없을 때는 열쇠 오브젝트를 가리킴
        {
            switch (gm.stagelevel)
            {
                case 1:
                    LookTarget(st1Key);
                    break;
                case 2:
                    LookTarget(st2Key);
                    break;
                case 3:
                    LookTarget(st3Key);
                    break;
                case 4:
                    LookTarget(st4Key);
                    break;
            }
        }
        else if (gm.getkey) // 키를 얻었을 때는 문으로 가리킴
        {
            if (gm.stagelevel == 3)
                LookTarget(st3door);
            else
                LookTarget(door);
        }
    }
    void LookItemsOn()
    {
        // 스테이지에 따라 아이템 화살표 활성화
        switch (gm.stagelevel)
        {
            case 1:
                LookItems(st1Items);
                break;
            case 2:
                LookItems(st2Items);
                break;
            case 3:
                LookItems(st3Items);
                break;
            case 4:
                LookItems(st4Items);
                break;
        }
    }

    void LookItems(GameObject stage)
    {
        stage.SetActive(true); // 아이템 화살표 활성화

        // 자식 객체들도 활성화
        for (int i = 0; i < stage.transform.childCount; i++)
        {
            stage.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    void LookItemsOff(GameObject stage)
    {
        stage.SetActive(false); // 아이템 화살표 비활성화

        // 자식 객체들도 비활성화
        for (int i = 0; i < stage.transform.childCount; i++)
        {
            stage.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
