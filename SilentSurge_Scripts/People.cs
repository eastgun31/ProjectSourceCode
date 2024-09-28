using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class People : MonoBehaviour
{
    Animator anim;
    GameManager gm;
    public UnityEvent peopleDieEvent;

    void Start()
    {
        anim = GetComponent<Animator>();
        gm = GameManager.instance;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("E_Bullet") || other.CompareTag("Bullet") )
        {
            if (!gm.peopledie)
            {
                Destroy(other.gameObject);
                gm.peopledie = true;
                gm.isGameOver = true;
                anim.SetTrigger("Die");
                //StartCoroutine(PeopleDie());
            }
            else
                return;
                

            peopleDieEvent.Invoke();
            
        }
    }

    IEnumerator PeopleDie()
    {
        Debug.Log("½Ã¹Î»ç¸Á");
        yield return new WaitForSeconds(1f);
        gm.peopledie = false;
        gm.isGameOver = false;
    }

    public void Realive()
    {
    }
}
