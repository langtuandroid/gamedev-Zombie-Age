using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace MANAGERS
{
    public class TutorialController : MonoBehaviour
    {
        [Inject] private UIController _uiController;
        [Inject] private DataController _dataController;
        public enum TUTORIAL
        {
            none,
            check_in,
            level_selection,
            gameplay,
            weapon,
            equip_fn90,
        }
        
        public static TutorialController Instance;
        private Camera _mainCamera;

        [FormerlySerializedAs("CURRENT_STEP_TUTORIAL")] public TutorialPage _currentStep;


        //TUTORIAL
        [System.Serializable]
        public class Tutorial
        {
            [FormerlySerializedAs("eTutorial")] public TUTORIAL _tutorialType;
            [FormerlySerializedAs("objGroup")] [SerializeField] private GameObject _objects;
            [FormerlySerializedAs("bCompleted")] [SerializeField] public bool _isCompleted;

            [FormerlySerializedAs("LIST_STEP")] [Space(10)]
            public List<TutorialPage> _stepList;
            private int _currentSteps = -1;

            public void Next()
            {
                Debug.Log("NEXT");
                _currentSteps++;

                int _total = _stepList.Count;
                if (_currentSteps == _total)
                {
                    Instance.GetTutorial(Instance._currentStep.TutorialState)._isCompleted = true;
                    Instance._currentStep = null;
                    _objects.SetActive(false);
                    return;
                }



                for (int i = 0; i < _total; i++)
                {
                    if (i == _currentSteps)
                    {
                        Instance.Activate(_stepList[i].gameObject, true, _stepList[i].Delay);
                        // LIST_STEP[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        _stepList[i].gameObject.SetActive(false);
                    }
                }
            }

            public void SetCompleted(bool _completed)
            {
                _isCompleted = _completed;
                _objects.SetActive(!_isCompleted);
            }
        }


        [Space(20)]
        [FormerlySerializedAs("LIST_TUTORIAL")] public List<Tutorial> _tutorialList;
        public Tutorial GetTutorial(TUTORIAL _tut)
        {
            int _total = _tutorialList.Count;
            for (int i = 0; i < _total; i++)
            {
                if (_tutorialList[i]._tutorialType == _tut)
                    return _tutorialList[i];
            }
            return null;
        }

        private void SkipStep()
        {
            if (_currentStep != null)
            {
                GetTutorial(_currentStep.TutorialState).SetCompleted(true);
            }
            _currentStep = null;



            int _total = _tutorialList.Count;
            for (int i = 0; i < _total; i++)
            {
                for (int j = 0; j < _tutorialList[i]._stepList.Count; j++)
                {
                    if (!_tutorialList[i]._isCompleted)
                        _tutorialList[i]._stepList[j].gameObject.SetActive(false);
                }

            }
        }
        
        public bool IsRightInput()
        {
            if (_currentStep == null)
            {
                Debug.Log("NULL - TRUE");
                return true;
            }
            if (_currentStep.IsRightInputPos())
            {
                Debug.Log("RIGHT POS");
                return true;
            }
            Debug.Log("WRONG POS");
            return false;
        }
        
        private void Awake()
        {
            if (Instance == null) Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }

        
        private Vector2 _lastInputPosDown;
        private Vector2 _lastInputPosUp;
        private float _distance;

        private void Update()
        {
            if (_currentStep == null) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (!_currentStep.IsRightInputPos()) return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_currentStep.IsRightInputPos())
                    GetTutorial(_currentStep.TutorialState).Next();
            }
        }
        
        public void Activate(GameObject _obj, bool _active, float _timedelay)
        {
            StartCoroutine(IeSetActive(_obj, _active, _timedelay));
        }
        private IEnumerator IeSetActive(GameObject _obj, bool _active, float _timedelay)
        {
            yield return new WaitForSeconds(_timedelay);
            _obj.SetActive(_active);
        }

        private bool IsCompleted()
        {
            int _total = _tutorialList.Count;
            if (_tutorialList[_total - 1]._isCompleted)
                return true;
            return false;
        }


        private void HandleShowPopup(UIController.POP_UP _popup)
        {
            if (_popup == UIController.POP_UP.gameover || _popup == UIController.POP_UP.victory)
            {
                SkipStep();
            }
            else if (_popup == UIController.POP_UP.check_in)
            {
                if (_dataController.playerData._day == 0)
                {
                    SkipStep();
                    GetTutorial(TUTORIAL.check_in).Next();
                }
            }
            else if (_popup == UIController.POP_UP.reward)
            {
                if (_uiController._cuurentScene == UIController.SCENE.Weapon
                    && _dataController.playerData.GetAllStars() == 0)
                {
                    SkipStep();
                    GetTutorial(TUTORIAL.weapon).Next();
                }
            }
        }

        private void HandleHidePopup(UIController.POP_UP _popup)
        {
            if (_popup == UIController.POP_UP.reward)
            {
                if (_uiController._cuurentScene == UIController.SCENE.LevelSelection
                    && _dataController.playerData.GetAllStars() == 0)
                {
                    SkipStep();
                    GetTutorial(TUTORIAL.level_selection).Next();
                }
            }
        }


        private void HandleGetReward(EnumController.REWARD _reward)
        {
            if (_reward == EnumController.REWARD.unlock_gun_ar15)
            {
                SkipStep();
                GetTutorial(TUTORIAL.equip_fn90).Next();
            }

        }

        private void HandeStartNewScene()
        {
            if (_uiController._cuurentScene == UIController.SCENE.Gameplay)
            {
                if (_dataController.playerData.GetAllStars() == 0)
                {
                    SkipStep();
                    GetTutorial(TUTORIAL.gameplay).Next();//show level selection
                }
            }

            if (IsCompleted())
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            EventController.OnShowPopup += HandleShowPopup;
            EventController.OnHidePopup += HandleHidePopup;
            EventController.OnGetReward += HandleGetReward;
            EventController.OnStartNewScene += HandeStartNewScene;
        }



        private void OnDisable()
        {
            EventController.OnShowPopup -= HandleShowPopup;
            EventController.OnHidePopup -= HandleHidePopup;
            EventController.OnGetReward -= HandleGetReward;
            EventController.OnStartNewScene -= HandeStartNewScene;

            Instance = null;
        }
    }
}


