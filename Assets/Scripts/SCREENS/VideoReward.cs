using MANAGERS;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SCREENS
{
    public class VideoReward : MonoBehaviour
    {
        [FormerlySerializedAs("buClose")] public Button _closeButton;
        [FormerlySerializedAs("buWacthAds1")] public Button _watchAdd1;
        [FormerlySerializedAs("buWacthAds2")] public Button _watchAdd2;
        [FormerlySerializedAs("buWacthAds3")] public Button _watchAdd3;
        [FormerlySerializedAs("buWacthAds4")] public Button _watchAdd4;
        private static int _index = 1;

        private void Start()
        {
            _closeButton.onClick.AddListener(() => AssignButton(_closeButton));

            _watchAdd1.onClick.AddListener(() => AssignButton(_watchAdd1));
            _watchAdd2.onClick.AddListener(() => AssignButton(_watchAdd2));
            _watchAdd3.onClick.AddListener(() => AssignButton(_watchAdd3));
            _watchAdd4.onClick.AddListener(() => AssignButton(_watchAdd4));
        }

        private void AssignButton(Button _bu)
        {
            if (_bu == _closeButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                UIController.Instance.HidePopup(UIController.POP_UP.video_reward);
            }
            else if (_bu == _watchAdd1)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                ButtonWatchAds(1);
            }
            else if (_bu == _watchAdd2)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                ButtonWatchAds(2);
            }
            else if (_bu == _watchAdd3)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                ButtonWatchAds(3);
            }
            else if (_bu == _watchAdd4)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                ButtonWatchAds(4);
            }
        }


        private void ButtonWatchAds(int _index)
        {
            if (VideoReward._index != _index) return;
            UIController.Instance.HidePopup(UIController.POP_UP.video_reward);
        }


        private void SetUp()
        {
            if (false)
            {

                if (_index == 1)
                {
                    _watchAdd1.image.color = Color.white;
                    _watchAdd2.image.color = Color.gray;
                    _watchAdd3.image.color = Color.gray;
                    _watchAdd4.image.color = Color.gray;

                    _watchAdd1.transform.GetChild(0).GetComponent<Image>().color = Color.white * 1.0f;
                    _watchAdd2.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    _watchAdd3.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    _watchAdd4.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                }
                else if (_index == 2)
                {
                    _watchAdd1.image.color = Color.gray;
                    _watchAdd2.image.color = Color.white;
                    _watchAdd3.image.color = Color.gray;
                    _watchAdd4.image.color = Color.gray;

                    _watchAdd1.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    _watchAdd2.transform.GetChild(0).GetComponent<Image>().color = Color.white * 1.0f;
                    _watchAdd3.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    _watchAdd4.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;

                }
                else if (_index == 3)
                {
                    _watchAdd1.image.color = Color.gray;
                    _watchAdd2.image.color = Color.gray;
                    _watchAdd3.image.color = Color.white;
                    _watchAdd4.image.color = Color.gray;

                    _watchAdd1.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    _watchAdd2.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    _watchAdd3.transform.GetChild(0).GetComponent<Image>().color = Color.white * 1.0f;
                    _watchAdd4.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;


                }
                else if (_index == 4)
                {
                    _watchAdd1.image.color = Color.gray;
                    _watchAdd2.image.color = Color.gray;
                    _watchAdd3.image.color = Color.gray;
                    _watchAdd4.image.color = Color.white;

                    _watchAdd1.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    _watchAdd2.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    _watchAdd3.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    _watchAdd4.transform.GetChild(0).GetComponent<Image>().color = Color.white * 1.0f;


                }
                else
                {
                    _watchAdd1.image.color = Color.gray;
                    _watchAdd2.image.color = Color.gray;
                    _watchAdd3.image.color = Color.gray;
                    _watchAdd4.image.color = Color.gray;

                    _watchAdd1.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    _watchAdd2.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    _watchAdd3.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                    _watchAdd4.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;

                }
            }
            else
            {
                _watchAdd1.image.color = Color.gray;
                _watchAdd2.image.color = Color.gray;
                _watchAdd3.image.color = Color.gray;
                _watchAdd4.image.color = Color.gray;

                _watchAdd1.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                _watchAdd2.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                _watchAdd3.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
                _watchAdd4.transform.GetChild(0).GetComponent<Image>().color = Color.white * 0.0f;
            }
        }

        private void OnEnable()
        {
            SetUp();
        }
    }
}
