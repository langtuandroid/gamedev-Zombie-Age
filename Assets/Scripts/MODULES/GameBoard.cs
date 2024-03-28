using MANAGERS;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MODULES
{
    public class GameBoard : MonoBehaviour
    {
        public enum TYPE
        {
            gem,
        }
        [FormerlySerializedAs("eType")] public TYPE _boardType;

        private Button _thisButton;
        private Text _text;
        
        private void Awake()
        {

            _thisButton = GetComponent<Button>();
            _text = this.GetComponentInChildren<Text>();
        }

        private void Start()
        {
            _thisButton.onClick.AddListener(() => AssignButton());
        }


        private void AssignButton()
        {
            //for tutorial
            if (TutorialController.Instance)
            {
                if (!TutorialController.Instance.IsRightInput()) return;
            }

            SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
            switch (_boardType)
            {
                case TYPE.gem:
                    UIController.Instance.PopUpShow(UIController.POP_UP.shop);//
                    break;

            }
        }

        private void ViewValue()
        {

            switch (_boardType)
            {
                case TYPE.gem:
                    _text.text = DataController.Instance.playerData.Gem.ToString();
                    break;

            }
        }

        private void OnEnable()
        {
            ViewValue();
            EventController.OnUpdatedBoard += ViewValue;

        }
        private void OnDisable()
        {
            EventController.OnUpdatedBoard -= ViewValue;
        }
    }
}
