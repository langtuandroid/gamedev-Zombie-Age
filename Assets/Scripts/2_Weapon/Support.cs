using MANAGERS;
using MODULES.Scriptobjectable;
using TMPro;
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
        [Inject] private WeaponsManager _weaponsManager;
        [Inject] private TutorialController _tutorialController;
        [FormerlySerializedAs("eSupport")] [SerializeField] private EnumController.SUPPORT _eSupport;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _valueText;
        private Button _thisButton;
        private SupportData _supportData;
        public SupportData SupportData => _supportData;
        void Awake()
        {
            _thisButton = GetComponent<Button>();
            _thisButton.onClick.AddListener(() => ButtonAssign());

            _supportData = _weaponController.Support(_eSupport);
            _nameText.text = _supportData.name;

            ShowData();
            if (_supportData.DATA._support == EnumController.SUPPORT.big_bomb)
                _weaponsManager.supportPanel.ViewTrack(this);
        }


        private void ButtonAssign()
        { 
            if (_tutorialController)
            {
                if (!_tutorialController.IsRightInput()) return;
            }

            _soundController.Play(SoundController.SOUND.ui_wood_board);//sound
            _weaponsManager.supportPanel.ViewTrack(this);
        }

        public void ShowData()
        {
            _valueText.text = _supportData.DATA.iCurrentValue.ToString()+"/"+_supportData.iMaxValue;
        }
    }
}
