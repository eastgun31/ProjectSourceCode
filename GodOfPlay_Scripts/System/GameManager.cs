using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject win;  //승리화면
    public GameObject lose; //패배화면
    public GameObject draw; //무승부화면

    //유닛스탯
    public int health = 100;
    public int attackPower = 10;
    public int defense = 3;
    public float moveSpeed = 7;

    public Text Allgold_text; //현재 재화 상태
    public int gold = 300; //초기 재화 + 

    public Text All_ObjText;//유닛 수 보여주는 텍스트
    public int All_Obj = 0;//현재 총 유닛 수

    public int[] check = { 0, 0, 0, 0, 0 };   //거점, 0 중립, 1 적이점령, 2플레이어, 3거점에서 전투

    public bool attacking = false;  //적이 공격받는상태
    public int attackPoint = 0;     //적이 공격받는 거점
    public int e_population = 0;    //적유닛수

    public bool pointBattle = false; //전투상태
    public bool battle = false;      //전투상태

    public bool pointCan = false;

    public int p_score = 0;
    public int e_score = 0;

    //게임 시간
    public Text timerText;
    private float startTime;
    private float currentTime = 181.0f;

    //스킬 쿨타임
    public Text active_Skill;
    public Text buff_Skill;
    //소모 스킬 남은 횟수
    public Text item_Skill;

    public void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);

        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        Aobj();
        e_population = 0;
        startTime = currentTime;
        active_Skill.gameObject.SetActive(false);
        buff_Skill.gameObject.SetActive(false);

        gold = 300;

        InvokeRepeating("Upgold", 1.0f, 1.0f); //1초 후에 1초마다
    }

    private void Update()
    {
        //시연용 시간정지 코드 추후 지울것
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Time.timeScale == 0)
                Time.timeScale = 1;
            else
                Time.timeScale = 0;
        }

        Allgold_text.text = " " + gold; //현재 재화

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            Time.timeScale = 0;
            currentTime = 0;
            // 타이머가 끝났을 때 게임종료 추후에 여기 추가
            int a = 0;
            int b = 0;
            for (int i = 0; i < 5; i++)
            {
                if (check[i] == 1)
                    a++;
                if (check[i] == 2)
                    b++;
            }

            if (a > b)
            {
                Debug.Log("패배");
                lose.SetActive(true);
            }
            else if (a < b)
            {
                Debug.Log("승리");
                win.SetActive(true);
            }
            else if (a == b)
            {
                if (e_score > p_score)
                {
                    Debug.Log("패배");
                    lose.SetActive(true);
                }
                else if (e_score < p_score)
                {
                    Debug.Log("승리");
                    win.SetActive(true);
                }
            }

        }

        // 시간을 분:초 형식으로 표시
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        SkillCool_UI();
        ItemSkill_UI();
    }

    private void Upgold()
    {
        gold += 5; //재화 2씩 증가
    }

    public void Aobj()
    {
        All_ObjText.text = All_Obj.ToString() + " / 30"; //현재 유닛
    }

    public void SkillCool_UI()
    {
        if (Skill.instance.currentCooldown_1 > 0.0f)
        {
            active_Skill.gameObject.SetActive(true);
            active_Skill.text = " " + Skill.instance.currentCooldown_1.ToString("0");
        }
        else
            active_Skill.gameObject.SetActive(false);

        if (Skill.instance.currentCooldown_2 > 0.0f)
        {
            buff_Skill.gameObject.SetActive(true);
            buff_Skill.text = " " + Skill.instance.currentCooldown_2.ToString("0");
        }
        else
            buff_Skill.gameObject.SetActive(false);
    }

    public void ItemSkill_UI()
    {
        if (Skill.instance.itemLimit > 0)
        {
            item_Skill.text = " " + Skill.instance.itemLimit.ToString("0");
        }
        else
        {
            // 아이템 리미트가 0 이하인 경우 텍스트를 빨간색으로 변경
            item_Skill.text = " " + Skill.instance.itemLimit.ToString("0");
            item_Skill.color = Color.red;
        }
    }
}
