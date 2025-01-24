using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상자트랩 클래스
public class FakeBoxTrap : MonoBehaviour
{
    [SerializeField]
    private bool canAttack;
    [SerializeField] 
    Transform throwpos;

    public GameObject _target;
    public GameObject spiderprefab;

    private string player = "Player";
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        canAttack = true;
        GameManager.instance.enemyreset += _Reset;  //초기화 함수를 action 대리자에 더한다
    }

    void ThrowSpider()  //플레이어 방향으로 거미 발사, 애니메이션 이벤트로 호출
    {
        GameObject spider = Instantiate(spiderprefab, throwpos.position, Quaternion.identity); //거미 생성
        Rigidbody spiderigid = spider.GetComponent<Rigidbody>();
        spiderigid.velocity = -_target.transform.forward * 10f; //플레이어 방향으로 발사
        SoundManager.instance.Effect_paly(18);

        canAttack = false;
    }

    public void _Reset()    //초기화 함수
    {
        canAttack = true;
        anim.SetBool("Attack", false);
    }

    private void OnTriggerEnter(Collider other) //범위내 플레이어가 들어오면 함정발동
    {
        if(other.CompareTag(player))
        {
            _target = other.gameObject;

            if(canAttack)
            {
                anim.SetBool("Attack", true);
            }
        }
    }
}
