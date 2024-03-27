using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.UI;

namespace _1_Menu
{
    public class MainCode_Menu : MonoBehaviour
    {
        [SerializeField] private float fTimeToloading = 5.0f;
        private float _fCountTime = 0;
        [SerializeField] private Image imaLoadingRender;
        [SerializeField] private GameObject objLogoGame;
        [SerializeField] private GameObject objLoadingPopup;


        [Space(30)] [SerializeField] private Button buPlay;

        private void Start()
        {
            buPlay.onClick.AddListener(() => SetButton(buPlay));
            MusicManager.Instance.Play();
            TheUiManager.Instance.SetCameraForPopupCanvas(Camera.main);//set camera       
            objLogoGame.SetActive(false);
        }

        private void SetButton(Button _button)
        {
            if (_button == buPlay)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheUiManager.Instance.LoadScene(TheUiManager.SCENE.LevelSelection);
            }
        }

   



        private void Update()
        {
            if (_fCountTime < fTimeToloading)
            {
                _fCountTime += Time.deltaTime;
                imaLoadingRender.fillAmount = _fCountTime / fTimeToloading;
                if (_fCountTime >= fTimeToloading)
                {

                    objLoadingPopup.SetActive(false);
                    objLogoGame.SetActive(true);


                }
            }
        }
    }
}
