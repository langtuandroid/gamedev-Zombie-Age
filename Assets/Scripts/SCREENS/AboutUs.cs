using MANAGERS;
using UnityEngine;
using UnityEngine.UI;

namespace SCREENS
{
    public class AboutUs : MonoBehaviour
    {
        [SerializeField]
        private Button buBack,buResetGame,buMoregame;
        [SerializeField] Text txtContent;
        [SerializeField] Image imaLogo;

        // Start is called before the first frame update
        void Start()
        {
            buBack.onClick.AddListener(() => SetButton(buBack));
            buResetGame.onClick.AddListener(() => SetButton(buResetGame));
            buMoregame.onClick.AddListener(() => SetButton(buMoregame));


            imaLogo.sprite = TheDataManager.Instance.GAME_INFO.sprLogoPNG;
            txtContent.text = "Add: " + TheDataManager.Instance.GAME_INFO.strAddress + "\n Email: " + TheDataManager.Instance.GAME_INFO.strEmailReport;
        }

        //SET BUTTON
        private void SetButton(Button _bu)
        {
            if (_bu == buBack)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_back);//sound
                TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.about_us);
            }
            else  if (_bu == buResetGame )
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.note);
                Note.SetNote(Note.NOTE.reset_game.ToString());
                
            }
            else if (_bu == buMoregame)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next );//sound
                TheUiManager.Instance.LoadLink(TheDataManager.Instance.GAME_INFO.strLinkMoreGame);
            }
        }
    }
}
