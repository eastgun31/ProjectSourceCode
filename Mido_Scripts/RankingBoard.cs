using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingBoard : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] rankings = new TMP_Text[10]; //기록 표시할 텍스트 배열

    public void RankToString()  //랭킹보드에 기록 표시
    {
        for(int i = 0; i < rankings.Length; i++)
        {   //TimeSpan으로 저장된 시간을 분과 초로 나누고 문자열로 변환
            string t = TimeSpan.FromSeconds(ScoreManager.instance.timeRecords[i]).ToString("mm\\:ss"); 
            string[] strings = t.Split(':');
            //기록 초기값이면 빈칸으로 표시하고 아니면 기록 표시
            if (ScoreManager.instance.timeRecords[i] == 99999)   
            {
                rankings[i].text = "-" + "             --분 --초";
            }
            else
            {
                if (i == 0)
                    rankings[i].text = (i + 1) + "              " + strings[0] + "분 " + strings[1] + "초";
                else if (i == 9)
                    rankings[i].text = (i + 1) + "           " + strings[0] + "분 " + strings[1] + "초";
                else
                    rankings[i].text = (i + 1) + "             " + strings[0] + "분 " + strings[1] + "초";
            }
        }
    }
}
