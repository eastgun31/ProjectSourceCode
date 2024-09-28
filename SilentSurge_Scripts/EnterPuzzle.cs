using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using ItemInfo;

public class EnterPuzzle : MonoBehaviour
{
    public int level;
    public GameObject puzzle;
    Casing cas;
    //GameManager gm;
    //UiManager uiManager;

    private void Start()
    {
        cas = new Casing();
        //gm = GameManager.instance;
        //uiManager = UiManager.instance;
    }


    public int PuzzleActive1()
    {
        return cas.gm.puzzleLevel switch
        {
            1 when level == 1 => cas.ui.ActivePipeFst(),
            2 when level == 2 => cas.ui.ActiveSinFst(),
            3 when level == 3 => cas.ui.ActiveHackingFst(),
            4 when level == 4 && cas.gm.scenenum == 1 => cas.ui.ActivePipeKeypadNumEight(),
            4 when level == 4 && cas.gm.scenenum == 2 => cas.ui.ActivePipeKeypadNumNine(),
            5 when level == 5 && cas.gm.scenenum == 1 => cas.ui.ActivePipeKeypadNumThree(),
            5 when level == 5 && cas.gm.scenenum == 2 => cas.ui.ActivePipeKeypadNumSix(),
            6 when level == 6 && cas.gm.scenenum == 1 => cas.ui.ActivePipeKeypadNumTwo(),
            6 when level == 6 && cas.gm.scenenum == 2 => cas.ui.ActivePipeKeypadNumThree(),
            7 when level == 7 && cas.gm.scenenum == 1 => cas.ui.ActivePipeKeypadNumFour(),
            7 when level == 7 && cas.gm.scenenum == 2 => cas.ui.ActivePipeKeypadNumFive(),
            8 when level == 8 => cas.ui.ActiveKeypad(),
            _ => 0
        };
    }

    public int PuzzleActive2()
    {
        return cas.gm.puzzleLevel switch
        {
            1 when level == 1 => cas.ui.ActiveHackingFst(),
            2 when level == 2 => cas.ui.ActiveKeypad(),
            3 when level == 3 => cas.ui.ActivePipeSec(),
            _ => 0
        };
    }

    public int PuzzleActive3()
    {
        return cas.gm.puzzleLevel switch
        {
            1 when level == 1 => cas.ui.ActiveKeypad(),
            _ => 0
        };
    }

}