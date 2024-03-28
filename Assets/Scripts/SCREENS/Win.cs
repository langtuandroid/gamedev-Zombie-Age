using System.Collections;
using System.Collections.Generic;
using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SCREENS
{
    public class Win : MonoBehaviour
    {
        [FormerlySerializedAs("buContinue")] [SerializeField] Button _continueButton;
        [FormerlySerializedAs("buRate")] [SerializeField] Button _buttonRate;
        [FormerlySerializedAs("txtLevel")] [SerializeField] Text _levelText;

        [FormerlySerializedAs("LIST_STAR")] [SerializeField] List<Image> _starList;

        
        [Space(20)]
        [FormerlySerializedAs("m_tranOfRay")][SerializeField] Transform _rayTransform;
        private Vector3 _euler;
        private bool _showRate;

        private void Start()
        {
            _continueButton.onClick.AddListener(() => SetButton(_continueButton));
            _buttonRate.onClick.AddListener(() => SetButton(_buttonRate));
        }

        private void Update()
        {
            _euler.z -= 0.2f;
            _rayTransform.eulerAngles = _euler;
        }
        
        private void SetButton(Button _bu)
        {
            if (_bu == _continueButton)
            {
                if (_continueButton.image.color != Color.white) return;

                MusicManager.Instance.Play();
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound         
             
                #region RATE
                if (DataController.Instance.playerData.CurrentLevel > 0
                    && (DataController.Instance.playerData.CurrentLevel + 1) % 4 == 0)
                {
                    if (!_showRate)
                    {
                        _showRate = true;
                        UIController.Instance.PopUpShow(UIController.POP_UP.rate);
                        return;
                    }
                    else
                    {
                        goto HERE;
                    }
                }
                #endregion

                HERE:



                UIController.Instance.LoadScene(UIController.SCENE.LevelSelection);

            }

            else if (_bu == _buttonRate)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
            }
        }


        private IEnumerator WinRoutine()
        {
            _continueButton.image.color = Color.gray;
            int _star = DataController.Instance.GetStars();//get star
            int _level = DataController.Instance.playerData.CurrentLevel;
            _levelText.text = "LEVEL " + (_level + 1).ToString();

            if (DataController.Instance.mode == DataController.Mode.Release)
            {
                //save star        
                DataController.Instance.playerData.SetStar(_level, _star);

            }


            #region STAR ENIMATION        
            RewardData _reward = null;

            _starList[0].gameObject.SetActive(false);
            _starList[1].gameObject.SetActive(false);
            _starList[2].gameObject.SetActive(false);



            if (DataController.Instance.playerData.Difficuft == EnumController.DIFFICUFT.easy)
            {          
                _reward = DataController.Instance.GetReward(EnumController.REWARD.victory_gem_easy);//
            }
            else if (DataController.Instance.playerData.Difficuft == EnumController.DIFFICUFT.normal)
            {
                _reward = DataController.Instance.GetReward(EnumController.REWARD.victory_gem_normal);//
            }
            else if (DataController.Instance.playerData.Difficuft == EnumController.DIFFICUFT.nightmare)
            {         
                _reward = DataController.Instance.GetReward(EnumController.REWARD.victory_gem_nightmate);//
            }


            yield return new WaitForSecondsRealtime(1.0f);
            switch (_star)
            {
                case 1:
                    yield return new WaitForSecondsRealtime(0.5f);             
                    _starList[0].gameObject.SetActive(true);

                    SoundController.Instance.Play(SoundController.SOUND.sfx_gun_ak);//sound
                    break;
                case 2:
                    yield return new WaitForSecondsRealtime(0.5f);
                    _starList[0].gameObject.SetActive(true);
                    SoundController.Instance.Play(SoundController.SOUND.sfx_gun_ak);//sound

                    yield return new WaitForSecondsRealtime(0.5f);
                    _starList[1].gameObject.SetActive(true);
                    SoundController.Instance.Play(SoundController.SOUND.sfx_gun_ar15);//sound



                    break;
                case 3:
                    yield return new WaitForSecondsRealtime(0.5f);
                    _starList[0].gameObject.SetActive(true);
                    SoundController.Instance.Play(SoundController.SOUND.sfx_gun_ak);//sound

                    yield return new WaitForSecondsRealtime(0.5f);
                    _starList[1].gameObject.SetActive(true);
                    SoundController.Instance.Play(SoundController.SOUND.sfx_gun_ar15);//sound

                    yield return new WaitForSecondsRealtime(0.5f);
                    _starList[2].gameObject.SetActive(true);
                    SoundController.Instance.Play(SoundController.SOUND.sfx_gun_shotgun);//sound
                    break;

            }

            #endregion

            yield return new WaitForSecondsRealtime(1.2f);
            _continueButton.image.color = Color.white;

            SoundController.Instance.Play(SoundController.SOUND.ui_wood_board);//sound
            UIController.Instance.PopUpShow(UIController.POP_UP.reward);
            WinReward.LoadRevardReward(_reward);


        

        }

        private void OnEnable()
        {
            _showRate = false;
            SoundController.Instance.Play(SoundController.SOUND.ui_wood_board);//sound
            MusicManager.Instance.Stop();
            SoundController.Instance.Play(SoundController.SOUND.ui_victory);//sound
            StartCoroutine(WinRoutine());

        }

        private void OnDisable()
        {
            DataController.Instance.SaveData();//save
            StopAllCoroutines();
        }
    }
}
