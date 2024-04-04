using System.Collections;
using System.Collections.Generic;
using MANAGERS;
using MODULES.Scriptobjectable;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace SCREENS
{
    public class Win : MonoBehaviour
    {
        [Inject] private UIController _uiController;
        [Inject] private SoundController _soundController;
        [Inject] private MusicController _musicController;
        [Inject] private DataController _dataController;
        [FormerlySerializedAs("buContinue")] [SerializeField] Button _continueButton;
        [FormerlySerializedAs("txtLevel")] [SerializeField] TMP_Text _levelText;
        [FormerlySerializedAs("LIST_STAR")] [SerializeField] List<Image> _starList;
        [SerializeField] private Sprite _starEmpty, _starFilled;
        [Space(20)]
        [FormerlySerializedAs("m_tranOfRay")][SerializeField] Transform _rayTransform;
        private Vector3 _euler;

        private void Start()
        {
            _continueButton.onClick.AddListener(() => SetButton(_continueButton));
        }

        private void Update()
        {
            _euler.z -= 0.2f;
            _rayTransform.eulerAngles = _euler;
        }
        
        private void SetButton(Button _bu)
        {
            if (_bu != _continueButton) return;
            if (_continueButton.image.color != Color.white) return;

            _musicController.Play();
            _soundController.Play(SoundController.SOUND.ui_click_next);//sound         
            
            _uiController.LoadScene(UIController.SCENE.LevelSelection);
        }


        private IEnumerator WinRoutine()
        {
            _continueButton.image.color = Color.gray;
            int _star = _dataController.GetStars();//get star
            int _level = _dataController.playerData.CurrentLevel;
            _levelText.text = "LEVEL " + (_level + 1).ToString();

            if (_dataController.mode == DataController.Mode.Release)
            {
                _dataController.playerData.SetStar(_level, _star);

            }


            #region STAR ENIMATION        
            RewardData _reward = null;

            _starList[0].sprite = _starEmpty;
            _starList[1].sprite = _starEmpty;
            _starList[2].sprite = _starEmpty;



            if (_dataController.playerData.Difficuft == EnumController.DIFFICUFT.easy)
            {          
                _reward = _dataController.GetReward(EnumController.REWARD.victory_gem_easy);//
            }
            else if (_dataController.playerData.Difficuft == EnumController.DIFFICUFT.normal)
            {
                _reward = _dataController.GetReward(EnumController.REWARD.victory_gem_normal);//
            }
            else if (_dataController.playerData.Difficuft == EnumController.DIFFICUFT.nightmare)
            {         
                _reward = _dataController.GetReward(EnumController.REWARD.victory_gem_nightmate);//
            }


            yield return new WaitForSecondsRealtime(1.0f);
            switch (_star)
            {
                case 1:
                    yield return new WaitForSecondsRealtime(0.5f);             
                    _starList[0].sprite = _starFilled;
                    _soundController.Play(SoundController.SOUND.sfx_gun_ak);//sound
                    break;
                case 2:
                    yield return new WaitForSecondsRealtime(0.5f);
                    _starList[0].sprite = _starFilled;
                    _soundController.Play(SoundController.SOUND.sfx_gun_ak);//sound

                    yield return new WaitForSecondsRealtime(0.5f);
                    _starList[1].sprite = _starFilled;
                    _soundController.Play(SoundController.SOUND.sfx_gun_ar15);//sound



                    break;
                case 3:
                    yield return new WaitForSecondsRealtime(0.5f);
                    _starList[0].sprite = _starFilled;
                    _soundController.Play(SoundController.SOUND.sfx_gun_ak);//sound

                    yield return new WaitForSecondsRealtime(0.5f);
                    _starList[1].sprite = _starFilled;
                    _soundController.Play(SoundController.SOUND.sfx_gun_ar15);//sound

                    yield return new WaitForSecondsRealtime(0.5f);
                    _starList[2].sprite = _starFilled;
                    _soundController.Play(SoundController.SOUND.sfx_gun_shotgun);//sound
                    break;

            }

            #endregion

            yield return new WaitForSecondsRealtime(1.2f);
            _continueButton.image.color = Color.white;

            _soundController.Play(SoundController.SOUND.ui_wood_board);//sound
            _uiController.PopUpShow(UIController.POP_UP.reward);
            WinReward.LoadRevardReward(_reward);
        }

        private void OnEnable()
        {
            _soundController.Play(SoundController.SOUND.ui_wood_board);//sound
            _musicController.Stop();
            _soundController.Play(SoundController.SOUND.ui_victory);//sound
            StartCoroutine(WinRoutine());

        }

        private void OnDisable()
        {
            _dataController.SaveData();//save
            StopAllCoroutines();
        }
    }
}
