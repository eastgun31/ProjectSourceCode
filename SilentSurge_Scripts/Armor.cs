using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;

public class Armor : MonoBehaviour, IItem
{
    public int indexNum { get; set; }
    public int value { get; set; }
    Item itemvalues = new Item();
    public int sequence;
    void Start()
    {
        value = 5;
        itemvalues.count = 3;
        //if (sequence == 1)
        indexNum = sequence;
    }

    public void GetItem()
    {
        Debug.Log("πÊ≈∫∫π»πµÊ");
        GameManager.instance.itemcheck[4] = true;
        //player = gameObject.GetComponent<Player>();
        //player.itemGet[0] = true;
    }
    public void ItemCharge()
    {
        GameManager.instance.itemcount[4] = itemvalues.count;
    }
}
