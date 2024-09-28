using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using System.Threading;

public class HandGun : MonoBehaviour, IItem
{
    public int indexNum { get; set; }
    public int value {  get; set; }
    public int sequence;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    Item itemvalues = new Item();

    Player player;
    
    void Start()
    {
        value = 1;
        itemvalues.count = 6;
        //if(sequence == 1)
        indexNum = sequence;
    }

    public void GetItem()
    {
        Debug.Log("±ÇÃÑÈ¹µæ");
        GameManager.instance.itemcheck[0] = true;
        //player = gameObject.GetComponent<Player>();
        //player.itemGet[0] = true;
    }
    public void ItemCharge()
    {
        GameManager.instance.itemcount[0] = itemvalues.count;
    }
}
