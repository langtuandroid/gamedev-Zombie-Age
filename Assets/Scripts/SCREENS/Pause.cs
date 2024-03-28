using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace SCREENS
{
    public class Pause : MonoBehaviour
    {
        [Inject] private UIController _uiController;
        [Inject] private SoundController _soundController;
        [Inject] private MusicController _musicController;
        [FormerlySerializedAs("buBack")] [SerializeField] private Button _backButton;
        [FormerlySerializedAs("buReplay")] [SerializeField] private Button _replayButton;
        [FormerlySerializedAs("buLevelSelection")] [SerializeField] private Button _buttonLevelSelection;

        void Start()
        {
            _backButton.onClick.AddListener(() => AssignButton(_backButton));
            _replayButton.onClick.AddListener(() => AssignButton(_replayButton));
            _buttonLevelSelection.onClick.AddListener(() => AssignButton(_buttonLevelSelection));
        }
        
        private void AssignButton(Button _bu)
        {
            if (_bu == _backButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_back);//sound
                _musicController.Play();
                _uiController.HidePopup(UIController.POP_UP.pause);
            }
            else if (_bu == _replayButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                _uiController.LoadScene(UIController.SCENE.Gameplay);
            }
            else if (_bu == _buttonLevelSelection)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                _musicController.Play();
                _uiController.LoadScene(UIController.SCENE.LevelSelection);
                _uiController.HidePopup(UIController.POP_UP.pause);
            }
        }

        private void OnEnable()
        {
            _musicController.Stop();
        }
    }
}
