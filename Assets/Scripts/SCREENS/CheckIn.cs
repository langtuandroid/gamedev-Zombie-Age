using System;
using System.Collections.Generic;
using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SCREENS
{
    public class CheckIn : MonoBehaviour
    {
        [Inject] private SoundController _soundController;
        [Inject] private DataController _dataController;
        [Inject] private TutorialController _tutorialController;
        [SerializeField] Text txtCountPage;
        [SerializeField] Button buClose, buNextPage, buBackPage;
        [SerializeField] Animator m_animator;
        [Space(20)]
        public List<RewardData> LIST_REWARD;
        [Space(30)]
        public List<Track> LIST_TRACK;
        
        private int iTotalTrack;
        private Vector3 vEuler;
        private int iTotalReward;
        private int iMaxPage;
        private int iCurrentPage;
        public int _currentHour { get; private set; }
        private void Awake()
        {
            _currentHour = _dataController.playerData._hour;
            iTotalTrack = LIST_TRACK.Count;
            iTotalReward = LIST_REWARD.Count;
            iMaxPage = iTotalReward / 4;

            buClose.onClick.AddListener(() => SetButton(buClose));
            buNextPage.onClick.AddListener(() => SetButton(buNextPage));
            buBackPage.onClick.AddListener(() => SetButton(buBackPage));
        }

        private void OnEnable()
        {
            iCurrentPage = (int)(_currentHour / 4.0f);
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
            if (_tutorialController)
            {
                if (!_tutorialController.IsRightInput()) return;
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
                _soundController.Play(SoundController.SOUND.ui_click_back);//sound
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
                LIST_TRACK[i].Init(4 * _page + i, this);
            }
        }

    }
}
