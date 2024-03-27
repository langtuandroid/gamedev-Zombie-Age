using System.Collections;
using _4_Gameplay;
using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;

namespace MODULES.Zombies
{
    public class Zombie : MonoBehaviour
    {
        public TheEnumManager.ZOMBIE_STATUS eStatus;
        public ZombieData DATA;
        public ItemsSystem ITEM_SYSTEM;
        public ZombieHealth HEALTH;



        private GameObject _gameobject;
        protected Transform _tranOfThis;
        public Transform _tranOfHpBar;
        public Transform _tranOfCenter;
        public Transform _tranOfFreeze;
        public Animator m_animator;

        //Info-----------------------------------
        public bool ALIVE = false;
        public bool SPECIAL_STATUS = false;
        protected bool IsFreezing; //dang bi dong băng


        private float fSpeedMove;
        public float fCURRENT_SPEED
        {
            get { return fSpeedMove; }
            set { fSpeedMove = value; }
        }
        private float fReloadAttackTime;


        [Space(30)]
        public SpriteRenderer sprItem_Hat;
        public SpriteRenderer sprItem_Shield;
        public SpriteRenderer sprItem_Weapon;


        [Header("_____ TINH CHINH______")]
        public Vector3 vScaleOfFree;
        public Vector3 vScaleOfFire;


        private void Awake()
        {
            _tranOfThis = transform;
            _gameobject = gameObject;

            ITEM_SYSTEM = new ItemsSystem(this);
        }


        public virtual void Init()
        {

        }


        public void Init(Vector2 _startingPos, bool _specialStatus = false)
        {

            int _randSound = Random.Range(0, 100);
            if (_randSound < 50) TheSoundManager.Instance.ZombieGruzz();//sound

            TheEventManager.ZombieEvent_Born(this);//event
            ALIVE = true;
            SPECIAL_STATUS = _specialStatus;
            bMoveBack = false;

            //Get data config
            fSpeedMove = DATA.GetSpeed() * Random.Range(0.5f, 1.5f);
            fReloadAttackTime = DATA.fReloadAttackTime;
            _tranOfThis.position = _startingPos;
            _tranOfThis.localScale = Vector3.one;
            m_animator.speed = 1;
            vTargetPos = new Vector2(-20, _startingPos.y);

            if (!DATA.bIsBoss && SPECIAL_STATUS)//Trạng thái đặc biệt: di chuyển nhanh hơn, to hơn
            {
                _tranOfThis.localScale = Vector3.one * 1.5f;
                m_animator.speed = 2.5f;

                if (DATA.bIsFlying) fSpeedMove = DATA.GetSpeed() * 2.5f;
                else fSpeedMove = DATA.GetSpeed() * 4.0f;

            }
            //-------------
            Init();

            SetStatus(TheEnumManager.ZOMBIE_STATUS.moving);

        }


        // Update is called once per frame
        void Update()
        {
            if (!ALIVE) return;
            if (eStatus == TheEnumManager.ZOMBIE_STATUS.moving)
                Move();
        }



        //SET STATUS
        GameObject _effBlood;
        public void SetStatus(TheEnumManager.ZOMBIE_STATUS _status)
        {
            if (!ALIVE) return;
            eStatus = _status;
            switch (_status)
            {
                case TheEnumManager.ZOMBIE_STATUS.moving:
                    AnimatorPlay(TheEnumManager.ZOMBIE_STATUS.moving);
                    break;
                case TheEnumManager.ZOMBIE_STATUS.die:
                    ALIVE = false;
                    StopAllCoroutines();
                    CancelInvoke();
                    AnimatorPlay(TheEnumManager.ZOMBIE_STATUS.die);

                    if (!DATA.bIsFlying)
                    {
                        //blood
                        _effBlood = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.blood_of_zombie).GetObject();
                        _effBlood.transform.position = _tranOfThis.position;
                        _effBlood.SetActive(true);

                    }


                    //Blood Exploison
                    _effBlood = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.zombie_blood_exploison).GetObject();
                    _effBlood.transform.position = _tranOfCenter.position;
                    _effBlood.SetActive(true);

                    TheSoundManager.Instance.ZombieExplosion();//sound

                    //remove freeze
                    RemoveObjFreeze();

