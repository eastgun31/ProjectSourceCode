using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;

public class FlashBang : MonoBehaviour, IItem
{
    public int indexNum { get; set; }
    public int value { get; set; }
    Item itemvalues = new Item();
    public int sequence;
    Player player;

    void Start()
    {
        itemvalues.count = 3;
        value = 3;
        //if (sequence == 1)
        indexNum = sequence;
    }


    public void GetItem()
    {
        Debug.Log("¼¶±¤ÅºÈ¹µæ");
        GameManager.instance.itemcheck[2] = true;
        //player = gameObject.GetComponent<Player>();
        //player.itemGet[2] = true;
    }
    public void ItemCharge()
    {
        GameManager.instance.itemcount[2] = itemvalues.count;
    }
}
