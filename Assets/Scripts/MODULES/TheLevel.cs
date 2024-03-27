using System.Collections;
using System.Collections.Generic;
using _4_Gameplay;
using MANAGERS;
using MODULES.Scriptobjectable;
using MODULES.Zombies;
using UnityEngine;

namespace MODULES
{
    public class TheLevel : MonoBehaviour
    {
        public LevelData LEVEL_DATA;
        //class wave
        [System.Serializable]
        public class UnitWave
        {
            public LevelData.Zombie _ZombieDataConfig;
            public ZombieData _ZombieData;
            public int iNumber;
        }

        [System.Serializable]
        public class Wave
        {
            public List<UnitWave> LIST_UNIT_WAVE;

            public void Init(int _currentWave, int _maxWave)
            {
                LIST_UNIT_WAVE = new List<UnitWave>();
                for (int i = 0; i < _currentWave + 1; i++)
                {
                    UnitWave _temp = new UnitWave();
                    int _index = Random.Range(0, TheLevel.Instance.LIST_ZOMBIE_IN_LEVEL.Count);
                    _temp._ZombieData = TheLevel.Instance.LIST_ZOMBIE_IN_LEVEL[_index];
                    _temp._ZombieDataConfig = Instance.LEVEL_DATA.GetZombie(_temp._ZombieData.eZombie);

                    if (_currentWave < 5)
                        _temp.iNumber = 10;
                    else
                        _temp.iNumber = 5;

                    LIST_UNIT_WAVE.Add(_temp);
                }
            }


            //GET TOTAL ZOMBIE IN WAVE
            public int GetTotalZombie()
            {
                int _total = 0;
                for (int i = 0; i < LIST_UNIT_WAVE.Count; i++)
                {
                    _total += LIST_UNIT_WAVE[i].iNumber;
                }
                return _total;
            }

            //GET GROUP ZOMBIE
            public int GetTotalGroupZombie()
            {
                return LIST_UNIT_WAVE.Count;
            }
        }



        public static TheLevel Instance;

        [Header("*** Config level ***")]

        [SerializeField] SpriteRenderer BACKGROUND_FRAME;
        [SerializeField] SpriteRenderer BACKGROUND;

        private bool IsBossComming = false;
        public int iCurrentWave = -1;



        [Space(30)]
        public List<ZombieData> LIST_ZOMBIE_IN_LEVEL;

        [Header("LIST WAVE IN LEVEL")]
        [Space(30)]
        public List<Wave> LIST_WAVE;

        private int iCountZombie = 0;
        private int iTotalZombie;




        private void Awake()
        {

            TheEventManager.PostEvent_OnStartLevel(); //event firebase


            if (Instance == null)
                Instance = this;

            #region LOAD DATA
            LevelData _data = Resources.Load<LevelData>("Levels/Configs/Level_" + (TheDataManager.Instance.THE_DATA_PLAYER.iCurrentLevel + 1).ToString());
            if (_data == null) _data = Resources.Load<LevelData>("Levels/Configs/Level_default");
            LEVEL_DATA = _data;
            BACKGROUND_FRAME.sprite = LEVEL_DATA.sprBackgroundFrame;
            BACKGROUND.sprite = LEVEL_DATA.sprBackground;
            #endregion
        }
        private void Start()
        {
            SetZombieInLevel();
            TheZombieManager.Instance.InitPool(LIST_ZOMBIE_IN_LEVEL);
            ConfigWave();
        }
        private void OnDisable()
        {
            Instance = null;
        }



        //Get zombie in level
        private void SetZombieInLevel()
        {
            int _total = LEVEL_DATA.LIST_ZOMBIE_IN_LEVEL.Count;
            for (int i = 0; i < _total; i++)
            {
                if (!TheZombieManager.Instance.GetZombie(LEVEL_DATA.LIST_ZOMBIE_IN_LEVEL[i].ZOMBIE).bIsBoss)
                    LIST_ZOMBIE_IN_LEVEL.Add(TheZombieManager.Instance.GetZombie(LEVEL_DATA.LIST_ZOMBIE_IN_LEVEL[i].ZOMBIE));
            }

        }


        //Config wave
        private void ConfigWave()
        {
            for (int i = 0; i < LEVEL_DATA.iTotalWave; i++)
            {
                Wave _wave = new Wave();
                _wave.Init(i, LEVEL_DATA.iTotalWave);
                LIST_WAVE.Add(_wave);
            }


            iTotalZombie = GetTotalZombieInLevel();
        }


        //LOAD WAVE
        public void LoadWave()
        {
            StartCoroutine(ieLoadWave());
        }


        //GET TOTAL ZOMBIE IN THIS LEVEL
        public int GetTotalZombieInLevel()
        {
            int length = LIST_WAVE.Count;
            int _totalZombie = 0;
            for (int i = 0; i < length; i++)
            {
                int _length = LIST_WAVE[i].LIST_UNIT_WAVE.Count;
                for (int j = 0; j < _length; j++)
                {
                    _totalZombie += LIST_WAVE[i].LIST_UNIT_WAVE[j].iNumber;
                }

            }
            return _totalZombie;
        }



