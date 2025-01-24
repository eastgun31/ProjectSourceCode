using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[System.Serializable] //데이터 직렬화
public class SaveData //데이터 저장할 클래스
{
    public float time;  //시간 변수
    public float[] ranking = new float[10] 
                            {99999, 99999, 99999, 99999, 99999,
                             99999, 99999, 99999, 99999, 99999};  //기록 저장할 배열
}
public class ScoreManager : MonoBehaviour
{
    public float timeRecord; //시간 변수
    public List<float> timeRecords; //기록들 저장할 리스트
    public static ScoreManager instance;
    public string savename = "/save"; //저장할 json 이름
    private string SAVEDAT; //저장 경로 변수
    private SaveData saveData = new SaveData();
    public bool isStart;

    private void Awake() //싱글턴 패턴으로 클래스 구현
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(this.gameObject);
        //json 저장 경로
        SAVEDAT = Application.persistentDataPath + "/Save/";
        //저장경로가 있는지 없는지 체크해 없다면 생성
        if (!Directory.Exists(SAVEDAT))  
            Directory.CreateDirectory(SAVEDAT);

        isStart = false;
        LoadData();
    }
    private void Start()
    {
        LoadRank();
    }
    private void Update()
    {
        if (isStart)    //게임이 정지상태가 아니면 시간 흐름
            timeRecord += Time.deltaTime;
    }
    public void RankReset() //랭킹 최신화 함수
    {
        timeRecords.Add(timeRecord); //마지막 기록을 리스트에 추가하고
        timeRecords.Sort();          //리스트 정렬
        //리스트에 최상위 10개의 기록만 존재하게 하고 그외의 나머지 기록들은 제거
        if(timeRecords != null && timeRecords.Count > 10 )
            timeRecords.RemoveAt(timeRecords.Count - 1);
        SaveRank(); //기록저장
    }

    public void SaveRank() //기록 저장 함수
    {
        for(int i  = 0; i < timeRecords.Count; i++)
            saveData.ranking[i] = timeRecords[i];
    }
    public void LoadRank() //기록 불러오기 함수
    {
        for (int i = 0; i < saveData.ranking.Length; i++)
            timeRecords.Add(saveData.ranking[i]);
    }

    public void SaveData()  //json에 현재 기록 저장
    {
        saveData.time = timeRecord;

        string json = JsonUtility.ToJson(saveData);
        string filePath = SAVEDAT + savename;
        File.WriteAllText(filePath, json);
    }

    public void LoadData()  //json에 저장된 기록 불러오기
    {
        string filePath = SAVEDAT + savename;

        if (File.Exists(filePath))
        {
            string localJson = File.ReadAllText(filePath);
            saveData = JsonUtility.FromJson<SaveData>(localJson);

            timeRecord = saveData.time;
        }
    }
    public void StartTime() //시간책정 시작 함수
    {
        timeRecord = 0;
        isStart = true;
    }
    public void EndTime() //시간책정 종료 함수
    {
        isStart = false;    
        SaveData();
    }
}
