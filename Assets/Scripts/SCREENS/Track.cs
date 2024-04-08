using MANAGERS;
using MODULES.Scriptobjectable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCREENS
{
    public class Track : MonoBehaviour
    {
        public GameObject objRay;
        private RewardData m_reward;
        [SerializeField] private GameObject objNote;
        [SerializeField] private Image imaIcon;
        [SerializeField] private TMP_Text txtValue;
        [SerializeField] private TMP_Text txtDay;
        [SerializeField] private Button buGetReward;
        private CheckIn _checkIn;
        private int iDay;
        public void Init(int _day, CheckIn checkIn)
        {
            _checkIn = checkIn;
            iDay = _day;
            buGetReward.onClick.RemoveAllListeners();
            buGetReward.onClick.AddListener(() => GetReward());
            
            m_reward = _checkIn.LIST_REWARD[iDay];

            if (_checkIn._currentDay == iDay)
            {
                objNote.SetActive(true);
                imaIcon.color = Color.white;
                objRay.SetActive(true);
                buGetReward.transform.localScale = Vector3.one * 1.1f;
            }
            else
            {
                objNote.SetActive(false);
                imaIcon.color = Color.gray;
                objRay.SetActive(false);
                buGetReward.transform.localScale = Vector3.one * 0.98f;
            }

            txtDay.text = "HOUR " + (iDay + 1);
            txtValue.text = "+" + m_reward.iValue;
            imaIcon.sprite = m_reward.sprIcon;
        }

        private void GetReward()
        {
            //tutorial
            if (TutorialController.Instance)
            {
                if (!TutorialController.Instance.IsRightInput()) return;
            }


            if (_checkIn._currentDay != iDay) return;

            UIController.Instance.PopUpShow(UIController.POP_UP.reward);
            WinReward.LoadRevardReward(m_reward);
            _checkIn.gameObject.SetActive(false);
            Debug.Log("Get reward me!");
        }
    }
}