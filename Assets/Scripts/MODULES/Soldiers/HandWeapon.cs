﻿using _4_Gameplay;
using MANAGERS;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;


namespace MODULES.Soldiers
{
    public class HandWeapon : MonoBehaviour
    {
        [Inject] protected SoundController SoundController;
        [Inject] protected GameplayController _gameplayController;
        [Inject] private SupportManager _supportManager;
        [Inject] protected Soldier _soldier;
        [Inject] protected ObjectPoolController _objectPoolController;
        private Camera _camera;
        [FormerlySerializedAs("objBeam")] public GameObject _beam;
        [FormerlySerializedAs("m_animator")] [SerializeField] protected Animator _animator;
        [FormerlySerializedAs("aniGunShake")] [SerializeField] protected AnimationClip _gunShakeAnimation;
        protected Transform Transform;
        private bool _isTouching; 
        [Space(30)]
        protected int _ammoInMagazine;
        [FormerlySerializedAs("fTimeloadOfBullet")] public float _loadTine;
        [FormerlySerializedAs("iDamageOfGun")] public int _damage; 
        [FormerlySerializedAs("fRangeOfGBullet")] public float _bulletRange;
        [FormerlySerializedAs("sprBullet")] public Sprite _bulletSprite;
        [FormerlySerializedAs("vScaleOfBullet")] public Vector3 _bulletScale;
        

        private void Awake()
        {
            Transform = transform;
            _camera = Camera.main;
            
        }
        private void Start()
        {
            Construct();
            ReloadingMagazine();
        }

        protected virtual void Construct()
        {

        }

        private Vector2 Pos()
        {
            return Transform.position;
        }
        
        [FormerlySerializedAs("vInputPos")] public Vector2 _inputPosition;
        
