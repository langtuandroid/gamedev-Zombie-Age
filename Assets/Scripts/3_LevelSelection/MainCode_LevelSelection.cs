using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainCode_LevelSelection : MonoBehaviour
{
    //CLASS POP UP DIFFICFUT
    [System.Serializable]
    public class DifficuftPopup
    {
        [SerializeField]
        private Button buClose, buEasy, buNormal, buNightmare, buStart;
        [SerializeField] Transform m_tranOfButtonChoose;


        [SerializeField] Image imaStarOfLevel;

        [Space(30)]
        [SerializeField] Image sprBackgroundLevel, sprFrameBackground;

        [SerializeField] Sprite sprButton_Easy;

        [SerializeField] Sprite sprButton_Normal;
        [SerializeField] Sprite sprButton_Normal_lock;

        [SerializeField] Sprite sprButton_Nightmare;
        [SerializeField] Sprite sprButton_Nightmare_lock;


        [Space(20)]
        [SerializeField]
        private Text txtLevel;

        [Header(" ***SPRITE OF DIFFICUFT BUTTON***")]
        [Space(30)]

        [SerializeField]
        private Sprite sprButtonDifficuft_Current;
        [SerializeField]
        private Sprite sprButtonDifficuft_Unlocked;


        [Header(" ***SPRITE OF STAR***")]
        [Space(30)]
        [SerializeField] Sprite SPRITE_OF_ZERO_STAR;
        public List<Sprite> LIST_STAR_SPRITE;


        private int iCurrentStar;



        //INIT
        public void Init()
        {
            buClose.onClick.AddListener(() => SetButton(buClose));
            buEasy.onClick.AddListener(() => SetButton(buEasy));
            buNormal.onClick.AddListener(() => SetButton(buNormal));
            buNightmare.onClick.AddListener(() => SetButton(buNightmare));
            buStart.onClick.AddListener(() => SetButton(buStart));
        }

        //SHOW INFO
        public void ShowInfo()
        {
            txtLevel.text = "LEVEL " + (TheDataManager.Instance.THE_DATA_PLAYER.iCurrentLevel + 1).ToString();


            int _level = TheDataManager.Instance.THE_DATA_PLAYER.iCurrentLevel;
            iCurrentStar = TheDataManager.Instance.THE_DATA_PLAYER.GetStar(_level);

            StateOfDifficuftButton();
            if (iCurrentStar == 2 || iCurrentStar == 3)
            {
                TheDataManager.Instance.THE_DATA_PLAYER.CURRENT_DIFFICUFT = TheEnumManager.DIFFICUFT.nightmare;
                m_tranOfButtonChoose.position = buNightmare.transform.position;
            }
            else if (iCurrentStar == 1)
            {
                TheDataManager.Instance.THE_DATA_PLAYER.CURRENT_DIFFICUFT = TheEnumManager.DIFFICUFT.normal;
                m_tranOfButtonChoose.position = buNormal.transform.position;
            }
            else
            {
                TheDataManager.Instance.THE_DATA_PLAYER.CURRENT_DIFFICUFT = TheEnumManager.DIFFICUFT.easy;
                m_tranOfButtonChoose.position = buEasy.transform.position;
            }

            //show star          
            imaStarOfLevel.sprite = GetSpriteOfStar(iCurrentStar);

            //-------------------- ICON BACKGROUND
            LevelData _leveldata = Resources.Load<LevelData>("Levels/Configs/Level_" + (TheDataManager.Instance.THE_DATA_PLAYER.iCurrentLevel + 1));
            sprBackgroundLevel.sprite = _leveldata.sprBackground;
            sprFrameBackground.sprite = _leveldata.sprBackgroundFrame;

        }

        //STATE OF DIFFICUFT BUTTON
        private void StateOfDifficuftButton()
        {
            if (iCurrentStar == 1)
            {
                buNormal.image.sprite = sprButton_Normal;
                buNightmare.image.sprite = sprButton_Nightmare_lock;
            }
            else if (iCurrentStar == 2)
            {
                buNormal.image.sprite = sprButton_Normal;
                buNightmare.image.sprite = sprButton_Nightmare;
            }
            else if (iCurrentStar == 3)
            {
                buNormal.image.sprite = sprButton_Normal;
                buNightmare.image.sprite = sprButton_Nightmare;
            }
            else if (iCurrentStar <= 0)
            {
                buNormal.image.sprite = sprButton_Normal_lock;
                buNightmare.image.sprite = sprButton_Nightmare_lock;
            }

        }


        //SET BUTTON
        private void SetButton(Button _bu)
        {
            //for tutorial
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }


            if (_bu == buClose)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_back);//sound
                MainCode_LevelSelection.Instance.SetActiveDifficuftPopup(false);
            }
            else if (_bu == buEasy)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheDataManager.Instance.THE_DATA_PLAYER.CURRENT_DIFFICUFT = TheEnumManager.DIFFICUFT.easy;
                // StateOfDifficuftButton();
                m_tranOfButtonChoose.position = buEasy.transform.position;
            }
            else if (_bu == buNormal)
            {

                if (iCurrentStar >= 1 || TheDataManager.Instance.eMode == TheDataManager.MODE.Debug)
                {
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.CURRENT_DIFFICUFT = TheEnumManager.DIFFICUFT.normal;
                    // StateOfDifficuftButton();
                    m_tranOfButtonChoose.position = buNormal.transform.position;
                }
                else
                {
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                    TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.note);
                    Note.SetNote(Note.NOTE.no_enought_star.ToString());
                    Debug.Log("LOCKED");
                }


            }
            else if (_bu == buNightmare)
            {
                if (iCurrentStar >= 2 || TheDataManager.Instance.eMode == TheDataManager.MODE.Debug)
                {
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.CURRENT_DIFFICUFT = TheEnumManager.DIFFICUFT.nightmare;
                    // StateOfDifficuftButton();
                    m_tranOfButtonChoose.position = buNightmare.transform.position;
                }
                else
                {
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                    TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.note);
                    Note.SetNote(Note.NOTE.no_enought_star.ToString());
                    Debug.Log("LOCKED");
                }

            }
            else if (_bu == buStart)
            {
                Debug.Log("START");
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_gun_shotgun);//sound
                TheUiManager.Instance.LoadScene(TheUiManager.SCENE.Weapon);
            }
        }

        //GET SPRITE OF STAR
        private Sprite GetSpriteOfStar(int _star)
        {
            if (_star <= 0) return SPRITE_OF_ZERO_STAR;
            return LIST_STAR_SPRITE[_star - 1];

        }
    }



    public static MainCode_LevelSelection Instance;

    public GameObject POPUP_DIFFICUFT;
    //public Transform m_tranOfRay;
    public DifficuftPopup m_DifficuftPopup;
    [SerializeField] Animator m_animatorOfLevelBoard;

    [Space(30)]
    [SerializeField] Button buMapNext;
    [SerializeField] Button buMapBack;
    [SerializeField] Button buShop;
    [SerializeField] Button buVideoReward;
    [SerializeField] Button buSetting;
    [SerializeField] Button buUpgrade;
    [SerializeField] Button buBugReport;


    [Space(20)]
    [SerializeField] Text txtCountMap;
    [SerializeField] Text txtTotalStar;


    [Header("*** CONFIG SPRITE ***")]
    [Space(30)]
    public Transform GROUP_LEVEL_BUTTONS;
    public Sprite sprCurrentLevelSprite;
    public Sprite sprLockedLevelSprite;
    public List<Sprite> SPRITE_OF_LEVEL;

    [Space(20)]
    public Transform tranRay;
    private Vector3 vEuler;


    private int iIndexOfMap;
    private int iTotalLevelButton;
    private int iTotalMap = 3;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        TheUiManager.Instance.SetCameraForPopupCanvas(Camera.main);//set camera

        //----------
        iTotalMap = Mathf.CeilToInt(TheDataManager.Instance.TOTAL_LEVEL_IN_GAME / 15.0f);
        iIndexOfMap = (int)(TheDataManager.Instance.THE_DATA_PLAYER.LIST_STAR.Count / 15.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        buMapNext.onClick.AddListener(() => SetButton(buMapNext));
        buMapBack.onClick.AddListener(() => SetButton(buMapBack));

        buSetting.onClick.AddListener(() => SetButton(buSetting));
        buShop.onClick.AddListener(() => SetButton(buShop));
        buVideoReward.onClick.AddListener(() => SetButton(buVideoReward));
        buBugReport.onClick.AddListener(() => SetButton(buBugReport));
        buUpgrade.onClick.AddListener(() => SetButton(buUpgrade));



        iTotalLevelButton = GROUP_LEVEL_BUTTONS.childCount;
        InitMap(iIndexOfMap);
        txtTotalStar.text = TheDataManager.Instance.THE_DATA_PLAYER.GetTotalStar().ToString();


        m_DifficuftPopup.Init();

        //CHECK IN
        CheckIn(0.5f);
    }


    private void Update()
    {
        vEuler.z -= 0.2f;
        tranRay.eulerAngles = vEuler;
    }

    //SET BUTTON FUNCTION
    private void SetButton(Button _bu)
    {
        //for tutorial
        if (TheTutorialManager.Instance)
        {
            if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
        }

        if (_bu == buMapNext)
        {

            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
            iIndexOfMap++;
            if (iIndexOfMap == iTotalMap)
                iIndexOfMap = 0;

            InitMap(iIndexOfMap);
        }
        else if (_bu == buMapBack)
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_back);//sound
            iIndexOfMap--;
            if (iIndexOfMap < 0)
                iIndexOfMap = iTotalMap - 1;

            InitMap(iIndexOfMap);
        }
        else if (_bu == buShop)
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
            TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.shop);
        }
        else if (_bu == buVideoReward)
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);
            TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.video_reward);
        }
        else if (_bu == buSetting)
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);
            TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.setting);
        }
        else if (_bu == buUpgrade)
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);
            TheUiManager.Instance.LoadScene(TheUiManager.SCENE.Upgrade);
        }
        else if (_bu == buBugReport)
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);
            TheUiManager.Instance.ReportEmail();
        }
    }








    //SHOT INDEX LEVEL BUTTON
    private void InitMap(int _index)
    {
        tranRay.position = Vector3.one * 1000;
        StartCoroutine(IeInitMap(_index));

    }
    private IEnumerator IeInitMap(int _index)
    {
        yield return new WaitForSecondsRealtime(0.02f);
        Transform _child;
        for (int i = 0; i < iTotalLevelButton; i++)
        {
            _child = GROUP_LEVEL_BUTTONS.GetChild(i);
            _child.GetComponent<ButtonLevel>().Init(i + _index * iTotalLevelButton);

            //ray
            if (_child.GetComponent<ButtonLevel>().Unlock && _child.GetComponent<ButtonLevel>().iStar <= 0)
                tranRay.position = _child.position;
        }


        txtCountMap.text = (_index + 1) + "/" + (iTotalMap);
        m_animatorOfLevelBoard.Play(0);
    }



    //SHOW / HDE POPUP DIFFICUFT
    public void SetActiveDifficuftPopup(bool _active)
    {
        POPUP_DIFFICUFT.SetActive(_active);
        if (_active)
            m_DifficuftPopup.ShowInfo();
    }



    //CHECK IN
    public void CheckIn(float _timedelay)
    {
        StartCoroutine(IeShowCheckIn(_timedelay));
    }
    private IEnumerator IeShowCheckIn(float _timedelay)
    {
        yield return new WaitForSecondsRealtime(_timedelay);

        if (TheDataManager.Instance.THE_DATA_PLAYER.IsReadyToGetCheckIn())
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_wood_board);//sound
            TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.check_in);

        }
    }



}

