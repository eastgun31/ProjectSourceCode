using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace ItemInfo
{
    public class Item
    {
        public int count { get; set; }
        public float cooltime { get; set; }
        public bool canUse { get; set; }
        public WaitForSeconds itemcool = new WaitForSeconds(3f);
        public WaitForSeconds heartseeDuration = new WaitForSeconds(5f);
        public WaitForSeconds animDelay = new WaitForSeconds(0.5f);
        public WaitForSeconds amJeonDelay = new WaitForSeconds(0.7f);
    }
    public class UseRay
    {
        public Ray ray;
        public RaycastHit hit;
    }
    public class StringMoum
    {
        public string hoirzon = "Horiznotal";
        public string vertic = "Vertical";
        public string walk = "Walk";
        public string die = "Die";
        public string handgunMode = "HandGun";
        public string throwcoin = "ThrowCoin";
        public string throwflashbang = "ThrowFlashBang";
        public string _run = "_Runing";
        public string _gunrun = "_GunRuning";
        public string itemTag = "Item";
        public string eBullet = "E_Bullet";
        public string puzzle = "Puzzle";
        public string door = "Doorhandle";
        public string enemy = "Enemy";
    }
    public class EnemyString
    {
        public string Walk = "Walk";
        public string Shot = "Shot";
        public string GunRuning = "GunRuning";
        public string Death = "Death";
        public string Death2 = "Death2";
        public string Flash = "Flash";
        public string PlayerListen = "PlayerListen";
        public string bullet = "Bullet";
        public string amsal = "AmSal";
    }
    public class CoolTime
    {
        public WaitForSeconds cool1sec = new WaitForSeconds(1f);
        public WaitForSeconds coolhalf1sec = new WaitForSeconds(0.5f);
    }
    public class E_CoolTime
    {
        public WaitForSeconds cool1sec = new WaitForSeconds(1f);
        public WaitForSeconds cool2sec = new WaitForSeconds(2f);
        public WaitForSeconds cool3sec = new WaitForSeconds(3f);
        public WaitForSeconds cool5sec = new WaitForSeconds(5f);
        public WaitForSeconds cool10sec = new WaitForSeconds(10f);
    }
    public class Casing
    {
        public GameManager gm = GameManager.instance;
        public SoundManager sm = SoundManager.instance;
        public UiManager ui = UiManager.instance;
    }


    public class CctvCool
    {
        public WaitForSeconds cool3sec = new WaitForSeconds(3f);
    }
    public class EnemyInfo
    {
        public int enemycount1 = 12;
    }
}