        WaitForSeconds _wait = new WaitForSeconds(1.0f);
        Zombie _tempZombie = new Zombie();
        IEnumerator ieLoadWave()
        {
            yield return new WaitForSeconds(0.5f);
            if (MainCode_Gameplay.Instance.eGameStatus != MainCode_Gameplay.GAME_STATUS.playing) yield break;

            iCurrentWave++;
            if (iCurrentWave == LEVEL_DATA.iTotalWave)
            {
                MainCode_Gameplay.Instance.SetGameStartus(MainCode_Gameplay.GAME_STATUS.victory);
            }
            else
            {
                //Get total zombie in this wave



                //-------------------------------------------
                TheEventManager.PostEvent_OnStartNewWave();//event

                Wave _wave = LIST_WAVE[iCurrentWave];
                int _totalGroup = _wave.GetTotalGroupZombie();
                TheZombieManager.Instance.iTotalZombieInWave = _wave.GetTotalZombie();
                Vector3 _tempPos = new Vector3();

                float _specialStatusForZombie = 0;//random special zombie


                for (int i = 0; i < _totalGroup; i++)
                {
                    UnitWave _unitWave = _wave.LIST_UNIT_WAVE[i];
                    yield return _wait;
                    int _totalZombieInGroup = _unitWave.iNumber;
                    TheEnumManager.ZOMBIE _zombie = _unitWave._ZombieData.eZombie;


                    for (int j = 0; j < _totalZombieInGroup; j++)
                    {
                        _tempPos = new Vector2(10, Random.Range(-4.7f, 2.3f));
                        _tempPos.z = _tempPos.y;


                        #region BOSS
                        if (j == 2
                            && LEVEL_DATA.prefabBoss != null
                            && !IsBossComming
                            && _wave == LIST_WAVE[LIST_WAVE.Count - 1])//WAVE CUOI CUNG
                        {
                            IsBossComming = true;
                            TheZombieManager.Instance.iTotalZombieInWave++;
                            GameObject _boss = Instantiate(LEVEL_DATA.prefabBoss, _tempPos, Quaternion.identity);//boss
                            Zombie _Boss = _boss.GetComponent<Zombie>();
                            _Boss.Init(_tempPos);
                            _Boss.HEALTH = new ZombieHealth(_Boss);
                        }
                        #endregion


                        //ZOMBIE
                        _tempZombie = TheZombieManager.Instance.GetZombieInPool(_zombie);

                        _specialStatusForZombie = Random.Range(0, 100);
                        if (_specialStatusForZombie <= LEVEL_DATA.iConfigSpecialStatus)
                            _tempZombie.Init(_tempPos, true); //zombie o trang thai dac biet
                        else
                            _tempZombie.Init(_tempPos);


                        //-------------- items-------------------
                        if (_unitWave._ZombieDataConfig.IsHat()) _tempZombie.ITEM_SYSTEM.SetItem(_unitWave._ZombieDataConfig.eHat);
                        else _tempZombie.ITEM_SYSTEM.SetItem(TheEnumManager.HAT_OF_ZOMBIE.NO_HAT);

                        if (_unitWave._ZombieDataConfig.IsWeapon()) _tempZombie.ITEM_SYSTEM.SetItem(_unitWave._ZombieDataConfig.eWeapon);
                        else _tempZombie.ITEM_SYSTEM.SetItem(TheEnumManager.WEAPON_OF_ZOMBIE.NO_WEAPON);


                        if (_unitWave._ZombieDataConfig.IsShield()) _tempZombie.ITEM_SYSTEM.SetItem(_unitWave._ZombieDataConfig.eShield);
                        else _tempZombie.ITEM_SYSTEM.SetItem(TheEnumManager.SHIELD_OF_ZOMBIE.NO_SHIELD);




                        //-------------- health -----------------
                        _tempZombie.HEALTH = new ZombieHealth(_tempZombie);

                        //-------------- update zombie wave bar -----------
                        iCountZombie++;
                        MainCode_Gameplay.Instance.m_ZombieWaveBar.UpdateBar(iCountZombie * 1.0f / iTotalZombie);

                        _tempZombie.gameObject.SetActive(true);

                        yield return _wait;
                    }


                }


            }
        }



        //TRIGGER
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision)
            {
                collision.GetComponent<Zombie>().SetStatus(TheEnumManager.ZOMBIE_STATUS.attack);
            }
        }


        //DEBUG============================================================
        [ContextMenu("GET TOTAL ZZOMBIE IN THIS LEVEL")]
        public void GetTotalZombieInThisLevel()
        {
            int length = LIST_WAVE.Count;
            int _totalZombie = 0;
            for (int i = 0; i < length; i++)
            {
                int _length = LIST_WAVE[i].LIST_UNIT_WAVE.Count;
                for (int j = 0; j < _length; j++)
                {
                    _totalZombie += LIST_WAVE[i].LIST_UNIT_WAVE[j].iNumber;
                }

            }

            Debug.Log("TOTAL ZOMBIE IN THIS LEVEL: " + _totalZombie);
        }

        [ContextMenu("GET TOTAL ZOMBIE 'HP IN THIS LEVEL")]
        public void GetTotalZombieHPInThisLevel()
        {
            int length = LIST_WAVE.Count;
            int _totalHp = 0;
            for (int i = 0; i < length; i++)
            {
                int _length = LIST_WAVE[i].LIST_UNIT_WAVE.Count;
                for (int j = 0; j < _length; j++)
                {
                    _totalHp += LIST_WAVE[i].LIST_UNIT_WAVE[j].iNumber * TheZombieManager.Instance.GetZombie(LIST_WAVE[i].LIST_UNIT_WAVE[j]._ZombieData.eZombie).GetHp(
                        TheDataManager.Instance.THE_DATA_PLAYER.iCurrentLevel + 1, i + 1
                    );
                }

            }

            Debug.Log("TOTAL ZOMBIE' HP IN THIS LEVEL: " + _totalHp);
        }
    }
}
