using MANAGERS;
using MODULES.Scriptobjectable;
using SCREENS;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _5_Upgrade
{
    public class UpgradeManager : MonoBehaviour
    {
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
            [SerializeField] UpgradeData DATA;
            [SerializeField] UpgradeButton m_ButtonUpgrade;
            
            [SerializeField] Image imaIcon;
            [SerializeField] Image imaStar;

            [Space(20)]
            [SerializeField] Text txtValueStar;
            [SerializeField] Text txtName;
            [SerializeField] Text txtContent;

            [SerializeField] Button buReset;
            [SerializeField] Button buUpgrade;
            
            public void Show(UpgradeButton _ButtonUpgrade)
            {
                m_ButtonUpgrade = _ButtonUpgrade;

                buReset.onClick.RemoveAllListeners();
                buUpgrade.onClick.RemoveAllListeners();

                DATA = _upgradeController.GetUpgrade(m_ButtonUpgrade._upgradeType);
                buReset.onClick.AddListener(() => SetButton(buReset));
                buUpgrade.onClick.AddListener(() => SetButton(buUpgrade));



                txtValueStar.text = DATA.iStar.ToString();
                txtName.text = DATA.strName;
                txtContent.text = DATA.strContent;

                imaStar.sprite = _upgradeManager.sprStar;
                
                if (DATA.bEQUIPED)
                {
                    imaIcon.sprite = DATA.sprIcon;
                    buUpgrade.image.color = Color.gray;
                    buReset.image.color = Color.white;


                }
                else
                {
                    imaIcon.sprite = DATA.sprIcon_gray;
                    buUpgrade.image.color = Color.white;
                    buReset.image.color = Color.gray;
                }



            }

            private void SetButton(Button _bu)
            {
                if (_bu == buReset)
                {
                    if (DATA.bEQUIPED)
                    {
                        _soundController.Play(SoundController.SOUND.ui_click_next);//s
                        DATA.Remove();
                        Show(m_ButtonUpgrade);
                        m_ButtonUpgrade.Construct();
                        _upgradeManager.UpdateTextStar();

                        _dataController.SaveData();//save
                    }
                    else
                        _soundController.Play(SoundController.SOUND.ui_cannot);//s
                }
                else if (_bu == buUpgrade)
                {
                    if (!DATA.bEQUIPED)
                    {
                        int _star = 0;

                        _star = _dataController.playerData.GetAllStars()
                                - _upgradeController.GetTotalStarEquied();


                        if (_dataController.mode == DataController.Mode.Release)
                        {
                            if (_star < DATA.iStar)
                            {
                                _soundController.Play(SoundController.SOUND.ui_cannot);//sound
                                _uiController.PopUpShow(UIController.POP_UP.note);
                                Note.AssignNote(Note.NOTE.no_enought_star_to_upgrade.ToString());
                                return;
                            }
                        }

                        _soundController.Play(SoundController.SOUND.ui_click_next);//s
                        DATA.Upgrade();
                        Show(m_ButtonUpgrade);
                        m_ButtonUpgrade.Construct();
                        _upgradeManager.UpdateTextStar();

                        _dataController.SaveData();//save
                    }
                    else
                        _soundController.Play(SoundController.SOUND.ui_cannot);//s
                }
            }
        }
        
        public BoardInfo m_BoardInfo;

        [SerializeField] Button buBack;

        [Space(20)]  
        [SerializeField] Text txtStar_Yellow;


        [Space(20)]
        public Sprite sprStar;

        [Space(20)]
        public Transform tranOfYellowCirle;



        private void Awake()
        {
            _diContainer.Inject(m_BoardInfo);
        }

        private void Start()
        {
            buBack.onClick.AddListener(() => SetButton(buBack));
            UpdateTextStar();
        }


        private void SetButton(Button bu)
        {
            if (bu == buBack)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                _uiController.LoadScene(UIController.SCENE.LevelSelection);
            }
        }
        
        private void UpdateTextStar()
        {
            txtStar_Yellow.text = _upgradeController.GetTotalStarEquied()
                                  + "/" + _dataController.playerData.GetAllStars();

        }

        private void OnDisable()
        {
            _dataController.SaveData();//save
        }
    }
}



