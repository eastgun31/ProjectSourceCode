using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill : MonoBehaviour
{
    public string e_active_s;
    public string e_passive_s;
    public string e_buff_s;
    public string e_item_s;


    private void OnEnable()
    {
        bool[] actives = { EnemySkillManager.instance.e_Zeus_S, EnemySkillManager.instance.e_Poseidon_S, 
                           EnemySkillManager.instance.e_Hades_S };
        bool[] passives = { EnemySkillManager.instance.e_Hephaestus_S, EnemySkillManager.instance.e_Artemis_S, 
                           EnemySkillManager.instance.e_Ares_S };
        bool[] buffes = { EnemySkillManager.instance.e_Hera_S, EnemySkillManager.instance.e_Apollo_S, 
                           EnemySkillManager.instance.e_Athena_S, EnemySkillManager.instance.e_Aphrodite_S };
        bool[] items = { EnemySkillManager.instance.e_Hermes_S, EnemySkillManager.instance.e_Hestia_S, 
                           EnemySkillManager.instance.e_Dionysus_S, EnemySkillManager.instance.e_Demeter_S  };

        for (int i = 0; i < 3; i++)
        {
            if (actives[i] == true)
            {
                if (i == 0)
                {
                    e_active_s = "제우스";
                    EnemySkillManager.instance.e_active_skillnum = 1;
                }
                else if (i == 1)
                {
                    e_active_s = "포세이돈";
                    EnemySkillManager.instance.e_active_skillnum = 2;
                }
                else
                {
                    e_active_s = "하데스";
                    EnemySkillManager.instance.e_active_skillnum = 3;
                }
            }

            if (passives[i] == true)
            {
                if (i == 0)
                {
                    e_passive_s = "헤파이토스";
                    EnemySkillManager.instance.e_passiveNow = 1;
                }
                else if (i == 1)
                {
                    e_passive_s = "아르테미스";
                    EnemySkillManager.instance.e_passiveNow = 2;
                }
                else
                {
                    e_passive_s = "아레스";
                    EnemySkillManager.instance.e_passiveNow = 3;
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (buffes[i] == true)
            {
                if (i == 0)
                {
                    e_buff_s = "헤라";
                    EnemySkillManager.instance.e_buff_skillnum = 1;
                }
                else if (i == 1)
                {
                    e_buff_s = "아폴로";
                    EnemySkillManager.instance.e_buff_skillnum = 2;
                }
                else if (i == 2)
                {
                    e_buff_s = "아테나";
                    EnemySkillManager.instance.e_buff_skillnum = 3;
                }
                else
                {
                    e_buff_s = "아프로디테";
                    EnemySkillManager.instance.e_buff_skillnum = 4;
                }
            }

            if (items[i] == true)
            {
                if (i == 0)
                {
                    e_item_s = "헤르메스";
                    EnemySkillManager.instance.e_item_skillnum = 1;
                }
                else if (i == 1)
                {
                    e_item_s = "헤스티아";
                    EnemySkillManager.instance.e_item_skillnum = 2;
                }
                else if (i == 2)
                {
                    e_item_s = "디오니소스";
                    EnemySkillManager.instance.e_item_skillnum = 3;
                }
                else
                {
                    e_item_s = "데메테르";
                    EnemySkillManager.instance.e_item_skillnum = 4;
                }
            }
        }

    }
}
