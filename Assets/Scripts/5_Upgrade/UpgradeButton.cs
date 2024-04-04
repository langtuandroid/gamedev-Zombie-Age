using MANAGERS;
using MODULES.Scriptobjectable;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace _5_Upgrade
{
    public class UpgradeButton : MonoBehaviour
    {
        [Inject] private SoundController _soundController;
        [Inject] private UpgradeController _upgradeController;
        [Inject] private UpgradeManager _upgradeManager;
        [FormerlySerializedAs("eUpgrade")] public EnumController.UpgradeType _upgradeType;
        
        [Space(20)]
        [SerializeField] private Image _starImage;
        [SerializeField] private TMP_Text _valueText;
        private UpgradeData _data;
        private Button _thisButton;
        private void Start()
        {
            _thisButton = this.GetComponent<Button>();
            _thisButton.onClick.AddListener(() => Click());

            Construct();

            if (_upgradeType == EnumController.UpgradeType.zombie_all_speed10)
            {
                Invoke("Click", 0.1f);
            }
        }


        public void Construct()
        {
            UpgradeData _temp = _upgradeController.GetUpgrade(_upgradeType);
            _valueText.text = _temp.iStar.ToString();


            if (_temp.bEQUIPED)
            {

                _thisButton.image.sprite = _temp.sprIcon;

            }
            else
            {
                _thisButton.image.sprite = _temp.sprIcon_gray;
            }

            _starImage.sprite = _upgradeManager.sprStar;

        }
        
        private void Click()
        {
            _soundController.Play(SoundController.SOUND.ui_click_next);
            _upgradeManager.m_BoardInfo.Show(this);
            _upgradeManager.tranOfYellowCirle.position = transform.position;
        }
    }
}
