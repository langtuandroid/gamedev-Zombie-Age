using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace SCREENS
{
    public class Gameover : MonoBehaviour
    {
        [Inject] private UIController _uiController;
        [Inject] private SoundController _soundController;
        [Inject] private MusicController _musicController;
        [SerializeField]
        [FormerlySerializedAs("buShop")]  private Button _shopButton;
        [FormerlySerializedAs("buLevelSelection")] [SerializeField] private Button _levelSelectButton;
        [FormerlySerializedAs("buReplay")] [SerializeField] private Button _replayButton;

        private void Start()
        {
            _replayButton.onClick.AddListener(() => SetButton(_replayButton));
            _shopButton.onClick.AddListener(() => SetButton(_shopButton));
            _levelSelectButton.onClick.AddListener(() => SetButton(_levelSelectButton));
        }

        private void SetButton(Button _bu)
        {
            if (_bu == _shopButton)
            {
                _musicController.Play();
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                _uiController.PopUpShow(UIController.POP_UP.shop);
            }
            else if (_bu == _levelSelectButton)
            {
                _musicController.Play();
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                _uiController.LoadScene(UIController.SCENE.LevelSelection);
           
            }
            else if (_bu == _replayButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                _uiController.LoadScene(UIController.SCENE.Gameplay);           
            }
        }
        private void OnEnable()
        {
            _soundController.Play(SoundController.SOUND.ui_game_over);//sound
            _musicController.Stop();
        }
    }
}
