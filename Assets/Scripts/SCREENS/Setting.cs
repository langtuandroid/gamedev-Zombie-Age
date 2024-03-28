using MANAGERS;
using MODULES.Scriptobjectable;
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
        [FormerlySerializedAs("buMusic")] [SerializeField] private Button _musicButton;
        [FormerlySerializedAs("buSound")] [SerializeField] private Button _soundButton;
        [FormerlySerializedAs("buFacebook")] [SerializeField] private Button _facebookButton;
        [FormerlySerializedAs("buLikeUs")] [SerializeField] private Button _likeUsButton;
        [FormerlySerializedAs("buReport")] [SerializeField] private Button _bugReportButton;
        [FormerlySerializedAs("buAbout")] [SerializeField] private Button _aboutButton;
        [FormerlySerializedAs("buMoregame")] [SerializeField] private Button _buttonMoreGame;

        void Start()
        {
            _backButton.onClick.AddListener(() => AssignButton(_backButton));
            _musicButton.onClick.AddListener(() => AssignButton(_musicButton));
            _soundButton.onClick.AddListener(() => AssignButton(_soundButton));
            _facebookButton.onClick.AddListener(() => AssignButton(_facebookButton));
            _likeUsButton.onClick.AddListener(() => AssignButton(_likeUsButton));
            _bugReportButton.onClick.AddListener(() => AssignButton(_bugReportButton));
            _aboutButton.onClick.AddListener(() => AssignButton(_aboutButton));
            _buttonMoreGame.onClick.AddListener(() => AssignButton(_buttonMoreGame));
        }

        private void Construct()
        {
            if (_musicController.Mute)
            {
                _musicButton.image.color = Color.gray;
                _musicButton.GetComponentInChildren<Text>().text = "MUSIC OFF";
            }
            else
            {
                _musicButton.image.color = Color.white;
                _musicButton.GetComponentInChildren<Text>().text = "MUSIC ON";
            }

            if (_soundController.Mute)
            {
                _soundButton.image.color = Color.gray;
                _soundButton.GetComponentInChildren<Text>().text = "SOUND OFF";
            }
            else
            {
                _soundButton.image.color = Color.white;
                _soundButton.GetComponentInChildren<Text>().text = "SOUND ON";
            }
        }
        
        private void AssignButton(Button button)
        {
            if (button == _backButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_back );//sound
                _uiController.HidePopup(UIController.POP_UP.setting);
            }
            else if (button == _musicButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                _musicController.Mute = !_musicController.Mute;
                Construct();
            }
            else if (button == _soundButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                _soundController.Mute = !_soundController.Mute;
                Construct();
            }
            else if (button == _facebookButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                
            }

            else if (button == _likeUsButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                
            }
            else if (button == _bugReportButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                
            }
            else if (button == _aboutButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                _uiController.PopUpShow(UIController.POP_UP.about_us);
            }
            else if (button == _buttonMoreGame)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                
            }
        }

        private void OnEnable()
        {
            Construct();
        }
    }
}
