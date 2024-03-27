using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.UI;

namespace SCREENS
{
    public class Gameover : MonoBehaviour
    {
        [SerializeField]
        private Button buShop, buLevelSelection, buReplay;
        // Start is called before the first frame update
        void Start()
        {
            buReplay.onClick.AddListener(() => SetButton(buReplay));
            buShop.onClick.AddListener(() => SetButton(buShop));
            buLevelSelection.onClick.AddListener(() => SetButton(buLevelSelection));
        }

        //SET BUTTON
        private void SetButton(Button _bu)
        {
            if (_bu == buShop)
            {
                MusicManager.Instance.Play();
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                UIController.Instance.PopUpShow(UIController.POP_UP.shop);
            }
            else if (_bu == buLevelSelection)
            {
                MusicManager.Instance.Play();
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                UIController.Instance.LoadScene(UIController.SCENE.LevelSelection);
           
            }
            else if (_bu == buReplay)
            {
                // MusicManager.Instance.Play();
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
