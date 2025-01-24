using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockData",menuName = "Scriptable Object/BlockData", order = int.MaxValue)]
public class BlockData : ScriptableObject
{
    [SerializeField]
    private bool installBlock;

    public bool _installblock { get { return installBlock; } }
}
