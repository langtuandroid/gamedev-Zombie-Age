using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SCREENS
{
    public class Gameover : MonoBehaviour
    {
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
                MusicManager.Instance.Play();
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                UIController.Instance.PopUpShow(UIController.POP_UP.shop);
            }
            else if (_bu == _levelSelectButton)
            {
                MusicManager.Instance.Play();
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                UIController.Instance.LoadScene(UIController.SCENE.LevelSelection);
           
            }
            else if (_bu == _replayButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                UIController.Instance.LoadScene(UIController.SCENE.Gameplay);           
            }
        }
        private void OnEnable()
        {
            SoundController.Instance.Play(SoundController.SOUND.ui_game_over);//sound
            MusicManager.Instance.Stop();
        }
    }
}
