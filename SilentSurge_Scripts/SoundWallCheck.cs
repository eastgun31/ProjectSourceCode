using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWallCheck : MonoBehaviour
{
    LayerMask mask;
    RaycastHit hit;
    WaitForSeconds wait;

    public bool canhear;
    public string floor = "Floor";
    public string room = "InRoom";
    
    private void Start()
    {
        mask = LayerMask.GetMask(floor) | LayerMask.GetMask(room);
        wait = new WaitForSeconds(0.5f);
    }

    private void OnEnable()
    {
        StartCoroutine(SoundPosCheck());
    }

    IEnumerator SoundPosCheck()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0), Vector3.down, out hit, 2f, mask))
        {
            //Debug.Log(hit.collider.gameObject.layer);
            if (hit.collider.gameObject.layer == 6)
            {
                //Debug.Log("적이 들음");
                canhear = true;
            }
            else if (hit.collider.gameObject.layer == 9)
            {
                //Debug.Log("벽에 소리 막힘");
                canhear = false;
            }
        }

        yield return wait;
        StartCoroutine(SoundPosCheck());
    }

}
