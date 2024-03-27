using _4_Gameplay;
using MANAGERS;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MODULES.Soldiers
{
    public class HandOfWeapon : MonoBehaviour
    {
        private Camera m_MainCamera;

        public GameObject objBeam;
        [SerializeField] protected Animator m_animator;
        [SerializeField] protected AnimationClip aniGunShake, aniReload;

        [HideInInspector]
        public Transform m_tranform;
        [HideInInspector]

        protected bool bTouchingInput; // player đang chạm vào màn hinh

        // GUN 
        [Space(30)]
        protected int iAmmoInMagazine; //đạn trong băng đạn
        //  public float fTimeLoadOfGroupBullet; // thoi gian load giữa 2 băng đạn
        public float fTimeloadOfBullet; // thoi gian loan giữa 2 viên
        public int iDamageOfGun; // sat thuong cua đạn
        public float fRangeOfGBullet;//range of bullet
        public Sprite sprBullet;
        public Vector3 vScaleOfBullet;


        private void Awake()
        {
            m_tranform = transform;
            m_MainCamera = Camera.main;
        }
        private void Start()
        {
            Init();
            ReloadMagazine();
        }
        public virtual void Init()
        {

        }

        public Vector2 GetPos()
        {
            return m_tranform.position;
        }

        // Update is called once per frame
        public Vector2 vInputPos;

        protected Vector2 GetPosOfBullet()
        {
            return (vInputPos + Random.insideUnitCircle * 1.0f);
        }

        void Update()
        {
            if (GameplayController.Instance.GameStatus != GameplayController.GAME_STATUS.playing) return;


            //=========reload
            if (_fTimeWaitToReloadMagazine > 0)
            {
                if (!IsInvoking("PlaySoundReload"))
                    InvokeRepeating("PlaySoundReload", 0.01f, 0.3f);

                m_tranform.eulerAngles = Vector3.zero;
                m_animator.SetBool("isShooting", false);

                _fTimeWaitToReloadMagazine -= Time.deltaTime;
                if (_fTimeWaitToReloadMagazine <= 0)
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                    ReloadMagazine();
                    CancelInvoke("PlaySoundReload");
                }
            }

            if (_fTimeWaitToReloadBullet > 0)
            {

                _fTimeWaitToReloadBullet -= Time.deltaTime;
                if (_fTimeWaitToReloadBullet <= 0)
                {
                    bLoadingBullet = false;
                }

            }



            if (Input.GetMouseButton(0))
            {
                //move
                if (fCountTimeToMove > 0 && bTouchingInput)
                {
                    fCountTimeToMove -= Time.deltaTime;
                }
                if (fCountTimeToMove < 0)
                {
                    Move(vInputPos);
                }

                //===================================
                if (GameplayController.Instance.InputType != GameplayController.INPUT_TYPE.shooting) return;
                if (bLoadingBullet) return;
                if (bLoadingMagazine) return;
                if (SupportManager.Instance.IsSupport) return;
                //--------------------------------------------

                vInputPos = m_MainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (vInputPos.x < -6.0f) return;
           
                bTouchingInput = true;
                RotateGun(vInputPos);


                //SHOT
                if (_fCountTimeToShot >= 0f)
                {
                    _fCountTimeToShot -= Time.deltaTime;
                    if (!Soldier.Instance.IsMoving)
                        Soldier.Instance.PlayAnimator(EnumController.SOLDIER_STATUS.idie);
                    if (_fCountTimeToShot <= 0f)
                    {
                        if (!Soldier.Instance.IsMoving)
                            Soldier.Instance.PlayAnimator(EnumController.SOLDIER_STATUS.shooting);

                        Shot();
                        _fCountTimeToShot = fTimeloadOfBullet;
                    }
                }
                //if (Soldier.Instance.eSoldierStatus == TheEnumManager.SOLDIER_STATUS.idie)
                //{

                //    InvokeRepeating("Shot", 0.0f, fTimeLoadOfGroupBullet);
                //}



            }
            else if (Input.GetMouseButtonUp(0))
            {
                Soldier.Instance.PlayAnimator(EnumController.SOLDIER_STATUS.idie);
                _fCountTimeToShot = 0f;
                Soldier.Instance.IsMoving = false;
                bTouchingInput = false;
                fCountTimeToMove = 1.0f;

            }


            UpdateInput();


        }



        //Gun's Rotation
        Vector2 _tempPos;
        float _angle;
        private void RotateGun(Vector2 _target)
        {

            _tempPos = GetPos() - _target;
            _angle = Vector2.Angle(_tempPos, Vector2.up) - 90;
            m_tranform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);

        }
        protected float GetRoatateZ(Vector2 _from, Vector2 _to)
        {
            return Vector2.Angle((_from - _to), Vector2.up) - 90;
        }


        //Shot
        public virtual void Shot()
        {

        }


        //allow to shot?
        private float _fTimeWaitToReloadMagazine;//thoi gian de reload ban dan
        private float _fTimeWaitToReloadBullet;
        private float _fCountTimeToShot;



        private bool bLoadingMagazine = false;//đang thay băng đạn.
        private bool bLoadingBullet = false;//đang nạp đạn

        public bool bLOADING_BULLET
        {
            get
            {
                return bLoadingBullet;
            }
            set
            {
                bLoadingBullet = value;
                if (value)
                {
                    _fTimeWaitToReloadBullet = Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.fTimeloadOrBullet;
                }
            }
        }
        public bool bLOADING_MAGAZINE
        {
            get
            {
                return bLoadingMagazine;
            }
            set
            {
                bLoadingMagazine = value;
                if (value)
                {
                    _fTimeWaitToReloadMagazine = 2.0f;
                }
            }
        }


        //FACTOR OF BULLET AND MAGAZINE
        public float GetFactorBullet()
        {
            return iAmmoInMagazine * 1.0f / _tempAmmonOnMagazine;
        }



        //Reload magazine
        protected int _tempAmmonOnMagazine;
        public void ReloadMagazine()
        {

            int _totalAmmo = Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo;
            _tempAmmonOnMagazine = Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.iAmmoInMagazine;

            // if (_tempAmmonOnMagazine == Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.iAmmoInMagazine) return;//full
            if (_totalAmmo > 0)
            {
                if (_totalAmmo >= Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.iAmmoInMagazine)
                    iAmmoInMagazine = _tempAmmonOnMagazine;
                else
                    iAmmoInMagazine = _totalAmmo;
            }

            GameplayController.Instance.weaponShell.Show(GetFactorBullet());//show shell
            bLoadingMagazine = false;

            m_animator.SetBool("isShooting", true);
        }
        private void ResetMagazineBulletFromPlayer()
        {
            if (Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmoInMagazin
                < Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.iAmmoInMagazine)
                bLOADING_MAGAZINE = true;
            else
                SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
        }



        //update input
        public virtual void UpdateInput()
        {

        }

        //Move
        float _disY = 0;
        Vector3 _targetPos;
        private float fCountTimeToMove = 1.0f;

        public void Move(Vector2 _input)
        {

            _disY = Mathf.Abs(_input.y - Soldier.Instance._tranOfSoldier.position.y);
            if (_disY > 0.5f)
            {
                Soldier.Instance.IsMoving = true;

                _targetPos = Soldier.Instance._tranOfSoldier.position;
                _targetPos.y = _input.y;
                _targetPos.z = _input.y;
                if (_targetPos.y > 1.1) _targetPos.y = 1.1f;
                if (_targetPos.y < 0.8) _targetPos.y = -0.8f;

                Soldier.Instance._tranOfSoldier.position = Vector3.MoveTowards(Soldier.Instance._tranOfSoldier.position, _targetPos, 0.01f);
            }
            else
            {
                Soldier.Instance.IsMoving = false;

            }
        }

        //PLAY SOUND RELOAD
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
