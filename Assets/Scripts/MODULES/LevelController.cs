using System.Collections;
using System.Collections.Generic;
using _4_Gameplay;
using MANAGERS;
using MODULES.Scriptobjectable;
using MODULES.Zombies;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace MODULES
{
    public class LevelController : MonoBehaviour
    {
        [Inject] private DiContainer _diContainer;   
        [Inject] private DataController _dataController;
        [Inject] private GameplayController _gameplayController;
        [Inject] private ZombieController _zombieController;
        [FormerlySerializedAs("LEVEL_DATA")] public LevelData _levelData;
        [System.Serializable]
        public class UnitWave
        {
            [FormerlySerializedAs("_ZombieDataConfig")] public LevelData.Zombie _zombieConfig;
            [FormerlySerializedAs("_ZombieData")] public ZombieData _zombieData;
            [FormerlySerializedAs("iNumber")] public int _numberOfZombies;
        }

        [System.Serializable]
        public class Wave
        {
            [FormerlySerializedAs("LIST_UNIT_WAVE")] public List<UnitWave> _waveUnits;

            public void Construct(int _currentWave)
            {
                _waveUnits = new List<UnitWave>();
                for (int i = 0; i < _currentWave + 1; i++)
                {
                    UnitWave _temp = new UnitWave();
                    int _index = Random.Range(0, LevelController.Instance._zombiesInLevel.Count);
                    _temp._zombieData = LevelController.Instance._zombiesInLevel[_index];
                    _temp._zombieConfig = Instance._levelData.GetZombie(_temp._zombieData.eZombie);

                    if (_currentWave < 5)
                        _temp._numberOfZombies = 10;
                    else
                        _temp._numberOfZombies = 5;

                    _waveUnits.Add(_temp);
                }
            }
            
            public int AllZOmbies()
            {
                int _total = 0;
                for (int i = 0; i < _waveUnits.Count; i++)
                {
                    _total += _waveUnits[i]._numberOfZombies;
                }
                return _total;
            }

            public int GetAllZombiesGroups()
            {
                return _waveUnits.Count;
            }
        }
        
        public static LevelController Instance;

        
        [Header("*** Config level ***")]

        [FormerlySerializedAs("BACKGROUND_FRAME")][SerializeField] SpriteRenderer _bgFrame;
        [FormerlySerializedAs("BACKGROUND")] [SerializeField] SpriteRenderer _bg;

        private bool IsBossComming;
        public int CurrentWave { get; private set; } = -1;



         [Space(30)]
         [FormerlySerializedAs("LIST_ZOMBIE_IN_LEVEL")] public List<ZombieData> _zombiesInLevel;

       
        [Header("LIST WAVE IN LEVEL")]
        [Space(30)]
        [FormerlySerializedAs("LIST_WAVE")]public List<Wave> _waveList;

        private int _zombieCount = 0;
        private int _zombiesTotal;

        
        private void Awake()
        {
            EventController.OnStartLevelInvoke(); 
            
            if (Instance == null)
                Instance = this;

            #region LOAD DATA
            LevelData _data = Resources.Load<LevelData>("Levels/Configs/Level_" + (_dataController.playerData.CurrentLevel + 1).ToString());
            if (_data == null) _data = Resources.Load<LevelData>("Levels/Configs/Level_default");
            _levelData = _data;
            _bgFrame.sprite = _levelData.sprBackgroundFrame;
            _bg.sprite = _levelData.sprBackground;
            #endregion
        }
        private void Start()
        {
            AssignAllZombies();
            _zombieController.ConstructPool(_zombiesInLevel);
            ConfigureWave();
        }
        private void OnDisable()
        {
            Instance = null;
        }
        
        private void AssignAllZombies()
        {
            int _total = _levelData.LIST_ZOMBIE_IN_LEVEL.Count;
            for (int i = 0; i < _total; i++)
            {
                if (!_zombieController.GetZombie(_levelData.LIST_ZOMBIE_IN_LEVEL[i].ZOMBIE).bIsBoss)
                    _zombiesInLevel.Add(_zombieController.GetZombie(_levelData.LIST_ZOMBIE_IN_LEVEL[i].ZOMBIE));
            }

        }
        
        private void ConfigureWave()
        {
            for (int i = 0; i < _levelData.iTotalWave; i++)
            {
                Wave _wave = new Wave();
                _wave.Construct(i);
                _waveList.Add(_wave);
            }


            _zombiesTotal = CalculateTotalZombies();
        }
        
        public void LoadLevel()
        {
            StartCoroutine(LoadWaveRoutine());
        }

        private int CalculateTotalZombies()
        {
            int length = _waveList.Count;
            int _totalZombie = 0;
            for (int i = 0; i < length; i++)
            {
                int _length = _waveList[i]._waveUnits.Count;
                for (int j = 0; j < _length; j++)
                {
                    _totalZombie += _waveList[i]._waveUnits[j]._numberOfZombies;
                }

            }
            return _totalZombie;
        }


        private WaitForSeconds _waitTime = new(1.0f);
        private Zombie _tempZombie = new();
        IEnumerator LoadWaveRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            if (_gameplayController.GameStatus != GameplayController.GAME_STATUS.playing) yield break;

            CurrentWave++;
            if (CurrentWave == _levelData.iTotalWave) 
            {
                _gameplayController.SetStatusOfGame(GameplayController.GAME_STATUS.victory);
            }
            else
            {
                EventController.OnStartNewWaveInvoke();

                Wave _wave = _waveList[CurrentWave];
                int _totalGroup = _wave.GetAllZombiesGroups();
                _zombieController._zombiesInWave = _wave.AllZOmbies();
                Vector3 _tempPos = new Vector3();

                float _specialStatusForZombie = 0;
                
                for (int i = 0; i < _totalGroup; i++)
                {
                    UnitWave _unitWave = _wave._waveUnits[i];
                    yield return _waitTime;
                    int _totalZombieInGroup = _unitWave._numberOfZombies;
                    EnumController.ZOMBIE _zombie = _unitWave._zombieData.eZombie;


                    for (int j = 0; j < _totalZombieInGroup; j++)
                    {
                        _tempPos = new Vector2(10, Random.Range(-4.7f, 2.3f));
                        _tempPos.z = _tempPos.y;


                        #region BOSS
                        if (j == 2
                            && _levelData.prefabBoss != null
                            && !IsBossComming
                            && _wave == _waveList[_waveList.Count - 1])
                        {
                            IsBossComming = true;
                            _zombieController._zombiesInWave++;
                            GameObject _boss = _diContainer.InstantiatePrefab(_levelData.prefabBoss, _tempPos, Quaternion.identity, null);
                            Zombie _Boss = _boss.GetComponent<Zombie>();
                            ZombieHealth zombieHealth = new ZombieHealth(_Boss);
                            _Boss.Constuct(_tempPos);
                            _Boss._health = zombieHealth;
                        }
                        #endregion


                        //ZOMBIE
                        _tempZombie = _zombieController.GetZombieInPool(_zombie);

                        _specialStatusForZombie = Random.Range(0, 100);
                        if (_specialStatusForZombie <= _levelData.iConfigSpecialStatus)
                            _tempZombie.Constuct(_tempPos, true); //zombie o trang thai dac biet
                        else
                            _tempZombie.Constuct(_tempPos);


                        //-------------- items-------------------
                        if (_unitWave._zombieConfig.IsHat()) _tempZombie._itemsSystem.SetItem(_unitWave._zombieConfig.eHat);
                        else _tempZombie._itemsSystem.SetItem(EnumController.HAT_OF_ZOMBIE.NO_HAT);

                        if (_unitWave._zombieConfig.IsWeapon()) _tempZombie._itemsSystem.SetItem(_unitWave._zombieConfig.eWeapon);
                        else _tempZombie._itemsSystem.SetItem(EnumController.WEAPON_OF_ZOMBIE.NO_WEAPON);


                        if (_unitWave._zombieConfig.IsShield()) _tempZombie._itemsSystem.SetItem(_unitWave._zombieConfig.eShield);
                        else _tempZombie._itemsSystem.SetItem(EnumController.SHIELD_OF_ZOMBIE.NO_SHIELD);

                        ZombieHealth zombieHealthh = new ZombieHealth(_tempZombie);
                        _tempZombie._health = zombieHealthh;

                        _zombieCount++;
                        _gameplayController.zombieWaveBar.Update(_zombieCount * 1.0f / _zombiesTotal);

                        _tempZombie.gameObject.SetActive(true);

                        yield return _waitTime;
                    }


                }


            }
        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision)
            {
                collision.GetComponent<Zombie>().ChangeStatus(EnumController.ZOMBIE_STATUS.attack);
            }
        }

        
        [ContextMenu("GET TOTAL ZZOMBIE IN THIS LEVEL")]
        public void GetTotalZombieInThisLevel()
        {
            int length = _waveList.Count;
            int _totalZombie = 0;
            for (int i = 0; i < length; i++)
            {
                int _length = _waveList[i]._waveUnits.Count;
                for (int j = 0; j < _length; j++)
                {
                    _totalZombie += _waveList[i]._waveUnits[j]._numberOfZombies;
                }

            }

            Debug.Log("TOTAL ZOMBIE IN THIS LEVEL: " + _totalZombie);
        }

        [ContextMenu("GET TOTAL ZOMBIE 'HP IN THIS LEVEL")]
        public void CalculateAllLevelZombieHP()
        {
            int length = _waveList.Count;
            int _totalHp = 0;
            for (int i = 0; i < length; i++)
            {
                int _length = _waveList[i]._waveUnits.Count;
                for (int j = 0; j < _length; j++)
                {
                    _totalHp += _waveList[i]._waveUnits[j]._numberOfZombies * _zombieController.GetZombie(_waveList[i]._waveUnits[j]._zombieData.eZombie).GetHp(
                        _dataController.playerData.CurrentLevel + 1, i + 1
                    );
                }

            }

            Debug.Log("TOTAL ZOMBIE' HP IN THIS LEVEL: " + _totalHp);
        }
    }
}
