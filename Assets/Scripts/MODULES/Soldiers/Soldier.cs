using System.Collections;
using System.Collections.Generic;
using _4_Gameplay;
using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace MODULES.Soldiers
{
    public class Soldier : MonoBehaviour
    {
        [Inject] private WeaponController _weaponController;
        [Inject] private SoundController _soundController;
        public static Soldier Instance;
        [FormerlySerializedAs("objHandToThrow")] [SerializeField] private GameObject _throwObject;
        [FormerlySerializedAs("m_Animator")] [SerializeField] private Animator _animator;
        private const int IDIE = 3, MOVE = 2, SHAKE = 1;
        [FormerlySerializedAs("aniShake")] public AnimationClip _shake;
        public bool IsMoving;
        [Space(30)]
        [FormerlySerializedAs("WEAPON_MANAGER")] public WeaponSystems _weaponManager;
        [FormerlySerializedAs("DEFENSE_MANAGER")] public Defense _defenceManager;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            _weaponManager.Construct(_weaponController.equipedWeaponList);
            _defenceManager.Construct();
            
            _throwObject.SetActive(false);

            EventController.OnWeaponNoBullet += NoBullet;
            EventController.OnZombieAttack += _defenceManager.HandleZombieAttack;
        }
        
        private void Update()
        {
            if (IsMoving)
            {
                PlayAnimations(EnumController.SOLDIER_STATUS.walk);
            }

        }

        [FormerlySerializedAs("_tranOfSoldier")] [Space(30)]
        public Transform _soldierTransform;
        [FormerlySerializedAs("_tranOfBody")] public Transform _bodyTransform;

        private HandWeapon _hands;

        public void PlayAnimations(EnumController.SOLDIER_STATUS _status)
        {
            switch (_status)
            {
                case EnumController.SOLDIER_STATUS.idie:
                    // m_Animator.Play(aniIdie.name, -1, 0);
                    _animator.SetInteger("AnimationState", IDIE);
                    break;
                case EnumController.SOLDIER_STATUS.shooting:
                    _animator.Play(_shake.name, -1, 0);
                    _animator.SetInteger("AnimationState", SHAKE);
                    break;
                case EnumController.SOLDIER_STATUS.walk:
                    // m_Animator.Play(aniMove.name, -1, 0);
                    _animator.SetInteger("AnimationState", MOVE);
                    break;

            }

        }

        public void SetHandPositions(GameObject _handgun)
        {
            _handgun.transform.SetParent(_bodyTransform);
            _handgun.transform.localPosition = new Vector3(-0.847f, 1.696f, 0);
        }

        [FormerlySerializedAs("sprSpriteOfItemsSupport")] [SerializeField] private SpriteRenderer _supportSpriteRenderer;
        public void PlayerThrow(EnumController.SUPPORT _support)
        {
            _supportSpriteRenderer.sprite = SpriteController.Instance.GetSupportSprite(_support)._sprite;
            StartCoroutine(PlayerThrowRoutine());
        }
        private IEnumerator PlayerThrowRoutine()
        {
            _soundController.Play(SoundController.SOUND.sfx_throw);//sound
            _throwObject.SetActive(true);
            _weaponManager._thisWeapon.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            _throwObject.SetActive(false);
            _weaponManager._thisWeapon.gameObject.SetActive(true);

        }

        private void NoBullet(GunData _gundata)
        {
            _weaponManager.WeaponChoose(_weaponController.equipedWeaponList[0].DATA.eWeapon);//cho khau dau tien
        }
        private void OnDisable()
        {
            EventController.OnWeaponNoBullet -= NoBullet;
            EventController.OnZombieAttack -= _defenceManager.HandleZombieAttack;
        }

    }

    [System.Serializable]
    public class WeaponSystems
    {
        [FormerlySerializedAs("CURRENT_GUN_DATA")] public GunData _gunData;
        [FormerlySerializedAs("CURRENT_WEAPON")] public HandWeapon _thisWeapon;

        public void Construct(List<GunData> _list)
        {
            int _total = _list.Count;
            for (int i = 0; i < _total; i++)
            {
                Weapon _new = new Weapon(_list[i]);
                _weaponList.Add(_new);
            }
        }

        #region WEAPON
        [System.Serializable]
        public class Weapon
        {
            [FormerlySerializedAs("GUN_DATA")] public GunData _gunData;
            [FormerlySerializedAs("objHandWeapon")] public GameObject _weaponPrefab;

            public Weapon(GunData _gundata)
            {
                _gunData = _gundata;
                Construct();
            }
            
            public void Construct()
            {
                _weaponPrefab = Soldier.Instantiate(_gunData.objPrefabHand);
                _weaponPrefab.SetActive(false);
                Soldier.Instance.SetHandPositions(_weaponPrefab);//set pos
            }
        }

        [FormerlySerializedAs("LIST_WEAPON")] public List<Weapon> _weaponList = new List<Weapon>();
        
        public void WeaponChoose(EnumController.WEAPON _weapon)
        {
            int _total = _weaponList.Count;
            for (int i = 0; i < _total; i++)
            {
                if (_weaponList[i]._gunData.DATA.eWeapon == _weapon)
                {
                    _weaponList[i]._weaponPrefab.SetActive(true);
                    _thisWeapon = _weaponList[i]._weaponPrefab.GetComponent<HandWeapon>();
                    _gunData = _weaponList[i]._gunData;

                    EventController.OnChangedWeaponInvoke(_gunData);
            
                    _thisWeapon._loadTine = _gunData.fTimeloadOrBullet;
                    _thisWeapon._damage = _gunData.GetDamage(_gunData.DATA.eLevel);
                    _thisWeapon._bulletRange = _gunData.fRangeOfBullet;
                    _thisWeapon._bulletSprite = _gunData.sprBullet;
                    _thisWeapon._bulletScale = _gunData.vScaleOfBullet;
                
                }
                else
                {
                    _weaponList[i]._weaponPrefab.SetActive(false);
                }
            }
        }
        #endregion
    }

    [System.Serializable]
    public class Defense
    {
        [System.Serializable]
        public class UnitDefense
        {
            public EnumController.DEFENSE _defense;
            public float fDefenseValue;
            public GameObject objDefense;
        }
        private int _defaultUnit;

        [FormerlySerializedAs("LIST_DEFENSE")] public List<UnitDefense> _defenceList = new List<UnitDefense>();
        public UnitDefense TakeDefense(EnumController.DEFENSE _defense)
        {
            for (int i = 0; i < _defaultUnit; i++)
            {
                if (_defense == _defenceList[i]._defense)
                {
                    return _defenceList[i];
                }
            }
            return null;
        }
        private int homeIndex = 1;

        private float CurrDefence()
        {
            _currentDefence = 0;
            for (int i = 0; i < _defaultUnit; i++)
            {
                _currentDefence += _defenceList[i].fDefenseValue;
            }

            return _currentDefence;
        }

        private float _defenceTotal;
        private float _currentDefence;
        
        public void Construct()
        {

            int _total = WeaponController.Instance._defenceList.Count;
            for (int i = 0; i < _total; i++)
            {
                if (WeaponController.Instance._defenceList[i].DATA.bEquiped)
                {
                    UnitDefense _unit = new UnitDefense();
                    _unit._defense = WeaponController.Instance._defenceList[i].DATA.eDefense;
                    _unit.fDefenseValue = WeaponController.Instance._defenceList[i].GetDefense(WeaponController.Instance._defenceList[i].DATA.eLevel);

                    if (!WeaponController.Instance._defenceList[i].DATA.bDefault)
                        _unit.objDefense = Soldier.Instantiate(WeaponController.Instance._defenceList[i].bjPrefab);
                    _defenceList.Add(_unit);
                }
            }

            _defaultUnit = _defenceList.Count;
            _defenceTotal = CurrDefence();

            GameplayController.Instance.ShowHpBar(1.0f, (int)_currentDefence + "/" + (int)_defenceTotal);
        }



        //REDUCE 
        public void RemoveDamage(float _damage)
        {
            for (int i = 0; i < _defaultUnit; i++)
            {
                if (_defenceList[i].fDefenseValue >= _damage)
                {
                    _defenceList[i].fDefenseValue -= _damage;
                    _damage = 0;


                }
                else if (_defenceList[i].fDefenseValue > 0 && _defenceList[i].fDefenseValue < _damage)
                {
                    _damage -= _defenceList[i].fDefenseValue;
                    _defenceList[i].fDefenseValue = 0;
                    //  LIST_DEFENSE[i].objDefense.SetActive(false);
                }
            }



            //show bar
            _currentDefence = CurrDefence();
            if (_currentDefence <= 0)
            {
                _currentDefence = 0;
                GameplayController.Instance.SetStatusOfGame(GameplayController.GAME_STATUS.gameover);//game over
            }
            GameplayController.Instance.ShowHpBar(GetFactorDefense(), (int)_currentDefence + "/" + (int)_defenceTotal);//show bar


            //FIRE
            if (GetFactorDefense() < 0.6f && homeIndex == 1)
            {
                Soldier.Instantiate(ObjectPoolController.Instance._fireOnHomeLevel1);//fire of home
                homeIndex = 2;
            }
            if (GetFactorDefense() < 0.4f && homeIndex == 2)
            {
                Soldier.Instantiate(ObjectPoolController.Instance._fireOnHomeLevel2);//fire of home
                homeIndex = 3;
            }
            if (GetFactorDefense() < 0.3f && homeIndex == 3)
            {
                Soldier.Instantiate(ObjectPoolController.Instance._fireOnHomeLevel3);//fire of home
                homeIndex = 4;
            }
        }
        
        public float GetFactorDefense()
        {
            return _currentDefence / _defenceTotal;
        }

        public void HandleZombieAttack(float _damage)
        {
            Soldier.Instance._defenceManager.RemoveDamage(_damage);
        }
    }
}