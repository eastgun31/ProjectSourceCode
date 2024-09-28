using ItemInfo;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public enum PlayerState //경계레벨 상태머신
    {
        idle, hide, puzzling, die, find
    }

    public PlayerState state;

    [SerializeField]
    private float playerspeed;

    public bool handgunacivate = false;
    public bool coinacivate = false;
    public bool flashbangacivate = false;
    public bool heartseeacivate = false;
    public bool heartseecool = true;

    [SerializeField]
    private int armor;
    [SerializeField]
    public bool[] itemGet;

    [SerializeField]
    UseItem useItem;
    //[SerializeField]
    //private GameObject playerModel;
    //Material mat;
    IItem item;

    public GameObject handGunModel;
    public GameObject bulletPrefab;
    public GameObject footSound;
    public Animator playerAnim;

    //캐싱
    Casing cas;
    //GameManager gmManager;
    //SoundManager soundManager;
    //UiManager uiManager;
    StringMoum s;
    WaitForSeconds delay;

    float rotDeg;
    Rigidbody rigid;
    Camera cam;
    Vector3 mousePos;
    public Vector3 velocity;
    public UnityEvent playerDie;

    //레이캐스트
    Ray ray;
    RaycastHit hit;
    int mask;
    public bool canAmsal = false; //암살가능 체크변수
    PlayerInteractive playerInteractive;
    public Sight sight;
    Enemy enemyState;
    public float maxdist;

    void Start()
    {
        playerspeed = 2.5f;
        itemGet =new bool[5] { false,false,false,false,false};
        state = PlayerState.idle;
        useItem = GetComponent<UseItem>();
        rigid = transform.GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerInteractive = GetComponent<PlayerInteractive>();
        //mat = playerModel.GetComponent<Material>();
        cam = Camera.main;
        cas = new Casing();
        //gmManager = GameManager.instance;
        //soundManager = SoundManager.instance;
        //uiManager = UiManager.instance;
        maxdist = 1f;
        s = new StringMoum();
        delay = new WaitForSeconds(0.5f);
    }

    void Update()
    {
        if (cas.gm.nowpuzzle)
        {            
            footSound.SetActive(false);
            state = PlayerState.puzzling;

        }
        else if (cas.gm.isHide)
        {
            state = PlayerState.hide;
            useItem.ErageDraw();
        }        
        else if (cas.gm.isDie)
        {
            state = PlayerState.die;
            rigid.velocity = Vector3.zero;
        }
        else if (!cas.gm.nowpuzzle || !cas.gm.isHide || !cas.gm.isDie)
        {
            state = PlayerState.idle;
        }
           
        if (state == PlayerState.idle && !cas.ui.isPauseWin)
        {
            if (rigid.velocity != Vector3.zero)
                rigid.velocity = Vector3.zero;
            PlayerControll();
        }
        else
            return;

        if (itemGet[4])
            armor = cas.gm.itemcount[4];

        if (Input.GetKeyDown(KeyCode.T))
        {
            useItem.OnOffMap();
        }
    }

    private void FixedUpdate()
    {
        rigid.MoveRotation(Quaternion.Euler(0, rotDeg, 0));
        rigid.MovePosition(rigid.position + velocity * Time.deltaTime);
    }

    void PlayerUseItem()
    {
        if (itemGet[0])
        {
            ItemActivate1();
            if (handgunacivate && Input.GetMouseButtonDown(0))
            {
                if (cas.gm.itemcount[0] > 0)
                    useItem.GunFire(mousePos);
            }
        }
        if (itemGet[1])
        {
            ItemActivate2();
            if (coinacivate)
            {
                useItem.ThrowPosition(coinacivate, flashbangacivate);
            }

            if (coinacivate && Input.GetMouseButtonDown(0))
            {
                if (cas.gm.canUse && cas.gm.itemcount[1] > 0)
                {
                    playerAnim.SetTrigger(s.throwcoin);
                    StartCoroutine(useItem.ThrowCoin());
                }
            }
        }
        if (itemGet[2])
        {
            ItemActivate3();
            if (flashbangacivate)
            {
                useItem.ThrowPosition(coinacivate, flashbangacivate);
            }

            if (flashbangacivate && Input.GetMouseButtonDown(0))
            {
                if (cas.gm.canUse && cas.gm.itemcount[2] > 0)
                {
                    playerAnim.SetTrigger(s.throwflashbang);
                    StartCoroutine(useItem.ThrowFlashBang());
                }
            }
        }
        if (itemGet[3])
        {
            ItemActivate4();
            if (heartseeacivate && Input.GetMouseButtonDown(0) && useItem.heartCanUse)
            {
                StartCoroutine(useItem.HeartSee());
            }
        }
    }

    void PlayerControll()
    {
        if (cas.gm.isGameOver)
            return;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        float zDeg = mousePos.z - rigid.position.z;
        float xDeg = mousePos.x - rigid.position.x;
        rotDeg = -(Mathf.Rad2Deg * Mathf.Atan2(zDeg, xDeg) - 90);
        velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * playerspeed;


        

        if (handgunacivate && playerspeed >=5)
            playerAnim.SetFloat(s._gunrun, velocity.magnitude);
        else if(!handgunacivate && playerspeed >=5)
            playerAnim.SetFloat(s._run, velocity.magnitude);
        else
            playerAnim.SetFloat(s.walk, velocity.magnitude);

        PlayerUseItem();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.DrawRay(transform.position, transform.forward * maxdist, Color.blue, 2f);
            mask = LayerMask.GetMask(s.puzzle) | LayerMask.GetMask(s.door);
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxdist, mask))
            {
                Debug.Log(mask);
                playerInteractive.InteractiveObj(hit);
            }
        }



        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerspeed = 5f;
            playerAnim.SetFloat(s.walk, 0);

            //if (handgunacivate)
            //    playerAnim.SetFloat(s._gunrun, velocity.magnitude);
            //else
            //    playerAnim.SetFloat(s._run, velocity.magnitude);

        }
        if (Input.GetKey(KeyCode.LeftShift) && velocity.magnitude >=5)
        {
            footSound.SetActive(true);

            //playerspeed = 5f;
            //playerAnim.SetFloat(s.walk, 0);

            //if (playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime ==0)
            //{
            //    Debug.Log("애니실행");
            //    if (handgunacivate)
            //        playerAnim.SetFloat(s._gunrun, velocity.magnitude);
            //    else
            //        playerAnim.SetFloat(s._run, velocity.magnitude);
            //}

            if (!cas.sm.effectPlayer.isPlaying)
                cas.sm.EffectPlay(0, true, 1f); 
            else if (cas.sm.effectPlayer.isPlaying)
                return;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerspeed = 2.5f;
            RunOff();
        }


        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out hit, 1f, LayerMask.GetMask(s.enemy)))
        {
            sight = hit.transform.gameObject.GetComponent<Sight>();
            enemyState = hit.transform.transform.gameObject.GetComponent<Enemy>();
            if (!sight.findT && enemyState.state != Enemy.EnemyState.die && !handgunacivate && !coinacivate && !flashbangacivate && !heartseeacivate)
            {
                canAmsal = true;
                if (canAmsal  && Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(useItem.Assassination());
                    sight = null;
                    canAmsal = false;
                }
            }
        }
        else
        {
            if (canAmsal)
            {
                canAmsal = false;
            }
            else
                return;
        }
    }

    void PlayerRuning()
    {
        if (handgunacivate)
            playerAnim.SetFloat(s._gunrun, velocity.magnitude);
        else
            playerAnim.SetFloat(s._run, velocity.magnitude);
    }

    void ItemActivate1()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !handgunacivate)
        {
            if(cas.gm.scenenum != 5)
                transform.position = new Vector3(transform.position.x, 0.01f, transform.position.z);
            handGunModel.SetActive(true);
            Debug.Log("권총 활성화");
            playerAnim.SetBool(s.handgunMode, true);

            ItemActivateControll(true, false, false, false);
            useItem.ErageDraw();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && handgunacivate)
        {
            playerAnim.SetBool(s.handgunMode, false);
            handGunModel.SetActive(false);
            handgunacivate = false;
            Debug.Log("권총 비활성화");
            handGunModel.SetActive(false);
        }
    }
    void ItemActivate2()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && !coinacivate)
        {
            playerAnim.SetBool(s.handgunMode, false);
            //playerAnim.SetBool(gunrun, false);
            Debug.Log("코인 활성화");
            handGunModel.SetActive(false);

            ItemActivateControll(false, true, false, false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && coinacivate)
        {
            coinacivate = false;
            Debug.Log("코인 비활성화");
            useItem.ErageDraw();
        }
    }
    void ItemActivate3()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && !flashbangacivate)
        {
            playerAnim.SetBool(s.handgunMode, false);
            //playerAnim.SetBool(gunrun, false);
            Debug.Log("섬광탄 활성화");
            handGunModel.SetActive(false);

            ItemActivateControll(false, false, true, false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && flashbangacivate)
        {
            flashbangacivate = false;
            Debug.Log("섬광탄 비활성화");
            useItem.ErageDraw();
        }
    }
    void ItemActivate4()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4) && !heartseeacivate)
        {
            playerAnim.SetBool(s.handgunMode, false);
            //playerAnim.SetBool(gunrun, false);
            Debug.Log("심장박동측정기 활성화");
            handGunModel.SetActive(false);

            ItemActivateControll(false, false, false, true);
            useItem.ErageDraw();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && heartseeacivate)
        {
            heartseeacivate = false;
            Debug.Log("심장박동측정기 비활성화");
        }
    }
    void ItemActivateControll(bool a,bool b, bool c, bool d)
    {   
        handgunacivate = a;
        coinacivate = b;
        flashbangacivate = c;
        heartseeacivate = d;
    }
    public void ItemActivateFalse()
    {
        handgunacivate = false;
        coinacivate = false;
        flashbangacivate = false;
        heartseeacivate = false;
    }

    public void RunOff()
    {
        playerAnim.SetFloat(s.walk, 0);
        playerAnim.SetFloat(s._run, 0);
        playerAnim.SetFloat(s._gunrun, 0);
        velocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
        footSound.SetActive(false);
        cas.sm.EffectOff();
    }

    IEnumerator PlayerDie()
    {
        cas.gm.isDie = true;
        cas.gm.isGameOver = true;
        ItemActivateControll(false, false, false, false);
        handGunModel.SetActive(false);
        RunOff();
        useItem.ItemOff();
        playerAnim.SetTrigger(s.die);
        yield return new WaitForSeconds(3f);
        Debug.Log("플레이어 죽음");
        //DataManager.instance.LoadData();
        playerDie.Invoke();
        cas.gm.isGameOver = false;
    }

    //IEnumerator PlayerDamage()
    //{
    //    mat.color = Color.red;
    //    yield return delay;
    //    mat.color = Color.white;
    //}

    public void PlayerRecharge()
    {
        if (itemGet[0])
            cas.gm.itemcount[0] = 6;

        if (itemGet[4])
            cas.gm.itemcount[4] = 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(s.itemTag))
        {
            item = other.GetComponent<IItem>();

            switch(item.value)
            {
                case 1:
                    if (itemGet[0])
                        item.ItemCharge();
                    else
                    {
                        item.GetItem();
                        item.ItemCharge();
                        itemGet[0] = true;
                    }
                    break;
                case 2:
                    if (itemGet[1])
                        item.ItemCharge();
                    else
                    {
                        item.GetItem();
                        item.ItemCharge();
                        itemGet[1] = true;
                    }
                    break;
                case 3:
                    if (itemGet[2])
                        item.ItemCharge();
                    else
                    {
                        item.GetItem();
                        item.ItemCharge();
                        itemGet[2] = true;
                    }
                    break;
                case 4:
                    item.GetItem();
                    itemGet[3] = true;
                    break;
                case 5:
                    if (itemGet[4])
                        item.ItemCharge();
                    else
                    {
                        item.GetItem();
                        item.ItemCharge();
                        itemGet[4] = true;
                    }
                    break;
            }
            cas.gm.existItem[item.indexNum] = false;
            other.gameObject.SetActive(false);
        }
        if(other.CompareTag(s.eBullet))
        {
            if (itemGet[4] && cas.gm.itemcount[4] >0)
            {
                cas.gm.itemcount[4]--;
                //StartCoroutine(PlayerDamage());
            }
            else
            {
                if(state != PlayerState.die)
                {
                    //StartCoroutine(PlayerDamage());
                    StartCoroutine(PlayerDie());
                }
                    
            }
        }
    }
}
