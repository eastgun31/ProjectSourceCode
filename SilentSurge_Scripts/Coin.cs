using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using Unity.VisualScripting;

public class Coin : MonoBehaviour, IItem
{
    public int indexNum { get; set; }
    public int value { get; set; }
    public GameObject coin;
    public int sequence;
    Item itemvalues = new Item();

    Player player;

    void Start()
    {
        itemvalues.count = 5;
        value = 2;
        //if (sequence == 1)
        indexNum = sequence;
    }

    public void GetItem()
    {
        Debug.Log("ƒ⁄¿Œ»πµÊ");
        GameManager.instance.itemcheck[1] = true;
        //player = gameObject.GetComponent<Player>();
        //player.itemGet[1] = true;
    }
    public void ItemCharge()
    {
        GameManager.instance.itemcount[1] = itemvalues.count;
    }
}
