using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//지진 함정 클래스
public class HoleTrap : MonoBehaviour
{
    public GameObject earthquake;//움직이는 부모오브젝트
    public GameObject mazes;    //미로 오브젝트
    public GameObject mazeRoom; //방 오브젝트
    public GameObject st3;      //스테이지3 맵 오브젝트
    public GameObject plane;    //바닥 오브젝트
    public PlayerMovement playerObject; //플레이어 오브젝트
    public GameObject hole;     //구멍오브젝트
    public ParticleSystem dust; //먼지 파티클
    public GameObject sightControll; //시야
    public GameObject st3HiddenRoom; //숨겨진방 오브젝트
    public UnityEvent _st3Event;    //스테이지3 전용 연출 이벤트
    public GameObject bombhole;     //스테이지4 연결 구멍
    [SerializeField] private Animator wallanim;
    //캐싱
    GameManager gm;
    Animator anim;
    string player = "Player";
    string trapactive = "TrapActive";
    string blockdown = "BlockDown";
    float time;
    WaitForSeconds delay = new WaitForSeconds(4f);
    bool startDown = false;
    SoundManager sm;

    void Start()
    {
        playerObject = GameObject.FindAnyObjectByType<PlayerMovement>();
        time = 0f;
        anim = earthquake.GetComponent<Animator>();
        gm = GameManager.instance;
        sm = SoundManager.instance;
        gm.trapreset += _Reset; //action 대리자에 리셋함수 추가
    }
    void ParentSet()    //움직일 earthquake오브젝트에 미로 오브젝트들을 자식으로 지정
    {
        if(gm.stagelevel == 3)
            st3HiddenRoom.transform.parent = mazeRoom.transform;
        mazes.transform.parent = earthquake.transform;
        mazeRoom.transform.parent = earthquake.transform;
        plane.transform.SetParent(earthquake.transform);
    }
    void ParentNull()   //다시 오브젝트들 부모해제하여 초기화
    {
        mazes.transform.SetParent(null);
        mazeRoom.transform.SetParent(null);
        plane.transform.SetParent(null);
        mazes.transform.position = Vector3.zero;
        mazeRoom.transform.position = Vector3.zero;
        plane.transform.position = Vector3.zero;
    }

    IEnumerator WallDownStart() //스테이지3 지진연출과 미로 땅으로 꺼지는 연출 코루틴함수
    {
        if (startDown)
            yield break;
        startDown = true;
        gm.isCutScene = true;

        ParentSet();
        sm.Effect_paly(11, true);
        anim.SetBool(trapactive, true);
        wallanim.SetTrigger(blockdown);
        sightControll.SetActive(false);
        RenderSettings.fogDensity = 0.02f;  //시야값 초기화
        st3.SetActive(false);
        yield return delay; //딜레이 시간 후 이벤트 호출로 타임라인 실행
        anim.SetBool(trapactive, false);
        ParentNull();
        sm.Effectsource.Stop();
        _st3Event.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {//스테이지2 방에 플레이어가 머무를 동안 지진함정 발생후 플레이어 hole 함정으로 포지션 이동
        if (gm.stagelevel == 2 && other.CompareTag(player))
        {
            time += Time.deltaTime;
            if(time > 5f)
            {
                sm.Effectsource.Stop();
                playerObject.controller.enabled = false;
                playerObject.gameObject.transform.position 
                    = hole.transform.position + new Vector3 (0, 10f, 0);
                playerObject.controller.enabled = true;
                ParentNull();
                anim.SetBool(trapactive, false);
                time = 0f;
            }
        }
    }

    public void _Reset()
    {
        anim.SetBool(trapactive, false);
        dust.Stop();
        ParentNull();
    }

    public void _EventReset()   //스테이지3 연출 리셋함수
    {
        if(startDown)
        {
            st3.SetActive(true);
            anim.SetBool(trapactive, false);
            wallanim.Rebind();  //애니메이션 초기화
            ParentNull();
            startDown=false;
            sightControll.SetActive(true);
            RenderSettings.fogDensity = 0.1f;
            bombhole.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {   //스테이지2 지진함정 애니메이션과 파티클 실행
        if (gm.stagelevel == 2 && other.CompareTag(player)) 
        {
            sm.Effect_paly(11,true);
            anim.SetBool(trapactive, true);
            dust.Play();
            ParentSet();
        }
        else if(gm.stagelevel == 3 && other.CompareTag(player)) 
        {   //스테이지3 벽 사라짐 연출 시작
            StartCoroutine(WallDownStart());
        }
    }

    private void OnTriggerExit(Collider other)
    {   //플레이어가 방에서 나갔을시 함정 초기화
        if (gm.stagelevel == 2 && other.CompareTag(player)) 
        {
            time = 0f;
            sm.Effectsource.Stop();
            anim.SetBool(trapactive, false);
            dust.Stop();
            ParentNull();
        }
    }
}
