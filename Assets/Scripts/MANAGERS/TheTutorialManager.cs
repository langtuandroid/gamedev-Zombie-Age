using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheTutorialManager : MonoBehaviour
{
    public enum TUTORIAL
    {
        none,
        check_in,
        level_selection,
        gameplay,
        weapon,
        equip_fn90,
    }



    public static TheTutorialManager Instance;
    private Camera m_MainCamera;

    public StepTutorial CURRENT_STEP_TUTORIAL;


    //TUTORIAL
    [System.Serializable]
    public class Tutorial
    {
        public TUTORIAL eTutorial;
        public GameObject objGroup;
        public bool bCompleted;

        [Space(10)]
        public List<StepTutorial> LIST_STEP;
        private int iCurrentStepIndex = -1;

        public void NextStep()
        {
            Debug.Log("NEXT");
            iCurrentStepIndex++;

            int _total = LIST_STEP.Count;
            if (iCurrentStepIndex == _total)
            {
                Instance.GetTutorial(Instance.CURRENT_STEP_TUTORIAL.eTutorial).bCompleted = true;
                Instance.CURRENT_STEP_TUTORIAL = null;
                objGroup.SetActive(false);
                return;
            }



            for (int i = 0; i < _total; i++)
            {
                if (i == iCurrentStepIndex)
                {
                    Instance.SetActive(LIST_STEP[i].gameObject, true, LIST_STEP[i].fTimeDelay);
                    // LIST_STEP[i].gameObject.SetActive(true);
                }
                else
                {
                    LIST_STEP[i].gameObject.SetActive(false);
                }
            }
        }

        public void SetCompleted(bool _completed)
        {
            bCompleted = _completed;
            objGroup.SetActive(!bCompleted);
        }
    }


    [Space(20)]
    public List<Tutorial> LIST_TUTORIAL;
    public Tutorial GetTutorial(TUTORIAL _tut)
    {
        int _total = LIST_TUTORIAL.Count;
        for (int i = 0; i < _total; i++)
        {
            if (LIST_TUTORIAL[i].eTutorial == _tut)
                return LIST_TUTORIAL[i];
        }
        return null;
    }


    //SKIP TUTORIAL
    public void Skip()
    {
        if (CURRENT_STEP_TUTORIAL != null)
        {
            GetTutorial(CURRENT_STEP_TUTORIAL.eTutorial).SetCompleted(true);
        }
        CURRENT_STEP_TUTORIAL = null;



        int _total = LIST_TUTORIAL.Count;
        for (int i = 0; i < _total; i++)
        {
            for (int j = 0; j < LIST_TUTORIAL[i].LIST_STEP.Count; j++)
            {
                if (!LIST_TUTORIAL[i].bCompleted)
                    LIST_TUTORIAL[i].LIST_STEP[j].gameObject.SetActive(false);
            }

        }
    }


    //RIGHT INPUT POS FROM PLAYER
    public bool IsCheckRightInput()
    {
        if (CURRENT_STEP_TUTORIAL == null)
        {
            Debug.Log("NULL - TRUE");
            return true;
        }
        if (CURRENT_STEP_TUTORIAL.IsRightInputPos())
        {
            Debug.Log("RIGHT POS");
            return true;
        }
        Debug.Log("WRONG POS");
        return false;
    }


    //TUTORIAL
    public bool IsConducting() //Dang hướng dẫn
    {
        if (CURRENT_STEP_TUTORIAL != null)
            return true;
        else
            return false;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }




    private Vector2 vLAST_INPUT_POS_Down;
    private Vector2 vLAST_INPUT_POS_Up;
    private float _dis;

    private void Update()
    {
        if (CURRENT_STEP_TUTORIAL == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!CURRENT_STEP_TUTORIAL.IsRightInputPos()) return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (CURRENT_STEP_TUTORIAL.IsRightInputPos())
                GetTutorial(CURRENT_STEP_TUTORIAL.eTutorial).NextStep();
        }

    }


    //SET ACTIVE
    public void SetActive(GameObject _obj, bool _active, float _timedelay)
    {
        StartCoroutine(IeSetActive(_obj, _active, _timedelay));
    }
    private IEnumerator IeSetActive(GameObject _obj, bool _active, float _timedelay)
    {
        yield return new WaitForSeconds(_timedelay);
        _obj.SetActive(_active);
    }


    //CHECK TUTORIAL COMPLETED
    public bool IsCompleted()
    {
        int _total = LIST_TUTORIAL.Count;
        if (LIST_TUTORIAL[_total - 1].bCompleted)
            return true;
        return false;
    }


    //EVENT
    private void HandleShowPopup(TheUiManager.POP_UP _popup)
    {
        if (_popup == TheUiManager.POP_UP.gameover || _popup == TheUiManager.POP_UP.victory)
        {
            Skip();
        }
        else if (_popup == TheUiManager.POP_UP.check_in)
        {
            if (TheDataManager.Instance.THE_DATA_PLAYER.iCurrentDay == 0)
            {
                Skip();
                GetTutorial(TUTORIAL.check_in).NextStep();
            }
        }
        else if (_popup == TheUiManager.POP_UP.reward)
        {
            if (TheUiManager.Instance.CURRENT_SCENE == TheUiManager.SCENE.Weapon
                && TheDataManager.Instance.THE_DATA_PLAYER.GetTotalStar() == 0)
            {
                Skip();
                GetTutorial(TUTORIAL.weapon).NextStep();
            }
        }
    }

    private void HandleHidePopup(TheUiManager.POP_UP _popup)
    {
        if (_popup == TheUiManager.POP_UP.reward)
        {
            if (TheUiManager.Instance.CURRENT_SCENE == TheUiManager.SCENE.LevelSelection
                && TheDataManager.Instance.THE_DATA_PLAYER.GetTotalStar() == 0)
            {
                Skip();
                GetTutorial(TUTORIAL.level_selection).NextStep();
            }
        }
    }


    private void HandleGetReward(TheEnumManager.REWARD _reward)
    {
        if (_reward == TheEnumManager.REWARD.unlock_gun_ar15)
        {
            Skip();
            GetTutorial(TUTORIAL.equip_fn90).NextStep();
        }

    }

    private void HandeStartNewScene()
    {
        if (TheUiManager.Instance.CURRENT_SCENE == TheUiManager.SCENE.Gameplay)
        {
            if (TheDataManager.Instance.THE_DATA_PLAYER.GetTotalStar() == 0)
            {
                Skip();
                GetTutorial(TUTORIAL.gameplay).NextStep();//show level selection
            }
        }

        if (IsCompleted())
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        TheEventManager.OnShowPopup += HandleShowPopup;
        TheEventManager.OnHidePopup += HandleHidePopup;
        TheEventManager.OnGetReward += HandleGetReward;
        TheEventManager.OnStartNewScene += HandeStartNewScene;
    }



    private void OnDisable()
    {
        TheEventManager.OnShowPopup -= HandleShowPopup;
        TheEventManager.OnHidePopup -= HandleHidePopup;
        TheEventManager.OnGetReward -= HandleGetReward;
        TheEventManager.OnStartNewScene -= HandeStartNewScene;

        Instance = null;
    }
}


