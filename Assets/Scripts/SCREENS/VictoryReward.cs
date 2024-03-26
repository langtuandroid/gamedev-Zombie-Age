using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryReward : MonoBehaviour
{
    [SerializeField] Transform m_tranOfRay;
    private Vector3 vEuler;

    [SerializeField] Button buReceive, buX2Gem;
    [SerializeField] Image imaIcon;
    [SerializeField] Text txtContent;

    public bool bX2Gem = false;
    private RewardData REWARD;

    public static VictoryReward Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        buReceive.onClick.AddListener(() => SetButton(buReceive));
        buX2Gem.onClick.AddListener(() => SetButton(buX2Gem));
    }


    private void Update()
    {
        vEuler.z -= 0.2f;
        m_tranOfRay.eulerAngles = vEuler;
    }

    private void SetButton(Button _bu)
    {
        //tutorial
        if (TheTutorialManager.Instance)
        {
            if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
        }

        if (_bu == buReceive)
        {
            REWARD.GetReward();
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
            TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.reward);

        }
        else if (_bu == buX2Gem)
        {
            buX2Gem.gameObject.SetActive(false);
        }

    }



    public static void SetReward(RewardData _reward)
    {
        if (_reward == null) return;

        //event
        TheEventManager.PostEvent_OnGetReward(_reward.eReward);

        Instance.bX2Gem = false;
        TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_wood_board);//sound
        Instance.REWARD = _reward;
        Instance.imaIcon.sprite = Instance.REWARD.sprIcon;

        if (_reward.eReward == TheEnumManager.REWARD.victory_gem_easy
            || _reward.eReward == TheEnumManager.REWARD.victory_gem_normal
            || _reward.eReward == TheEnumManager.REWARD.victory_gem_nightmate)
        {
            Instance.REWARD.strContent = "Get +" + _reward.GetVictoryGem() + " gems now! ";
        }
        else
        {
            Instance.REWARD.strContent = _reward.strContent;
            Instance.buX2Gem.gameObject.SetActive(false);
        }

        Instance.txtContent.text = Instance.REWARD.strContent;
    }


    public static void GetX2Gem()
    {
        Instance.txtContent.text = "Get +" + (Instance.REWARD.GetVictoryGem() * 2) + " gems now! ";
        Instance.bX2Gem = true;
    }


    private IEnumerator IeGetReward()
    {
        yield return new WaitForSeconds(1.0f);
    }





    private void OnDisable()
    {
        TheEventManager.PostEvent_OnUpdatedBoard();

    }

}
