﻿using MANAGERS;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace SCREENS
{
    public class Note : MonoBehaviour
    {
        [Inject] private UIController _uiController;
        [Inject] private SoundController _soundController;
        [Inject] private DataController _dataController;
        public enum NOTE
        {
            no_gem,
            reset_game,
            no_enought_star,
            no_enought_star_to_upgrade,
        }

        public static Note Instance;
        private static string _note;

        [FormerlySerializedAs("buBack")] [SerializeField] private Button _backButton;
        [FormerlySerializedAs("buDone")] [SerializeField] private Button _doneButton;
        [FormerlySerializedAs("txtContent")] [SerializeField] private TMP_Text _contentText;

        private void Start()
        {
            _backButton.onClick.AddListener(() => AssignButton(_backButton));
            _doneButton.onClick.AddListener(() => AssignButton(_doneButton));
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);
        }
        
        private void AssignButton(Button _bu)
        {
            if (_bu == _backButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_back);//sound
                _uiController.HidePopup(UIController.POP_UP.note);
            }
            if (_bu == _doneButton)
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound

                if (_note.Equals(NOTE.no_gem.ToString()))
                {
                    //_uiController.PopUpShow(UIController.POP_UP.shop);
                }
                else if (_note.Equals(NOTE.reset_game.ToString()))
                {
                    _dataController.ResetAll();
                }

                _uiController.HidePopup(UIController.POP_UP.note);
            }
        }

        public static void AssignNote(string _id)
        {
            Instance._backButton.gameObject.SetActive(true);
            _note = _id;
            //-------------------------------------------
            if (_note.Equals(NOTE.no_gem.ToString()))
            {
                Instance._contentText.text = "You don't have enough gems. \n Do you want more?";
            }
            else if (_note.Equals(NOTE.reset_game.ToString()))
            {
                Instance._contentText.text = "You will lost all data! \n Do you want continue?";
            }
            //-----------------
            else if (_note.Equals(NOTE.no_enought_star.ToString()))
            {
                Instance._contentText.text = "You don't have enough stars to unlock it!";
            }
            else if (_note.Equals(NOTE.no_enought_star_to_upgrade.ToString()))
            {
                Instance._contentText.text = "You don't have enough stars to upgrade!";
            }
        }
    }
}