        private void Update()
        {
            if (_gameplayController.GameStatus != GameplayController.GAME_STATUS.playing) return;
            
            if (_timeToReloadMagazine > 0)
            {
                if (!IsInvoking("PlaySoundReload"))
                    InvokeRepeating("PlaySoundReload", 0.01f, 0.3f);

                Transform.eulerAngles = Vector3.zero;
                _animator.SetBool("isShooting", false);

                _timeToReloadMagazine -= Time.deltaTime;
                if (_timeToReloadMagazine <= 0)
                {
                    SoundController.Play(SoundController.SOUND.ui_cannot);//sound
                    ReloadingMagazine();
                    CancelInvoke("PlaySoundReload");
                }
            }

            if (_timeToReloadBullet > 0)
            {

                _timeToReloadBullet -= Time.deltaTime;
                if (_timeToReloadBullet <= 0)
                {
                    _isLoadingBullet = false;
                }

            }



            if (Input.GetMouseButton(0))
            {
                //move
                if (_timeToMove > 0 && _isTouching)
                {
                    _timeToMove -= Time.deltaTime;
                }
                if (_timeToMove < 0)
                {
                    Move(_inputPosition);
                }
                
                if (_gameplayController.InputType != GameplayController.INPUT_TYPE.shooting) return;
                if (_isLoadingBullet) return;
                if (_isLoadingMagazine) return;
                if (_supportManager.IsSupport) return;

                _inputPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                if (_inputPosition.x < -6.0f) return;
           
                _isTouching = true;
                RotateGun(_inputPosition);
                
                if (_timeToShot >= 0f)
                {
                    _timeToShot -= Time.deltaTime;
                    if (!_soldier.IsMoving)
                        _soldier.PlayAnimations(EnumController.SOLDIER_STATUS.idie);
                    if (_timeToShot <= 0f)
                    {
                        if (!_soldier.IsMoving)
                            _soldier.PlayAnimations(EnumController.SOLDIER_STATUS.shooting);

                        Shoot();
                        _timeToShot = _loadTine;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _soldier.PlayAnimations(EnumController.SOLDIER_STATUS.idie);
                _timeToShot = 0f;
                _soldier.IsMoving = false;
                _isTouching = false;
                _timeToMove = 1.0f;

            }


            UpdateInput();


        }


        private Vector2 _tempPos;
        private float _angle;
        private void RotateGun(Vector2 _target)
        {

            _tempPos = Pos() - _target;
            _angle = Vector2.Angle(_tempPos, Vector2.up) - 90;
            Transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);

        }
        protected float GetRoatateZ(Vector2 _from, Vector2 _to)
        {
            return Vector2.Angle((_from - _to), Vector2.up) - 90;
        }

        protected virtual void Shoot()
        {

        }

        private float _timeToReloadMagazine;
        private float _timeToReloadBullet;
        private float _timeToShot;
        
        private bool _isLoadingMagazine;
        private bool _isLoadingBullet;

        protected bool IsLoadingBullet
        {
            get => _isLoadingBullet;
            set
            {
                _isLoadingBullet = value;
                if (value)
                {
                    _timeToReloadBullet = _soldier._weaponManager._gunData.fTimeloadOrBullet;
                }
            }
        }

        protected bool IsLoadingMagazine
        {
            get => _isLoadingMagazine;
            set
            {
                _isLoadingMagazine = value;
                if (value)
                {
                    _timeToReloadMagazine = 2.0f;
                }
            }
        }

        public float GetFactorBullet()
        {
            return _ammoInMagazine * 1.0f / _tempAmmonOnMagazine;
        }

        private int _tempAmmonOnMagazine;

        private void ReloadingMagazine()
        {

            int _totalAmmo = _soldier._weaponManager._gunData.DATA.iCurrentAmmo;
            _tempAmmonOnMagazine = _soldier._weaponManager._gunData.iAmmoInMagazine;

            if (_totalAmmo > 0)
            {
                if (_totalAmmo >= _soldier._weaponManager._gunData.iAmmoInMagazine)
                    _ammoInMagazine = _tempAmmonOnMagazine;
                else
                    _ammoInMagazine = _totalAmmo;
            }

            _gameplayController.weaponShell.Show(GetFactorBullet());//show shell
            _isLoadingMagazine = false;

            _animator.SetBool("isShooting", true);
        }
        private void ResetMagazineBulletFromPlayer()
        {
            if (_soldier._weaponManager._gunData.DATA.iCurrentAmmoInMagazin
                < _soldier._weaponManager._gunData.iAmmoInMagazine)
                IsLoadingMagazine = true;
            else
                SoundController.Play(SoundController.SOUND.ui_cannot);//sound
        }

        public virtual void UpdateInput()
        {

        }

        private float _distanceY = 0;
        private Vector3 _targetPos;
        private float _timeToMove = 1.0f;

        private void Move(Vector2 _input)
        {
            _distanceY = Mathf.Abs(_input.y - _soldier._soldierTransform.position.y);
            if (_distanceY > 0.5f)
            {
                _soldier.IsMoving = true;

                _targetPos = _soldier._soldierTransform.position;
                _targetPos.y = _input.y;
                _targetPos.z = _input.y;
                if (_targetPos.y > 1.1) _targetPos.y = 1.1f;
                if (_targetPos.y < 0.8) _targetPos.y = -0.8f;

                _soldier._soldierTransform.position = Vector3.MoveTowards(_soldier._soldierTransform.position, _targetPos, 0.01f);
            }
            else
            {
                _soldier.IsMoving = false;

            }
        }

        private void PlaySoundReload()
        {
            SoundController.Play(SoundController.SOUND.sfx_reload);//sound
        }


        private void OnEnable()
        {
            _gameplayController.weaponShell.Show(GetFactorBullet());//show shell
            EventController.OnResetMagazinBullet += ResetMagazineBulletFromPlayer;
        }
        
        private void OnDisable()
        {
            CancelInvoke();
            EventController.OnResetMagazinBullet -= ResetMagazineBulletFromPlayer;
        }
    }
}
