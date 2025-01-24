using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//바위 함정 클래스
public class RockTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject player;  //플레이어
    [SerializeField]
    private Vector3 playerpos;  //플레이어 위치
    [SerializeField]
    private GameObject rockprefab;  //바위 오브젝트
    public bool trapActive;
    //캐싱
    string _player = "Player";
    WaitForSeconds delay = new WaitForSeconds(5f);
    GameManager gm;

    void Start()
    {
        trapActive = false;
        gm = GameManager.instance;
        gm.trapreset += _Reset; //초기화 함수를 action 대리자에 더한다
    }
    IEnumerator RockDrop()  //플레이어의 위치에서 머리 위로 랜덤하게 세방향으로 바위오브젝트 생성
    {
        if(!trapActive)
            yield break;
        int rand = Random.Range(0, 3);  

        if(rand == 0)   //플레이어 정면쪽 생성 및 백터값 더함
        {
            GameObject rock = Instantiate(rockprefab);
            rock.transform.position = playerpos + new Vector3(0, 10f, 3f);
        }
        else if(rand == 1)  //플레이어 우측 생성 및 백터값 더함
        {
            GameObject rock = Instantiate(rockprefab);
            rock.transform.position = playerpos + new Vector3(2f, 10f, 3f);
        }
        else if (rand == 2) //플레이어 좌측 생성 및 백터값 더함
        {
            GameObject rock = Instantiate(rockprefab);
            rock.transform.position = playerpos + new Vector3(-2f, 10f, 3f);
        }
        yield return delay;

        StartCoroutine(RockDrop());
    }

    public void _Reset()    //리셋 함수
    {
        playerpos = Vector3.zero;
        trapActive = false;
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_player))   //범위 내 플레이어가 들어오면 바위생성 코루틴함수 시작
        {
            trapActive = true;
            StartCoroutine(RockDrop());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(_player))   //플레이어가 범위내에서 나가면 코루틴 정지 및 함정 비활성화
        {
            trapActive = false;
            StopAllCoroutines();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(_player))   //실시간 플레이어의 위치를 얻어옴
        {
            playerpos = other.transform.position;
        }
    }
}
