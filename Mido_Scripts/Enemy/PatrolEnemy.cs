using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolEnemy : Enemy, IDamage
{
    public int type;
    private float time = 2f;
    string moveLimit = "MoveLimit";

    public float damageCount;
    public GameObject attackRange;
    public int[] soundIndex;

    public void AttackOn()
    {
        attackRange.SetActive(true);
    }
    public void AttackOff()
    {
        attacknow = false;
        attackRange.SetActive(false);
    }

    public override void SetPatrolPath()
    {
        if (type == 1)
        {
            movepos[0] = firstpos;
            movepos[1] = transform.position + new Vector3(0, 0, 5f);
            movepos[2] = transform.position + new Vector3(5f, 0, 5f);
            movepos[3] = transform.position + new Vector3(5f, 0, 0);
        }
        else
            return;
    }

    public override void Idle()
    {      
       //anim.SetBool(dead, false);
       state = EState.Move;
    }

    public override void Move()
    {
        anim.SetBool(run, false);
        anim.SetBool(attack, false);

        if (Vector3.Distance(movepos[posindex], transform.position) < 2f)
        {
            posindex++;

            if (posindex >= movepos.Length)
                posindex = 0;

            agent.SetDestination(movepos[posindex]);
        }
    }

    public override void Attack()
    {
        time += Time.deltaTime;

        if(time >= 2 && damageCount > 0)
        {
            //anim.SetBool(run, false);
            time = 0;
            attacknow = true;
            transform.LookAt(target);
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            anim.SetBool(attack,true);
            sm.Effect_paly(soundIndex[1]);

            Debug.Log("공격");

            if (Vector3.Distance(target.position, transform.position) > 2.2f)
                anim.SetBool(attack, false);
        }

        if (Vector3.Distance(target.position, transform.position) > 2.2f && !attacknow)
        {
            anim.SetBool(attack, false);
            //agent.isStopped = false;
            state = EState.Chase;
        }

    }

    public override void Chase()
    {
        if(agent.isStopped)
            agent.isStopped = false;

        anim.SetBool(run, true);

        if (Vector3.Distance(target.position, transform.position) < 5f && !gm.playerdie)
        {
            agent.SetDestination(target.position);

            if(Vector3.Distance(target.position, transform.position) <= 2.2f && !gm.playerdie)
            {
                //anim.SetBool(run, false);
                state = EState.Attack;
            }
                
        }
        else
        {
            anim.SetBool(run, false);
            state = EState.Move;
            agent.SetDestination(movepos[posindex]);
        }
            
    }

    //public void TakeEDamage()
    //{
    //    damageCount--;

    //    if (damageCount <= 0)
    //    {
    //        Die();
    //    }
    //}

    public void TakeEDamage(int a)
    {
        StartCoroutine(Hit());

        if (type == 1)
        {
            if (hitnow)
                damageCount = damageCount - a;

            if (damageCount <= 0)
            {
                Die();
            }
        }
        else if (type == 0 && a >=2)
        {
            if (hitnow)
                damageCount = damageCount - a;

            if (damageCount <= 0)
            {
                Die();
            }
        }
            
    }

    public override void Die()
    {
        if(state != EState.Die)
        {
            state = EState.Die;
            target = null;
            agent.velocity = Vector3.zero;
            anim.SetBool(attack, false);
            AttackOff();
            col.enabled = false;
            agent.isStopped = true;
            anim.SetBool(dead, true);
            Debug.Log("die");
        }
    }

    public override void _Reset()
    {
        state = EState.Idle;
        target = null;
        damageCount = 3;
        agent.isStopped = false;
        col.enabled = true;
        posindex = 0;
        gameObject.transform.position = firstpos;
        model.SetActive(true);
    }

    public void DieReset()
    {
        if (state == EState.Die)
        {
            Debug.Log("ddd");
            model.SetActive(false);
            anim.SetBool(dead, false);
            transform.position = firstpos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state != EState.Die && other.gameObject.CompareTag(player))
        {
            if (gm.playerdie)
                return;
            else
            {
                sm.Effect_paly(soundIndex[0]);
                anim.SetBool(run, true);
                state = EState.Chase;
                target = other.gameObject.transform;
                agent.speed = speed;
            }
        }

        if(other.gameObject.CompareTag(moveLimit))
        {
            Debug.Log("제한");
            anim.SetBool(run, false);
            state = EState.Move;
            agent.SetDestination(movepos[posindex]);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (state != EState.Die && other.gameObject.CompareTag(player))
        {
            if (Vector3.Distance(target.position, transform.position) > 5f && !gm.playerdie)
            {
                anim.SetBool(run, false);
                state = EState.Move;
                agent.SetDestination(firstpos);
            }
        }
    }
}
