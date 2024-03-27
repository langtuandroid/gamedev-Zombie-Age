using System.Collections;
using System.Collections.Generic;
using _4_Gameplay;
using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;

namespace MODULES.Soldiers
{
    public class Soldier : MonoBehaviour
    {
        public static Soldier Instance;
        [SerializeField] private GameObject objHandToThrow;
        [SerializeField] private Animator m_Animator;
        private const int IDIE = 3, MOVE = 2, SHAKE = 1;
        public AnimationClip aniIdie, aniMove, aniShake;

        public bool IsMoving;

        [Space(30)]
        public WeaponSystems WEAPON_MANAGER;
        public DefenseSystems DEFENSE_MANAGER;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;

        }

        private void Start()
        {
            //contractor
            WEAPON_MANAGER.Init(WeaponController.Instance.equipedWeaponList);
            DEFENSE_MANAGER.Init();


            objHandToThrow.SetActive(false);

            EventController.OnWeaponNoBullet += HandleNoBullet;
            EventController.OnZombieAttack += DEFENSE_MANAGER.HandleZombieAttack;

        }



        private void Update()
        {
            if (IsMoving)
            {
                PlayAnimator(EnumController.SOLDIER_STATUS.walk);
            }

        }

        [Space(30)]
        public Transform _tranOfSoldier;
        public Transform _tranOfBody;

        // [HideInInspector]
        private HandOfWeapon m_HandOfWeapon;

        //public SpriteRenderer renWeapon;
        // Start is called before the first frame update

        public Vector2 GetCurrentPos()
        {
            return _tranOfSoldier.position;
        }

        //animator
        public void PlayAnimator(EnumController.SOLDIER_STATUS _status)
        {
            switch (_status)
            {
                case EnumController.SOLDIER_STATUS.idie:
                    // m_Animator.Play(aniIdie.name, -1, 0);
                    m_Animator.SetInteger("AnimationState", IDIE);
                    break;
                case EnumController.SOLDIER_STATUS.shooting:
                    m_Animator.Play(aniShake.name, -1, 0);
                    m_Animator.SetInteger("AnimationState", SHAKE);
                    break;
                case EnumController.SOLDIER_STATUS.walk:
                    // m_Animator.Play(aniMove.name, -1, 0);
                    m_Animator.SetInteger("AnimationState", MOVE);
                    break;

            }

        }





        //CHANGE WEAPON
        public void SetPosForHandGun(GameObject _handgun)
        {
            _handgun.transform.SetParent(_tranOfBody);
            _handgun.transform.localPosition = new Vector3(-0.847f, 1.696f, 0);
        }





        //Animation throw
        [SerializeField] private SpriteRenderer sprSpriteOfItemsSupport;
        public void PlayerAnimationThrow(EnumController.SUPPORT _support)
        {
            sprSpriteOfItemsSupport.sprite = SpriteController.Instance.GetSupportSprite(_support)._sprite;
            StartCoroutine(IePlayerAnimationThrow());
        }
        private IEnumerator IePlayerAnimationThrow()
        {
            SoundController.Instance.Play(SoundController.SOUND.sfx_throw);//sound
            objHandToThrow.SetActive(true);
            WEAPON_MANAGER.CURRENT_WEAPON.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            objHandToThrow.SetActive(false);
            WEAPON_MANAGER.CURRENT_WEAPON.gameObject.SetActive(true);

        }


        //CHANGE WEAPON
        private void HandleNoBullet(GunData _gundata)
        {
            WEAPON_MANAGER.ChooseWeapon(WeaponController.Instance.equipedWeaponList[0].DATA.eWeapon);//cho khau dau tien
        }




        private void OnDisable()
        {
            // DEFENSE_MANAGER.Save();
            EventController.OnWeaponNoBullet -= HandleNoBullet;
            EventController.OnZombieAttack -= DEFENSE_MANAGER.HandleZombieAttack;
        }

    }



//EDUIPED WEAPON FOR SOLDIER
    [System.Serializable]
    public class WeaponSystems
    {
        public GunData CURRENT_GUN_DATA;
        public HandOfWeapon CURRENT_WEAPON;

        //INIT
        public void Init(List<GunData> _list)
        {
            int _total = _list.Count;
            for (int i = 0; i < _total; i++)
            {
                Weapon _new = new Weapon(_list[i]);
                LIST_WEAPON.Add(_new);
            }

        }



        #region WEAPON
        [System.Serializable]
        public class Weapon
        {
            public GunData GUN_DATA;
            public GameObject objHandWeapon;


            //init
            public Weapon(GunData _gundata)
            {
                GUN_DATA = _gundata;
                Init();
            }


            public void Init()
            {
                objHandWeapon = Soldier.Instantiate(GUN_DATA.objPrefabHand);
                objHandWeapon.SetActive(false);
                Soldier.Instance.SetPosForHandGun(objHandWeapon);//set pos
            }
        }

        public List<Weapon> LIST_WEAPON = new List<Weapon>();


        //Get weapon
        public void ChooseWeapon(EnumController.WEAPON _weapon)
        {
            int _total = LIST_WEAPON.Count;
            for (int i = 0; i < _total; i++)
            {
                if (LIST_WEAPON[i].GUN_DATA.DATA.eWeapon == _weapon)
                {
                    LIST_WEAPON[i].objHandWeapon.SetActive(true);
                    CURRENT_WEAPON = LIST_WEAPON[i].objHandWeapon.GetComponent<HandOfWeapon>();
                    CURRENT_GUN_DATA = LIST_WEAPON[i].GUN_DATA;


                    //changed gun
                    EventController.OnChangedWeaponInvoke(CURRENT_GUN_DATA);



                    //get data               
                    CURRENT_WEAPON.fTimeloadOfBullet = CURRENT_GUN_DATA.fTimeloadOrBullet;
                    CURRENT_WEAPON.iDamageOfGun = CURRENT_GUN_DATA.GetDamage(CURRENT_GUN_DATA.DATA.eLevel);
                    CURRENT_WEAPON.fRangeOfGBullet = CURRENT_GUN_DATA.fRangeOfBullet;
                    CURRENT_WEAPON.sprBullet = CURRENT_GUN_DATA.sprBullet;
                    CURRENT_WEAPON.vScaleOfBullet = CURRENT_GUN_DATA.vScaleOfBullet;
                
                }
                else
                {
                    LIST_WEAPON[i].objHandWeapon.SetActive(false);
                }
            }
        }

        #endregion



    }


