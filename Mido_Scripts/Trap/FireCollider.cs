using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//불 함정 콜라이더 재활성화
public class FireCollider : MonoBehaviour
{
    [SerializeField]
    private BoxCollider col;

    WaitForSeconds delay = new WaitForSeconds(5f);
    string player = "Player";

    private void OnEnable()
    {
        col.enabled = true;
    }
    //불오브젝트 콜라이더 딜레이 시간후 재활성화
    IEnumerator ColOnOff()  
    {
        col.enabled = false;
        yield return delay;
        col.enabled = true;
    }
    //플레이어가 불에 닿으면 콜라이더를 비활성화 후 다시 재활성화
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag(player))
        {
            StartCoroutine(ColOnOff());
        }
    }
}
