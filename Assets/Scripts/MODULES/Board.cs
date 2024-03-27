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
            if (TutorialController.Instance)
            {
                if (!TutorialController.Instance.IsRightInput()) return;
            }

            SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
            switch (eType)
            {
                case TYPE.gem:
                    UIController.Instance.PopUpShow(UIController.POP_UP.shop);//
                    break;

            }
        }

        private void ShowValue()
        {

            switch (eType)
            {
                case TYPE.gem:
                    txtValue.text = DataController.Instance.playerData.Gem.ToString();
                    break;

            }
        }

        private void OnEnable()
        {
            ShowValue();
            EventController.OnUpdatedBoard += ShowValue;

        }
        private void OnDisable()
        {
            EventController.OnUpdatedBoard -= ShowValue;
        }
    }
}
