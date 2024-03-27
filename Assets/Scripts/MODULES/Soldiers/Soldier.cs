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
            WEAPON_MANAGER.Init(TheWeaponManager.Instance.LIST_EQUIPED_WEAPON);
            DEFENSE_MANAGER.Init();


            objHandToThrow.SetActive(false);

            TheEventManager.OnWeaponNoBullet += HandleNoBullet;
            TheEventManager.OnZombieAttack += DEFENSE_MANAGER.HandleZombieAttack;

        }



        private void Update()
        {
            if (IsMoving)
            {
                PlayAnimator(TheEnumManager.SOLDIER_STATUS.walk);
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
        public void PlayAnimator(TheEnumManager.SOLDIER_STATUS _status)
        {
            switch (_status)
            {
                case TheEnumManager.SOLDIER_STATUS.idie:
                    // m_Animator.Play(aniIdie.name, -1, 0);
                    m_Animator.SetInteger("AnimationState", IDIE);
                    break;
                case TheEnumManager.SOLDIER_STATUS.shooting:
                    m_Animator.Play(aniShake.name, -1, 0);
                    m_Animator.SetInteger("AnimationState", SHAKE);
                    break;
                case TheEnumManager.SOLDIER_STATUS.walk:
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
        public void PlayerAnimationThrow(TheEnumManager.SUPPORT _support)
        {
            sprSpriteOfItemsSupport.sprite = TheSpriteManager.Instance.GetSpriteOfSupport(_support).spriSprite;
            StartCoroutine(IePlayerAnimationThrow());
        }
        private IEnumerator IePlayerAnimationThrow()
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_throw);//sound
            objHandToThrow.SetActive(true);
            WEAPON_MANAGER.CURRENT_WEAPON.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            objHandToThrow.SetActive(false);
            WEAPON_MANAGER.CURRENT_WEAPON.gameObject.SetActive(true);

        }


        //CHANGE WEAPON
        private void HandleNoBullet(GunData _gundata)
        {
            WEAPON_MANAGER.ChooseWeapon(TheWeaponManager.Instance.LIST_EQUIPED_WEAPON[0].DATA.eWeapon);//cho khau dau tien
        }




        private void OnDisable()
        {
            // DEFENSE_MANAGER.Save();
            TheEventManager.OnWeaponNoBullet -= HandleNoBullet;
            TheEventManager.OnZombieAttack -= DEFENSE_MANAGER.HandleZombieAttack;
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
        public void ChooseWeapon(TheEnumManager.WEAPON _weapon)
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
                    TheEventManager.Weapon_OnChangedWeapon(CURRENT_GUN_DATA);



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
            public TheEnumManager.DEFENSE _defense;
            public float fDefenseValue;
            public GameObject objDefense;
        }
        private int iTotalDefenseUnit;

        public List<UnitDefense> LIST_DEFENSE = new List<UnitDefense>();
        public UnitDefense GetDefense(TheEnumManager.DEFENSE _defense)
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

            int _total = TheWeaponManager.Instance.LIST_DEFENSE.Count;
            for (int i = 0; i < _total; i++)
            {
                if (TheWeaponManager.Instance.LIST_DEFENSE[i].DATA.bEquiped)
                {
                    UnitDefense _unit = new UnitDefense();
                    _unit._defense = TheWeaponManager.Instance.LIST_DEFENSE[i].DATA.eDefense;
                    _unit.fDefenseValue = TheWeaponManager.Instance.LIST_DEFENSE[i].GetDefense(TheWeaponManager.Instance.LIST_DEFENSE[i].DATA.eLevel);

                    if (!TheWeaponManager.Instance.LIST_DEFENSE[i].DATA.bDefault)
                        _unit.objDefense = Soldier.Instantiate(TheWeaponManager.Instance.LIST_DEFENSE[i].bjPrefab);
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
                Soldier.Instantiate(TheObjectPoolingManager.Instance.preabFireOfHome_level1);//fire of home
                iIndexOfFireOfHome = 2;
            }
            if (GetFactorDefense() < 0.4f && iIndexOfFireOfHome == 2)
            {
                Soldier.Instantiate(TheObjectPoolingManager.Instance.preabFireOfHome_level2);//fire of home
                iIndexOfFireOfHome = 3;
            }
            if (GetFactorDefense() < 0.3f && iIndexOfFireOfHome == 3)
            {
                Soldier.Instantiate(TheObjectPoolingManager.Instance.preabFireOfHome_level3);//fire of home
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