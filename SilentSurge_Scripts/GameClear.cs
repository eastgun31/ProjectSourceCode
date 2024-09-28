using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using ItemInfo;

public class GameClear : MonoBehaviour
{
    [SerializeField]
    private GameObject clear;
    [SerializeField]
    private GameObject goal;
    [SerializeField]
    private GameObject enemyFootSound;
    [SerializeField]
    private List<AudioSource> eFootSounds;
    public int value;
    public UnityEvent lastAction;
    public UnityEvent Ending;
    bool activewan;

    public List<GameObject> enemys;
    public GameObject items;
    CoolTime cool;
    Casing cas;
    //GameManager gm;

    private void Start()
    {
        activewan = true;
        cool = new CoolTime();
        if(value == 2)
        {
            StartCoroutine(EndingCheck());
        }
        //gm = GameManager.instance;
        cas = new Casing();
    }

    IEnumerator EndingCheck()
    {
        while (true)
        {
            yield return cool.cool1sec;

            foreach (GameObject enemy in enemys)
            {
                if (cas.gm.last && !enemy.activeSelf)
                {
                    enemys.Remove(enemy);
                    break;
                }

            }

            if(enemys.FirstOrDefault() == null && activewan)
            {
                goal.SetActive(true);
                items.SetActive(true);

            }
        }
    }

    void EnemySoundOff()
    {
        cas.sm.enemyPlayer.Stop();
        enemyFootSound.SetActive(false);

        foreach (AudioSource enemy in eFootSounds)
            enemy.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && value == 1)
        {
            if (cas.gm.scenenum == 1 && cas.gm.clublast)
            {
                cas.sm.EffectOff();
                EnemySoundOff();
                clear.SetActive(true);
                cas.gm.isGameOver = true;
                cas.sm.stage1Clear = true;
            }
                

            else if (cas.gm.scenenum == 3)
            {
                if (cas.gm.rescueHostage)
                {
                    cas.sm.EffectOff();
                    EnemySoundOff();
                    clear.SetActive(true);
                    cas.gm.isGameOver = true;
                    cas.sm.stage2Clear = true;
                }
                else
                    return;
            }
        }
        else if(other.CompareTag("Player") && value == 2 && goal.activeSelf)
        {
            activewan = false;
            goal.SetActive(false);
            cas.gm.clublast = true;
            lastAction.Invoke();


            if (cas.gm.scenenum == 5)
            {
                cas.gm.people.SetActive(false);
            }
            
        }
        else if(other.CompareTag("Player") && value == 3)
        {
            if(cas.gm.scenenum == 2 && cas.gm.clublast)
            {
                cas.sm.stage1Clear = true;
                Ending.Invoke();
            }
            else if(cas.gm.scenenum == 4)
            {
                if (cas.gm.rescueHostage)
                {
                    cas.sm.stage2Clear = true;
                    Ending.Invoke();
                }
                else
                    return;
            }
            else if(cas.gm.scenenum == 5)
            {
                if(cas.gm.clublast)
                    Ending.Invoke();
            }
        }
    }

}
