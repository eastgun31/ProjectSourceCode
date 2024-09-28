using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public static EnemySpawn instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }


    public float gold = 300;
    public int population;
    int random;
    float spawnCool = 0;

    public GameObject[] unitController;

    public GameObject[] e_unit;

    public int upgradeLV = 0;
    string upgradecheck = "UpgradeCheck";

    // Start is called before the first frame update
    void Start()
    {
        gold = 300;
        InvokeRepeating("Upgold", 1.0f, 1.0f); //1초 후에 1초마다

        StartCoroutine(upgradecheck);
    }

    // Update is called once per frame
    void Update()
    {
        population = GameManager.instance.e_population;

        if (unitController[0].transform.childCount <= 6)
        {
            RandomSpawn(0);
        }

    }

    private void Upgold()
    {
        gold += 5; //재화 2씩 증가
    }

    void RandomSpawn(int i)
    {
        spawnCool += Time.deltaTime;

        if (spawnCool > 1f)
        {
            if (population < 30 && upgradeLV == 0)
            {
                random = Random.Range(0, 4);

                if (random == 0 && gold >= 50)
                {
                    gold -= 50;
                    SpawnUnit(random, i);
                }
                if (random == 1 && gold >= 50)
                {
                    gold -= 50;
                    SpawnUnit(random, i);
                }
                if (random == 2 && gold >= 80)
                {
                    gold -= 80;
                    SpawnUnit(random, i);
                }
                if (random == 3 && gold >= 150)
                {
                    gold -= 150;
                    SpawnUnit(random, i);
                }
            }

            if (population < 30 && upgradeLV == 1)
            {
                random = Random.Range(4, 8);

                if (random == 4 && gold >= 50)
                {
                    gold -= 50;
                    SpawnUnit(random, i);
                }
                if (random == 5 && gold >= 50)
                {
                    gold -= 50;
                    SpawnUnit(random, i);
                }
                if (random == 6 && gold >= 80)
                {
                    gold -= 80;
                    SpawnUnit(random, i);
                }
                if (random == 7 && gold >= 150)
                {
                    gold -= 150;
                    SpawnUnit(random, i);
                }
            }

            if (population < 30 && upgradeLV == 2)
            {
                random = Random.Range(8, 12);

                if (random == 8 && gold >= 50)
                {
                    gold -= 50;
                    SpawnUnit(random, i);
                }
                if (random == 9 && gold >= 50)
                {
                    gold -= 50;
                    SpawnUnit(random, i);
                }
                if (random == 10 && gold >= 80)
                {
                    gold -= 80;
                    SpawnUnit(random, i);
                }
                if (random == 11 && gold >= 150)
                {
                    gold -= 150;
                    SpawnUnit(random, i);
                }
            }



            spawnCool = 0;
        }
    }

    void SpawnUnit(int i, int j)
    {
        GameObject enemy = Instantiate(e_unit[i], this.transform.position, Quaternion.identity);
        enemy.transform.parent = unitController[j].transform;
        GameManager.instance.e_population++;
    }

    IEnumerator UpgradeCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        if (upgradeLV == 2)
        {
            StopCoroutine(upgradecheck);
        }

        if (gold >= 350 && upgradeLV == 0)
        {
            gold -= 350;
            E_Upgrade();
        }

        if (gold >= 600 && upgradeLV == 1)
        {
            gold -= 600;
            E_Upgrade();
        }

        yield return wait;

        StartCoroutine(upgradecheck);
    }

    void E_Upgrade()
    {
        upgradeLV++;
    }

    public void Aphrodite_Warrior(Vector3 spawnPoint, Vector3 pointPosition)
    {
        GameObject newObject = Instantiate(e_unit[0], spawnPoint, Quaternion.identity);
        E_unitMove e_newunit = newObject.GetComponent<E_unitMove>();
        e_newunit.lastDesti = pointPosition;
        e_newunit.e_State = E_unitMove.E_UnitState.Battle;
        GameManager.instance.e_population++;
        GameManager.instance.All_Obj--;
        GameManager.instance.Aobj();
    }
    public void Aphrodite_Shield(Vector3 spawnPoint, Vector3 pointPosition)
    {
        GameObject newObject = Instantiate(e_unit[1], spawnPoint, Quaternion.identity);
        E_unitMove e_newunit = newObject.GetComponent<E_unitMove>();
        e_newunit.lastDesti = pointPosition;
        e_newunit.e_State = E_unitMove.E_UnitState.Battle;
        GameManager.instance.e_population++;
        GameManager.instance.All_Obj--;
        GameManager.instance.Aobj();
    }
    public void Aphrodite_Archer(Vector3 spawnPoint, Vector3 pointPosition)
    {
        GameObject newObject = Instantiate(e_unit[2], spawnPoint, Quaternion.identity);
        E_unitMove e_newunit = newObject.GetComponent<E_unitMove>();
        e_newunit.lastDesti = pointPosition;
        e_newunit.e_State = E_unitMove.E_UnitState.Battle;
        GameManager.instance.e_population++;
        GameManager.instance.All_Obj--;
        GameManager.instance.Aobj();
    }
    public void Aphrodite_HorseMan(Vector3 spawnPoint, Vector3 pointPosition)
    {
        GameObject newObject = Instantiate(e_unit[3], spawnPoint, Quaternion.identity);
        E_unitMove e_newunit = newObject.GetComponent<E_unitMove>();
        e_newunit.lastDesti = pointPosition;
        e_newunit.e_State = E_unitMove.E_UnitState.Battle;
        GameManager.instance.e_population++;
        GameManager.instance.All_Obj--;
        GameManager.instance.Aobj();
    }

}
