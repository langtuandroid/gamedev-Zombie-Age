using System;
using System.Collections;
using _4_Gameplay;
using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace MODULES.Zombies
{
    public class Zombie : MonoBehaviour
    {
        [Inject] private WeaponController _weaponController;
        [Inject] protected SoundController SoundController;
        [Inject] private GameplayController _gameplayController;
        [FormerlySerializedAs("eStatus")] public EnumController.ZOMBIE_STATUS _zombieStatus;
        [FormerlySerializedAs("DATA")] public ZombieData _zombieData;
        [FormerlySerializedAs("ITEM_SYSTEM")] public ItemsSystem _itemsSystem;
        [FormerlySerializedAs("HEALTH")] public ZombieHealth _health;
        
        private GameObject _gameobject;
        protected Transform _Transform;
        [FormerlySerializedAs("_tranOfHpBar")] public Transform _transformHPBar;
        [FormerlySerializedAs("_tranOfCenter")] public Transform _transformCenter;
        [FormerlySerializedAs("_tranOfFreeze")] public Transform _transformFreeze;
        [FormerlySerializedAs("m_animator")] public Animator Animator;
        
        [FormerlySerializedAs("ALIVE")] public bool _isAlive;
        [FormerlySerializedAs("SPECIAL_STATUS")] public bool _isSpecial;
        protected bool IsFreezing;


        private float _moveSpeed;

        protected float Speed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }
        

        [Space(30)] 
        [FormerlySerializedAs("sprItem_Hat")]public SpriteRenderer _hatSprite;
        [FormerlySerializedAs("sprItem_Shield")] public SpriteRenderer _shieldSprite;
        [FormerlySerializedAs("sprItem_Weapon")] public SpriteRenderer _weaponSprite;


        [Header("_____ TINH CHINH______")]
        [FormerlySerializedAs("vScaleOfFree")] public Vector3 _scaleFree;
        [FormerlySerializedAs("vScaleOfFire")] public Vector3 _scaleFire;


        private void Awake()
        {
            _Transform = transform;
            _gameobject = gameObject;

            _itemsSystem = new ItemsSystem(this);
            _weaponController = WeaponController.Instance;
            SoundController = SoundController.Instance;
        }
        
        protected virtual void Construct()
        {

        }


        public void Constuct(Vector2 _startingPos, bool _specialStatus = false)
        {

            int _randSound = Random.Range(0, 100);
            if (_randSound < 50) SoundController.ZombieGruzz();//sound

            EventController.ZombieEvent_Born(this);//event
            _isAlive = true;
            _isSpecial = _specialStatus;
            bMoveBack = false;
            
            _moveSpeed = _zombieData.GetSpeed() * Random.Range(0.5f, 1.5f);
            _Transform.position = _startingPos;
            _Transform.localScale = Vector3.one;
            Animator.speed = 1;
            vTargetPos = new Vector2(-20, _startingPos.y);

            if (!_zombieData.bIsBoss && _isSpecial)
            {
                _Transform.localScale = Vector3.one * 1.5f;
                Animator.speed = 2.5f;

                if (_zombieData.bIsFlying) _moveSpeed = _zombieData.GetSpeed() * 2.5f;
                else _moveSpeed = _zombieData.GetSpeed() * 4.0f;

            }

            Construct();

            ChangeStatus(EnumController.ZOMBIE_STATUS.moving);

        }

        private void Update()
        {
            if (!_isAlive) return;
            if (_zombieStatus == EnumController.ZOMBIE_STATUS.moving)
                Move();
        }

        private GameObject _bloodEffect;
        public void ChangeStatus(EnumController.ZOMBIE_STATUS _status)
        {
            if (!_isAlive) return;
            _zombieStatus = _status;
            switch (_status)
            {
                case EnumController.ZOMBIE_STATUS.moving:
                    PlayAnimator(EnumController.ZOMBIE_STATUS.moving);
                    break;
                case EnumController.ZOMBIE_STATUS.die:
                    _isAlive = false;
                    StopAllCoroutines();
                    CancelInvoke();
                    PlayAnimator(EnumController.ZOMBIE_STATUS.die);

                    if (!_zombieData.bIsFlying)
                    {
                        //blood
                        _bloodEffect = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.blood_of_zombie).Get();
                        _bloodEffect.transform.position = _Transform.position;
                        _bloodEffect.SetActive(true);

                    }

                    _bloodEffect = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.zombie_blood_exploison).Get();
                    _bloodEffect.transform.position = _transformCenter.position;
                    _bloodEffect.SetActive(true);

                    SoundController.ZombieExplosion();//sound

                    RemoveObjFreeze();

                    //BOSS
                    if (_zombieData.bIsBoss)
                    {
                        Animator.speed = 0.0f;
                        Instantiate(ObjectPoolController.Instance._bigBloodPrefab, _transformCenter.position, Quaternion.identity);//eff blood    
                        ActivateBoss(_gameobject, false, 3.0f);
                        return;
                    }

                    EventController.ZombieEvent_Die(this);//event
                    _Transform.position = new Vector2(100, 100);
                    _gameobject.SetActive(false);
                    break;
                case EnumController.ZOMBIE_STATUS.attack:
                    CancelInvoke("Attack");
                    InvokeRepeating("Attack", _zombieData.fReloadAttackTime, _zombieData.fReloadAttackTime);
                    PlayAnimator(EnumController.ZOMBIE_STATUS.attack);
                    break;

            }
        }

        protected virtual void PlayAnimator(EnumController.ZOMBIE_STATUS _status)
        {

        }

        private void ActivateBoss(GameObject _object, bool _active, float _timedelay)
        {
            StartCoroutine(IeSetBossActive(_object, _active, _timedelay));
        }
        private IEnumerator IeSetBossActive(GameObject _object, bool _active, float _timedelay)
        {
            yield return new WaitForSeconds(_timedelay);

            Instantiate(ObjectPoolController.Instance._explosionBossPrefab, _transformCenter.position, Quaternion.identity);//explosion
            Instantiate(ObjectPoolController.Instance._bossBloodPrefab, vCURRENT_POS, Quaternion.identity);//bloss
            SoundController.Play(SoundController.SOUND.sfx_zombie_gruzz_boss);//sound
            EventController.ZombieEvent_Die(this);//event
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
            vCurrentPos = _Transform.position;
            if (!bMoveBack)
                vCurrentPos = Vector2.MoveTowards(vCurrentPos, vTargetPos, _moveSpeed * Time.deltaTime);
            else
                vCurrentPos = Vector2.MoveTowards(vCurrentPos, -vTargetPos, 3 * _moveSpeed * Time.deltaTime);

            vCurrentPos.z = vCurrentPos.y;
            _Transform.position = vCurrentPos;
        }


        WaitForSeconds _waitmoveback = new WaitForSeconds(0.1f);
        private IEnumerator IeMoveBack()
        {
            if (_zombieStatus == EnumController.ZOMBIE_STATUS.moving)
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
            if (_gameplayController.GameStatus != GameplayController.GAME_STATUS.playing) return;
            if (IsFreezing) return;

            //play sound
            if (!_zombieData.bIsFlying) SoundController.ZombieAttack();//sound
            EventController.ZombieEvent_OnZombieAttack(_health.GetDamage());
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
        public void HitBullet(EnumController.WEAPON _weapon, Vector2 _posOfBullet, float _range, int _damage)
        {
            if (!_isAlive) return;
            _disFromBullet = Vector2.Distance(_transformCenter.position, _posOfBullet);
            //for boss
            if (_zombieData.bIsBoss && _disFromBullet > _range * 1.5f) return;
            if (!_zombieData.bIsBoss && _disFromBullet > _range) return;

            if (_weapon != EnumController.WEAPON.firegun)
            {
                StartCoroutine(IeMoveBack());
                _health.ReduceHp(_damage); //trừ máu     
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
            _fire = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.fire_of_enemies).Get();
            _fire.transform.localScale = _scaleFire;
            if (_isSpecial)
                _fire.transform.localScale *= 1.3f;

            _fire.transform.SetParent(_transformCenter);
            _fire.transform.localPosition = Vector3.zero;
            _fire.SetActive(true);
            for (int i = 0; i < 15; i++)
            {
                _health.ReduceHp(_damage / 15);
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
            if (!_zombieData.bIsBoss)
            {
                if (_transformHPBar == null) return;
                vHpScale.x = _factor;
                _transformHPBar.localScale = vHpScale;
            }
            else
            {
                //is boss
                _gameplayController.bossHpBar.Show(_factor);
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
            EventController.OnBulletCompleted += HitBullet;
            EventController.OnSupportCompleted += EffectFromSupport;
        }
        private void OnDisable()
        {
            EventController.OnBulletCompleted -= HitBullet;
            EventController.OnSupportCompleted -= EffectFromSupport;
            CancelInvoke();
            StopAllCoroutines();
            RemoveFireOfFiregun();

        }



        #region  PLAYER'S SUPPORT EFFECT

        // SUPPORTER FROM SOLDIER
        float _disFromSupportPos;
        float _disOfSupport;
        SupportData _supportdata;

        public void EffectFromSupport(EnumController.SUPPORT _support, Vector2 _pos)
        {
            _supportdata = _weaponController.Support(_support);
            _disFromSupportPos = Vector2.Distance(vCURRENT_POS, _pos);
            _disOfSupport = _supportdata.GetRange();

            if (_disFromSupportPos > _disOfSupport) return;


            //content
            switch (_support)
            {
                case EnumController.SUPPORT.grenade:
                    _health.ReduceHp(_supportdata.GetDamage());
                    break;

                case EnumController.SUPPORT.freeze:
                    StartCoroutine(IeGetFreeze(_supportdata.GetTime()));
                    break;


                case EnumController.SUPPORT.poison:
                    StartCoroutine(IeGetPoison(_supportdata.GetTime(), _supportdata.GetDamage()));

                    break;
                case EnumController.SUPPORT.big_bomb:
                    _health.ReduceHp(_supportdata.GetDamage());

                    break;
            }
        }

        //Get poison
        WaitForSeconds _wait = new WaitForSeconds(0.8f);
        private IEnumerator IeGetPoison(float _time, int _basedamage)
        {
            float _unitdamage = _health.iOriginalHp * 0.01f+_basedamage; // đốt 1% sau 1 giây
            for (int i = 0; i < _time; i++)
            {
                yield return _wait;
                _health.ReduceHp(_unitdamage);
            }
        }


        //Get freeze
        GameObject _effectFreeze = null;
        private IEnumerator IeGetFreeze(float _time)
        {
            if (Speed != 0)
            {
                IsFreezing = true;

                //EFFECT
                _effectFreeze = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.effect_freeze).Get();
                _effectFreeze.transform.localScale = _scaleFree;

                if (_isSpecial)
                    _effectFreeze.transform.localScale *= 1.3f;

                _effectFreeze.transform.position = _transformFreeze.position;
                _effectFreeze.transform.SetParent(_transformFreeze);
                _effectFreeze.SetActive(true);

                float _curentspeed = Speed;
                Speed = 0;
                Animator.speed = 0;


                yield return new WaitForSeconds(_time);

                RemoveObjFreeze();

                Speed = _curentspeed;
                Animator.speed = 1;
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
            fDamage = _zombie._zombieData.GetDamage();



            iHP = _zombie._zombieData.GetHp(DataController.Instance.playerData.CurrentLevel + 1, LevelController.Instance.CurrentWave);


            if (_zombie._itemsSystem.eHat != EnumController.HAT_OF_ZOMBIE.NO_HAT) iItemHat_Hp = (int)(iHP * 0.3f);
            if (_zombie._itemsSystem.eShield != EnumController.SHIELD_OF_ZOMBIE.NO_SHIELD) iItemShield_Hp = (int)(iHP * 0.5f);
            if (_zombie._itemsSystem.eWeapon != EnumController.WEAPON_OF_ZOMBIE.NO_WEAPON) fDamage += fDamage * 0.5f;

            //total
            iCurrentTotalHeath = iHP + iItemHat_Hp + iItemShield_Hp;
            iOriginalHp = iCurrentTotalHeath;
            _zombie.ShowHpBar(1.0f);
        }

        public float GetDamage()
        {
            return fDamage;
        }

        GameObject _textBlood = null;
        public void ReduceHp(float _value)
        {
            //show text
            _textBlood = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.text_blood).Get();
            _textBlood.GetComponent<TextValue>().SetText((int)_value);
            _textBlood.transform.position = ZOMBIE._transformCenter.position;
            _textBlood.SetActive(true);

            //shield
            if (iItemShield_Hp > 0)
            {
                if (_value >= iItemShield_Hp)
                {
                    _value -= iItemShield_Hp;
                    iItemShield_Hp = 0;
                    ZOMBIE._itemsSystem.RemoveShield();
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
                    ZOMBIE._itemsSystem.RemoveHat();
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
                ZOMBIE.ChangeStatus(EnumController.ZOMBIE_STATUS.die);
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
    
    [System.Serializable]
    public class ItemsSystem
    {
        public ItemsSystem(Zombie _zombie)
        {
            ZOMBIE = _zombie;

            sprItem_Hat = ZOMBIE._hatSprite;
            sprItem_Shield = ZOMBIE._shieldSprite;
            sprItem_Weapon = ZOMBIE._weaponSprite;
        }

        private Zombie ZOMBIE;

        public EnumController.HAT_OF_ZOMBIE eHat;
        public EnumController.WEAPON_OF_ZOMBIE eWeapon;
        public EnumController.SHIELD_OF_ZOMBIE eShield;


        private SpriteRenderer sprItem_Hat;
        private SpriteRenderer sprItem_Shield;
        private SpriteRenderer sprItem_Weapon;



        public void SetItem(EnumController.HAT_OF_ZOMBIE _item)
        {
            if (sprItem_Hat == null)
            {
                eHat = EnumController.HAT_OF_ZOMBIE.NO_HAT;
                return;
            }


            eHat = _item;
            if (_item == EnumController.HAT_OF_ZOMBIE.NO_HAT)
            {
                sprItem_Hat.gameObject.SetActive(false);
                return;
            }
            if (sprItem_Hat != null)
            {
                sprItem_Hat.sprite = SpriteController.Instance._zombieItem.TakeHat(_item)._sprite;
                sprItem_Hat.gameObject.SetActive(true);
            }
        }
        public void SetItem(EnumController.WEAPON_OF_ZOMBIE _item)
        {
            if (sprItem_Weapon == null)
            {
                eWeapon = EnumController.WEAPON_OF_ZOMBIE.NO_WEAPON;
                return;
            }

            eWeapon = _item;
            if (_item == EnumController.WEAPON_OF_ZOMBIE.NO_WEAPON)
            {
                sprItem_Weapon.gameObject.SetActive(false);
                return;
            }
            if (sprItem_Weapon != null)
            {
                sprItem_Weapon.sprite = SpriteController.Instance._zombieItem.TakeWeapon(_item)._sprite;
                sprItem_Weapon.gameObject.SetActive(true);
            }
        }
        public void SetItem(EnumController.SHIELD_OF_ZOMBIE _item)
        {
            if (sprItem_Shield == null)
            {
                eShield = EnumController.SHIELD_OF_ZOMBIE.NO_SHIELD;
                return;
            }


            eShield = _item;
            if (_item == EnumController.SHIELD_OF_ZOMBIE.NO_SHIELD)
            {
                sprItem_Shield.gameObject.SetActive(false);
                return;
            };
            if (sprItem_Shield != null)
            {
                sprItem_Shield.sprite = SpriteController.Instance._zombieItem.TakeShield(_item)._sprite;
                sprItem_Shield.gameObject.SetActive(true);
            }
        }



        GameObject _effect = null;
        public void RemoveHat()
        {
            if (eHat == EnumController.HAT_OF_ZOMBIE.NO_HAT) return;
            eHat = EnumController.HAT_OF_ZOMBIE.NO_HAT;

            sprItem_Hat.gameObject.SetActive(false);
            //effect
            _effect = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.remove_item).Get();
            _effect.transform.position = sprItem_Hat.transform.position;
            _effect.GetComponent<TakeOffItem>().SetSprite(sprItem_Hat.sprite);//add sprite
            _effect.SetActive(true);

        }

        public void RemoveShield()
        {
            if (eShield == EnumController.SHIELD_OF_ZOMBIE.NO_SHIELD) return;
            eShield = EnumController.SHIELD_OF_ZOMBIE.NO_SHIELD;

            sprItem_Shield.gameObject.SetActive(false);
            //explosion
            _effect = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.remove_item).Get();
            _effect.transform.position = sprItem_Shield.transform.position;
            _effect.GetComponent<TakeOffItem>().SetSprite(sprItem_Shield.sprite);//add sprite
            _effect.SetActive(true);

        }

    }
}