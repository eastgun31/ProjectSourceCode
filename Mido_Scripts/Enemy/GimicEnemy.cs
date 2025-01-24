using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;
using static UnityEngine.GraphicsBuffer;

public class GimicEnemy : Enemy, IDamage
{
    public int type;
    public float damageCount;
    public GameObject stoneprefab;
    [SerializeField] Transform throwpos;

    private float time = 2f;
    private string playerfront = "PlayerFront";
    //private string enemylayer = "EnemyRange";
    Vector3 rayheight = new Vector3(0, 1f, 0);
    RaycastHit hit;
    public GameObject _target;
    public Collider[] colliders;

    public LayerMask layer;

    Vector3 velocity;
    float rot;

    public override void SetPatrolPath()
    {
        return;
    }

    void PlayerCheck()
    {
        if (state == EState.Die)
            return;

        colliders = Physics.OverlapSphere(transform.position, 5f, layer);

        if (colliders.Length <= 0)
        {
            anim.SetBool(attack, false);
            state = EState.Idle;
        }
        else
            LookTarget();
    }

    public override void Idle()
    {
        if (state == EState.Die)
            return;

        colliders = Physics.OverlapSphere(transform.position, 5f, layer);

        if (colliders.Length > 0)
        {
            state = EState.Chase;
            _target = colliders[0].gameObject;
        }
            
        return;
    }

    public override void Attack()
    {
        PlayerCheck();

        time += Time.deltaTime;

        //anim.SetBool(attack, false);

        if (Physics.Raycast(transform.position + rayheight, transform.forward, out hit, 10f,LayerMask.GetMask(playerfront)))
        {
            anim.SetBool(attack, false);
            state = EState.Chase;
        }
        else if (time >= 4f && damageCount > 0)
        {
            time = 0;
            anim.SetBool(attack,true);
            sm.Effect_paly(5);
            GameObject stone = Instantiate(stoneprefab, throwpos.position, Quaternion.identity);
            Rigidbody stonerigid = stone.GetComponent<Rigidbody>();
            stonerigid.velocity = transform.forward * 10f;
            //anim.SetBool(attack, false);
            Debug.Log("АјАн");
        }
        else if(time < 4f && damageCount > 0)
        {
            anim.SetBool(attack, false);
        }

    }

    public override void Move()
    {
        return;
    }

    public override void Chase()
    {
        PlayerCheck();

        Debug.DrawRay(transform.position + rayheight, transform.forward * 10f,Color.red);
        if(Physics.Raycast(transform.position + rayheight ,transform.forward,out hit, 10f, LayerMask.GetMask(playerfront)))
            state = EState.Chase;
        else if(Physics.Raycast(transform.position + rayheight, transform.forward, out hit, 10f, LayerMask.GetMask(player)))
            state = EState.Attack;
        else
            state = EState.Idle;
    }

    public void TakeEDamage(int a)
    {
        StartCoroutine(Hit());

        if(hitnow)
            damageCount = damageCount - a;

        if (damageCount <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        if (state != EState.Die)
        {
            state = EState.Die;
            _target = null;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            anim.SetBool(dead, true);
            col.enabled = false;
            Debug.Log("die");
        }
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

    public override void _Reset()
    {
        state = EState.Idle;
        target = null;
        damageCount = 3;
        agent.isStopped = false;
        posindex = 0;
        gameObject.transform.position = firstpos;
        //anim.SetBool(dead, false);
        col.enabled = true;
        model.SetActive(true);
        hitnow = false;
    }

    void LookTarget()
    {
        velocity = _target.transform.position - transform.position;

        float turnAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
        rot = Mathf.LerpAngle(transform.eulerAngles.y, turnAngle,Time.deltaTime * 100f);

        transform.eulerAngles = new Vector3(0, rot, 0);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag(player))
    //    {
    //        if (gm.playerdie)
    //            return;
    //        else
    //        {
    //            state = EState.Idle;
    //            _target = other.gameObject;
    //        }
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag(player))
    //    {
    //        if (gm.playerdie)
    //            return;
    //        else
    //        {
    //            state = EState.Chase;
    //            _target = other.gameObject;
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag(player))
    //    {
    //        if (gm.playerdie)
    //            return;
    //        else
    //        {
    //            state = EState.Idle;
    //            _target = null;
    //        }
    //    }
    //}
}
