using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public Vector3 target;
    public List<GameObject> targets = new List<GameObject>();

    public UnitController p_unit;
    public E_unitMove parent;

    string player = "Player";

    private void OnEnable()
    {
        parent = transform.GetComponentInParent<E_unitMove>();

        //StartCoroutine("Find_Target");
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(player))
        {
            StartCoroutine("Find_Target");
            targets.Add(col.gameObject);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag(player))
        {
            targets.Remove(col.gameObject);
            p_unit = null;

            if (targets == null)
            {
                StopCoroutine("Find_Target");
                parent.e_State = E_unitMove.E_UnitState.Idle;
            }
        }
    }

    IEnumerator Find_Target()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        if (targets != null)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                target = targets[i].transform.position;
                p_unit = targets[i].GetComponent<UnitController>();
                if (p_unit.uhealth > 0 && parent.ehealth > 0)
                {
                    parent.Attakc(target, p_unit);
                    parent.e_State = E_unitMove.E_UnitState.Battle;
                }
                if (p_unit.uhealth <= 0)
                {
                    p_unit = null;
                    targets.Remove(targets[i]);
                    parent.e_State = E_unitMove.E_UnitState.Idle;
                }
            }
        }

        yield return wait;

        StartCoroutine("Find_Target");
    }


}
