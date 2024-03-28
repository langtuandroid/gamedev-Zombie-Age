using System.Collections;
using System.Collections.Generic;
using MANAGERS;
using MODULES.Scriptobjectable;
using SCREENS;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _3_LevelSelection
{
    public class LevelSelectionController : MonoBehaviour
    {
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
                txtLevel.text = "LEVEL " + (DataController.Instance.playerData.CurrentLevel + 1).ToString();


                int _level = DataController.Instance.playerData.CurrentLevel;
                iCurrentStar = DataController.Instance.playerData.NumOfStars(_level);

                StateOfDifficuftButton();
                if (iCurrentStar == 2 || iCurrentStar == 3)
                {
                    DataController.Instance.playerData.Difficuft = EnumController.DIFFICUFT.nightmare;
                    m_tranOfButtonChoose.position = buNightmare.transform.position;
                }
                else if (iCurrentStar == 1)
                {
                    DataController.Instance.playerData.Difficuft = EnumController.DIFFICUFT.normal;
                    m_tranOfButtonChoose.position = buNormal.transform.position;
                }
                else
                {
                    DataController.Instance.playerData.Difficuft = EnumController.DIFFICUFT.easy;
                    m_tranOfButtonChoose.position = buEasy.transform.position;
                }

                //show star          
                imaStarOfLevel.sprite = GetSpriteOfStar(iCurrentStar);

                //-------------------- ICON BACKGROUND
                LevelData _leveldata = Resources.Load<LevelData>("Levels/Configs/Level_" + (DataController.Instance.playerData.CurrentLevel + 1));
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
                if (TutorialController.Instance)
                {
                    if (!TutorialController.Instance.IsRightInput()) return;
                }


                if (_bu == buClose)
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_click_back);//sound
                    LevelSelectionController.Instance.SetDifficultPopUp(false);
                }
                else if (_bu == buEasy)
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                    DataController.Instance.playerData.Difficuft = EnumController.DIFFICUFT.easy;
                    // StateOfDifficuftButton();
                    m_tranOfButtonChoose.position = buEasy.transform.position;
                }
                else if (_bu == buNormal)
                {

                    if (iCurrentStar >= 1 || DataController.Instance.mode == DataController.Mode.Debug)
                    {
                        SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                        DataController.Instance.playerData.Difficuft = EnumController.DIFFICUFT.normal;
                        // StateOfDifficuftButton();
                        m_tranOfButtonChoose.position = buNormal.transform.position;
                    }
                    else
                    {
                        SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                        UIController.Instance.PopUpShow(UIController.POP_UP.note);
                        Note.AssignNote(Note.NOTE.no_enought_star.ToString());
                        Debug.Log("LOCKED");
                    }


                }
                else if (_bu == buNightmare)
                {
                    if (iCurrentStar >= 2 || DataController.Instance.mode == DataController.Mode.Debug)
                    {
                        SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                        DataController.Instance.playerData.Difficuft = EnumController.DIFFICUFT.nightmare;
                        // StateOfDifficuftButton();
                        m_tranOfButtonChoose.position = buNightmare.transform.position;
                    }
                    else
                    {
                        SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                        UIController.Instance.PopUpShow(UIController.POP_UP.note);
                        Note.AssignNote(Note.NOTE.no_enought_star.ToString());
                        Debug.Log("LOCKED");
                    }

                }
                else if (_bu == buStart)
                {
                    Debug.Log("START");
                    SoundController.Instance.Play(SoundController.SOUND.sfx_gun_shotgun);//sound
                    UIController.Instance.LoadScene(UIController.SCENE.Weapon);
                }
            }

            //GET SPRITE OF STAR
            private Sprite GetSpriteOfStar(int _star)
            {
                if (_star <= 0) return SPRITE_OF_ZERO_STAR;
                return LIST_STAR_SPRITE[_star - 1];

            }
        }
        
        public static LevelSelectionController Instance;

        [FormerlySerializedAs("POPUP_DIFFICUFT")] [SerializeField] private GameObject _popupDifficult;
        [FormerlySerializedAs("m_DifficuftPopup")] [SerializeField] private DifficuftPopup _difficultPopup;
        [FormerlySerializedAs("m_animatorOfLevelBoard")] [SerializeField] private Animator _animatorOfLevelBoard;

        
        [Space(30)]
        [FormerlySerializedAs("buMapNext")] [SerializeField] private Button _mapNextButton;
        [FormerlySerializedAs("buMapBack")] [SerializeField] private Button _backMapButton;
        [FormerlySerializedAs("buShop")] [SerializeField] private Button _shopButton;
        [FormerlySerializedAs("buVideoReward")] [SerializeField] private Button _vireoRevardButton;
        [FormerlySerializedAs("buSetting")] [SerializeField] private Button _settingsButton;
        [FormerlySerializedAs("buUpgrade")] [SerializeField] private Button _upgradeButton;
        [FormerlySerializedAs("buBugReport")] [SerializeField] private Button _bugSupportButton;


        
        [Space(20)]
        [FormerlySerializedAs("txtCountMap")] [SerializeField] private Text _countMapText;
        [FormerlySerializedAs("txtTotalStar")] [SerializeField] private Text _totalStarText;


        
        [Header("*** CONFIG SPRITE ***")]
        [Space(30)]
        [FormerlySerializedAs("GROUP_LEVEL_BUTTONS")][SerializeField] private Transform _levelButtonGroup;
        [FormerlySerializedAs("sprCurrentLevelSprite")] [SerializeField] private Sprite _levelSprites;
        [FormerlySerializedAs("sprLockedLevelSprite")] [SerializeField] private Sprite _lockedLevelSprite;
        [FormerlySerializedAs("SPRITE_OF_LEVEL")] [SerializeField] private List<Sprite> _spriteOfLevel;

        [Space(20)]
        [SerializeField] private Transform tranRay;
        
        private Vector3 vEuler;
        private int iIndexOfMap;
        private int iTotalLevelButton;
        private int iTotalMap = 3;

        public Sprite LevelCurrSprite => _levelSprites;
        public Sprite LockedSprite => _lockedLevelSprite;
        public List<Sprite> SpriteOfLevels => _spriteOfLevel;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            UIController.Instance.SetCameraPopup(Camera.main);//set camera

            iTotalMap = Mathf.CeilToInt(DataController.Instance.LevelsTotal / 15.0f);
            iIndexOfMap = (int)(DataController.Instance.playerData.StarList.Count / 15.0f);
        }
        
        private void Start()
        {
            _mapNextButton.onClick.AddListener(() => AssignButtons(_mapNextButton));
            _backMapButton.onClick.AddListener(() => AssignButtons(_backMapButton));

            _settingsButton.onClick.AddListener(() => AssignButtons(_settingsButton));
            _shopButton.onClick.AddListener(() => AssignButtons(_shopButton));
            _vireoRevardButton.onClick.AddListener(() => AssignButtons(_vireoRevardButton));
            _bugSupportButton.onClick.AddListener(() => AssignButtons(_bugSupportButton));
            _upgradeButton.onClick.AddListener(() => AssignButtons(_upgradeButton));



            iTotalLevelButton = _levelButtonGroup.childCount;
            ConstructMap(iIndexOfMap);
            _totalStarText.text = DataController.Instance.playerData.GetAllStars().ToString();


            _difficultPopup.Init();

            Check(0.5f);
        }


        private void Update()
        {
            vEuler.z -= 0.2f;
            tranRay.eulerAngles = vEuler;
        }
        
        private void AssignButtons(Button button)
        {
            //for tutorial
            if (TutorialController.Instance)
            {
                if (!TutorialController.Instance.IsRightInput()) return;
            }

            if (button == _mapNextButton)
            {

                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                iIndexOfMap++;
                if (iIndexOfMap == iTotalMap)
                    iIndexOfMap = 0;

                ConstructMap(iIndexOfMap);
            }
            else if (button == _backMapButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_back);//sound
                iIndexOfMap--;
                if (iIndexOfMap < 0)
                    iIndexOfMap = iTotalMap - 1;

                ConstructMap(iIndexOfMap);
            }
            else if (button == _shopButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                UIController.Instance.PopUpShow(UIController.POP_UP.shop);
            }
            else if (button == _vireoRevardButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);
                UIController.Instance.PopUpShow(UIController.POP_UP.video_reward);
            }
            else if (button == _settingsButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);
                UIController.Instance.PopUpShow(UIController.POP_UP.setting);
            }
            else if (button == _upgradeButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);
                UIController.Instance.LoadScene(UIController.SCENE.Upgrade);
            }
            else if (button == _bugSupportButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);
                
            }
        }
        
        //SHOT INDEX LEVEL BUTTON
        private void ConstructMap(int _index)
        {
            tranRay.position = Vector3.one * 1000;
            StartCoroutine(InitMapRoutine(_index));

        }
        private IEnumerator InitMapRoutine(int _index)
        {
            yield return new WaitForSecondsRealtime(0.02f);
            Transform _child;
            for (int i = 0; i < iTotalLevelButton; i++)
            {
                _child = _levelButtonGroup.GetChild(i);
                _child.GetComponent<LvlButton>().Consruct(i + _index * iTotalLevelButton);

                if (_child.GetComponent<LvlButton>().Unlock && _child.GetComponent<LvlButton>().IStar <= 0)
                    tranRay.position = _child.position;
            }


            _countMapText.text = (_index + 1) + "/" + (iTotalMap);
            _animatorOfLevelBoard.Play(0);
        }
        
        public void SetDifficultPopUp(bool _active)
        {
            _popupDifficult.SetActive(_active);
            if (_active)
                _difficultPopup.ShowInfo();
        }
        
        public void Check(float delay)
        {
            StartCoroutine(CheckRoutine(delay));
        }
        private IEnumerator CheckRoutine(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            if (DataController.Instance.playerData.CheckInReady())
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_wood_board);//sound
                UIController.Instance.PopUpShow(UIController.POP_UP.check_in);

            }
        }



    }
}

