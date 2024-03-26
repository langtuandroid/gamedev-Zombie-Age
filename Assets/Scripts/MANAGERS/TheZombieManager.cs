using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheZombieManager : MonoBehaviour
{
    public static TheZombieManager Instance;

    //ZOMBIE
    [Header("*** Zombie")]
    [Space(20)]
    private int iTotalZombieinGame;
    public List<ZombieData> LIST_ZOMBIE_IN_GAME;
    public ZombieData GetZombie(TheEnumManager.ZOMBIE _zombie)
    {
        for (int i = 0; i < iTotalZombieinGame; i++)
        {
            if (LIST_ZOMBIE_IN_GAME[i].eZombie == _zombie)
                return LIST_ZOMBIE_IN_GAME[i];
        }
        return null;
    }


    //WAVE
    public int iTotalZombieInWave; 

    //POOL ZOMBIE FOR LEVEL
    [System.Serializable]
    public class UnitZombie
    {
        public TheEnumManager.ZOMBIE _zombie;
        private GameObject objPrefabZombie;

        public List<Zombie> LIST = new List<Zombie>();
        private int CountZombie = 10;
        public void Init()
        {

            objPrefabZombie = TheZombieManager.Instance.GetZombie(_zombie).objPrefab;
            for (int i = 0; i < CountZombie; i++)
            {
                GameObject _zombie = Instantiate(objPrefabZombie, new Vector2(100, 100), Quaternion.identity);
                _zombie.SetActive(false);
                LIST.Add(_zombie.GetComponent<Zombie>());
            }
        }


        public Zombie GetZombie()
        {
            for (int i = 0; i < CountZombie; i++)
            {
                if (!LIST[i].ALIVE)
                {
                    return LIST[i];
                }
            }

            //---add more
            GameObject _zombie = Instantiate(objPrefabZombie, new Vector2(100, 100), Quaternion.identity);
            _zombie.SetActive(false);
            LIST.Add(_zombie.GetComponent<Zombie>());
            CountZombie++;
            return _zombie.GetComponent<Zombie>();
        }
    }
    public List<UnitZombie> LIST_POOL_OF_ZOMBIE;
    public Zombie GetZombieInPool(TheEnumManager.ZOMBIE _zombie)
    {
        int _total = LIST_POOL_OF_ZOMBIE.Count;
        for (int i = 0; i < _total; i++)
        {
            if (LIST_POOL_OF_ZOMBIE[i]._zombie == _zombie)
            {
                return LIST_POOL_OF_ZOMBIE[i].GetZombie();
            }
        }
        return null;
    }

    public void InitPool(List<ZombieData> _listOfLevel)
    {
        int i_total = _listOfLevel.Count;
        for (int i = 0; i < i_total; i++)
        {
            UnitZombie _groupZombie = new UnitZombie();
            _groupZombie._zombie = _listOfLevel[i].eZombie;
            _groupZombie.Init();
            LIST_POOL_OF_ZOMBIE.Add(_groupZombie);
        }
    }


    //SPRITE OF ZOMBIE BULLET
    [System.Serializable]
    public class SpriteBullet
    {
        public TheEnumManager.ZOMBIE eZombie;
        public Sprite sprBulletSprite;
    }
    public List<SpriteBullet> LIST_SPRITE_BULLET;
    public SpriteBullet GetSpriteBullet(TheEnumManager.ZOMBIE _zombie)
    {
        int length = LIST_SPRITE_BULLET.Count;
        for (int i = 0; i < length; i++)
        {
            if (LIST_SPRITE_BULLET[i].eZombie == _zombie)
                return LIST_SPRITE_BULLET[i];
        }
        return null;
    }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        iTotalZombieinGame = LIST_ZOMBIE_IN_GAME.Count;
    }

    private void HandleZombieBorn(Zombie _zombie)
    {
      //  iTotalZombieInWave++;
    }
    private void HandleZombieDie(Zombie _zombie)
    {
        iTotalZombieInWave--;
        if (iTotalZombieInWave <= 0)
        {
            TheLevel.Instance.LoadWave();
        }
    }


    private void OnEnable()
    {
        TheEventManager.OnZombieBorn += HandleZombieBorn;
        TheEventManager.OnZombieDie += HandleZombieDie;
    }
    private void OnDisable()
    {
        TheEventManager.OnZombieBorn -= HandleZombieBorn;
        TheEventManager.OnZombieDie -= HandleZombieDie;
    }
}