//DEFENSE SYSTEM
    [System.Serializable]
    public class DefenseSystems
    {
        [System.Serializable]
        public class UnitDefense
        {
            public EnumController.DEFENSE _defense;
            public float fDefenseValue;
            public GameObject objDefense;
        }
        private int iTotalDefenseUnit;

        public List<UnitDefense> LIST_DEFENSE = new List<UnitDefense>();
        public UnitDefense GetDefense(EnumController.DEFENSE _defense)
        {

            for (int i = 0; i < iTotalDefenseUnit; i++)
            {
                if (_defense == LIST_DEFENSE[i]._defense)
                {
                    return LIST_DEFENSE[i];
                }
            }
            return null;
        }
        private int iIndexOfFireOfHome = 1;
        // total current defense
        private float GetCurrentDefense()
        {
            fCurrentDefense = 0;
            for (int i = 0; i < iTotalDefenseUnit; i++)
            {
                fCurrentDefense += LIST_DEFENSE[i].fDefenseValue;
            }

            return fCurrentDefense;
        }

        private float fTotalDefense;
        private float fCurrentDefense;




        public void Init()
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
                    LIST_DEFENSE.Add(_unit);
                }
            }

            iTotalDefenseUnit = LIST_DEFENSE.Count;
            fTotalDefense = GetCurrentDefense();

            GameplayController.Instance.ShowHpBar(1.0f, (int)fCurrentDefense + "/" + (int)fTotalDefense);
        }



        //REDUCE 
        public void Reduce(float _damage)
        {
            for (int i = 0; i < iTotalDefenseUnit; i++)
            {
                if (LIST_DEFENSE[i].fDefenseValue >= _damage)
                {
                    LIST_DEFENSE[i].fDefenseValue -= _damage;
                    _damage = 0;


                }
                else if (LIST_DEFENSE[i].fDefenseValue > 0 && LIST_DEFENSE[i].fDefenseValue < _damage)
                {
                    _damage -= LIST_DEFENSE[i].fDefenseValue;
                    LIST_DEFENSE[i].fDefenseValue = 0;
                    //  LIST_DEFENSE[i].objDefense.SetActive(false);
                }
            }



            //show bar
            fCurrentDefense = GetCurrentDefense();
            if (fCurrentDefense <= 0)
            {
                fCurrentDefense = 0;
                GameplayController.Instance.SetStatusOfGame(GameplayController.GAME_STATUS.gameover);//game over
            }
            GameplayController.Instance.ShowHpBar(GetFactorDefense(), (int)fCurrentDefense + "/" + (int)fTotalDefense);//show bar


            //FIRE
            if (GetFactorDefense() < 0.6f && iIndexOfFireOfHome == 1)
            {
                Soldier.Instantiate(ObjectPoolController.Instance._fireOnHomeLevel1);//fire of home
                iIndexOfFireOfHome = 2;
            }
            if (GetFactorDefense() < 0.4f && iIndexOfFireOfHome == 2)
            {
                Soldier.Instantiate(ObjectPoolController.Instance._fireOnHomeLevel2);//fire of home
                iIndexOfFireOfHome = 3;
            }
            if (GetFactorDefense() < 0.3f && iIndexOfFireOfHome == 3)
            {
                Soldier.Instantiate(ObjectPoolController.Instance._fireOnHomeLevel3);//fire of home
                iIndexOfFireOfHome = 4;
            }
        }


        ////save Current defense value
        ////public void Save()
        ////{
        ////    for (int i = 0; i < iTotalDefenseUnit; i++)
        ////    {
        ////        TheDataManager.Instance.THE_DATA_PLAYER.GetDefense(LIST_DEFENSE[i]._defense).iCurrentDefenseValue
        ////            = LIST_DEFENSE[i].iDefenseValue;
        ////    }
        ////}


        //Facor of defense
        public float GetFactorDefense()
        {
            return fCurrentDefense / fTotalDefense;
        }

        //ZOMBIE ATTACK
        public void HandleZombieAttack(float _damage)
        {
            Soldier.Instance.DEFENSE_MANAGER.Reduce(_damage);
        }


    }
}