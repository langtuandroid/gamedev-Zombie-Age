using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevel : MonoBehaviour
{
    private Button buButtonLevel;
    private Text txtLevel;


    [SerializeField]
    public bool Unlock;


    [SerializeField]
    private int iLevel;
    public int iStar;
    // Start is called before the first frame update
    void Awake()
    {
        buButtonLevel = GetComponent<Button>();
        txtLevel = GetComponentInChildren<Text>();
        buButtonLevel.onClick.AddListener(() => SetButton());
    }



    private void SetButton()
    {
        //for tutorial
        if (TheTutorialManager.Instance)
        {
            if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
        }

        if (Unlock || TheDataManager.Instance.eMode == TheDataManager.MODE.Debug)
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
            TheDataManager.Instance.THE_DATA_PLAYER.iCurrentLevel = iLevel;
            //TheUiManager.Instance.LoadScene(TheUiManager.SCENE.Weapon);
            MainCode_LevelSelection.Instance.SetActiveDifficuftPopup(true);
        }
        else
        {
            if (iLevel + 1 <= TheDataManager.Instance.TOTAL_LEVEL_IN_GAME)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                Debug.Log("LOCKED!");
            }
        }
    }


    public void Init(int _level)
    {

        iLevel = _level;
        txtLevel.color = Color.white;


        if (iLevel + 1 > TheDataManager.Instance.TOTAL_LEVEL_IN_GAME)
        {
            Unlock = false;
            txtLevel.text = "";
        }
        else
        {
            txtLevel.text = (iLevel + 1).ToString();

            iStar = TheDataManager.Instance.THE_DATA_PLAYER.GetStar(iLevel);

            if (iStar > 0)
            {
                Unlock = true;

                if (iStar > 0)
                    buButtonLevel.image.sprite = MainCode_LevelSelection.Instance.SPRITE_OF_LEVEL[iStar - 1];
            }
            else
            {
                if (_level == 0)
                {
                    // MainCode_LevelSelection.Instance.m_tranOfRay.position = transform.position;
                    buButtonLevel.image.sprite = MainCode_LevelSelection.Instance.sprCurrentLevelSprite;
                    Unlock = true;
                }
                else
                {
                    if (TheDataManager.Instance.THE_DATA_PLAYER.GetStar(iLevel - 1) > 0)
                    {
                        // MainCode_LevelSelection.Instance.m_tranOfRay.position = transform.position;
                        buButtonLevel.image.sprite = MainCode_LevelSelection.Instance.sprCurrentLevelSprite;
                        Unlock = true;
                    }
                    else
                    {
                        buButtonLevel.image.sprite = MainCode_LevelSelection.Instance.sprLockedLevelSprite;
                        Unlock = true; //TODO Remove for test only
                        txtLevel.color = Color.white * 0.75f;
                    }

                }

            }
        }
    }
}
