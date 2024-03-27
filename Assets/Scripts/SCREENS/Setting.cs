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

            if (TheSoundManager.Instance.Mute)
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
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_back );//sound
                TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.setting);
            }
            else if (_bu == buMusic)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                MusicManager.Instance.Mute = !MusicManager.Instance.Mute;
                Init();
            }
            else if (_bu == buSound)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheSoundManager.Instance.Mute = !TheSoundManager.Instance.Mute;
                Init();
            }
            else if (_bu == buFacebook)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheUiManager.Instance.LoadLink(TheDataManager.Instance.GAME_INFO.strFacebook);
            }

            else if (_bu == buLikeUs)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheUiManager.Instance.LoadLink(TheDataManager.Instance.GAME_INFO.strLinkLike);
            }
            else if (_bu == buReport)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheUiManager.Instance.ReportEmail();
            }
            else if (_bu == buAbout)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.about_us);
            }
            else if (_bu == buMoregame)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheUiManager.Instance.LoadLink(TheDataManager.Instance.GAME_INFO.strLinkMoreGame);
            }
        }

        private void OnEnable()
        {
            Init();
        }
    }
}
