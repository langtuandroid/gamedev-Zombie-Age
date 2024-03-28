using MANAGERS;
using UnityEngine;

using UnityEngine.UI;
using Zenject;

namespace _3_LevelSelection
{
    public class LvlButton : MonoBehaviour
    {
        [Inject] private SoundController _soundController;
        [Inject] private DataController _dataController;
        [Inject] private LevelSelectionController _levelSelectionController;
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
            if (TutorialController.Instance)
            {
                if (!TutorialController.Instance.IsRightInput()) return;
            }

            if (Unlock || _dataController.mode == DataController.Mode.Debug)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);
                _dataController.playerData.CurrentLevel = _iLevel;
                _levelSelectionController.SetDifficultPopUp(true);
            }
            else
            {
                if (_iLevel + 1 <= _dataController.LevelsTotal)
                {
                    _soundController.Play(SoundController.SOUND.ui_cannot);
                    Debug.Log("LOCKED!");
                }
            }
        }


        public void Consruct(int lvl)
        {

            _iLevel = lvl;
            _levelText.color = Color.white;


            if (_iLevel + 1 > _dataController.LevelsTotal)
            {
                _unlock = false;
                _levelText.text = "";
            }
            else
            {
                _levelText.text = (_iLevel + 1).ToString();

                IStar = _dataController.playerData.NumOfStars(_iLevel);

                if (IStar > 0)
                {
                    _unlock = true;

                    if (IStar > 0)
                        _levelButton.image.sprite = _levelSelectionController.SpriteOfLevels[IStar - 1];
                }
                else
                {
                    if (lvl == 0)
                    {
                        _levelButton.image.sprite = _levelSelectionController.LevelCurrSprite;
                        _unlock = true;
                    }
                    else
                    {
                        if (_dataController.playerData.NumOfStars(_iLevel - 1) > 0)
                        {
                            _levelButton.image.sprite = _levelSelectionController.LevelCurrSprite;
                            _unlock = true;
                        }
                        else
                        {
                            _levelButton.image.sprite = _levelSelectionController.LockedSprite;
                            _unlock = true; //TODO Remove for test only
                            _levelText.color = Color.white * 0.75f;
                        }

                    }

                }
            }
        }
    }
}
