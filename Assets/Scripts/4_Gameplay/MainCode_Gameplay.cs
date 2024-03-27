using MANAGERS;
using MODULES;
using MODULES.Scriptobjectable;
using MODULES.Soldiers;
using MODULES.Zombies;
using UnityEngine;
using UnityEngine.UI;

namespace _4_Gameplay
{
    public class MainCode_Gameplay : MonoBehaviour
    {
        //INPUT OF PLAYER
        public enum INPUT_TYPE
        {
            shooting,
            using_support,
        }


        //ZOMBIE WAVE BAR
        [System.Serializable]
        public class ZombieWaveBar
        {
            public Slider m_Sldier;

            public void Init()
            {
                m_Sldier.value = 0;
            }

            public void UpdateBar(float _factor)
            {
                m_Sldier.value = _factor;
            }
        }


        //CLASS BAR HP OF BOSS
        [System.Serializable]
        public class BossHpBar
        {
            public void Init()
            {
                ShowBar(1.0f);
                SetActive(false);
            }

            public GameObject objBarObject;
            public Image imaBar;



            public void ShowBar(float _factor)
            {
                imaBar.fillAmount = _factor;
            }

            public void SetActive(bool _active)
            {
                objBarObject.SetActive(_active);
            }
        }

        //CLASS WEAPON SHELL
        [System.Serializable]
        public class WeaponShell
        {
            public Image imaShellBlack, imaBar;

            public void ChangeShell(Sprite _spr)
            {
                imaShellBlack.sprite = _spr;
                imaBar.sprite = _spr;
            }
            public void ShowBar(float _factor)
            {

                imaBar.fillAmount = _factor;
            }
        }
        public WeaponShell m_WeaponShell;

        public enum GAME_STATUS
        {
            loading,
            playing,
            pause,
            victory,
            gameover,
            resume,
        }
        public static MainCode_Gameplay Instance;
        public GAME_STATUS eGameStatus;
        public INPUT_TYPE eInputType;


        //========================
        [Space(30)]
        [SerializeField] private Button buPause;
        [SerializeField] private Button buResetGun; //reset magazin bullet
        [SerializeField] private Button buCallSupport; //reset magazin bullet
        [SerializeField] private GameObject PANEL_SUPPORT;
        [SerializeField] private GameObject PANEL_WEAPON;

        [Space(20)]
        [SerializeField] private Text txtWave;
        [SerializeField] private Text txtTestMode, txtHp;


        [Space(20)]
        [SerializeField] private Image imaHpBar;
        [SerializeField] private GameObject objLastWave;


        [Space(20)]
        public BossHpBar m_BossHpBar = new BossHpBar();
        public ZombieWaveBar m_ZombieWaveBar = new ZombieWaveBar();


        private void Awake()
        {
            Application.targetFrameRate = 60;
            if (Instance == null)
                Instance = this;
            TheUiManager.Instance.SetCameraForPopupCanvas(Camera.main);//set camera


            MusicManager.Instance.Stop();
            MusicManager.Instance.Play();

            m_BossHpBar.Init();
            m_ZombieWaveBar.Init();


            if (TheDataManager.Instance.eMode == TheDataManager.MODE.Release)
                txtTestMode.gameObject.SetActive(false);
        }


        private void Start()
        {

            LoadLevel(TheDataManager.Instance.THE_DATA_PLAYER.iCurrentLevel + 1);//load level

            objLastWave.SetActive(false);


            HandleShowTextWave();
            buPause.onClick.AddListener(() => SetButtonUI(buPause));
            buResetGun.onClick.AddListener(() => SetButtonUI(buResetGun));
            buCallSupport.onClick.AddListener(() => SetButtonUI(buCallSupport));


            SetGameStartus(GAME_STATUS.playing);


        }


