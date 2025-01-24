using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IDamage
{
    void TakeEDamage(int a);
}

public abstract class Enemy : MonoBehaviour
{
    public enum EState
    {
        Idle,
        Move,
        Chase,
        Attack,
        Die
    }
    public EState state;

    [SerializeField]
    protected GameObject model;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected Vector3 firstpos;
    [SerializeField]
    protected Vector3[] movepos;
    [SerializeField]
    protected Material mat;
    [SerializeField]
    protected BoxCollider col;

    protected WaitForSeconds hitdelay = new WaitForSeconds(2f);
    protected bool hitnow;
    protected bool attacknow;
    protected int posindex;
    protected string player = "Player";
    protected string playerattack = "PlayerAttack";
    protected NavMeshAgent agent;
    protected Animator anim;
    protected GameManager gm;
    protected SoundManager sm;
    protected Transform target;
    protected string walk = "isWalking";
    protected string dead = "isDead";
    protected string run = "isRunning";
    protected string attack = "isAttacking";

    protected void OnEnable()
    {
        state = EState.Idle;
        posindex = 0;
    }
    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        gm = GameManager.instance;
        sm = SoundManager.instance;
        firstpos = transform.position;
        movepos[0] = firstpos;
        SetPatrolPath();
        posindex = 0;
        hitnow = false;
        state = EState.Idle;
        gm.enemyreset += _Reset;
    }
    protected void Update()
    {
        switch (state)
        {
            case EState.Idle:
                Idle();
                break;
            case EState.Move:
                Move();
                break;
            case EState.Attack: 
                Attack();
                break;
            case EState.Chase:
                Chase();
                break;
        }
    }

    public abstract void SetPatrolPath();
    public abstract void Chase();
    public abstract void Idle();
    public abstract void Attack();
    public abstract void Move();
    public abstract void Die();
    public abstract void _Reset();

    protected IEnumerator Hit()
    {
        hitnow = true;
        mat.color = Color.red;
        yield return hitdelay;
        mat.color = Color.white;
        hitnow = false;
    }
}
