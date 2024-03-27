using MANAGERS;
using UnityEngine;

using UnityEngine.UI;

namespace _3_LevelSelection
{
    public class LvlButton : MonoBehaviour
    {
        private Button _levelButton;
        private Text _levelText;
        private bool _unlock;
        private int _iLevel;

        public bool Unlock => _unlock;
        public int IStar { get; private set; }

        void Awake()
        {
            _levelButton = GetComponent<Button>();
            _levelText = GetComponentInChildren<Text>();
            _levelButton.onClick.AddListener(() => AssignButton());
        }
        
        private void AssignButton()
        {
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }

            if (Unlock || TheDataManager.Instance.eMode == TheDataManager.MODE.Debug)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);
                TheDataManager.Instance.THE_DATA_PLAYER.iCurrentLevel = _iLevel;
                LevelSelectionController.Instance.SetDifficultPopUp(true);
            }
            else
            {
                if (_iLevel + 1 <= TheDataManager.Instance.TOTAL_LEVEL_IN_GAME)
                {
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);
                    Debug.Log("LOCKED!");
                }
            }
        }


        public void Consruct(int lvl)
        {

            _iLevel = lvl;
            _levelText.color = Color.white;


            if (_iLevel + 1 > TheDataManager.Instance.TOTAL_LEVEL_IN_GAME)
            {
                _unlock = false;
                _levelText.text = "";
            }
            else
            {
                _levelText.text = (_iLevel + 1).ToString();

                IStar = TheDataManager.Instance.THE_DATA_PLAYER.GetStar(_iLevel);

                if (IStar > 0)
                {
                    _unlock = true;

                    if (IStar > 0)
                        _levelButton.image.sprite = LevelSelectionController.Instance.SpriteOfLevels[IStar - 1];
                }
                else
                {
                    if (lvl == 0)
                    {
                        _levelButton.image.sprite = LevelSelectionController.Instance.LevelCurrSprite;
                        _unlock = true;
                    }
                    else
                    {
                        if (TheDataManager.Instance.THE_DATA_PLAYER.GetStar(_iLevel - 1) > 0)
                        {
                            _levelButton.image.sprite = LevelSelectionController.Instance.LevelCurrSprite;
                            _unlock = true;
                        }
                        else
                        {
                            _levelButton.image.sprite = LevelSelectionController.Instance.LockedSprite;
                            _unlock = true; //TODO Remove for test only
                            _levelText.color = Color.white * 0.75f;
                        }

                    }

                }
            }
        }
    }
}
