using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Record : MonoBehaviour
{
    [SerializeField]
    private TMP_Text record;

    void Start()
    {
        TimeToString();
        ScoreManager.instance.RankReset();
        ScoreManager.instance.SaveData();
    }

    void TimeToString()
    {
        string t = TimeSpan.FromSeconds(ScoreManager.instance.timeRecord).ToString("mm\\:ss");
        string[] strings = t.Split(':');

        record.text = "탈출 시간 : " + strings[0]+"분 "+strings[1]+"초";
    }

}
