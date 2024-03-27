using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.UI;

namespace SCREENS
{
    public class Setting : MonoBehaviour
    {
        [SerializeField]
        private Button buBack, buMusic, buSound, buFacebook, buLikeUs, buReport, buAbout,buMoregame;

        // Start is called before the first frame update
        void Start()
        {
            buBack.onClick.AddListener(() => SetButton(buBack));
            buMusic.onClick.AddListener(() => SetButton(buMusic));
            buSound.onClick.AddListener(() => SetButton(buSound));
            buFacebook.onClick.AddListener(() => SetButton(buFacebook));
            buLikeUs.onClick.AddListener(() => SetButton(buLikeUs));
            buReport.onClick.AddListener(() => SetButton(buReport));
            buAbout.onClick.AddListener(() => SetButton(buAbout));
            buMoregame.onClick.AddListener(() => SetButton(buMoregame));
        }

        private void Init()
        {
            if (MusicManager.Instance.Mute)
            {
                buMusic.image.color = Color.gray;
                buMusic.GetComponentInChildren<Text>().text = "MUSIC OFF";
            }
            else
            {
                buMusic.image.color = Color.white;
                buMusic.GetComponentInChildren<Text>().text = "MUSIC ON";
            }

            if (SoundController.Instance.Mute)
            {
                buSound.image.color = Color.gray;
                buSound.GetComponentInChildren<Text>().text = "SOUND OFF";
            }
            else
            {
                buSound.image.color = Color.white;
                buSound.GetComponentInChildren<Text>().text = "SOUND ON";
            }
        }


        //SET BUTTON
        private void SetButton(Button _bu)
        {
            if (_bu == buBack)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_back );//sound
                UIController.Instance.HidePopup(UIController.POP_UP.setting);
            }
            else if (_bu == buMusic)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                MusicManager.Instance.Mute = !MusicManager.Instance.Mute;
                Init();
            }
            else if (_bu == buSound)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                SoundController.Instance.Mute = !SoundController.Instance.Mute;
                Init();
            }
            else if (_bu == buFacebook)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                
            }

            else if (_bu == buLikeUs)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                
            }
            else if (_bu == buReport)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                
            }
            else if (_bu == buAbout)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                UIController.Instance.PopUpShow(UIController.POP_UP.about_us);
            }
            else if (_bu == buMoregame)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                
            }
        }

        private void OnEnable()
        {
            Init();
        }
    }
}
