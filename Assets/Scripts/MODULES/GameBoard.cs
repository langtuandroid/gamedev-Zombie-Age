using MANAGERS;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace MODULES
{
    public class GameBoard : MonoBehaviour
    {
        [Inject] private UIController _uiController;
        [Inject] private SoundController _soundController;
        [Inject] private DataController _dataController;
        [Inject] private TutorialController _tutorialController;
        public enum TYPE
        {
            gem,
        }
        [FormerlySerializedAs("eType")] public TYPE _boardType;

        private Button _thisButton;
        private TMP_Text _text;
        
        private void Awake()
        {

            _thisButton = GetComponent<Button>();
            _text = GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            _thisButton.onClick.AddListener(AssignButton);
        }


        private void AssignButton()
        {
            //for tutorial
            if (_tutorialController)
            {
                if (!_tutorialController.IsRightInput()) return;
            }

            _soundController.Play(SoundController.SOUND.ui_click_next);//sound
            switch (_boardType)
            {
                case TYPE.gem:
                    _uiController.PopUpShow(UIController.POP_UP.shop);//
                    break;

            }
        }

        private void ViewValue()
        {

            switch (_boardType)
            {
                case TYPE.gem:
                    _text.text = _dataController.playerData.Gem.ToString();
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
