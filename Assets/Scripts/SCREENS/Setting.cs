using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SCREENS
{
    public class Setting : MonoBehaviour
    {
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
            if (MusicManager.Instance.Mute)
            {
                _musicButton.image.color = Color.gray;
                _musicButton.GetComponentInChildren<Text>().text = "MUSIC OFF";
            }
            else
            {
                _musicButton.image.color = Color.white;
                _musicButton.GetComponentInChildren<Text>().text = "MUSIC ON";
            }

            if (SoundController.Instance.Mute)
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
                SoundController.Instance.Play(SoundController.SOUND.ui_click_back );//sound
                UIController.Instance.HidePopup(UIController.POP_UP.setting);
            }
            else if (button == _musicButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                MusicManager.Instance.Mute = !MusicManager.Instance.Mute;
                Construct();
            }
            else if (button == _soundButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                SoundController.Instance.Mute = !SoundController.Instance.Mute;
                Construct();
            }
            else if (button == _facebookButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                
            }

            else if (button == _likeUsButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                
            }
            else if (button == _bugReportButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                
            }
            else if (button == _aboutButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                UIController.Instance.PopUpShow(UIController.POP_UP.about_us);
            }
            else if (button == _buttonMoreGame)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                
            }
        }

        private void OnEnable()
        {
            Construct();
        }
    }
}
