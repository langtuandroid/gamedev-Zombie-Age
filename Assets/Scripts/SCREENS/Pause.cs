using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SCREENS
{
    public class Pause : MonoBehaviour
    {
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
                SoundController.Instance.Play(SoundController.SOUND.ui_click_back);//sound
                MusicManager.Instance.Play();
                UIController.Instance.HidePopup(UIController.POP_UP.pause);
            }
            else if (_bu == _replayButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                // MusicManager.Instance.Play();
                UIController.Instance.LoadScene(UIController.SCENE.Gameplay);
            }
            else if (_bu == _buttonLevelSelection)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                MusicManager.Instance.Play();
                UIController.Instance.LoadScene(UIController.SCENE.LevelSelection);
                UIController.Instance.HidePopup(UIController.POP_UP.pause);
            }
        }

        private void OnEnable()
        {
            MusicManager.Instance.Stop();
        }
    }
}
