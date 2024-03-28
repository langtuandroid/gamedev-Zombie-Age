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
            [SerializeField] UpgradeButton m_ButtonUpgrade;
            
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

            public void Show(UpgradeButton _ButtonUpgrade)
            {
                m_ButtonUpgrade = _ButtonUpgrade;

                buReset.onClick.RemoveAllListeners();
                buUpgrade.onClick.RemoveAllListeners();

                DATA = UpgradeController.Instance.GetUpgrade(m_ButtonUpgrade._upgradeType);
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
                        SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//s
                        DATA.Remove();
                        Show(m_ButtonUpgrade);
                        m_ButtonUpgrade.Construct();
                        Instance.UpdateTextStar();

                        DataController.Instance.SaveData();//save
                    }
                    else
                        SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//s
                }
                else if (_bu == buUpgrade)
                {
                    if (!DATA.bEQUIPED)
                    {
                        int _star = 0;

                        _star = DataController.Instance.playerData.GetAllStars()
                                - UpgradeController.Instance.GetTotalStarEquied();


                        if (DataController.Instance.mode == DataController.Mode.Release)
                        {
                            if (_star < DATA.iStar)
                            {
                                SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                                UIController.Instance.PopUpShow(UIController.POP_UP.note);
                                Note.AssignNote(Note.NOTE.no_enought_star_to_upgrade.ToString());
                                return;
                            }
                        }

                        SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//s
                        DATA.Upgrade();
                        Show(m_ButtonUpgrade);
                        m_ButtonUpgrade.Construct();
                        Instance.UpdateTextStar();

                        DataController.Instance.SaveData();//save
                    }
                    else
                        SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//s
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
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                UIController.Instance.LoadScene(UIController.SCENE.LevelSelection);
            }
        }


        private void UpdateTextStar()
        {
            txtStar_Yellow.text = UpgradeController.Instance.GetTotalStarEquied()
                                  + "/" + DataController.Instance.playerData.GetAllStars();

        }

        private void OnDisable()
        {
            //save
            DataController.Instance.SaveData();//save
        }
    }
}