        //debug====================================
        private float fTimeScale = 0;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Time.timeScale != 0.0f)
                {
                    fTimeScale = Time.timeScale;
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = fTimeScale;
                }
            }
        }
        //=============================================

        //-SET GAME STATUS-----------------
        public void SetGameStartus(GAME_STATUS _gameStatus)
        {
            if (eGameStatus == _gameStatus) return;
            eGameStatus = _gameStatus;

            switch (eGameStatus)
            {
                case GAME_STATUS.loading:

                    break;
                case GAME_STATUS.playing:

                    break;
                case GAME_STATUS.pause:

                    break;
                case GAME_STATUS.resume:
                    eGameStatus = GAME_STATUS.playing;

                    break;
                case GAME_STATUS.victory:

                    Debug.Log("VICTORY");
                    TheEventManager.PostEvent_OnGameWin(); //event firebase
                    TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.victory, 1.5f);
                    break;
                case GAME_STATUS.gameover:

                    Debug.Log("GAME OVER");
                    TheEventManager.PostEvent_OnGameover(); //event firebase
                    TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.gameover, 1.0f);
                    break;
            }
            Debug.Log("TIME SCALE: " + Time.timeScale);
        }


        //-LOAD LEVEL----------------------
        private void LoadLevel(int _level)
        {
            string path = "Levels/Level_" + _level;
            GameObject prelevel = Resources.Load<GameObject>(path);
            if (prelevel)
                Instantiate(prelevel);
            else
            {
                path = "Levels/Level_1";
                prelevel = Resources.Load<GameObject>(path);
                if (prelevel)
                    Instantiate(prelevel);
            }
        }


        //-SET BUTTON UI------------------
        private void SetButtonUI(Button _bu)
        { //for tutorial
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }

            if (_bu == buPause)
            {


                if (eGameStatus != GAME_STATUS.playing) return;
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound           
                eGameStatus = GAME_STATUS.pause;
                TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.pause);
            }
            else if (_bu == buResetGun)
            {
                TheEventManager.PostEvent_OnResetMagazinBullet();//event
            }
            else if (_bu == buCallSupport)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);

                if (PANEL_SUPPORT.activeInHierarchy)
                {
                    PANEL_SUPPORT.SetActive(false);
                    PANEL_WEAPON.SetActive(true);
                    buCallSupport.GetComponentInChildren<Text>().text = "1/2";
                }
                else
                {
                    PANEL_SUPPORT.SetActive(true);
                    PANEL_WEAPON.SetActive(false);
                    buCallSupport.GetComponentInChildren<Text>().text = "2/2";
                }

            }
        }


        //-SHOW TEXT WAVE----------------
        private void HandleShowTextWave()
        {
       
            if (TheLevel.Instance && TheLevel.Instance.iCurrentWave == -1)
                txtWave.text = "WAVE: 0/" + TheLevel.Instance.LEVEL_DATA.iTotalWave;

            else if (TheLevel.Instance && TheLevel.Instance.iCurrentWave == TheLevel.Instance.LEVEL_DATA.iTotalWave - 1)
            {
                // last wave
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_drum);//sound
                objLastWave.SetActive(true);
                //music
                MusicManager.Instance.Stop();
                MusicManager.Instance.Play();

                txtWave.text = "WAVE: " + (TheLevel.Instance.iCurrentWave + 1) + "/" + TheLevel.Instance.LEVEL_DATA.iTotalWave;
            }
            else
            {
                txtWave.text = "WAVE: " + (TheLevel.Instance.iCurrentWave + 1) + "/" + TheLevel.Instance.LEVEL_DATA.iTotalWave;
            }
        }


        //-SHOW HP BAR--------------------
        public void ShowHpBar(float _factor, string _string)
        {
            imaHpBar.fillAmount = _factor;
            txtHp.text = _string;
        }


        //-PAUSE--------------------------
        private void HandlePauseGame(TheUiManager.POP_UP _popup)
        {
            Time.timeScale = 0;
            SetGameStartus(GAME_STATUS.pause);
        }


        //-RESUME-------------------------
        private void HandleResumeGame(TheUiManager.POP_UP _popup)
        {
            if (TheUiManager.Instance.isShowing()) return;

            Time.timeScale = 1;
            SetGameStartus(GAME_STATUS.resume);

        }


        //-CHANGED WEPON------------------
        private void HandleChangedWeapon(GunData _gundata)
        {
            m_WeaponShell.ChangeShell(_gundata.spriteOfWeaponShell);

            //change shell
            if (_gundata.DATA.iCurrentAmmo <= 0)
                m_WeaponShell.ShowBar(0.0f);
            else
                m_WeaponShell.ShowBar(Soldier.Instance.WEAPON_MANAGER.CURRENT_WEAPON.GetFactorBullet());

        }


        //-HANDLE ZOMBIE BORN-------------
        private void HandleZombieBorn(Zombie _zombie)
        {
            if (_zombie.DATA.bIsBoss)
            {
                m_BossHpBar.SetActive(true);
            }
        }


        //-HANDLE ZOMBIE DIE--------------
        private void HandleZombieDie(Zombie _zombie)
        {
            if (_zombie.DATA.bIsBoss)
            {
                m_BossHpBar.SetActive(false);
            }
        }


        private void OnEnable()
        {
            TheEventManager.OnStartNewWave += HandleShowTextWave;
            TheEventManager.OnShowPopup += HandlePauseGame;
            TheEventManager.OnHidePopup += HandleResumeGame;
            TheEventManager.OnChangedWeapon += HandleChangedWeapon;

            TheEventManager.OnZombieBorn += HandleZombieBorn;
            TheEventManager.OnZombieDie += HandleZombieDie;
        }


        private void OnDisable()
        {

            TheDataManager.Instance.SaveDataPlayer();//save

            TheEventManager.OnStartNewWave -= HandleShowTextWave;
            TheEventManager.OnShowPopup -= HandlePauseGame;
            TheEventManager.OnHidePopup -= HandleResumeGame;
            TheEventManager.OnChangedWeapon -= HandleChangedWeapon;
            TheEventManager.OnZombieBorn -= HandleZombieBorn;
            TheEventManager.OnZombieDie -= HandleZombieDie;
        }
    }
}
