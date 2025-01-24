using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

//벽 페인팅 클래스
public class WallPaint : MonoBehaviour
{
    [SerializeField]
    public GameObject paints;   //페인트 저장할 부모 오브젝트
    [SerializeField]
    private GameObject paintprefab; //페인트 프리펩

    public WaitForSeconds paintcool = new WaitForSeconds(5f);

    string wall = "Block";
    RaycastHit hit;
    Vector3 rayheight = new Vector3(0, 0.5f, 0);
    float hitoffset = 0.01f;
    public bool canPaint = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // 플레이어 앞에 벽이 있으면 레이캐스트 발사
            if (Physics.Raycast(transform.position + rayheight, transform.forward, out hit, 1f, LayerMask.GetMask(wall)))
            {
                if (canPaint)   //쿨타임 아니라면 벽에 페인트를 그린다
                {
                    DrawPaint(hit);
                    StartCoroutine(PaintCool());
                }

            }
        }
    }

    void DrawPaint(RaycastHit hit)  //벽에 페인트 그리는 함수
    {
        Vector3 paintPosition = hit.point + hit.normal * hitoffset;
        Quaternion rotation = Quaternion.Euler(90, Quaternion.LookRotation(hit.normal).eulerAngles.y, 0); //페인트 회전각 설정
        GameObject wallPaint = Instantiate(paintprefab, paintPosition, rotation); //페인트 생성
        wallPaint.transform.SetParent(paints.transform, false);
    }

    IEnumerator PaintCool() //페인팅 쿨타임 코루틴 함수
    {
        canPaint = false;
        yield return paintcool;
        canPaint = true;
    }

    public void ResetPaint()    //게임 재시작시 그려져있던 페인트들 초기화
    {
        int paintsCount = paints.transform.childCount;

        if (paintsCount > 0)
        {
            for (int i = 0; i < paintsCount; i++)
            {
                Destroy(paints.transform.GetChild(i).gameObject);
            }
        }
    }
}
