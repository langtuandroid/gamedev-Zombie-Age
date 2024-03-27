using _4_Gameplay;
using MANAGERS;
using UnityEngine;
using UnityEngine.Serialization;


namespace MODULES.Soldiers
{
    public class HandWeapon : MonoBehaviour
    {
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
            if (GameplayController.Instance.GameStatus != GameplayController.GAME_STATUS.playing) return;
            
            if (_timeToReloadMagazine > 0)
            {
                if (!IsInvoking("PlaySoundReload"))
                    InvokeRepeating("PlaySoundReload", 0.01f, 0.3f);

                Transform.eulerAngles = Vector3.zero;
                _animator.SetBool("isShooting", false);

                _timeToReloadMagazine -= Time.deltaTime;
                if (_timeToReloadMagazine <= 0)
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
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
                
                if (GameplayController.Instance.InputType != GameplayController.INPUT_TYPE.shooting) return;
                if (_isLoadingBullet) return;
                if (_isLoadingMagazine) return;
                if (SupportManager.Instance.IsSupport) return;

                _inputPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                if (_inputPosition.x < -6.0f) return;
           
                _isTouching = true;
                RotateGun(_inputPosition);
                
                if (_timeToShot >= 0f)
                {
                    _timeToShot -= Time.deltaTime;
                    if (!Soldier.Instance.IsMoving)
                        Soldier.Instance.PlayAnimations(EnumController.SOLDIER_STATUS.idie);
                    if (_timeToShot <= 0f)
                    {
                        if (!Soldier.Instance.IsMoving)
                            Soldier.Instance.PlayAnimations(EnumController.SOLDIER_STATUS.shooting);

                        Shoot();
                        _timeToShot = _loadTine;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Soldier.Instance.PlayAnimations(EnumController.SOLDIER_STATUS.idie);
                _timeToShot = 0f;
                Soldier.Instance.IsMoving = false;
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
                    _timeToReloadBullet = Soldier.Instance._weaponManager._gunData.fTimeloadOrBullet;
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

            int _totalAmmo = Soldier.Instance._weaponManager._gunData.DATA.iCurrentAmmo;
            _tempAmmonOnMagazine = Soldier.Instance._weaponManager._gunData.iAmmoInMagazine;

            if (_totalAmmo > 0)
            {
                if (_totalAmmo >= Soldier.Instance._weaponManager._gunData.iAmmoInMagazine)
                    _ammoInMagazine = _tempAmmonOnMagazine;
                else
                    _ammoInMagazine = _totalAmmo;
            }

            GameplayController.Instance.weaponShell.Show(GetFactorBullet());//show shell
            _isLoadingMagazine = false;

            _animator.SetBool("isShooting", true);
        }
        private void ResetMagazineBulletFromPlayer()
        {
            if (Soldier.Instance._weaponManager._gunData.DATA.iCurrentAmmoInMagazin
                < Soldier.Instance._weaponManager._gunData.iAmmoInMagazine)
                IsLoadingMagazine = true;
            else
                SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
        }

        public virtual void UpdateInput()
        {

        }

        private float _distanceY = 0;
        private Vector3 _targetPos;
        private float _timeToMove = 1.0f;

        private void Move(Vector2 _input)
        {
            _distanceY = Mathf.Abs(_input.y - Soldier.Instance._soldierTransform.position.y);
            if (_distanceY > 0.5f)
            {
                Soldier.Instance.IsMoving = true;

                _targetPos = Soldier.Instance._soldierTransform.position;
                _targetPos.y = _input.y;
                _targetPos.z = _input.y;
                if (_targetPos.y > 1.1) _targetPos.y = 1.1f;
                if (_targetPos.y < 0.8) _targetPos.y = -0.8f;

                Soldier.Instance._soldierTransform.position = Vector3.MoveTowards(Soldier.Instance._soldierTransform.position, _targetPos, 0.01f);
            }
            else
            {
                Soldier.Instance.IsMoving = false;

            }
        }

        private void PlaySoundReload()
        {
            SoundController.Instance.Play(SoundController.SOUND.sfx_reload);//sound
        }


        private void OnEnable()
        {
            GameplayController.Instance.weaponShell.Show(GetFactorBullet());//show shell
            EventController.OnResetMagazinBullet += ResetMagazineBulletFromPlayer;
        }
        
        private void OnDisable()
        {
            CancelInvoke();
            EventController.OnResetMagazinBullet -= ResetMagazineBulletFromPlayer;
        }
    }
}
