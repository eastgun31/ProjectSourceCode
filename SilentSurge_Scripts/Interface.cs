using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    int indexNum { get; set; }
    int value { get; set; }
    void GetItem();
    void ItemCharge();
}

public interface ISound
{
    bool canhear { get; set; }
}

public interface Human
{
    string diepeople { get; set; }
}

public interface IDoor
{
    Door_Parent tDoor { get; set; }
}
