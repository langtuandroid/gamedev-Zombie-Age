using System.Collections.Generic;
using MODULES;
using MODULES.Scriptobjectable;
using MODULES.Zombies;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace MANAGERS
{
    public class ZombieController : MonoBehaviour
    {
        [Inject] private DiContainer _diContainer;
        [Header("*** Zombie")]
        [Space(20)]
        private int _zombiesTotalNum;
        [FormerlySerializedAs("_zombieTotal")] [FormerlySerializedAs("LIST_ZOMBIE_IN_GAME")] public List<ZombieData> _zombieList;
        public ZombieData GetZombie(EnumController.ZOMBIE _zombie)
        {
            for (int i = 0; i < _zombiesTotalNum; i++)
            {
                if (_zombieList[i].eZombie == _zombie)
                    return _zombieList[i];
            }
            return null;
        }

        [FormerlySerializedAs("iTotalZombieInWave")] public int _zombiesInWave; 

        [System.Serializable]
        public class UnitZombie
        {
            [Inject] private DiContainer _diContainer;
            [Inject] private ZombieController _zombieController;
            [FormerlySerializedAs("_zombie")] public EnumController.ZOMBIE _zombieType;
            private GameObject _zombiePrefab;

            [FormerlySerializedAs("LIST")] public List<Zombie> _zombiesList = new List<Zombie>();
            private int _zombieNum = 10;
            public void Init()
            {
                _zombiePrefab = _zombieController.GetZombie(_zombieType).objPrefab;
                for (int i = 0; i < _zombieNum; i++)
                {
                    Debug.Log(_zombiePrefab);
                    GameObject _zombie = _diContainer.InstantiatePrefab(_zombiePrefab, new Vector2(100, 100), Quaternion.identity, null);
                    _zombie.SetActive(false);
                    _zombiesList.Add(_zombie.GetComponent<Zombie>());
                }
            }


            public Zombie GetZombie()
            {
                for (int i = 0; i < _zombieNum; i++)
                {
                    if (!_zombiesList[i]._isAlive)
                    {
                        return _zombiesList[i];
                    }
                }

                //---add more
                GameObject _zombie = Instantiate(_zombiePrefab, new Vector2(100, 100), Quaternion.identity);
                _zombie.SetActive(false);
                _zombiesList.Add(_zombie.GetComponent<Zombie>());
                _zombieNum++;
                return _zombie.GetComponent<Zombie>();
            }
        }
        [FormerlySerializedAs("LIST_POOL_OF_ZOMBIE")] public List<UnitZombie> _poolsOfZombies;
        public Zombie GetZombieInPool(EnumController.ZOMBIE _zombie)
        {
            int _total = _poolsOfZombies.Count;
            for (int i = 0; i < _total; i++)
            {
                if (_poolsOfZombies[i]._zombieType == _zombie)
                {
                    return _poolsOfZombies[i].GetZombie();
                }
            }
            return null;
        }

        public void ConstructPool(List<ZombieData> _listOfLevel)
        {
            int i_total = _listOfLevel.Count;
            for (int i = 0; i < i_total; i++)
            {
                UnitZombie _groupZombie = new UnitZombie();
                _groupZombie._zombieType = _listOfLevel[i].eZombie;
                _diContainer.Inject(_groupZombie);
                _groupZombie.Init();
                _poolsOfZombies.Add(_groupZombie);
            }
        }


        //SPRITE OF ZOMBIE BULLET
        [System.Serializable]
        public class SpriteBullet
        {
            [FormerlySerializedAs("eZombie")] public EnumController.ZOMBIE _zombieType;
            [FormerlySerializedAs("sprBulletSprite")] public Sprite _bulletSprite;
        }
        public List<SpriteBullet> LIST_SPRITE_BULLET;
        public SpriteBullet GetSpriteBullet(EnumController.ZOMBIE _zombie)
        {
            int length = LIST_SPRITE_BULLET.Count;
            for (int i = 0; i < length; i++)
            {
                if (LIST_SPRITE_BULLET[i]._zombieType == _zombie)
                    return LIST_SPRITE_BULLET[i];
            }
            return null;
        }


        private void Awake()
        {
            _zombiesTotalNum = _zombieList.Count;
        }

        private void ZombieSpawned(Zombie _zombie)
        {
            
        }
        private void ZombieDied(Zombie _zombie)
        {
            _zombiesInWave--;
            if (_zombiesInWave <= 0)
            {
                LevelController.Instance.LoadLevel();
            }
        }


        private void OnEnable()
        {
            EventController.OnZombieBorn += ZombieSpawned;
            EventController.OnZombieDie += ZombieDied;
        }
        private void OnDisable()
        {
            EventController.OnZombieBorn -= ZombieSpawned;
            EventController.OnZombieDie -= ZombieDied;
        }
    }
}
