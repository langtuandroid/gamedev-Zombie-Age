using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace MODULES
{
    public class FreeGem : MonoBehaviour
    {
        [Inject] private DataController _dataController;
        [Inject] private TutorialController _tutorialController;
        private Button _thisButton;
        [FormerlySerializedAs("txtValue")] [SerializeField] private Text _valueText;

        private void Start()
        {
            RewardData _reward = _dataController.GetReward(EnumController.REWARD.ads_free_gem);

            _thisButton = GetComponent<Button>();

            _thisButton.onClick.AddListener(() => AssignButton(_thisButton));
            _valueText.text = "+" + _reward.iValue;
        }

        private void OnEnable()
        {
            gameObject.SetActive(false);
        }

        private void AssignButton(Button _bu)
        {
            if (_tutorialController)
            {
                if (!_tutorialController.IsRightInput()) return;
            }
            
            if (_bu == _thisButton)
            {
            
            }
        }
    }
}
