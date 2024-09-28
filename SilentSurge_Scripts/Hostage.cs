using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Hostage : MonoBehaviour
{
    NavMeshAgent nav;
    public Transform target;

    Animator anim;
    public UnityEvent hostageDie;
    bool die;
    string walk = "Walk";
    string death = "Death";
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        die = false;
        gm = GameManager.instance;
        target = this.transform;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gm.hostagedie)
        {
           transform.LookAt(target);
           nav.SetDestination(target.position);
           anim.SetFloat(walk, nav.velocity.magnitude);
        }
        else if(gm.hostagedie)
            return;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("E_Bullet"))
        {
            if(!die)
            {
                Destroy(other.gameObject);
                StartCoroutine(HostageDie());
            }
                
            hostageDie.Invoke();
        }
    }
    IEnumerator HostageDie()
    {
        die = true;
        anim.SetBool(death, true);
        gm.isGameOver = true;
        gm.hostagedie = true;
        nav.isStopped = true;
        nav.velocity = Vector3.zero;
        
        yield return new WaitForSeconds(1f);
        die = false;
    }

    public void Realive()
    {
        gm.isGameOver = false;
        anim.SetBool(death,false);
        gm.hostagedie = false;
        nav.isStopped = false;
    }
}
