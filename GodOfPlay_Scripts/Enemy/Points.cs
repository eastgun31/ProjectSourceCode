using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    public int value;   //Á¡·ÉÁö ³Ñ¹ö¸µ ABCDE Â÷·Ê´ë·Î 01234
    public int pointcheck; //Áß¸³»óÅÂ 0 ,ÀûÁ¡·É½Ã 1, ÇÃ·¹ÀÌ¾î°¡ Á¡·É½Ã 2
    public float time;

    public GameObject playerhome;
    public List<E_unitMove> e_unit = new List<E_unitMove>();
    public List<UnitController> p_unit = new List<UnitController>();
    public GameObject[] points;

    public float p_distance; //¹üÀ§³» ÇÃ·¹ÀÌ¾îÀ¯´Ö
    public float e_distance; //¹üÀ§³» ÀûÀ¯´Ö

    int random;

    private int ppoint = 0;

    string enemy = "Enemy";
    string player = "Player";

    public GameObject GobjWhite;
    public GameObject GobjBlue;
    public GameObject GobjRed;

    public GameObject GetPoint; //Æ÷ÀÎÆ® ÀÌÆåÆ®
    capture Capture;
    mini_capture Mini_capture;
    // Start is called before the first frame update
    void Start()
    {
        p_distance = 100f;
        e_distance = 100f;

        SetGameObjectActive(GobjRed, false);
        SetGameObjectActive(GobjWhite, true);
        SetGameObjectActive(GobjBlue, false);

        StartCoroutine(PointCheckCoroutine());
        Capture = FindAnyObjectByType<capture>();
        Mini_capture = FindAnyObjectByType<mini_capture>();
    }

    // Update is called once per frame
    private IEnumerator PointCheckCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        while (true)
        {
            if (pointcheck != ppoint)
            {
                if (pointcheck == 1)
                {
                    SetGameObjectActive(GobjRed, true);
                    SetGameObjectActive(GobjWhite, false);
                    SetGameObjectActive(GobjBlue, false);


                    if (ppoint == 0)
                    {
                        EnemySpawn.instance.gold += 250;
                        GameManager.instance.e_score++;
                    }
                    else if(ppoint == 2)
                    {
                        EnemySpawn.instance.gold += 150;
                        GameManager.instance.e_score++;
                        GameManager.instance.p_score--;
                        GameManager.instance.attacking = false;
                    }
                }
                else if (pointcheck == 2) //pointcheck°¡ 0ÀÌ¾ú´Ù°¡ 2°¡ µÇ¸é ÀçÈ­ È¹µæ
                {
                    if (ppoint == 0)
                    {
                        GameManager.instance.gold += 250;
                        GameManager.instance.p_score++;
                    }
                    else if (ppoint == 1)
                    {
                        GameManager.instance.gold += 150;
                        GameManager.instance.p_score++;
                        GameManager.instance.e_score--;
                    }
                    SetGameObjectActive(GobjRed, false);
                    SetGameObjectActive(GobjWhite, false);
                    SetGameObjectActive(GobjBlue, true);
                }
            }

            ppoint = pointcheck;
            yield return wait; // ´ÙÀ½ ÇÁ·¹ÀÓ±îÁö ´ë±â -> 1ÃÊ·Î ¹Ù²Þ
        }
    }

    void SetGameObjectActive(GameObject gameObject, bool isActive)
    {
        {
            if (gameObject != null)
                gameObject.SetActive(isActive);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemy))
        {
            e_unit.Add(other.gameObject.GetComponent<E_unitMove>());

            StartCoroutine("eUnitNewPoint", other.gameObject);
            random = Random.Range(0, 5);
        }

        if (other.CompareTag(player))
        {
            p_unit.Add(other.gameObject.GetComponent<UnitController>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if(other.tag == "Enemy" && other.tag == "Player")
        //{
        //    pointcheck = 0;
        //}

        if (other.CompareTag(enemy)) //Àû±º Á¡·ÉÁö¿¡ ÀÖ´ÂÁö Ã¼Å©
        {
            e_distance = Vector3.Distance(other.transform.position, transform.position);

            for (int i = 0; i < e_unit.Count; i++)
            {
                if (e_unit[i].ehealth <= 0)
                {
                    e_unit.Remove(e_unit[i]);
                }
            }

        }
        if (other.CompareTag(player)) //¾Æ±º Á¡·ÉÁö¿¡ ÀÖ´ÂÁö Ã¼Å©
        {
            p_distance = Vector3.Distance(other.transform.position, transform.position);

            for (int i = 0; i < p_unit.Count; i++)
            {
                if (p_unit[i].uhealth <= 0)
                {
                    p_unit.Remove(p_unit[i]);
                }
            }
        }

        if (e_distance <= 10f && p_distance > 10f) //Àû Á¡·É½Ã
        {
            time += Time.deltaTime;

            if (time > 15f)
            {
                pointcheck = 1;

                switch (value)
                {
                    case 0:
                        GameManager.instance.check[0] = pointcheck;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.A_Blue.SetActive(false);
                        Capture.A_Red.SetActive(true);
                        Capture.A_3.SetActive(false);

                        Mini_capture.A_Blue.SetActive(false);
                        Mini_capture.A_Red.SetActive(true);
                        Mini_capture.A_3.SetActive(false);
                        #endregion
                        break;
                    case 1:
                        GameManager.instance.check[1] = pointcheck;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.B_Blue.SetActive(false);
                        Capture.B_Red.SetActive(true);
                        Capture.B_3.SetActive(false);

                        Mini_capture.B_Blue.SetActive(false);
                        Mini_capture.B_Red.SetActive(true);
                        Mini_capture.B_3.SetActive(false);
                        #endregion
                        break;
                    case 2:
                        GameManager.instance.check[2] = pointcheck;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.C_Blue.SetActive(false);
                        Capture.C_Red.SetActive(true);
                        Capture.C_3.SetActive(false);

                        Mini_capture.C_Blue.SetActive(false);
                        Mini_capture.C_Red.SetActive(true);
                        Mini_capture.C_3.SetActive(false);
                        #endregion
                        break;
                    case 3:
                        GameManager.instance.check[3] = pointcheck;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.D_Blue.SetActive(false);
                        Capture.D_Red.SetActive(true);
                        Capture.D_3.SetActive(false);

                        Mini_capture.D_Blue.SetActive(false);
                        Mini_capture.D_Red.SetActive(true);
                        Mini_capture.D_3.SetActive(false);
                        #endregion
                        break;
                    case 4:
                        GameManager.instance.check[4] = pointcheck;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.E_Blue.SetActive(false);
                        Capture.E_Red.SetActive(true);
                        Capture.E_3.SetActive(false);

                        Mini_capture.E_Blue.SetActive(false);
                        Mini_capture.E_Red.SetActive(true);
                        Mini_capture.E_3.SetActive(false);
                        #endregion
                        break;
                }

                time = 0;
            }
        }

        if (p_distance <= 10f && e_distance > 10f)
        {
            time += Time.deltaTime;

            if (time > 15f) //ÇÃ·¹ÀÌ¾î°¡ Á¡·É½Ã
            {
                time = 0;
                pointcheck = 2;

                switch (value)
                {
                    case 0:
                        GameManager.instance.check[0] = pointcheck;
                        GameManager.instance.attacking = false;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.A_Blue.SetActive(true);
                        Capture.A_Red.SetActive(false);
                        Capture.A_3.SetActive(false);

                        Mini_capture.A_Blue.SetActive(true);
                        Mini_capture.A_Red.SetActive(false);
                        Mini_capture.A_3.SetActive(false);
                        #endregion
                        break;
                    case 1:
                        GameManager.instance.check[1] = pointcheck;
                        GameManager.instance.attacking = false;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.B_Blue.SetActive(true);
                        Capture.B_Red.SetActive(false);
                        Capture.B_3.SetActive(false);

                        Mini_capture.B_Blue.SetActive(true);
                        Mini_capture.B_Red.SetActive(false);
                        Mini_capture.B_3.SetActive(false);
                        #endregion
                        break;
                    case 2:
                        GameManager.instance.check[2] = pointcheck;
                        GameManager.instance.attacking = false;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.C_Blue.SetActive(true);
                        Capture.C_Red.SetActive(false);
                        Capture.C_3.SetActive(false);
                        
                        Mini_capture.C_Blue.SetActive(true);
                        Mini_capture.C_Red.SetActive(false);
                        Mini_capture.C_3.SetActive(false);
                        #endregion
                        break;
                    case 3:
                        GameManager.instance.check[3] = pointcheck;
                        GameManager.instance.attacking = false;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.D_Blue.SetActive(true);
                        Capture.D_Red.SetActive(false);
                        Capture.D_3.SetActive(false);

                        Mini_capture.D_Blue.SetActive(true);
                        Mini_capture.D_Red.SetActive(false);
                        Mini_capture.D_3.SetActive(false);
                        #endregion
                        break;
                    case 4:
                        GameManager.instance.check[4] = pointcheck;
                        GameManager.instance.attacking = false;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.E_Blue.SetActive(true);
                        Capture.E_Red.SetActive(false);
                        Capture.E_3.SetActive(false);

                        Mini_capture.E_Blue.SetActive(true);
                        Mini_capture.E_Red.SetActive(false);
                        Mini_capture.E_3.SetActive(false);
                        #endregion
                        break;
                }
            }
        }

        if (pointcheck == 1 && p_distance <= 10f) //ÇÃ·¹ÀÌ¾î°¡ Àû °ÅÁ¡ »¯¾úÀ»½Ã
        {
            GameManager.instance.attacking = true;

            switch (value)
            {
                case 0:
                    GameManager.instance.attackPoint = 0;
                    #region Capture ¸Ê & ¹Ì´Ï¸Ê
                    Capture.A_Blue.SetActive(false);
                    Capture.A_Red.SetActive(true);
                    Capture.A_3.SetActive(false);

                    Mini_capture.A_Blue.SetActive(false);
                    Mini_capture.A_Red.SetActive(true);
                    Mini_capture.A_3.SetActive(false);
                    #endregion
                    break;
                case 1:
                    GameManager.instance.attackPoint = 1;
                    #region Capture ¸Ê & ¹Ì´Ï¸Ê
                    Capture.B_Blue.SetActive(false);
                    Capture.B_Red.SetActive(true);
                    Capture.B_3.SetActive(false);

                    Mini_capture.B_Blue.SetActive(false);
                    Mini_capture.B_Red.SetActive(true);
                    Mini_capture.B_3.SetActive(false);
                    #endregion
                    break;
                case 2:
                    GameManager.instance.attackPoint = 2;
                    #region Capture ¸Ê & ¹Ì´Ï¸Ê
                    Capture.C_Blue.SetActive(false);
                    Capture.C_Red.SetActive(true);
                    Capture.C_3.SetActive(false);

                    Mini_capture.C_Blue.SetActive(false);
                    Mini_capture.C_Red.SetActive(true);
                    Mini_capture.C_3.SetActive(false);
                    #endregion
                    break;
                case 3:
                    GameManager.instance.attackPoint = 3;
                    #region Capture ¸Ê & ¹Ì´Ï¸Ê
                    Capture.D_Blue.SetActive(false);
                    Capture.D_Red.SetActive(true);
                    Capture.D_3.SetActive(false);

                    Mini_capture.D_Blue.SetActive(false);
                    Mini_capture.D_Red.SetActive(true);
                    Mini_capture.D_3.SetActive(false);
                    #endregion
                    break;
                case 4:
                    GameManager.instance.attackPoint = 4;
                    #region Capture ¸Ê & ¹Ì´Ï¸Ê
                    Capture.E_Blue.SetActive(false);
                    Capture.E_Red.SetActive(true);
                    Capture.E_3.SetActive(false);

                    Mini_capture.E_Blue.SetActive(false);
                    Mini_capture.E_Red.SetActive(true);
                    Mini_capture.E_3.SetActive(false);
                    #endregion
                    break;
            }
        }


        if (e_distance <= 10f && p_distance <= 10f) //°ÅÁ¡¿¡¼­ ÀüÅõ½Ã
        {
            time += Time.deltaTime;

            if (time > 10f)
            {
                pointcheck = 3;

                switch (value)
                {
                    case 0:
                        GameManager.instance.check[0] = pointcheck;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.A_Red.SetActive(false);
                        Capture.A_Blue.SetActive(false);
                        Capture.A_3.SetActive(true);

                        Mini_capture.A_Red.SetActive(false);
                        Mini_capture.A_Blue.SetActive(false);
                        Mini_capture.A_3.SetActive(true);
                        #endregion
                        break;
                    case 1:
                        GameManager.instance.check[1] = pointcheck;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.B_Red.SetActive(false);
                        Capture.B_Blue.SetActive(false);
                        Capture.B_3.SetActive(true);

                        Mini_capture.B_Red.SetActive(false);
                        Mini_capture.B_Blue.SetActive(false);
                        Mini_capture.B_3.SetActive(true);
                        #endregion
                        break;
                    case 2:
                        GameManager.instance.check[2] = pointcheck;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.C_Red.SetActive(false);
                        Capture.C_Blue.SetActive(false);
                        Capture.C_3.SetActive(true);

                        Mini_capture.C_Red.SetActive(false);
                        Mini_capture.C_Blue.SetActive(false);
                        Mini_capture.C_3.SetActive(true);
                        #endregion
                        break;
                    case 3:
                        GameManager.instance.check[3] = pointcheck;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.D_Red.SetActive(false);
                        Capture.D_Blue.SetActive(false);
                        Capture.D_3.SetActive(true);

                        Mini_capture.D_Red.SetActive(false);
                        Mini_capture.D_Blue.SetActive(false);
                        Mini_capture.D_3.SetActive(true);
                        #endregion
                        break;
                    case 4:
                        GameManager.instance.check[4] = pointcheck;
                        #region Capture ¸Ê & ¹Ì´Ï¸Ê
                        Capture.E_Red.SetActive(false);
                        Capture.E_Blue.SetActive(false);
                        Capture.E_3.SetActive(true);

                        Mini_capture.E_Red.SetActive(false);
                        Mini_capture.E_Blue.SetActive(false);
                        Mini_capture.E_3.SetActive(true);
                        #endregion
                        break;
                }

                time = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemy))
        {
            time = 0;
            e_distance = 100f;
        }

        if (other.CompareTag(player))
        {
            time = 0;
            GameManager.instance.attacking = false;
            p_distance = 100f;
        }
    }

    IEnumerator eUnitNewPoint(GameObject other) //Àû±º »õ·Î¿î ¸ñÇ¥ÁöÁ¤
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        if (pointcheck == 1)
        {
            if (GameManager.instance.attacking == true)
            {
                for (int i = 0; i < e_unit.Count; i++)
                {
                    e_unit[i].MovePoint(points[GameManager.instance.attackPoint].transform.position);
                    e_unit.Remove(e_unit[i]);
                }
            }
            else if (GameManager.instance.check[random] == 0 || GameManager.instance.check[random] == 2)
            {
                for (int i = 3; i < e_unit.Count; i++)
                {
                    e_unit[i].MovePoint(points[random].transform.position);
                    e_unit.Remove(e_unit[i]);
                }
            }
            else if (GameManager.instance.check[0] == 1 && GameManager.instance.check[1] == 1 && GameManager.instance.check[2] == 1 && GameManager.instance.check[3] == 1 && GameManager.instance.check[4] == 1)
            {
                for (int i = 0; i < e_unit.Count; i++)
                {
                    e_unit[i].MovePoint(playerhome.transform.position);
                    e_unit.Remove(e_unit[i]);
                }
            }
        }

        yield return wait;

        StartCoroutine("eUnitNewPoint", other.gameObject);
    }

    //IEnumerator DeactiveEffect(GameObject effect)
    //{
    //    WaitForSeconds wait = new WaitForSeconds(2f);

    //    yield return wait;

    //    GetPoint.SetActive(false);
    //}
}
