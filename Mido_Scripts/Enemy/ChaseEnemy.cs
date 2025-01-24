using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;
using static UnityEngine.GraphicsBuffer;

public class ChaseEnemy : Enemy, IDamage
{
    public int type;
    private float time = 2f;
    private bool hide = true;
    private bool sturn = false;
    
    public float damageCount;
    public GameObject attackRange;
    public int[] soundIndex;

    WaitForSeconds delay = new WaitForSeconds(3f);

    public void AttackOn()
    {
        attackRange.SetActive(true);
    }
    public void AttackOff()
    {
        attackRange.SetActive(false);
    }

    public override void SetPatrolPath()
    {
        return;
    }

    public override void Idle()
    {
        if (hide)
        {
            return;
        }
        else if(!hide)
        {
            model.SetActive(true);
            state = EState.Chase;
        }
            
    }

    public override void Attack()
    {
        time += Time.deltaTime;

        if (time >= 3 && damageCount > 0)
        {
            time = 0;
            transform.LookAt(target);
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            anim.SetBool(attack, true);
            sm.Effect_paly(soundIndex[1]);
            Debug.Log("공격");
        }

        if (Vector3.Distance(target.position, transform.position) > 2f)
        {
            anim.SetBool(attack, false);
            agent.isStopped = false;
            state = EState.Chase;
        }
    }

    public override void Move()
    {
        agent.SetDestination(target.position);
    }

    public override void Chase()
    {
        agent.SetDestination(target.position);

        if (Vector3.Distance(target.position, transform.position) <= 2f && !gm.playerdie && !sturn)
            state = EState.Attack;
    }

    public void TakeEDamage(int a)
    {
        StartCoroutine(Hit());

        if (hitnow)
            damageCount = damageCount - a;

        if (type == 0 && damageCount <= 0)
        {
            //damageCount--;
            Die();
        }
        else if(type == 1)
        {
            StartCoroutine(HitDelay());
        }
    }

    IEnumerator HitDelay()
    {
        Debug.Log("스턴");
        sturn = true;
        anim.SetBool(dead, true);
        agent.velocity = Vector3.zero ;
        agent.isStopped = true;
        yield return delay;
        sturn = false;
        anim.SetBool(dead, false);
        agent.isStopped = false ;
        agent.SetDestination(target.position);
        state = EState.Chase;
    }

    public override void Die()
    {
        if (state != EState.Die)
        {
            state = EState.Die;
            anim.SetBool(dead, true);
            target = null;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            AttackOff();
            Debug.Log("die");
        }
    }

    public void DieReset()
    {
        if (state == EState.Die)
        {
            Debug.Log("ddd");
            hide = true;
            model.SetActive(false);
            anim.SetBool(dead, false);
            transform.position = firstpos;
        }

    }

    public override void _Reset()
    {
        state = EState.Idle;
        damageCount = 3;
        target = null;
        hide = true;
        anim.SetBool(dead, false);
        agent.isStopped = false;
        gameObject.transform.position = firstpos;
        movepos[0] = firstpos;
        posindex = 0;
        agent.SetDestination(movepos[posindex]);
        model.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state != EState.Die && other.gameObject.CompareTag(player))
        {
            if (gm.playerdie)
                return;
            else
            {
                hide = false;
                state = EState.Idle;
                sm.Effect_paly(soundIndex[0]);
                target = other.gameObject.transform;
                agent.speed = speed;
            }
        }
    }
}
