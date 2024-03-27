using System.Collections;
using System.Collections.Generic;
using _2_Weapon;
using MANAGERS;
using MODULES.Scriptobjectable;
using SCREENS;
using UnityEngine;
using UnityEngine.UI;

public class TrackDefense : MonoBehaviour
{
    public DefenseData DEFENSE_DATA;

    [SerializeField]
    private Image imaIcon;
    [SerializeField]
    private Text txtName;
    [SerializeField]
    private Button buEquip, buThis;
    [SerializeField] GameObject imaLock;

    [SerializeField] Sprite sprButtonGray, sprButtonEquiped;

    // Start is called before the first frame update
    void Start()
    {
        buEquip.onClick.AddListener(() => SetButton(buEquip));
        buThis.onClick.AddListener(() => SetButton(buThis));
    }


    private void SetButton(Button _bu)
    { //for tutorial
        if (TheTutorialManager.Instance)
        {
            if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
        }

        if (_bu == buThis)
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_wood_board);//sound
            MainCode_Weapon.Instance.m_MainPanelDefense.ShowTrack(this);
        }
        else if (_bu == buEquip)
        {
            MainCode_Weapon.Instance.m_MainPanelDefense.ShowTrack(this);
            //equiped
            if (!DEFENSE_DATA.DATA.bEquiped)
            {
                DEFENSE_DATA.DATA.bEquiped = true;
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_equiped);//sound
                buEquip.image.sprite = sprButtonEquiped;
                MainCode_Weapon.Instance.m_mainEquitpedDefense.AddEquipedDefense(DEFENSE_DATA);
            }
            else
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_equiped);//sound
            }
        }
    }

    //INIT
    public void Init(DefenseData _defenseData)
    {
        if (_defenseData == null) return;
        DEFENSE_DATA = _defenseData;
        DEFENSE_DATA.CheckUnlockWithLevel();


        txtName.text = DEFENSE_DATA.strName;
        imaIcon.sprite = DEFENSE_DATA.sprIcon;
        //UNLOCK
        if (DEFENSE_DATA.bUNLOCKED)
        {
            imaLock.SetActive(false);
            buEquip.gameObject.SetActive(true);
            buEquip.image.sprite = sprButtonGray;
        }
        else
        {
            imaLock.SetActive(true);
            buEquip.gameObject.SetActive(false);
        }

        //EQUIPED
        if (DEFENSE_DATA.DATA.bEquiped)
        {
            buEquip.image.sprite = sprButtonEquiped;
        }


    }

    //UNLOCK NOW
    public void UnlockNow()
    {
        int _price = DEFENSE_DATA.iPriteToUnlock;
        if (TheDataManager.Instance.THE_DATA_PLAYER.iGem >= _price)
        {
            TheDataManager.Instance.THE_DATA_PLAYER.iGem -= _price;
            TheDataManager.Instance.SaveDataPlayer();
            TheEventManager.PostEvent_OnUpdatedBoard();

            //content
            if(!DEFENSE_DATA.bUNLOCKED)
            {
                RewardData _reward = null;
                switch (DEFENSE_DATA.DATA.eDefense)
                {
                    case TheEnumManager.DEFENSE.home:
                        break;
                    case TheEnumManager.DEFENSE.metal:
                        _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_defense_metal);
                        break;
                    case TheEnumManager.DEFENSE.thorn:
                        _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_defense_thorn );
                        break;
                }
                TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.reward);
                VictoryReward.SetReward(_reward);
            }


            DEFENSE_DATA.bUNLOCKED = true;
            MainCode_Weapon.Instance.m_MainPanelDefense.ShowTrack(this);
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
            
        }
        else
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                                                                                    //khong du tien
            TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.note);
            Note.SetNote(Note.NOTE.no_gem.ToString());
        }
        Init(DEFENSE_DATA);
    }


    //EVENT
    private void HandleAddToEquipedDefense(DefenseData _defense)
    {
        if (_defense != DEFENSE_DATA) return;
        DEFENSE_DATA.DATA.bEquiped = true;
        Init(DEFENSE_DATA);
    }

    private void HandleRemoveToEquipedDefense(DefenseData _defense)
    {
        if (_defense != DEFENSE_DATA) return;
        DEFENSE_DATA.DATA.bEquiped = false ;
        Init(DEFENSE_DATA);
    }



    private void OnEnable()
    {
        TheEventManager.OnAddToEquipedDefenseList += HandleAddToEquipedDefense;
        TheEventManager.OnRemoveToEquipedDefenseList += HandleRemoveToEquipedDefense;
    }
    private void OnDisable()
    {
        TheEventManager.OnAddToEquipedDefenseList -= HandleAddToEquipedDefense;
        TheEventManager.OnRemoveToEquipedDefenseList -= HandleRemoveToEquipedDefense;
    }
}
