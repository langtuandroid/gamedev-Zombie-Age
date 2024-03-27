using MANAGERS;
using MODULES;
using MODULES.Scriptobjectable;
using MODULES.Soldiers;
using MODULES.Zombies;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _4_Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        public enum INPUT_TYPE
        {
            shooting,
            using_support,
        }
        public enum GAME_STATUS
        {
            loading,
            playing,
            pause,
            victory,
            gameover,
            resume,
        }

        [System.Serializable]
        public class ZombieWaveBar
        {
            [FormerlySerializedAs("m_Sldier")] [SerializeField] private Slider _slider;

            public void Construct()
            {
                _slider.value = 0;
            }

            public void Update(float _factor)
            {
                _slider.value = _factor;
            }
        }
        
        [System.Serializable]
        public class BossHpBar
        {
            [FormerlySerializedAs("objBarObject")] [SerializeField] private GameObject _bar;
            [FormerlySerializedAs("imaBar")] [SerializeField] private Image _barImage;
            public void Construct()
            {
                Show(1.0f);
                Activate(false);
            }
            
            public void Show(float _factor)
            {
                _barImage.fillAmount = _factor;
            }

            public void Activate(bool isActive)
            {
                _bar.SetActive(isActive);
            }
        }
        
        [System.Serializable]
        public class WeaponShell
        {
            [FormerlySerializedAs("imaShellBlack")] [SerializeField] private Image _shellBack;
            [FormerlySerializedAs("imaBar")] [SerializeField] private Image _barImage;

            public void ChangeSprite(Sprite sprite)
            {
                _shellBack.sprite = sprite;
                _barImage.sprite = sprite;
            }
            public void Show(float _factor)
            {
                _barImage.fillAmount = _factor;
            }
        }
        public static GameplayController Instance;
        
        [FormerlySerializedAs("m_WeaponShell")] [SerializeField] private WeaponShell _weaponShell;
        [FormerlySerializedAs("eGameStatus")] [SerializeField] private GAME_STATUS _gameStatus;
        [FormerlySerializedAs("eInputType")] [SerializeField] private INPUT_TYPE _inputType;

        [Space(30)]
        [FormerlySerializedAs("buPause")] [SerializeField] private Button _pauseButton;
        [FormerlySerializedAs("buResetGun")] [SerializeField] private Button _resetGunButton; 
        [FormerlySerializedAs("buCallSupport")] [SerializeField] private Button _callSupportButton;
        [FormerlySerializedAs("PANEL_SUPPORT")] [SerializeField] private GameObject _supportPanel;
        [FormerlySerializedAs("PANEL_WEAPON")] [SerializeField] private GameObject _weaponPanel;
        
        [Space(20)]
        [FormerlySerializedAs("txtWave")] [SerializeField] private Text _waveText;
        [FormerlySerializedAs("txtTestMode")] [SerializeField] private Text _testModeText;
        [FormerlySerializedAs("txtHp")] [SerializeField] private Text _hpText;
        
        [Space(20)]
        [FormerlySerializedAs("imaHpBar")][SerializeField] private Image _hpBar;
        [FormerlySerializedAs("objLastWave")] [SerializeField] private GameObject _lastWaveObject;
        
        [Space(20)]
        [FormerlySerializedAs("m_BossHpBar")] [SerializeField] private BossHpBar _bossHpBar = new BossHpBar();
        [FormerlySerializedAs("m_ZombieWaveBar")] [SerializeField] private ZombieWaveBar _zombieWaveBar = new ZombieWaveBar();

        private float _timeScale = 0;

        public GAME_STATUS GameStatus => _gameStatus;
        public INPUT_TYPE InputType
        {
            get => _inputType;
            set => _inputType = value;
        }

        public BossHpBar bossHpBar => _bossHpBar;
        public ZombieWaveBar zombieWaveBar => _zombieWaveBar;
        public WeaponShell weaponShell => _weaponShell;
        private void Awake()
        {
            Application.targetFrameRate = 60;
            if (Instance == null)
                Instance = this;
            UIController.Instance.SetCameraPopup(Camera.main);//set camera


            MusicManager.Instance.Stop();
            MusicManager.Instance.Play();

            _bossHpBar.Construct();
            _zombieWaveBar.Construct();


            if (DataController.Instance.mode == DataController.Mode.Release)
                _testModeText.gameObject.SetActive(false);
        }


        private void Start()
        {
            LoadGame(DataController.Instance.playerData.CurrentLevel + 1);//load level

            _lastWaveObject.SetActive(false);


            HandleShowTextWave();
            _pauseButton.onClick.AddListener(() => AssignButton(_pauseButton));
            _resetGunButton.onClick.AddListener(() => AssignButton(_resetGunButton));
            _callSupportButton.onClick.AddListener(() => AssignButton(_callSupportButton));


            SetStatusOfGame(GAME_STATUS.playing);


        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Time.timeScale != 0.0f)
                {
                    _timeScale = Time.timeScale;
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = _timeScale;
                }
            }
        }

        public void SetStatusOfGame(GAME_STATUS _gameStatus)
        {
            if (this._gameStatus == _gameStatus) return;
            this._gameStatus = _gameStatus;

            switch (this._gameStatus)
            {
                case GAME_STATUS.loading:

                    break;
                case GAME_STATUS.playing:

                    break;
                case GAME_STATUS.pause:

                    break;
                case GAME_STATUS.resume:
                    this._gameStatus = GAME_STATUS.playing;

                    break;
                case GAME_STATUS.victory:

                    Debug.Log("VICTORY");
                    EventController.OnGameWinInvoke(); //event firebase
                    UIController.Instance.PopUpShow(UIController.POP_UP.victory, 1.5f);
                    break;
                case GAME_STATUS.gameover:

                    Debug.Log("GAME OVER");
                    EventController.OnGameoverInvoke(); //event firebase
                    UIController.Instance.PopUpShow(UIController.POP_UP.gameover, 1.0f);
                    break;
            }
            Debug.Log("TIME SCALE: " + Time.timeScale);
        }


        private void LoadGame(int _level)
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



        private void AssignButton(Button button)
        { //for tutorial
            if (TutorialController.Instance)
            {
                if (!TutorialController.Instance.IsRightInput()) return;
            }

            if (button == _pauseButton)
            {
                if (_gameStatus != GAME_STATUS.playing) return;
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound           
                _gameStatus = GAME_STATUS.pause;
                UIController.Instance.PopUpShow(UIController.POP_UP.pause);
            }
            else if (button == _resetGunButton)
            {
                EventController.OnResetMagazinBulletInvoke();//event
            }
            else if (button == _callSupportButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);

                if (_supportPanel.activeInHierarchy)
                {
                    _supportPanel.SetActive(false);
                    _weaponPanel.SetActive(true);
                    _callSupportButton.GetComponentInChildren<Text>().text = "1/2";
                }
                else
                {
                    _supportPanel.SetActive(true);
                    _weaponPanel.SetActive(false);
                    _callSupportButton.GetComponentInChildren<Text>().text = "2/2";
                }

            }
        }

        
        private void HandleShowTextWave()
        {
       
            if (TheLevel.Instance && TheLevel.Instance.iCurrentWave == -1)
                _waveText.text = "WAVE: 0/" + TheLevel.Instance.LEVEL_DATA.iTotalWave;

            else if (TheLevel.Instance && TheLevel.Instance.iCurrentWave == TheLevel.Instance.LEVEL_DATA.iTotalWave - 1)
            {
                // last wave
                SoundController.Instance.Play(SoundController.SOUND.sfx_drum);//sound
                _lastWaveObject.SetActive(true);
                //music
                MusicManager.Instance.Stop();
                MusicManager.Instance.Play();

                _waveText.text = "WAVE: " + (TheLevel.Instance.iCurrentWave + 1) + "/" + TheLevel.Instance.LEVEL_DATA.iTotalWave;
            }
            else
            {
                _waveText.text = "WAVE: " + (TheLevel.Instance.iCurrentWave + 1) + "/" + TheLevel.Instance.LEVEL_DATA.iTotalWave;
            }
        }
        
        public void ShowHpBar(float _factor, string _string)
        {
            _hpBar.fillAmount = _factor;
            _hpText.text = _string;
        }
        
        private void HandlePauseGame(UIController.POP_UP _popup)
        {
            Time.timeScale = 0;
            SetStatusOfGame(GAME_STATUS.pause);
        }
        
        private void HandleResumeGame(UIController.POP_UP _popup)
        {
            if (UIController.Instance.IsShowing()) return;

            Time.timeScale = 1;
            SetStatusOfGame(GAME_STATUS.resume);

        }

        private void HandleChangedWeapon(GunData _gundata)
        {
            _weaponShell.ChangeSprite(_gundata.spriteOfWeaponShell);
            
            if (_gundata.DATA.iCurrentAmmo <= 0)
                _weaponShell.Show(0.0f);
            else
                _weaponShell.Show(Soldier.Instance._weaponManager._thisWeapon.GetFactorBullet());

        }
        
        private void HandleZombieBorn(Zombie _zombie)
        {
            if (_zombie._zombieData.bIsBoss)
            {
                _bossHpBar.Activate(true);
            }
        }
        
        private void HandleZombieDie(Zombie _zombie)
        {
            if (_zombie._zombieData.bIsBoss)
            {
                _bossHpBar.Activate(false);
            }
        }


        private void OnEnable()
        {
            EventController.OnStartNewWave += HandleShowTextWave;
            EventController.OnShowPopup += HandlePauseGame;
            EventController.OnHidePopup += HandleResumeGame;
            EventController.OnChangedWeapon += HandleChangedWeapon;

            EventController.OnZombieBorn += HandleZombieBorn;
            EventController.OnZombieDie += HandleZombieDie;
        }


        private void OnDisable()
        {
            DataController.Instance.SaveData();

            EventController.OnStartNewWave -= HandleShowTextWave;
            EventController.OnShowPopup -= HandlePauseGame;
            EventController.OnHidePopup -= HandleResumeGame;
            EventController.OnChangedWeapon -= HandleChangedWeapon;
            EventController.OnZombieBorn -= HandleZombieBorn;
            EventController.OnZombieDie -= HandleZombieDie;
        }
    }
}
