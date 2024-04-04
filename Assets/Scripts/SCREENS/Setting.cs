using MANAGERS;
using MODULES.Scriptobjectable;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace SCREENS
{
    public class Setting : MonoBehaviour
    {
        [Inject] private UIController _uiController;
        [Inject] private SoundController _soundController;
        [Inject] private MusicController _musicController;
        [FormerlySerializedAs("buBack")] [SerializeField] private Button _backButton;
        [FormerlySerializedAs("buSound")] [SerializeField] private Button _soundButton;
        
        void Start()
        {
            _backButton.onClick.AddListener(() => AssignButton(_backButton));
            _soundButton.onClick.AddListener(() => AssignButton(_soundButton));
        }

        private void Construct()
        {
            if (_soundController.Mute)
            {
                _soundButton.image.color = Color.gray;
                _soundButton.GetComponentInChildren<TMP_Text>().text = "SOUND OFF";
            }
            else
            {
                _soundButton.image.color = Color.white;
                _soundButton.GetComponentInChildren<TMP_Text>().text = "SOUND ON";
            }
        }
        
        private void AssignButton(Button button)
        {
            if (button == _backButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_back );//sound
                _uiController.HidePopup(UIController.POP_UP.setting);
            }
            else if (button == _soundButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                _soundController.Mute = !_soundController.Mute;
                _musicController.Mute = !_musicController.Mute;
                Construct();
            }
        }

        private void OnEnable()
        {
            Construct();
        }
    }
}
