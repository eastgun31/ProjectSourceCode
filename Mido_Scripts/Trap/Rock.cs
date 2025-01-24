using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//바위 오브젝트 클래스
public class Rock : MonoBehaviour
{
    Rigidbody rigid;
    string ground = "Ground";
    string water = "Water";
    public float speed;

    void Start()
    {
        speed = 20f;
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(Vector3.down * speed, ForceMode.Impulse);    //생성위치에서 아래로 힘을 가한다
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ground))   //땅에 충돌시 오브젝트 삭제
            Destroy(gameObject,1f);

        if(other.CompareTag(water)) //물에 떨어질시 속도를 줄여서 힘을 가한다.
        {
            if(!GameManager.instance.deepwater)
                SoundManager.instance.Effect_paly(17);

            rigid.velocity = Vector3.zero;
            rigid.AddForce(Vector3.down * (speed - 17f), ForceMode.Impulse);
        }
    }
}
