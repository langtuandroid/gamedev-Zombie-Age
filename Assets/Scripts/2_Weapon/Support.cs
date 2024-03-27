using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _2_Weapon
{
    public class Support : MonoBehaviour
    {
         
        [FormerlySerializedAs("eSupport")] [SerializeField] private TheEnumManager.SUPPORT _eSupport;
        [FormerlySerializedAs("SUPPORT_DATA")] [SerializeField] private SupportData _supportData;

        [FormerlySerializedAs("txtName")] [SerializeField] private Text _nameText;
        [FormerlySerializedAs("txtCurrentValue")] [SerializeField] private Text _valueText;
        private Button _thisButton;

        public SupportData SupportData => _supportData;
        void Awake()
        {
            _thisButton = GetComponent<Button>();
            _thisButton.onClick.AddListener(() => ButtonAssign());

            _supportData = TheWeaponManager.Instance.GetSupport(_eSupport);
            _nameText.text = _supportData.name;

            ShowData();
            if (_supportData.DATA._support == TheEnumManager.SUPPORT.big_bomb)
                WeaponController.Instance.supportPanel.ViewTrack(this);
        }


        private void ButtonAssign()
        { 
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }

            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_wood_board);//sound
            WeaponController.Instance.supportPanel.ViewTrack(this);
        }

        public void ShowData()
        {
            _valueText.text = _supportData.DATA.iCurrentValue.ToString()+"/"+_supportData.iMaxValue;
        }
    }
}
