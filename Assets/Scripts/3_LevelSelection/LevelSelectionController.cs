using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Integration;
using MANAGERS;
using SCREENS;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace _3_LevelSelection
{
    public class LevelSelectionController : MonoBehaviour
    {
        [Inject] private BannerViewController _bannerViewController;
        [Inject] private AdMobController _adMobController;
        [Inject] private UIController _uiController;
        [Inject] private SoundController _soundController;
        [Inject] private DiContainer _diContainer;
        [Inject] private DataController _dataController;
        [Inject] private TutorialController _tutorialController;
        
        [System.Serializable]
        public class DifficuftPopup
        {
            [Inject] private UIController _uiController;
            [Inject] private DataController _dataController;
            [Inject] private SoundController _soundController;
            [Inject] private LevelSelectionController _levelSelectionController;
            [Inject] private TutorialController _tutorialController;
            [SerializeField] private Button buClose, buEasy, buNormal, buNightmare, buStart;
            [SerializeField] Transform m_tranOfButtonChoose;
            [SerializeField] private Image[] _starsImages;
            [SerializeField] private Sprite _starComplitedSprite;
            [SerializeField] private Sprite _starEmptySprite;

            [Space(20)]
            [SerializeField]
            private TMP_Text txtLevel;

            [Header(" ***SPRITE OF DIFFICUFT BUTTON***")]
            [Space(30)]

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
                txtLevel.text = "LEVEL " + (_dataController.playerData.CurrentLevel + 1).ToString();
                int _level = _dataController.playerData.CurrentLevel;
                iCurrentStar = _dataController.playerData.NumOfStars(_level);

                for (int i = 0; i < 3; i++)
                {
                    _starsImages[i].sprite = _starEmptySprite;
                }
                
                StateOfDifficuftButton();
                if (iCurrentStar == 2 || iCurrentStar == 3)
                {
                    _dataController.playerData.Difficuft = EnumController.DIFFICUFT.nightmare;
                    m_tranOfButtonChoose.position = buNightmare.transform.position;
                }
                else if (iCurrentStar == 1)
                {
                    _dataController.playerData.Difficuft = EnumController.DIFFICUFT.normal;
                    m_tranOfButtonChoose.position = buNormal.transform.position;
                }
                else
                {
                    _dataController.playerData.Difficuft = EnumController.DIFFICUFT.easy;
                    m_tranOfButtonChoose.position = buEasy.transform.position;
                }
                
                for (int i = 0; i < iCurrentStar; i++)
                {
                    _starsImages[i].sprite = _starComplitedSprite;
                }
            }
            
            private void StateOfDifficuftButton()
            {
                switch (iCurrentStar)
                {
                    case 1:
                        //buNormal.image.sprite = sprButton_Normal;
                        //buNightmare.image.sprite = sprButton_Nightmare_lock;
                        buNormal.interactable = true;
                        buNightmare.interactable = false;
                        break;
                    case 2 or 3:
                        //buNormal.image.sprite = sprButton_Normal;
                        //buNightmare.image.sprite = sprButton_Nightmare;
                        buNormal.interactable = true;
                        buNightmare.interactable = true;
                        break;
                    case <= 0:
                        //buNormal.image.sprite = sprButton_Normal_lock;
                        //buNightmare.image.sprite = sprButton_Nightmare_lock;
                        buNormal.interactable = false;
                        buNightmare.interactable = false;
                        break;
                }
            }


            //SET BUTTON
            private void SetButton(Button _bu)
            {
                //for tutorial
                if (_tutorialController)
                {
                    if (!_tutorialController.IsRightInput()) return;
                }


                if (_bu == buClose)
                {
                    _soundController.Play(SoundController.SOUND.ui_click_back);//sound
                    _levelSelectionController.SetDifficultPopUp(false);
                }
                else if (_bu == buEasy)
                {
                    _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                    _dataController.playerData.Difficuft = EnumController.DIFFICUFT.easy;
                    m_tranOfButtonChoose.position = buEasy.transform.position;
                }
                else if (_bu == buNormal)
                {

                    if (iCurrentStar >= 1 || _dataController.mode == DataController.Mode.Debug)
                    {
                        _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                        _dataController.playerData.Difficuft = EnumController.DIFFICUFT.normal;
                        m_tranOfButtonChoose.position = buNormal.transform.position;
                    }
                    else
                    {
                        _soundController.Play(SoundController.SOUND.ui_cannot);//sound
                        _uiController.PopUpShow(UIController.POP_UP.note);
                        Note.AssignNote(Note.NOTE.no_enought_star.ToString());
                        Debug.Log("LOCKED");
                    }


                }
                else if (_bu == buNightmare)
                {
                    if (iCurrentStar >= 2 || _dataController.mode == DataController.Mode.Debug)
                    {
                        _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                        _dataController.playerData.Difficuft = EnumController.DIFFICUFT.nightmare;
                        // StateOfDifficuftButton();
                        m_tranOfButtonChoose.position = buNightmare.transform.position;
                    }
                    else
                    {
                        _soundController.Play(SoundController.SOUND.ui_cannot);//sound
                        _uiController.PopUpShow(UIController.POP_UP.note);
                        Note.AssignNote(Note.NOTE.no_enought_star.ToString());
                        Debug.Log("LOCKED");
                    }

                }
                else if (_bu == buStart)
                {
                    Debug.Log("START");
                    _soundController.Play(SoundController.SOUND.sfx_gun_shotgun);//sound
                    _uiController.LoadScene(UIController.SCENE.Weapon);
                }
            }

            //GET SPRITE OF STAR
            private Sprite GetSpriteOfStar(int _star)
            {
                if (_star <= 0) return SPRITE_OF_ZERO_STAR;
                return LIST_STAR_SPRITE[_star - 1];

            }
        }
        
        [FormerlySerializedAs("POPUP_DIFFICUFT")] [SerializeField] private GameObject _popupDifficult;
        [FormerlySerializedAs("m_DifficuftPopup")] [SerializeField] private DifficuftPopup _difficultPopup;
        [FormerlySerializedAs("m_animatorOfLevelBoard")] [SerializeField] private Animator _animatorOfLevelBoard;
        
        [Space(30)]
        [FormerlySerializedAs("buMapNext")] [SerializeField] private Button _mapNextButton;
        [FormerlySerializedAs("buMapBack")] [SerializeField] private Button _backMapButton;
        [FormerlySerializedAs("buSetting")] [SerializeField] private Button _settingsButton;
        [FormerlySerializedAs("buUpgrade")] [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _gemButtons;
        
        [Space(20)]
        [FormerlySerializedAs("txtCountMap")] [SerializeField] private TMP_Text _countMapText;
        [FormerlySerializedAs("txtTotalStar")] [SerializeField] private TMP_Text _totalStarText;
        
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
        
        private void Awake()
        {
            _uiController.SetCameraPopup(Camera.main);//set camera

            iTotalMap = Mathf.CeilToInt(_dataController.LevelsTotal / 10.0f);
            iIndexOfMap = (int)(_dataController.playerData.StarList.Count / 10.0f);
        }
        
        private void Start()
        {
            _adMobController.ShowBanner(true);
            _bannerViewController.ChangePosition(AdPosition.Top);
            
            _mapNextButton.onClick.AddListener(() => AssignButtons(_mapNextButton));
            _backMapButton.onClick.AddListener(() => AssignButtons(_backMapButton));

            _settingsButton.onClick.AddListener(() => AssignButtons(_settingsButton));
            _upgradeButton.onClick.AddListener(() => AssignButtons(_upgradeButton));
            _gemButtons.onClick.AddListener((() => AssignButtons(_gemButtons)));


            iTotalLevelButton = _levelButtonGroup.childCount;
            ConstructMap(iIndexOfMap);
            _totalStarText.text = _dataController.playerData.GetAllStars().ToString();

            _diContainer.Inject(_difficultPopup);
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
            if (_tutorialController)
            {
                if (!_tutorialController.IsRightInput()) return;
            }

            if (button == _mapNextButton)
            {

                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                iIndexOfMap++;
                if (iIndexOfMap == iTotalMap)
                    iIndexOfMap = 0;

                ConstructMap(iIndexOfMap);
            }
            else if (button == _backMapButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_back);//sound
                iIndexOfMap--;
                if (iIndexOfMap < 0)
                    iIndexOfMap = iTotalMap - 1;

                ConstructMap(iIndexOfMap);
            }
            else if (button == _settingsButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);
                _uiController.PopUpShow(UIController.POP_UP.setting);
            }
            else if (button == _upgradeButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);
                _uiController.LoadScene(UIController.SCENE.Upgrade);
            }
            else if(button == _gemButtons)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);
                _uiController.PopUpShow(UIController.POP_UP.gems);
            }
        }
        
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

            if (_dataController.playerData.CheckInReady())
            {
                _soundController.Play(SoundController.SOUND.ui_wood_board);//sound
                _uiController.PopUpShow(UIController.POP_UP.check_in);

            }
        }
    }
}

