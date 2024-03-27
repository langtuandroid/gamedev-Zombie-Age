using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _1_Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private float _loadingTime = 6.0f;
        [SerializeField] private Image _renderImage;
        [SerializeField] private GameObject _gameLogo;
        [SerializeField] private GameObject _loadingPopUp;
        [Space(30)] [SerializeField] private Button _playButton;
        private float _loadingTimePassed;
        private void Start()
        {
            _playButton.onClick.AddListener(() => AssignButton(_playButton));
            MusicManager.Instance.Play();
            UIController.Instance.SetCameraPopup(Camera.main);//set camera       
            _gameLogo.SetActive(false);
        }

        private void AssignButton(Button _button)
        {
            if (_button == _playButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                UIController.Instance.LoadScene(UIController.SCENE.LevelSelection);
            }
        }
        private void Update()
        {
            if (_loadingTimePassed < _loadingTime)
            {
                _loadingTimePassed += Time.deltaTime;
                _renderImage.fillAmount = _loadingTimePassed / _loadingTime;
                if (_loadingTimePassed >= _loadingTime)
                {

                    _loadingPopUp.SetActive(false);
                    _gameLogo.SetActive(true);


                }
            }
        }
    }
}
