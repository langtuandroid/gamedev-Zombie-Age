using System.Collections;
using System.Collections.Generic;
using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.UI;

namespace SCREENS
{
    public class Victory : MonoBehaviour
    {

        [SerializeField] Button buContinue, buRate;  
        [SerializeField] Text txtLevel;

        [SerializeField] List<Image> LIST_STAR;

        [Space(20)]
        [SerializeField] Transform m_tranOfRay;
        private Vector3 vEuler;
        private bool bShowRate;


        // Start is called before the first frame update
        void Start()
        {
            buContinue.onClick.AddListener(() => SetButton(buContinue));
            buRate.onClick.AddListener(() => SetButton(buRate));
        }

        private void Update()
        {
            vEuler.z -= 0.2f;
            m_tranOfRay.eulerAngles = vEuler;
        }


        //SET BUTTON
        private void SetButton(Button _bu)
        {
            if (_bu == buContinue)
            {
                if (buContinue.image.color != Color.white) return;

                MusicManager.Instance.Play();
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound         
             
                //============================

                #region RATE
                if (DataController.Instance.playerData.CurrentLevel > 0
                    && (DataController.Instance.playerData.CurrentLevel + 1) % 4 == 0)
                {
                    if (!bShowRate)
                    {
                        bShowRate = true;
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

            else if (_bu == buRate)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
            }
        }


        private IEnumerator IeWin()
        {
            buContinue.image.color = Color.gray;
            int _star = DataController.Instance.GetStars();//get star
            int _level = DataController.Instance.playerData.CurrentLevel;
            txtLevel.text = "LEVEL " + (_level + 1).ToString();

            if (DataController.Instance.mode == DataController.Mode.Release)
            {
                //save star        
                DataController.Instance.playerData.SetStar(_level, _star);

            }


            #region STAR ENIMATION        
            RewardData _reward = null;

            LIST_STAR[0].gameObject.SetActive(false);
            LIST_STAR[1].gameObject.SetActive(false);
            LIST_STAR[2].gameObject.SetActive(false);



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
                    LIST_STAR[0].gameObject.SetActive(true);

                    SoundController.Instance.Play(SoundController.SOUND.sfx_gun_ak);//sound
                    break;
                case 2:
                    yield return new WaitForSecondsRealtime(0.5f);
                    LIST_STAR[0].gameObject.SetActive(true);
                    SoundController.Instance.Play(SoundController.SOUND.sfx_gun_ak);//sound

                    yield return new WaitForSecondsRealtime(0.5f);
                    LIST_STAR[1].gameObject.SetActive(true);
                    SoundController.Instance.Play(SoundController.SOUND.sfx_gun_ar15);//sound



                    break;
                case 3:
                    yield return new WaitForSecondsRealtime(0.5f);
                    LIST_STAR[0].gameObject.SetActive(true);
                    SoundController.Instance.Play(SoundController.SOUND.sfx_gun_ak);//sound

                    yield return new WaitForSecondsRealtime(0.5f);
                    LIST_STAR[1].gameObject.SetActive(true);
                    SoundController.Instance.Play(SoundController.SOUND.sfx_gun_ar15);//sound

                    yield return new WaitForSecondsRealtime(0.5f);
                    LIST_STAR[2].gameObject.SetActive(true);
                    SoundController.Instance.Play(SoundController.SOUND.sfx_gun_shotgun);//sound
                    break;

            }

            #endregion

            yield return new WaitForSecondsRealtime(1.2f);
            buContinue.image.color = Color.white;

            SoundController.Instance.Play(SoundController.SOUND.ui_wood_board);//sound
            UIController.Instance.PopUpShow(UIController.POP_UP.reward);
            VictoryReward.SetReward(_reward);


        

        }

        private void OnEnable()
        {
            bShowRate = false;
            SoundController.Instance.Play(SoundController.SOUND.ui_wood_board);//sound
            MusicManager.Instance.Stop();
            SoundController.Instance.Play(SoundController.SOUND.ui_victory);//sound
            StartCoroutine(IeWin());

        }

        private void OnDisable()
        {
            DataController.Instance.SaveData();//save
            StopAllCoroutines();
        }
    }
}
