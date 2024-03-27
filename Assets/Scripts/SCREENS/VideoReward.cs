using MANAGERS;
using UnityEngine;
using UnityEngine.UI;

namespace SCREENS
{
    public class VideoReward : MonoBehaviour
    {
        public Button buClose;
        public Button buWacthAds1, buWacthAds2, buWacthAds3, buWacthAds4;
        public static int iIndex = 1;

        private void Start()
        {
            buClose.onClick.AddListener(() => SetButton(buClose));

            buWacthAds1.onClick.AddListener(() => SetButton(buWacthAds1));
            buWacthAds2.onClick.AddListener(() => SetButton(buWacthAds2));
            buWacthAds3.onClick.AddListener(() => SetButton(buWacthAds3));
            buWacthAds4.onClick.AddListener(() => SetButton(buWacthAds4));
        }

        private void SetButton(Button _bu)
        {
            if (_bu == buClose)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.video_reward);
            }
            else if (_bu == buWacthAds1)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                ButtonWatchAds(1);
            }
            else if (_bu == buWacthAds2)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                ButtonWatchAds(2);
            }
            else if (_bu == buWacthAds3)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                ButtonWatchAds(3);
            }
            else if (_bu == buWacthAds4)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                ButtonWatchAds(4);
            }
        }


        private void ButtonWatchAds(int _index)
        {
            if (iIndex != _index) return;
            TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.video_reward);
        }


        private void SetUp()
        {
            if (false)
            {

                if (iIndex == 1)
                {
                    buWacthAds1.image.color = Color.white;
                    buWacthAds2.image.color = Color.gray;
                    buWacthAds3.image.color = Color.gray;
                    buWacthAds4.image.color = Color.gray;

                    buWacthAds1.transform.GetChild(0).GetComponent<Image>().color = Color.white * 1.0f;
                    buWacthAds2.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    buWacthAds3.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    buWacthAds4.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                }
                else if (iIndex == 2)
                {
                    buWacthAds1.image.color = Color.gray;
                    buWacthAds2.image.color = Color.white;
                    buWacthAds3.image.color = Color.gray;
                    buWacthAds4.image.color = Color.gray;

                    buWacthAds1.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    buWacthAds2.transform.GetChild(0).GetComponent<Image>().color = Color.white * 1.0f;
                    buWacthAds3.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    buWacthAds4.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;

                }
                else if (iIndex == 3)
                {
                    buWacthAds1.image.color = Color.gray;
                    buWacthAds2.image.color = Color.gray;
                    buWacthAds3.image.color = Color.white;
                    buWacthAds4.image.color = Color.gray;

                    buWacthAds1.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    buWacthAds2.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    buWacthAds3.transform.GetChild(0).GetComponent<Image>().color = Color.white * 1.0f;
                    buWacthAds4.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;


                }
                else if (iIndex == 4)
                {
                    buWacthAds1.image.color = Color.gray;
                    buWacthAds2.image.color = Color.gray;
                    buWacthAds3.image.color = Color.gray;
                    buWacthAds4.image.color = Color.white;

                    buWacthAds1.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    buWacthAds2.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    buWacthAds3.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    buWacthAds4.transform.GetChild(0).GetComponent<Image>().color = Color.white * 1.0f;


                }
                else
                {
                    buWacthAds1.image.color = Color.gray;
                    buWacthAds2.image.color = Color.gray;
                    buWacthAds3.image.color = Color.gray;
                    buWacthAds4.image.color = Color.gray;

                    buWacthAds1.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    buWacthAds2.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    buWacthAds3.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    buWacthAds4.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;

                }
            }
            else
            {
                buWacthAds1.image.color = Color.gray;
                buWacthAds2.image.color = Color.gray;
                buWacthAds3.image.color = Color.gray;
                buWacthAds4.image.color = Color.gray;

                buWacthAds1.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                buWacthAds2.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                buWacthAds3.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                buWacthAds4.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
            }
        }

        private void OnEnable()
        {
            SetUp();
        }
    }
}
