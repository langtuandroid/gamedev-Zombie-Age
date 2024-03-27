using System.Collections.Generic;
using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.UI;

namespace SCREENS
{
    public class CheckIn : MonoBehaviour
    {
        private static CheckIn Instance;
        public int iCurrentDay;

        [System.Serializable]
        public class Track
        {
            public int iDay;
            public RewardData m_reward;
            public GameObject objNote;
            public GameObject objRay;
            public Image imaIcon;
            public Text txtValue;
            public Text txtDay;
            public Button buGetReward;


            public void Init(int _day)
            {
                buGetReward.onClick.RemoveAllListeners();
                buGetReward.onClick.AddListener(() => GetReward());
                iDay = _day;
                m_reward = Instance.LIST_REWARD[iDay];

                if (Instance.iCurrentDay == iDay)
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

                txtDay.text = "Day " + (iDay + 1);
                txtValue.text = "+" + m_reward.iValue;
                imaIcon.sprite = m_reward.sprIcon;
            }

            private void GetReward()
            {
                //tutorial
                if (TheTutorialManager.Instance)
                {                
                    if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
                }


                if (Instance.iCurrentDay != iDay) return;

                TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.reward);
                VictoryReward.SetReward(m_reward);
                Instance.gameObject.SetActive(false);
                Debug.Log("Get reward me!");
            }
        }

        [SerializeField] Text txtCountPage;
        [SerializeField] Button buClose, buNextPage, buBackPage;
        [SerializeField] Animator m_animator;

        [Space(20)]
        public List<RewardData> LIST_REWARD;

        [Space(30)]
        private int iTotalTrack;
        public List<Track> LIST_TRACK;
        Vector3 vEuler = new Vector3();
        public int iTotalReward;
        private int iMaxPage;
        private int iCurrentPage;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            iTotalTrack = LIST_TRACK.Count;
            iTotalReward = LIST_REWARD.Count;
            iMaxPage = iTotalReward / 4;

            buClose.onClick.AddListener(() => SetButton(buClose));
            buNextPage.onClick.AddListener(() => SetButton(buNextPage));
            buBackPage.onClick.AddListener(() => SetButton(buBackPage));

        }
        // Start is called before the first frame update
        void OnEnable()
        {
       
            iCurrentDay = TheDataManager.Instance.THE_DATA_PLAYER.iCurrentDay;
            iCurrentPage = (int)(iCurrentDay / 4.0f);
            ShowTrack(iCurrentPage);
        }


        private void Update()
        {
            vEuler.z -= 0.3f;
            for (int i = 0; i < iTotalTrack; i++)
            {
                LIST_TRACK[i].objRay.transform.eulerAngles = vEuler;
            }
        }

        private void SetButton(Button _bu)
        {
            //for tutorial
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }


            if (_bu == buNextPage)
            {
                iCurrentPage++;
                if (iCurrentPage >= iMaxPage)
                    iCurrentPage = 0;

                ShowTrack(iCurrentPage);
            }
            else if (_bu == buBackPage)
            {

                iCurrentPage--;
                if (iCurrentPage < 0)
                    iCurrentPage = iMaxPage - 1;

                ShowTrack(iCurrentPage);
            }
            else if (_bu == buClose)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_back);//sound
                gameObject.SetActive(false);
            }
        }

        private void ShowTrack(int _page)
        {
            m_animator.Play(0);
            txtCountPage.text = (_page + 1) + "/" + iMaxPage;
            int _totalTrack = LIST_TRACK.Count;
            for (int i = 0; i < _totalTrack; i++)
            {
                LIST_TRACK[i].Init(4 * _page + i);
            }
        }



    }
}