                    //BOSS
                    if (DATA.bIsBoss)
                    {
                        m_animator.speed = 0.0f;
                        Instantiate(TheObjectPoolingManager.Instance.prefabBigBlood, _tranOfCenter.position, Quaternion.identity);//eff blood    
                        SetBossActive(_gameobject, false, 3.0f);
                        return;
                    }
                    else
                    {
                        TheEventManager.ZombieEvent_Die(this);//event
                        _tranOfThis.position = new Vector2(100, 100);
                        _gameobject.SetActive(false);

                    }
                    break;
                case TheEnumManager.ZOMBIE_STATUS.attack:
                    CancelInvoke("Attack");
                    InvokeRepeating("Attack", DATA.fReloadAttackTime, DATA.fReloadAttackTime);
                    AnimatorPlay(TheEnumManager.ZOMBIE_STATUS.attack);
                    break;

            }
        }


        //PLAY ANIMATOWR
        protected virtual void AnimatorPlay(TheEnumManager.ZOMBIE_STATUS _status)
        {

        }



        //SET ACTIVE
        public void SetBossActive(GameObject _object, bool _active, float _timedelay)
        {
            StartCoroutine(IeSetBossActive(_object, _active, _timedelay));
        }
        private IEnumerator IeSetBossActive(GameObject _object, bool _active, float _timedelay)
        {
            yield return new WaitForSeconds(_timedelay);

            Instantiate(TheObjectPoolingManager.Instance.prefabExplosionBoss, _tranOfCenter.position, Quaternion.identity);//explosion
            Instantiate(TheObjectPoolingManager.Instance.prefabBloodBoss, vCURRENT_POS, Quaternion.identity);//bloss
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_zombie_gruzz_boss);//sound
            TheEventManager.ZombieEvent_Die(this);//event
            _object.SetActive(_active);
        }



        #region  MOVE
        protected Vector3 vCurrentPos;
        public Vector3 vCURRENT_POS
        {
            get { return vCurrentPos; }
            set { vCurrentPos = value; }
        }
        protected Vector3 vTargetPos;
        private bool bMoveBack;

        public virtual void Move()
        {
            vCurrentPos = _tranOfThis.position;
            if (!bMoveBack)
                vCurrentPos = Vector2.MoveTowards(vCurrentPos, vTargetPos, fSpeedMove * Time.deltaTime);
            else
                vCurrentPos = Vector2.MoveTowards(vCurrentPos, -vTargetPos, 3 * fSpeedMove * Time.deltaTime);

            vCurrentPos.z = vCurrentPos.y;
            _tranOfThis.position = vCurrentPos;
        }


        WaitForSeconds _waitmoveback = new WaitForSeconds(0.1f);
        private IEnumerator IeMoveBack()
        {
            if (eStatus == TheEnumManager.ZOMBIE_STATUS.moving)
            {
                bMoveBack = true;
                yield return _waitmoveback;
                bMoveBack = false;
            }
        }

        #endregion

        #region ATTACK
        public void Attack()
        {
            if (MainCode_Gameplay.Instance.eGameStatus != MainCode_Gameplay.GAME_STATUS.playing) return;
            if (IsFreezing) return;

            //play sound
            if (!DATA.bIsFlying) TheSoundManager.Instance.ZombieAttack();//sound
            TheEventManager.ZombieEvent_OnZombieAttack(HEALTH.GetDamage());
        }


        //Ném đồ vât
        public virtual void Throw()
        {
            // [?]
        }

        #endregion

        #region  HIT BULLET
        private float _disFromBullet;
        private float _disToGetDamage;// khoan cach de dinh dan
        public void HitBullet(TheEnumManager.WEAPON _weapon, Vector2 _posOfBullet, float _range, int _damage)
        {
            if (!ALIVE) return;
            _disFromBullet = Vector2.Distance(_tranOfCenter.position, _posOfBullet);
            //for boss
            if (DATA.bIsBoss && _disFromBullet > _range * 1.5f) return;
            if (!DATA.bIsBoss && _disFromBullet > _range) return;

            if (_weapon != TheEnumManager.WEAPON.firegun)
            {
                StartCoroutine(IeMoveBack());
                HEALTH.ReduceHp(_damage); //trừ máu     
            }
            else // fire gun: Trừ theo thời gian.
            {
                StartCoroutine(IeHitFire(_damage));
            }
        }




        WaitForSeconds _delay = new WaitForSeconds(0.25f);
        bool _hitFire;
        GameObject _fire;
        private IEnumerator IeHitFire(int _damage)
        {
            if (_hitFire) yield break;
            _hitFire = true;

            //EFFECT
            _fire = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.fire_of_enemies).GetObject();
            _fire.transform.localScale = vScaleOfFire;
            if (SPECIAL_STATUS)
                _fire.transform.localScale *= 1.3f;

            _fire.transform.SetParent(_tranOfCenter);
            _fire.transform.localPosition = Vector3.zero;
            _fire.SetActive(true);
            for (int i = 0; i < 15; i++)
            {
                HEALTH.ReduceHp(_damage / 15);
                yield return _delay;
            }
            RemoveFireOfFiregun();
        }
        private void RemoveFireOfFiregun()
        {
            _hitFire = false;
            if (_fire)
            {
                _fire.transform.SetParent(null);
                _fire.gameObject.SetActive(false);

            }
        }
        #endregion




        //SHOW HP BAR
        Vector3 vHpScale = new Vector3(1, 0.9f, 1);
        public void ShowHpBar(float _factor)
        {
            if (!DATA.bIsBoss)
            {
                if (_tranOfHpBar == null) return;
                vHpScale.x = _factor;
                _tranOfHpBar.localScale = vHpScale;
            }
            else
            {
                //is boss
                MainCode_Gameplay.Instance.m_BossHpBar.ShowBar(_factor);
            }
        }



        //CHECK ZOMBIE ON GAMEPLAY
        private bool CheckZombieOnGameplay()//Zombie nằm trong gameplay hay chưa?
        {
            if (vCurrentPos.x > 9.5f) return false;
            return true;
        }



        private void OnEnable()
        {
            //  Init(transform.position);
            TheEventManager.OnBulletCompleted += HitBullet;
            TheEventManager.OnSupportCompleted += EffectFromSupport;
        }
        private void OnDisable()
        {
            TheEventManager.OnBulletCompleted -= HitBullet;
            TheEventManager.OnSupportCompleted -= EffectFromSupport;
            CancelInvoke();
            StopAllCoroutines();
            RemoveFireOfFiregun();

        }



        #region  PLAYER'S SUPPORT EFFECT

        // SUPPORTER FROM SOLDIER
        float _disFromSupportPos;
        float _disOfSupport;
        SupportData _supportdata;

        public void EffectFromSupport(TheEnumManager.SUPPORT _support, Vector2 _pos)
        {
            _supportdata = TheWeaponManager.Instance.GetSupport(_support);
            _disFromSupportPos = Vector2.Distance(vCURRENT_POS, _pos);
            _disOfSupport = _supportdata.GetRange();

            if (_disFromSupportPos > _disOfSupport) return;


            //content
            switch (_support)
            {
                case TheEnumManager.SUPPORT.grenade:
                    HEALTH.ReduceHp(_supportdata.GetDamage());
                    break;

                case TheEnumManager.SUPPORT.freeze:
                    StartCoroutine(IeGetFreeze(_supportdata.GetTime()));
                    break;


                case TheEnumManager.SUPPORT.poison:
                    StartCoroutine(IeGetPoison(_supportdata.GetTime(), _supportdata.GetDamage()));

                    break;
                case TheEnumManager.SUPPORT.big_bomb:
                    HEALTH.ReduceHp(_supportdata.GetDamage());

                    break;
            }
        }

        //Get poison
        WaitForSeconds _wait = new WaitForSeconds(0.8f);
        private IEnumerator IeGetPoison(float _time, int _basedamage)
        {
            float _unitdamage = HEALTH.iOriginalHp * 0.01f+_basedamage; // đốt 1% sau 1 giây
            for (int i = 0; i < _time; i++)
            {
                yield return _wait;
                HEALTH.ReduceHp(_unitdamage);
            }
        }


        //Get freeze
        GameObject _effectFreeze = null;
        private IEnumerator IeGetFreeze(float _time)
        {
            if (fCURRENT_SPEED != 0)
            {
                IsFreezing = true;

                //EFFECT
                _effectFreeze = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.effect_freeze).GetObject();
                _effectFreeze.transform.localScale = vScaleOfFree;

                if (SPECIAL_STATUS)
                    _effectFreeze.transform.localScale *= 1.3f;

                _effectFreeze.transform.position = _tranOfFreeze.position;
                _effectFreeze.transform.SetParent(_tranOfFreeze);
                _effectFreeze.SetActive(true);

                float _curentspeed = fCURRENT_SPEED;
                fCURRENT_SPEED = 0;
                m_animator.speed = 0;


                yield return new WaitForSeconds(_time);

                RemoveObjFreeze();

                fCURRENT_SPEED = _curentspeed;
                m_animator.speed = 1;
                IsFreezing = false;
            }
        }

        //Remove prefab freeze
        public void RemoveObjFreeze()
        {
            IsFreezing = false;

            if (_effectFreeze && _effectFreeze.activeInHierarchy)
            {

                _effectFreeze.transform.SetParent(null);
                _effectFreeze.SetActive(false);
                _effectFreeze = null;


            }
        }
        #endregion


    }





