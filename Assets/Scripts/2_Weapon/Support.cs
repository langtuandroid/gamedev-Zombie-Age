﻿using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace _2_Weapon
{
    public class Support : MonoBehaviour
    {
        [Inject] private WeaponController _weaponController;
        [Inject] private SoundController _soundController;
        [FormerlySerializedAs("eSupport")] [SerializeField] private EnumController.SUPPORT _eSupport;
        [FormerlySerializedAs("SUPPORT_DATA")] [SerializeField] private SupportData _supportData;

        [FormerlySerializedAs("txtName")] [SerializeField] private Text _nameText;
        [FormerlySerializedAs("txtCurrentValue")] [SerializeField] private Text _valueText;
        private Button _thisButton;

        public SupportData SupportData => _supportData;
        void Awake()
        {
            _thisButton = GetComponent<Button>();
            _thisButton.onClick.AddListener(() => ButtonAssign());

            _supportData = _weaponController.Support(_eSupport);
            _nameText.text = _supportData.name;

            ShowData();
            if (_supportData.DATA._support == EnumController.SUPPORT.big_bomb)
                WeaponsManager.Instance.supportPanel.ViewTrack(this);
        }


        private void ButtonAssign()
        { 
            if (TutorialController.Instance)
            {
                if (!TutorialController.Instance.IsRightInput()) return;
            }

            _soundController.Play(SoundController.SOUND.ui_wood_board);//sound
            WeaponsManager.Instance.supportPanel.ViewTrack(this);
        }

        public void ShowData()
        {
            _valueText.text = _supportData.DATA.iCurrentValue.ToString()+"/"+_supportData.iMaxValue;
        }
    }
}
