using RayFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeEnemy : MonoBehaviour, IDamage
{
    [SerializeField]
    protected Material mat;
    private string hit = "Hit";
    private bool hitnow;
    WaitForSeconds hitdelay = new WaitForSeconds(1f);
    Animator anim;
    int hitcount = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        hitnow = false;
        mat.color = Color.white;
    }

    public void TakeEDamage(int a)
    {
        if(!hitnow)
        {
            anim.SetTrigger(hit);
            StartCoroutine(Hit());
            hitcount ++;

            if(hitcount % 3 == 0)
            {
                ExcellTest.instance.Player_text(62);
                Debug.Log("미로속에 히든방이 있지롱~!");
            }

        }
    }

    protected IEnumerator Hit()
    {
        hitnow = true;
        mat.color = Color.red;
        yield return hitdelay;
        mat.color = Color.white;
        hitnow = false;
    }
}