// BLOOD + ATTACK
    [System.Serializable]
    public class ZombieHealth
    {
        private Zombie ZOMBIE;

        public float iHP; // hp cua zombie
        public float iItemHat_Hp;//hp cua HAT
        public float iItemShield_Hp;//hp cua shield;

        public float iOriginalHp;//Hp ban dau
        private float iCurrentTotalHeath;

        [SerializeField] private float fDamage;

        public ZombieHealth(Zombie _zombie)
        {
            if (_zombie == null) return;
            ZOMBIE = _zombie;
            fDamage = _zombie.DATA.GetDamage();



            iHP = _zombie.DATA.GetHp(TheDataManager.Instance.THE_DATA_PLAYER.iCurrentLevel + 1, TheLevel.Instance.iCurrentWave);
            // if (_zombie.SPECIAL_STATUS) iHP = (int)(iHP * 0.5f);//Nếu ở trạng thái special, thì hp giảm 1 nửa


            if (_zombie.ITEM_SYSTEM.eHat != TheEnumManager.HAT_OF_ZOMBIE.NO_HAT) iItemHat_Hp = (int)(iHP * 0.3f);
            if (_zombie.ITEM_SYSTEM.eShield != TheEnumManager.SHIELD_OF_ZOMBIE.NO_SHIELD) iItemShield_Hp = (int)(iHP * 0.5f);
            if (_zombie.ITEM_SYSTEM.eWeapon != TheEnumManager.WEAPON_OF_ZOMBIE.NO_WEAPON) fDamage += fDamage * 0.5f;

            //total
            iCurrentTotalHeath = iHP + iItemHat_Hp + iItemShield_Hp;
            iOriginalHp = iCurrentTotalHeath;
            _zombie.ShowHpBar(1.0f);
        }




        public float GetDamage()
        {
            return fDamage;
        }





        //Giam mau
        GameObject _textBlood = null;
        public void ReduceHp(float _value)
        {
            //show text
            _textBlood = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.text_blood).GetObject();
            _textBlood.GetComponent<TextValue>().SetValue((int)_value);
            _textBlood.transform.position = ZOMBIE._tranOfCenter.position;
            _textBlood.SetActive(true);

            //shield
            if (iItemShield_Hp > 0)
            {
                if (_value >= iItemShield_Hp)
                {
                    _value -= iItemShield_Hp;
                    iItemShield_Hp = 0;
                    ZOMBIE.ITEM_SYSTEM.RemoveShield();
                }
                else
                {
                    iItemShield_Hp -= _value;
                    _value = 0;
                }
            }


            //hat
            if (iItemHat_Hp > 0)
            {
                if (_value >= iItemHat_Hp)
                {
                    _value -= iItemHat_Hp;
                    iItemHat_Hp = 0;
                    ZOMBIE.ITEM_SYSTEM.RemoveHat();
                }
                else
                {
                    iItemHat_Hp -= _value;
                    _value = 0;
                }
            }


            //base
            if (_value >= iHP)
            {
                iHP = 0;
                ZOMBIE.SetStatus(TheEnumManager.ZOMBIE_STATUS.die);
            }
            else
            {
                iHP -= _value;

            }



            //show bar
            ZOMBIE.ShowHpBar(GetFactorOfHp());

        }





        //For Bar Hp show
        public float GetFactorOfHp()
        {
            iCurrentTotalHeath = iHP + iItemHat_Hp + iItemShield_Hp;
            return iCurrentTotalHeath * 1.0f / iOriginalHp;
        }
    }





