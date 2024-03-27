using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.UI;

namespace SCREENS
{
    public class Pause : MonoBehaviour
    {
        [SerializeField]
        private Button buBack, buReplay, buLevelSelection;

        void Start()
        {
            buBack.onClick.AddListener(() => SetButton(buBack));
            buReplay.onClick.AddListener(() => SetButton(buReplay));
            buLevelSelection.onClick.AddListener(() => SetButton(buLevelSelection));
        }

        //SET BUTTON
        private void SetButton(Button _bu)
        {
            if (_bu == buBack)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_back);//sound
                MusicManager.Instance.Play();
                TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.pause);
            }
            else if (_bu == buReplay)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                // MusicManager.Instance.Play();
                TheUiManager.Instance.LoadScene(TheUiManager.SCENE.Gameplay);
            }
            else if (_bu == buLevelSelection)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                MusicManager.Instance.Play();
                TheUiManager.Instance.LoadScene(TheUiManager.SCENE.LevelSelection);
                TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.pause);
            }
        }

        private void OnEnable()
        {
            MusicManager.Instance.Stop();
        }
    }
}
