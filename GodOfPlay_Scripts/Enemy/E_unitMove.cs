using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using static E_unitMove;
using static UnityEngine.UI.CanvasScaler;

public class E_unitMove : MonoBehaviour
{
    //유닛 내부 이펙트
    public GameObject E_attackRange;
    public GameObject Hermes;
    public GameObject Dionysus;
    public GameObject Poseidon;
    public GameObject Hera;
    public GameObject Hestia;
    public GameObject Hades;

    public GameObject[] points;
    public int unitNum;

    float speed = 7f;
    public float health;
    float time = 0;
    Rigidbody rigid;

    NavMeshAgent moving;
    public Vector3 lastDesti;
    public UnitController targetUnit;

    public float ehealth;
    public float eattackPower;
    public float edefense;
    public float emoveSpeed;

    float original_eshieldValue;
    public float eshieldValue; //보호막 변수

    public int unitType;

    public Slider Eslider;
    public float maxhp;
    public Points point;

    public Slider ESslider; //
    public float maxS; //
    public GameObject arrow;
    public Transform shotpos;

    private Animator enemyAnim;

    //최적화 변수들
    string run = "run";
    string attack = "attack";
    string _point = "Point";

    public bool isHades = false;

    public enum E_UnitState //적 상태머신
    {
        Battle, Idle, goPoint, noBattle, Stun
    }

    public E_UnitState e_State;

    // Start is called before the first frame update
    void Start()
    {
        e_State = E_UnitState.noBattle;
        rigid = GetComponent<Rigidbody>();
        moving = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<Animator>();

        maxhp = ehealth;
        maxS = eshieldValue; //
        maxS = maxhp; //
        StartCoroutine(Pcheck()); //유닛 hp체크후 죽음
        if(EnemySkillManager.instance.e_active_skillnum == 2)
        {
            StartCoroutine(Shieldcheck());
        }
        StartCoroutine(usingItem());
    }

    private void FixedUpdate()
    {
        Eslider.value = ehealth / maxhp; 
        ESslider.value = eshieldValue / maxS; //
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        switch (e_State)
        {
            case E_UnitState.Idle:
                E_Idle();
                break;
            case E_UnitState.goPoint:
                E_GoPoint();
                break;
        }

        enemyAnim.SetFloat(run, moving.desiredVelocity.magnitude);
    }

    void E_Idle()
    {
        if(ehealth>0)
        {
            time = 0;
            moving.isStopped = false;
            e_State = E_UnitState.goPoint;
        }
    }

    void E_GoPoint()
    {
        MovePoint(lastDesti);
        e_State = E_UnitState.noBattle;
    }

    public void MovePoint(Vector3 i)
    {
        lastDesti = i;
        moving.speed = emoveSpeed;
        moving.SetDestination(i);

        //enemyAnim.SetFloat("run", Vector3.Distance(transform.position,i));

        transform.SetParent(null);
    }

    public void Attakc(Vector3 dir, UnitController p_unit)
    {
        if (ehealth <= 0)
            return;

        if (p_unit.uhealth > 0)
        {
            targetUnit = p_unit;
            Find_Target(dir, targetUnit);
        }
    }

    void Find_Target(Vector3 dir, UnitController p_unit)
    {
        if (ehealth <= 0)
            return;

        if (ehealth > 0)
        {
            moving.SetDestination(dir);
            //moving.stoppingDistance = 2f;

            if(Vector3.Distance(transform.position, dir) <= 3f)
            {
                moving.isStopped = true;
                moving.velocity = Vector3.zero;
            }
        }

        //moving.SetDestination(dir);
        //moving.stoppingDistance = 2f;

        if (p_unit.uhealth <= 0)
        {
            targetUnit = null;
        }

        if (unitType == 1 && Vector3.Distance(transform.position, dir) <= 10f && p_unit.uhealth > 0)
        {
            moving.isStopped = true;
            moving.velocity = Vector3.zero;

            StartCoroutine(Damage(dir, p_unit));
        }
        else if (Vector3.Distance(transform.position, dir) <= 3f && p_unit.uhealth > 0)
        {
            moving.isStopped = true;
            moving.velocity = Vector3.zero;

            StartCoroutine(Damage(dir, p_unit));
        }
        else if (Vector3.Distance(transform.position, dir) > 5f)
        {
            moving.isStopped = false;
            moving.SetDestination(dir);
            moving.stoppingDistance = 2f;
            //enemyAnim.SetFloat("run", attackspeed);
        }
    }

    IEnumerator Damage(Vector3 dir, UnitController p_unit)
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        moving.isStopped = true;
        moving.velocity = Vector3.zero;
        transform.LookAt(dir);

        if (EnemySkillManager.instance.useSkill)
        {
            EnemySkillManager.instance.E_UseSkill(dir, gameObject.transform.position, p_unit, lastDesti);
            //EnemySkillManager.instance.useSkill = false;
        }