//ITEM SUPPORT
    [System.Serializable]
    public class ItemsSystem
    {
        public ItemsSystem(Zombie _zombie)
        {
            ZOMBIE = _zombie;

            sprItem_Hat = ZOMBIE.sprItem_Hat;
            sprItem_Shield = ZOMBIE.sprItem_Shield;
            sprItem_Weapon = ZOMBIE.sprItem_Weapon;
        }

        private Zombie ZOMBIE;

        public TheEnumManager.HAT_OF_ZOMBIE eHat;
        public TheEnumManager.WEAPON_OF_ZOMBIE eWeapon;
        public TheEnumManager.SHIELD_OF_ZOMBIE eShield;


        private SpriteRenderer sprItem_Hat;
        private SpriteRenderer sprItem_Shield;
        private SpriteRenderer sprItem_Weapon;



        public void SetItem(TheEnumManager.HAT_OF_ZOMBIE _item)
        {
            if (sprItem_Hat == null)
            {
                eHat = TheEnumManager.HAT_OF_ZOMBIE.NO_HAT;
                return;
            }


            eHat = _item;
            if (_item == TheEnumManager.HAT_OF_ZOMBIE.NO_HAT)
            {
                sprItem_Hat.gameObject.SetActive(false);
                return;
            }
            if (sprItem_Hat != null)
            {
                sprItem_Hat.sprite = TheSpriteManager.Instance.ITEM_FOR_ZOMBIE.GetHat(_item).sprSprite;
                sprItem_Hat.gameObject.SetActive(true);
            }
        }
        public void SetItem(TheEnumManager.WEAPON_OF_ZOMBIE _item)
        {
            if (sprItem_Weapon == null)
            {
                eWeapon = TheEnumManager.WEAPON_OF_ZOMBIE.NO_WEAPON;
                return;
            }

            eWeapon = _item;
            if (_item == TheEnumManager.WEAPON_OF_ZOMBIE.NO_WEAPON)
            {
                sprItem_Weapon.gameObject.SetActive(false);
                return;
            }
            if (sprItem_Weapon != null)
            {
                sprItem_Weapon.sprite = TheSpriteManager.Instance.ITEM_FOR_ZOMBIE.GetWeapon(_item).sprSprite;
                sprItem_Weapon.gameObject.SetActive(true);
            }
        }
        public void SetItem(TheEnumManager.SHIELD_OF_ZOMBIE _item)
        {
            if (sprItem_Shield == null)
            {
                eShield = TheEnumManager.SHIELD_OF_ZOMBIE.NO_SHIELD;
                return;
            }


            eShield = _item;
            if (_item == TheEnumManager.SHIELD_OF_ZOMBIE.NO_SHIELD)
            {
                sprItem_Shield.gameObject.SetActive(false);
                return;
            };
            if (sprItem_Shield != null)
            {
                sprItem_Shield.sprite = TheSpriteManager.Instance.ITEM_FOR_ZOMBIE.GetShield(_item).sprSprite;
                sprItem_Shield.gameObject.SetActive(true);
            }
        }



        GameObject _effect = null;
        public void RemoveHat()
        {
            if (eHat == TheEnumManager.HAT_OF_ZOMBIE.NO_HAT) return;
            eHat = TheEnumManager.HAT_OF_ZOMBIE.NO_HAT;

            sprItem_Hat.gameObject.SetActive(false);
            //effect
            _effect = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.remove_item).GetObject();
            _effect.transform.position = sprItem_Hat.transform.position;
            _effect.GetComponent<RemoveItem>().SetItem(sprItem_Hat.sprite);//add sprite
            _effect.SetActive(true);

        }

        public void RemoveShield()
        {
            if (eShield == TheEnumManager.SHIELD_OF_ZOMBIE.NO_SHIELD) return;
            eShield = TheEnumManager.SHIELD_OF_ZOMBIE.NO_SHIELD;

            sprItem_Shield.gameObject.SetActive(false);
            //explosion
            _effect = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.remove_item).GetObject();
            _effect.transform.position = sprItem_Shield.transform.position;
            _effect.GetComponent<RemoveItem>().SetItem(sprItem_Shield.sprite);//add sprite
            _effect.SetActive(true);

        }

    }
}