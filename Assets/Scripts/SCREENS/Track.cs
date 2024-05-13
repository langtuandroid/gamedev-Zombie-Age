using Integration;
using MANAGERS;
using MODULES.Scriptobjectable;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace SCREENS
{
    public class Track : MonoBehaviour
    {
        [Inject] private RewardedAdController _rewardedAdController;
        public GameObject objRay;
        private RewardData m_reward;
        [SerializeField] private GameObject objNote;
        [SerializeField] private Image imaIcon;
        [SerializeField] private TMP_Text txtValue;
        [SerializeField] private TMP_Text txtDay;
        [SerializeField] private Button _videoButton;
        [SerializeField] private Button _collectButton;
        private CheckIn _checkIn;
        private int iDay;
        public void Init(int _day, CheckIn checkIn)
        {
            _checkIn = checkIn;
            iDay = _day;
            _videoButton.onClick.RemoveAllListeners();
            _videoButton.onClick.AddListener(WatchVideo);
            
            _collectButton.onClick.RemoveAllListeners();
            _collectButton.onClick.AddListener(Collect);
            
            m_reward = _checkIn.LIST_REWARD[iDay];

            if (_checkIn._currentHour == iDay)
            {
                objNote.SetActive(true);
                imaIcon.color = Color.white;
                objRay.SetActive(true);
                _videoButton.transform.localScale = Vector3.one * 1.1f;
                if (iDay == 0)
                {
                    Debug.Log(1);
                    _videoButton.gameObject.SetActive(false);
                    _collectButton.gameObject.SetActive(true);
                }
            }
            else
            {
                _videoButton.interactable = false;
                _collectButton.interactable = false;
                objNote.SetActive(false);
                imaIcon.color = Color.gray;
                objRay.SetActive(false);
                _videoButton.transform.localScale = Vector3.one * 0.98f;
            }

            txtDay.text = "HOUR " + (iDay + 1);
            txtValue.text = "+" + m_reward.iValue;
            imaIcon.sprite = m_reward.sprIcon;
        }

        private void WatchVideo()
        {
            if (TutorialController.Instance)
            {
                if (!TutorialController.Instance.IsRightInput()) return;
            }
            
            if (_checkIn._currentHour != iDay) return;
            
            _rewardedAdController.ShowAd();
            _rewardedAdController.GetRewarded += GetReward;
            _rewardedAdController.OnVideoClosed += VideoClosed;
        }

        private void VideoClosed()
        {
            _rewardedAdController.GetRewarded -= GetReward;
            _rewardedAdController.OnVideoClosed -= VideoClosed;
        }

        private void Collect()
        {
            if (TutorialController.Instance)
            {
                if (!TutorialController.Instance.IsRightInput()) return;
            }
            
            if (_checkIn._currentHour != iDay) return;
            
            GetReward();
        }

        private void GetReward()
        {
            UIController.Instance.PopUpShow(UIController.POP_UP.reward);
            WinReward.LoadRevardReward(m_reward);
            _checkIn.gameObject.SetActive(false);
            Debug.Log("Get reward me!");
        }
    }
}