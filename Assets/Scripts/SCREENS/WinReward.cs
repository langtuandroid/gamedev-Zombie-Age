using System.Collections;
using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace SCREENS
{
    public class WinReward : MonoBehaviour
    {
        [Inject] private UIController _uiController;
        [Inject] private SoundController _soundController;
        [Inject] private TutorialController _tutorialController;
        [FormerlySerializedAs("m_tranOfRay")] [SerializeField] Transform _rayTransform;
       
        [FormerlySerializedAs("buReceive")] [SerializeField] private Button _receiveButton;
        [FormerlySerializedAs("buX2Gem")] [SerializeField] private Button _x2GemsButton;
        [FormerlySerializedAs("imaIcon")] [SerializeField] private Image _imageIcon;
        [FormerlySerializedAs("txtContent")] [SerializeField] private Text _contentText;
        public bool _isX2Gems { get; private set; }
        private RewardData _revard;
        private Vector3 _euler;
        public static WinReward Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            _receiveButton.onClick.AddListener(() => AssignButtonButton(_receiveButton));
            _x2GemsButton.onClick.AddListener(() => AssignButtonButton(_x2GemsButton));
        }


        private void Update()
        {
            _euler.z -= 0.2f;
            _rayTransform.eulerAngles = _euler;
        }

        private void AssignButtonButton(Button _bu)
        {
            if (_tutorialController)
            {
                if (!_tutorialController.IsRightInput()) return;
            }

            if (_bu == _receiveButton)
            {
                _revard.GetReward();
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                _uiController.HidePopup(UIController.POP_UP.reward);

            }
            else if (_bu == _x2GemsButton)
            {
                _x2GemsButton.gameObject.SetActive(false);
            }

        }



        public static void LoadRevardReward(RewardData _reward)
        {
            if (_reward == null) return;

            EventController.OnGetRewardInvoke(_reward.eReward);

            Instance._isX2Gems = false;
            SoundController.Instance.Play(SoundController.SOUND.ui_wood_board);//sound
            Instance._revard = _reward;
            Instance._imageIcon.sprite = Instance._revard.sprIcon;

            if (_reward.eReward == EnumController.REWARD.victory_gem_easy
                || _reward.eReward == EnumController.REWARD.victory_gem_normal
                || _reward.eReward == EnumController.REWARD.victory_gem_nightmate)
            {
                Instance._revard.strContent = "Get +" + _reward.GetVictoryGem() + " gems now! ";
            }
            else
            {
                Instance._revard.strContent = _reward.strContent;
                Instance._x2GemsButton.gameObject.SetActive(false);
            }

            Instance._contentText.text = Instance._revard.strContent;
        }


        public static void GetX2Gem()
        {
            Instance._contentText.text = "Get +" + (Instance._revard.GetVictoryGem() * 2) + " gems now! ";
            Instance._isX2Gems = true;
        }


        private IEnumerator IeGetReward()
        {
            yield return new WaitForSeconds(1.0f);
        }





        private void OnDisable()
        {
            EventController.OnUpdatedBoardInvoke();

        }

    }
}
