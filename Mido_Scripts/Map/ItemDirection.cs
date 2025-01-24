using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDirection : LooKClass
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    public BoxOpen boxState;
    bool firsttime = true;

    private void OnEnable()
    {
        if(!firsttime)
        {
            if (boxState.currentState != BoxOpen.BoxState.Closed)
            {
                Debug.Log(boxState.currentState);
                gameObject.SetActive(false);
            }
                
        }
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        boxState = target.GetComponent<BoxOpen>();
        firsttime = false;
    }

    void Update()
    {
        LookTarget(target);
    }
}
