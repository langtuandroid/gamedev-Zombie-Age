using MANAGERS;
using UnityEngine;
using UnityEngine.UI;

namespace MODULES
{
    public class Board : MonoBehaviour
    {
        public enum TYPE
        {
            gem,
        }
        public TYPE eType;

        private Button buButtonThis;
        private Text txtValue;

        // Start is called before the first frame update

        private void Awake()
        {

            buButtonThis = GetComponent<Button>();
            txtValue = this.GetComponentInChildren<Text>();
        }
        void Start()
        {
            buButtonThis.onClick.AddListener(() => SetButton());
        }


        private void SetButton()
        {
            //for tutorial
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }

            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
            switch (eType)
            {
                case TYPE.gem:
                    TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.shop);//
                    break;

            }
        }

        private void ShowValue()
        {

            switch (eType)
            {
                case TYPE.gem:
                    txtValue.text = TheDataManager.Instance.THE_DATA_PLAYER.iGem.ToString();
                    break;

            }
        }

        private void OnEnable()
        {
            ShowValue();
            TheEventManager.OnUpdatedBoard += ShowValue;

        }
        private void OnDisable()
        {
            TheEventManager.OnUpdatedBoard -= ShowValue;
        }
    }
}
