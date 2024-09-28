using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyLevel : MonoBehaviour
{
    public enum ELevel //경계레벨 상태머신
    {
        level1, level2, level3
    }
    public ELevel LvStep;

    public static EnemyLevel enemylv;
    public GameObject lv3enemy;
    public GameObject lastenemy;
    public GameObject[] Enemies;

    WaitForSeconds downTime;
    WaitForSeconds upTime;
    GameManager gm;
    private bool addcomplete;
    private bool enemyadd;

    public void Awake()
    {
        if (enemylv != null)
            Destroy(gameObject);
        else
            enemylv = this;

        LvStep = ELevel.level1;
        downTime = new WaitForSeconds(1f);
        upTime = new WaitForSeconds(1f);
        enemyadd = false;
    }

    private void Start()
    {
        gm = GameManager.instance;
        StartCoroutine(LvUp());
        StartCoroutine(LvDown());
    }

    private void Update()
    {
        //if (gm.playerchasing < 0)
        //    gm.playerchasing = 0;
        if (gm.playerchasing != 0 )
            gm.playerchasing -= Time.deltaTime;

    }

    IEnumerator LvUp()
    {
        while (true)
        { 
            if(gm.playerchasing > 20f && !gm.isDie)
            {
                if (LvStep == ELevel.level1)
                    LvStep = ELevel.level2;
                else if(gm.playerchasing >= 40f && LvStep == ELevel.level2)
                {
                    LvStep = ELevel.level3;
                    if(!enemyadd)
                    {
                        enemyadd=true;
                        ODaeGi();
                    }
                }
                else if(LvStep == ELevel.level3)
                    LvStep = ELevel.level3;
                yield return upTime;
            }
            yield return null;
        }
    }
    IEnumerator LvDown()
    {
        while (true)
        {
            if (gm.playerchasing != 0 && LvStep != ELevel.level1)
            {
                if (gm.playerchasing <= 20f && LvStep == ELevel.level2)
                {
                    LvStep = ELevel.level1;
                    gm.playerchasing = 0;
                }
                else if (gm.playerchasing <= 30f && LvStep == ELevel.level3)
                {
                    LvStep = ELevel.level2;
                    lv3enemy.SetActive(false);
                    for (int i = 0; i < lv3enemy.transform.childCount; i++)
                    {
                        lv3enemy.transform.GetChild(i).gameObject.transform.position = lv3enemy.transform.position;
                    }
                    enemyadd = false;
                }
                yield return downTime;
            }
            else if (gm.isDie)
            {
                LvStep = ELevel.level1;
            }
            else if (gm.playerchasing <= 0f && LvStep == ELevel.level1)
             {
                  LvStep = ELevel.level1;
                  gm.playerchasing = 0;
             }
            yield return null;
        }
    }

    public void StateClear()
    {
        gm.playerchasing = 0;
        StopCoroutine(LvUp());
        StopCoroutine(LvDown());
        StartCoroutine(LvUp());
        StartCoroutine(LvDown());
        lv3enemy.SetActive(false);
        for (int i = 0; i < lv3enemy.transform.childCount; i++)
        {
            lv3enemy.transform.GetChild(i).gameObject.transform.position = lv3enemy.transform.position;
        }
        if(gm.scenenum == 1 || gm.scenenum == 2)
        {
            for (int i = 0; i < lastenemy.transform.childCount; i++)
            {
                lastenemy.transform.GetChild(i).gameObject.transform.position = lastenemy.transform.position;
            }

            if (gm.clublast)
                ODaeGi2();
        }
        enemyadd = false;
        LvStep = ELevel.level1;
        gm.playerchasing = 0;
    }

    public void SetEnemy()
    {
        for (int i = 0; i < gm.existEnemy.Length; i++)
        {
            if (gm.existEnemy[i])
            {
                Enemies[i].SetActive(true);
            }
            else if (!gm.existEnemy[i])
            {
                Enemies[i].SetActive(false);
            }
        }
    }

    public void ODaeGi()
    {
        lv3enemy.SetActive(true);
        for (int i = 0; i < lv3enemy.transform.childCount; i++)
        {
            lv3enemy.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void ODaeGi2()
    {
        Debug.Log("111");
        lastenemy.SetActive(true);
        for (int i = 0; i < lastenemy.transform.childCount; i++)
        {
            lastenemy.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void LastEvent()
    {

        if(gm.scenenum == 3 ||  gm.scenenum == 4)
        {
            if (gm.puzzleLevel == 2)
                ODaeGi2();
            else
                return;
        }
        else if(gm.scenenum == 5)
        {
            if(gm.clublast)
            {
                ODaeGi2();
                SetEnemy();
            }
                
        }
    }

}
