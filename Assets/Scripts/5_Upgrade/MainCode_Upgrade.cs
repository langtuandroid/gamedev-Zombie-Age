using MANAGERS;
using MODULES.Scriptobjectable;
using SCREENS;
using UnityEngine;
using UnityEngine.UI;

namespace _5_Upgrade
{
    public class MainCode_Upgrade : MonoBehaviour
    {
        [System.Serializable]
        public class BoardInfo
        {
            [SerializeField] UpgradeData DATA;
            [SerializeField] ButtonUpgrade m_ButtonUpgrade;



            [SerializeField] Image imaIcon;
            [SerializeField] Image imaStar;

            [Space(20)]
            [SerializeField] Text txtValueStar;
            [SerializeField] Text txtName;
            [SerializeField] Text txtContent;

            [SerializeField] Button buReset;
            [SerializeField] Button buUpgrade;


            public BoardInfo()
            {

            }

            public void Show(ButtonUpgrade _ButtonUpgrade)
            {
                m_ButtonUpgrade = _ButtonUpgrade;

                buReset.onClick.RemoveAllListeners();
                buUpgrade.onClick.RemoveAllListeners();

                DATA = TheUpgradeManager.Instance.GetUpgrade(m_ButtonUpgrade.eUpgrade);
                buReset.onClick.AddListener(() => SetButton(buReset));
                buUpgrade.onClick.AddListener(() => SetButton(buUpgrade));



                txtValueStar.text = DATA.iStar.ToString();
                txtName.text = DATA.strName;
                txtContent.text = DATA.strContent;

                imaStar.sprite = Instance.sprStar;



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
                        TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//s
                        DATA.Remove();
                        Show(m_ButtonUpgrade);
                        m_ButtonUpgrade.Init();
                        Instance.UpdateTextStar();

                        TheDataManager.Instance.SaveDataPlayer();//save
                    }
                    else
                        TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//s
                }
                else if (_bu == buUpgrade)
                {
                    if (!DATA.bEQUIPED)
                    {
                        int _star = 0;

                        _star = TheDataManager.Instance.THE_DATA_PLAYER.GetTotalStar()
                                - TheUpgradeManager.Instance.GetTotalStarEquied();


                        if (TheDataManager.Instance.eMode == TheDataManager.MODE.Release)
                        {
                            if (_star < DATA.iStar)
                            {
                                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                                TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.note);
                                Note.SetNote(Note.NOTE.no_enought_star_to_upgrade.ToString());
                                return;
                            }
                        }

                        TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//s
                        DATA.Upgrade();
                        Show(m_ButtonUpgrade);
                        m_ButtonUpgrade.Init();
                        Instance.UpdateTextStar();

                        TheDataManager.Instance.SaveDataPlayer();//save
                    }
                    else
                        TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//s
                }
            }
        }




        public static MainCode_Upgrade Instance;
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
            if (Instance == null)
                Instance = this;

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
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheUiManager.Instance.LoadScene(TheUiManager.SCENE.LevelSelection);
            }
        }


        private void UpdateTextStar()
        {
            txtStar_Yellow.text = TheUpgradeManager.Instance.GetTotalStarEquied()
                                  + "/" + TheDataManager.Instance.THE_DATA_PLAYER.GetTotalStar();

        }

        private void OnDisable()
        {
            //save
            TheDataManager.Instance.SaveDataPlayer();//save
        }
    }
}