        if (ehealth > 0 && p_unit.uhealth > 0 && time > 1f && e_State == E_UnitState.Battle)
        {
            time = 0;
            transform.LookAt(dir);
            enemyAnim.SetTrigger(attack);

            //if (unitType == 1)
            //{
            //    Instantiate(arrow, shotpos.position, Quaternion.identity);
            //    //arrow.transform.rotation = Quaternion.Euler(-90, 0, 0);
            //    arrow.GetComponent<Arrow>().shotdir = dir;
            //}

            if (p_unit.ushieldValue > 0)
            {
                p_unit.ushieldValue -= eattackPower;
            }
            else if (p_unit.ushieldValue < 0)
            {
                p_unit.uhealth += p_unit.ushieldValue;

                p_unit.ushieldValue = 0;
            }
            else
                p_unit.uhealth -= eattackPower;


            yield return wait;

            StartCoroutine(Damage(dir, p_unit));
        }

        if (p_unit.uhealth <= 0 && ehealth > 0)
        {
            e_State = E_UnitState.Idle;
            targetUnit = null;

            if (targetUnit == null && ehealth > 0)
            {
                moving.isStopped = false;
                //enemyAnim.SetFloat("run", Vector3.Distance(transform.position, lastDesti));
                moving.SetDestination(lastDesti);
            }

            StopCoroutine("Damage");
        }
    }


    private void OnEnable()
    {
        if (unitNum == 0)
        {
            unitType = 0;
            ehealth = GameManager.instance.health;
            eattackPower = GameManager.instance.attackPower;
            edefense = GameManager.instance.defense;
            emoveSpeed = GameManager.instance.moveSpeed;
        }
        if (unitNum == 1)
        {
            unitType = 0;
            ehealth = GameManager.instance.health + 50;
            eattackPower = GameManager.instance.attackPower - 2;
            edefense = GameManager.instance.defense + 2;
            emoveSpeed = GameManager.instance.moveSpeed - 2;
        }
        if (unitNum == 2)
        {
            unitType = 1;
            ehealth = GameManager.instance.health - 20;
            eattackPower = GameManager.instance.attackPower + 3;
            edefense = GameManager.instance.defense - 1;
            emoveSpeed = GameManager.instance.moveSpeed + 1;
        }
        if (unitNum == 3)
        {
            unitType = 0;
            ehealth = GameManager.instance.health + 100;
            eattackPower = GameManager.instance.attackPower + 5;
            edefense = GameManager.instance.defense + 5;
            emoveSpeed = GameManager.instance.moveSpeed + 3;
        }

        if (unitNum == 4)
        {
            unitType = 0;
            ehealth = GameManager.instance.health + 50;
            eattackPower = GameManager.instance.attackPower + 5;
            edefense = GameManager.instance.defense + 5;
            emoveSpeed = GameManager.instance.moveSpeed;
        }
        if (unitNum == 5)
        {
            unitType = 0;
            ehealth = GameManager.instance.health + 100;
            eattackPower = GameManager.instance.attackPower + 3;
            edefense = GameManager.instance.defense + 7;
            emoveSpeed = GameManager.instance.moveSpeed - 2;
        }
        if (unitNum == 6)
        {
            unitType = 1;
            ehealth = GameManager.instance.health + 30;
            eattackPower = GameManager.instance.attackPower + 8;
            edefense = GameManager.instance.defense + 4;
            emoveSpeed = GameManager.instance.moveSpeed + 1;
        }
        if (unitNum == 7)
        {
            unitType = 0;
            ehealth = GameManager.instance.health + 150;
            eattackPower = GameManager.instance.attackPower + 10;
            edefense = GameManager.instance.defense + 10;
            emoveSpeed = GameManager.instance.moveSpeed + 3;
        }

        if (unitNum == 8)
        {
            unitType = 0;
            ehealth = GameManager.instance.health + 100;
            eattackPower = GameManager.instance.attackPower + 10;
            edefense = GameManager.instance.defense + 10;
            emoveSpeed = GameManager.instance.moveSpeed;
        }
        if (unitNum == 9)
        {
            unitType = 0;
            ehealth = GameManager.instance.health + 150;
            eattackPower = GameManager.instance.attackPower + 8;
            edefense = GameManager.instance.defense + 12;
            emoveSpeed = GameManager.instance.moveSpeed - 2;
        }
        if (unitNum == 10)
        {
            unitType = 1;
            ehealth = GameManager.instance.health + 80;
            eattackPower = GameManager.instance.attackPower + 13;
            edefense = GameManager.instance.defense + 9;
            emoveSpeed = GameManager.instance.moveSpeed + 1;
        }
        if (unitNum == 11)
        {
            unitType = 0;
            ehealth = GameManager.instance.health + 200;
            eattackPower = GameManager.instance.attackPower + 15;
            edefense = GameManager.instance.defense + 15;
            emoveSpeed = GameManager.instance.moveSpeed + 3;
        }

        //패시브--------------------------------------------------------------------
        //검사 패시브가 켜지면   
        if (EnemySkillManager.instance.e_passiveNow == 3)
        {
            if (unitNum == 0 || unitNum == 4 || unitNum == 8)
            {
                ehealth += 30;
                eattackPower += 3;
                edefense += 3;
            }
        }
        //방패병
        if (EnemySkillManager.instance.e_passiveNow == 1)
        {
            if (unitNum == 1 || unitNum == 5 || unitNum == 9)
            {
                ehealth += 30;
                eattackPower += 3;
                edefense += 3;
            }
        }
        //궁수
        if (EnemySkillManager.instance.e_passiveNow == 2)
        {
            if (unitNum == 2 || unitNum == 6 || unitNum == 10)
            {
                ehealth += 30;
                eattackPower += 3;
                edefense += 3;
            }
        }

    }

    IEnumerator Pcheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        if (ehealth <= 0 && isHades)
        {
            ehealth = maxhp / 2;
            isHades = false;
        }

        if (ehealth <= 0 && !isHades)
        {
            moving.isStopped = true;
            moving.velocity = Vector3.zero;
            E_attackRange.SetActive(false);
            targetUnit = null;

            GameManager.instance.gold += 10;

            enemyAnim.SetTrigger("death");

            yield return new WaitForSeconds(2f);

            GameManager.instance.e_population--;

            if (point)
            {
                point.e_distance = 100f;
            }

            Destroy(gameObject);

            StopCoroutine(Pcheck());
        }

        yield return wait;
        StartCoroutine(Pcheck());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_point))
        {
            point = other.GetComponent<Points>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_point))
        {
            point.e_distance = 100f;
        }
    }

    public void ZeusDamage(float damage)
    {
        ehealth -= damage;
        enemyAnim.SetTrigger("hit");
    }

    public void PoseidonShield(int shield)
    {
        eshieldValue += shield;

        StartCoroutine(Delay(original_eshieldValue, 7f));
    }

    IEnumerator Shieldcheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        if (eshieldValue <= 0)
        {
            Poseidon.SetActive(false);
        }

        yield return wait;
        StartCoroutine(Shieldcheck());
    }

    private IEnumerator Delay(float originalValue, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        eshieldValue = originalValue;

        Poseidon.SetActive(false);
    }

    public void ApolloHeal(float heal)
    {
        ehealth += heal;
    }

    public IEnumerator HeraStun(float sec)
    {
        e_State = E_UnitState.Stun;

        if (e_State == E_UnitState.Stun)
        {
            moving.isStopped = true;
            moving.velocity = Vector3.zero;
        }

        E_attackRange.SetActive(false);
        targetUnit = null;
        Hera.SetActive(true);

        yield return new WaitForSeconds(sec);

        e_State = E_UnitState.Idle;
        moving.isStopped = false;
        E_attackRange.SetActive(true);
        Hera.SetActive(false);
    }

    public IEnumerator HadesDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (isHades)
        {
            isHades = false;
            Hades.SetActive(false);
        }
            
    }

    public void AphroditeChange(Vector3 spawnPoint)
    {
        StartCoroutine(_AphroditeChange(spawnPoint));
    }

    public IEnumerator _AphroditeChange(Vector3 spawnPoint)
    {
        if (unitNum == 0 || unitNum == 4 || unitNum == 8)
            All_Lv_LCL.instance.Aphrodite_Warrior(spawnPoint);
        else if (unitNum == 1 || unitNum == 5 || unitNum == 9)
            All_Lv_LCL.instance.Aphrodite_Shield(spawnPoint);
        else if (unitNum == 2 || unitNum == 6 || unitNum == 10)
            All_Lv_LCL.instance.Aphrodite_Archer(spawnPoint);
        else
            All_Lv_LCL.instance.Aphrodite_HorseMan(spawnPoint);

        ehealth = 0;

        yield return new WaitForSeconds(0.2f);

        ehealth = 0;

        if (point)
        {
            point.e_distance = 100f;
        }
        
        Destroy(gameObject);
    }
    IEnumerator usingItem()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        if (EnemySkillManager.instance.usingItem)
        {
            if (EnemySkillManager.instance.e_item_skillnum == 1)
            {
                EnemySkillManager.instance.usingItem = false;
                Hermes.SetActive(true);
                //EnemySkillManager.instance.usingItem = false;

                float originalSpeed = emoveSpeed;
                emoveSpeed += 3;

                yield return new WaitForSeconds(5f);

                Hermes.SetActive(false);
                emoveSpeed = originalSpeed;
            }

            if (EnemySkillManager.instance.e_item_skillnum == 2)
            {
                EnemySkillManager.instance.usingItem = false;
                Hestia.SetActive(true);
                //EnemySkillManager.instance.usingItem = false;

                ehealth += 15;

                yield return new WaitForSeconds(2f);

                Hestia.SetActive(false);
            }

            if (EnemySkillManager.instance.e_item_skillnum == 3)
            {
                EnemySkillManager.instance.usingItem = false;
                Dionysus.SetActive(true);
                //EnemySkillManager.instance.usingItem = false;

                float originalDamage = eattackPower;
                eattackPower *= 2;

                yield return new WaitForSeconds(5f);

                Dionysus.SetActive(false);
                eattackPower = originalDamage;
            }

            EnemySkillManager.instance.usingItem = false;
        }

        yield return wait;

        if (EnemySkillManager.instance.itemLimit == 0)
        {
            StopCoroutine("usingItem");
        }

        StartCoroutine(usingItem());
    }
}
