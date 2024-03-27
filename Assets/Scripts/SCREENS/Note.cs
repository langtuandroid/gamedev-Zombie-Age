using MANAGERS;
using UnityEngine;
using UnityEngine.UI;

namespace SCREENS
{
    public class Note : MonoBehaviour
    {
        public enum NOTE
        {
            no_gem,//thieu gem
            reset_game,
            no_enought_star,
            no_enought_star_to_upgrade,
        }

        public static Note Instance;
        private static string NOTE_CONTENT;

        [SerializeField]
        private Button buBack, buDone;
        [SerializeField] Text txtContent;

        // Start is called before the first frame update
        void Start()
        {
            buBack.onClick.AddListener(() => SetButton(buBack));
            buDone.onClick.AddListener(() => SetButton(buDone));
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);
        }

        //SET BUTTON
        private void SetButton(Button _bu)
        {
            if (_bu == buBack)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_back);//sound
                TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.note);
            }
            if (_bu == buDone)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound

                if (NOTE_CONTENT.Equals(NOTE.no_gem.ToString()))
                {
                    TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.shop);
                }
                else if (NOTE_CONTENT.Equals(NOTE.reset_game.ToString()))
                {
                    TheDataManager.Instance.ResetGame();
                }

                TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.note);
            }
        }

        public static void SetNote(string _id)
        {
            Instance.buBack.gameObject.SetActive(true);
            NOTE_CONTENT = _id;
            //-------------------------------------------
            if (NOTE_CONTENT.Equals(NOTE.no_gem.ToString()))
            {
                Instance.txtContent.text = "You don't have enough gems. \n Do you want more?";
            }
            else if (NOTE_CONTENT.Equals(NOTE.reset_game.ToString()))
            {
                Instance.txtContent.text = "You will lost all data! \n Do you want continue?";
            }
            //-----------------
            else if (NOTE_CONTENT.Equals(NOTE.no_enought_star.ToString()))
            {
                Instance.txtContent.text = "You don't have enough stars to unlock it!";
            }
            else if (NOTE_CONTENT.Equals(NOTE.no_enought_star_to_upgrade.ToString()))
            {
                Instance.txtContent.text = "You don't have enough stars to upgrade!";
            }
        }
    }
}
