using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.UI;

namespace _2_Weapon
{
    public class TrackSupport : MonoBehaviour
    {
        public TheEnumManager.SUPPORT eSupport;
        public SupportData SUPPORT_DATA;

        [SerializeField]
        private Text txtName, txtCurrentValue;
        private Button buThis;
        void Awake()
        {
            buThis = GetComponent<Button>();
            buThis.onClick.AddListener(() => ButtonThis());

            SUPPORT_DATA = TheWeaponManager.Instance.GetSupport(eSupport);
            txtName.text = SUPPORT_DATA.name;

            ShowInfo();
            if (SUPPORT_DATA.DATA._support == TheEnumManager.SUPPORT.big_bomb)
                MainCode_Weapon.Instance.m_MainPanelSupport.ShowTrack(this);
        }


        private void ButtonThis()
        { 
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }

            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_wood_board);//sound
            MainCode_Weapon.Instance.m_MainPanelSupport.ShowTrack(this);
        }

        public void ShowInfo()
        {
            txtCurrentValue.text = SUPPORT_DATA.DATA.iCurrentValue.ToString()+"/"+SUPPORT_DATA.iMaxValue;
        }
    }
}
