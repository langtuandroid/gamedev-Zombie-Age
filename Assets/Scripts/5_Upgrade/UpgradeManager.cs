using GoogleMobileAds.Api;
using Integration;
using MANAGERS;
using MODULES.Scriptobjectable;
using SCREENS;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace _5_Upgrade
{
    public class UpgradeManager : MonoBehaviour
    {
        [Inject] private BannerViewController _bannerViewController;
        [Inject] private AdMobController _adMobController;
        [Inject] private SoundController _soundController;
        [Inject] private UpgradeController _upgradeController;
        [Inject] private DiContainer _diContainer;
        [Inject] private DataController _dataController;
        [Inject] private UIController _uiController;
        
        [System.Serializable]
        public class BoardInfo
        {
            [Inject] private SoundController _soundController;
            [Inject] private DataController _dataController;
            [Inject] private UpgradeController _upgradeController;
            [Inject] private UIController _uiController;
            [Inject] private UpgradeManager _upgradeManager;
            
            private UpgradeData _data;
            private UpgradeButton _upgradeButton;
            
            [SerializeField] private Image imaIcon;
            [SerializeField] private Image imaStar;
            
            [Space(20)]
            [SerializeField] private TMP_Text _starValue;
            [SerializeField] private TMP_Text _nameText;
            [SerializeField] private TMP_Text _contentText;

            [SerializeField] private Button _resetButton;
            [SerializeField] private Button _upgradeB;
            
            public void Show(UpgradeButton _ButtonUpgrade)
            {
                _upgradeButton = _ButtonUpgrade;
                _upgradeManager.tranOfYellowCirle.position = _upgradeButton.transform.position;
                _resetButton.onClick.RemoveAllListeners();
                _upgradeB.onClick.RemoveAllListeners();

                _data = _upgradeController.GetUpgrade(_upgradeButton._upgradeType);
                _resetButton.onClick.AddListener(() => SetButton(_resetButton));
                _upgradeB.onClick.AddListener(() => SetButton(_upgradeB));



                _starValue.text = _data.iStar.ToString();
                _nameText.text = _data.strName;
                _contentText.text = _data.strContent;

                imaStar.sprite = _upgradeManager.sprStar;
                
                if (_data.bEQUIPED)
                {
                    imaIcon.sprite = _data.sprIcon;
                    _upgradeB.image.color = Color.gray;
                    _resetButton.image.color = Color.white;
                }
                else
                {
                    imaIcon.sprite = _data.sprIcon_gray;
                    _upgradeB.image.color = Color.white;
                    _resetButton.image.color = Color.gray;
                }



            }

            private void SetButton(Button _bu)
            {
                if (_bu == _resetButton)
                {
                    if (_data.bEQUIPED)
                    {
                        _soundController.Play(SoundController.SOUND.ui_click_next);//s
                        _data.Remove();
                        Show(_upgradeButton);
                        _upgradeButton.Construct();
                        _upgradeManager.UpdateTextStar();

                        _dataController.SaveData();//save
                    }
                    else
                        _soundController.Play(SoundController.SOUND.ui_cannot);//s
                }
                else if (_bu == _upgradeB)
                {
                    if (!_data.bEQUIPED)
                    {
                        int _star = 0;

                        _star = _dataController.playerData.GetAllStars()
                                - _upgradeController.GetTotalStarEquied();


                        if (_dataController.mode == DataController.Mode.Release)
                        {
                            if (_star < _data.iStar)
                            {
                                _soundController.Play(SoundController.SOUND.ui_cannot);//sound
                                _uiController.PopUpShow(UIController.POP_UP.note);
                                Note.AssignNote(Note.NOTE.no_enought_star_to_upgrade.ToString());
                                return;
                            }
                        }

                        _soundController.Play(SoundController.SOUND.ui_click_next);//s
                        _data.Upgrade();
                        Show(_upgradeButton);
                        _upgradeButton.Construct();
                        _upgradeManager.UpdateTextStar();

                        _dataController.SaveData();//save
                    }
                    else
                        _soundController.Play(SoundController.SOUND.ui_cannot);//s
                }
            }
        }
        
        public BoardInfo m_BoardInfo;
        [SerializeField] private Button _backButton;
        
        [SerializeField] private TMP_Text _starsText;
        public Sprite sprStar;
        public Transform tranOfYellowCirle;



        private void Awake()
        {
            _diContainer.Inject(m_BoardInfo);
            _adMobController.ShowBanner(true);
            _bannerViewController.ChangePosition(AdPosition.Top);
        }

        private void Start()
        {
            _backButton.onClick.AddListener(() => SetButton(_backButton));
            
            UpdateTextStar();
        }


        private void SetButton(Button bu)
        {
            if (bu == _backButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                _uiController.LoadScene(UIController.SCENE.LevelSelection);
            }
            
        }
        
        private void UpdateTextStar()
        {
            _starsText.text = _upgradeController.GetTotalStarEquied()
                                  + "/" + _dataController.playerData.GetAllStars();

        }

        private void OnDisable()
        {
            _dataController.SaveData();//save
        }
    }
}



