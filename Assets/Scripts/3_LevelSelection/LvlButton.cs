using MANAGERS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _3_LevelSelection
{
    public class LvlButton : MonoBehaviour
    {
        [SerializeField] private Image[] _starsImages;
        [SerializeField] private Sprite _starComplitedSprite;
        [SerializeField] private Sprite _emptyStar;
        [SerializeField] private GameObject _lockedImage;
        [SerializeField] private TMP_Text _levelText;
        [Inject] private SoundController _soundController;
        [Inject] private DataController _dataController;
        [Inject] private LevelSelectionController _levelSelectionController;
        [Inject] private TutorialController _tutorialController;
        private Button _levelButton;

        private int _iLevel;

        public bool Unlock { get; private set; }

        public int IStar { get; private set; }

        private void Awake()
        {
            _levelButton = GetComponent<Button>();
            _levelButton.onClick.AddListener(AssignButton);
        }
        
        private void AssignButton()
        {
            if (_tutorialController)
            {
                if (!_tutorialController.IsRightInput()) return;
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
            LockButton();
            for (int i = 0; i < 3; i++)
            {
                _starsImages[i].sprite = _emptyStar;
            }
            _iLevel = lvl;
            
            if (_iLevel + 1 > _dataController.LevelsTotal)
            {
                Unlock = false;
                _levelText.text = "";
            }
            else
            {
                _levelText.text = (_iLevel + 1).ToString();
                IStar = _dataController.playerData.NumOfStars(_iLevel);
                for (int i = 0; i < IStar; i++)
                {
                    _starsImages[i].sprite = _starComplitedSprite;
                }
                
                if (IStar > 0)
                {
                    UnlockButton();
                }
                else
                {
                    if (lvl == 0)
                    {
                        UnlockButton();
                    }
                    else
                    {
                        if (_dataController.playerData.NumOfStars(_iLevel - 1) > 0)
                        {
                            UnlockButton();
                        }
                    }

                }
            }
        }

        private void UnlockButton()
        {
            Unlock = true;
            _levelText.gameObject.SetActive(true);
            _lockedImage.SetActive(false);
        }

        private void LockButton()
        {
            Unlock = false;
            _levelText.gameObject.SetActive(false);
            _lockedImage.SetActive(true);
        }
    }
}
