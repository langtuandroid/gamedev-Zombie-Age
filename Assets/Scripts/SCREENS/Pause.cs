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
                SoundController.Instance.Play(SoundController.SOUND.ui_click_back);//sound
                MusicManager.Instance.Play();
                UIController.Instance.HidePopup(UIController.POP_UP.pause);
            }
            else if (_bu == buReplay)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                // MusicManager.Instance.Play();
                UIController.Instance.LoadScene(UIController.SCENE.Gameplay);
            }
            else if (_bu == buLevelSelection)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                MusicManager.Instance.Play();
                UIController.Instance.LoadScene(UIController.SCENE.LevelSelection);
                UIController.Instance.HidePopup(UIController.POP_UP.pause);
            }
        }

        private void OnEnable()
        {
            MusicManager.Instance.Stop();
        }
    }
}
