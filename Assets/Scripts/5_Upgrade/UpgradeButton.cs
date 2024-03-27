using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _5_Upgrade
{
    public class UpgradeButton : MonoBehaviour
    {
        [FormerlySerializedAs("eUpgrade")] public TheEnumManager.UpgradeType _upgradeType;
        private Button _thisButton;

       
        [Space(20)]
        [FormerlySerializedAs("DATA")] [SerializeField] private UpgradeData _data;
        [FormerlySerializedAs("imaStar")] [SerializeField] private Image _starImage;
        [FormerlySerializedAs("txtValue")] [SerializeField] private Text _valueText;


        private void Start()
        {
            _thisButton = this.GetComponent<Button>();
            _thisButton.onClick.AddListener(() => Click());

            Construct();

            if (_upgradeType == TheEnumManager.UpgradeType.zombie_all_speed10)
            {
                Invoke("Click", 0.1f);
            }
        }


        public void Construct()
        {
            UpgradeData _temp = TheUpgradeManager.Instance.GetUpgrade(_upgradeType);
            _valueText.text = _temp.iStar.ToString();


            if (_temp.bEQUIPED)
            {

                _thisButton.image.sprite = _temp.sprIcon;

            }
            else
            {
                _thisButton.image.sprite = _temp.sprIcon_gray;
            }

            _starImage.sprite = MainCode_Upgrade.Instance.sprStar;

        }
        
        private void Click()
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);
            MainCode_Upgrade.Instance.m_BoardInfo.Show(this);
            MainCode_Upgrade.Instance.tranOfYellowCirle.position = transform.position;
        }
    }
}
