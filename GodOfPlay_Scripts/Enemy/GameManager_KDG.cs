using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_KDG : MonoBehaviour
{
    public static GameManager_KDG Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    //public int unitcheck = 0;

    public int aCheck = 0;
    public int bCheck = 0;
    public int cCheck = 0;

    public int[] check = { 0, 0, 0 };

    public bool attacking = false;
    public int attackPoint = 0;
    public int e_population = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
