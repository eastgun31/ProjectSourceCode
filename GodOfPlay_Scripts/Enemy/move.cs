using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public GameObject[] points;
    public List<GameObject> units = new List<GameObject>();
    public GameObject home;
    public GameObject playerhome;
    public E_unitMove[] e_unit;

    public int unitCount;
    int random;
    float time;
    float starttimer = 1.5f;
    float speed = 5f;

    enum EnemyState
    {
        Idle, move, spawn, back, firstMove
    }

    EnemyState enemies;

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < 4; i++)
        //{
        //    transform.GetChild(i).gameObject.SetActive(true);
        //}

        random = Random.Range(0, 5);
        enemies = EnemyState.spawn;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        unitCount = transform.childCount;

        switch (enemies)
        {
            case EnemyState.Idle:
                E_Idle();
                break;
            case EnemyState.move:
                E_Move();
                break;
            case EnemyState.spawn:
                E_Spawn();
                break;
        }
    }

    void E_Idle()
    {
        random = Random.Range(0, 5);

        if (transform.childCount == 0)
        {
            units.Clear();
            enemies = EnemyState.spawn;
        }

        if (time > starttimer)
        {
            time = 0;
            enemies = EnemyState.move;
        }
    }

    void E_firstMove()
    {
        if (transform.childCount == 10)
        {
            gameObject.transform.position =
            Vector3.MoveTowards(transform.position, points[random].transform.position, Time.deltaTime * speed);

            if (gameObject.transform.position == points[random].transform.position)
            {
                enemies = EnemyState.Idle;
                time = 0;
            }
        }
    }

    void E_Move()
    {
        if (GameManager.instance.attacking == true)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                e_unit[i] = transform.GetChild(i).gameObject.GetComponent<E_unitMove>();
                e_unit[i].MovePoint(points[GameManager.instance.attackPoint].transform.position);
            }
            if (gameObject.transform.position == points[GameManager.instance.attackPoint].transform.position)
            {
                GameManager.instance.attacking = false;
                enemies = EnemyState.Idle;
                time = 0;
            }
        }
        else if (GameManager.instance.check[random] == 0 || GameManager.instance.check[random] == 2)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                e_unit[i] = transform.GetChild(i).gameObject.GetComponent<E_unitMove>();
                e_unit[i].MovePoint(points[random].transform.position);
            }
        }
        else if (GameManager.instance.check[0] == 1 && GameManager.instance.check[1] == 1 && GameManager.instance.check[2] == 1 && GameManager.instance.check[3] == 1 && GameManager.instance.check[4] == 1)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                e_unit[i] = transform.GetChild(i).gameObject.GetComponent<E_unitMove>();
                e_unit[i].MovePoint(playerhome.transform.position);
            }
        }
        else
        {
            enemies = EnemyState.Idle;
        }

        if (transform.childCount == 0)
        {
            units.Clear();
            enemies = EnemyState.spawn;
        }
    }

    void E_Spawn()
    {
        if (GameManager.instance.attacking == true && transform.childCount >= 5)
        {
            enemies = EnemyState.move;
        }

        if (transform.childCount >= 6)
        {
            enemies = EnemyState.Idle;
        }
    }

    void E_Back()
    {
        gameObject.transform.position =
        Vector3.MoveTowards(transform.position, home.transform.position, Time.deltaTime * speed);

        if (gameObject.transform.position == home.transform.position)
        {
            enemies = EnemyState.spawn;
        }
    }

}
