using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackWeapon : MonoBehaviour
{

    public GunData GUN_DATA;


    [SerializeField]
    private Button buThis, buEquip;
    [SerializeField] Sprite sprButtonGray, sprButtonEquiped;

    [SerializeField]
    private Text txtName;

    [SerializeField]
    private Image imaIcon, imaLock;


    // Start is called before the first frame update
    void Start()
    {
        buThis.onClick.AddListener(() => SetButton(buThis));
        buEquip.onClick.AddListener(() => SetButton(buEquip));
    }

    public void Init(GunData _gundata)
    {

        GUN_DATA = _gundata;
        GUN_DATA.CheckUnlockWithLevel();
        txtName.text = GUN_DATA.strNAME;
        buEquip.image.sprite = sprButtonGray;

        //UNLOCK
        if (GUN_DATA.bUNLOCKED)
        {
            imaLock.color = Color.white * 0.0f;
            imaIcon.sprite = GUN_DATA.sprIcon;
            buEquip.gameObject.SetActive(true);
            buEquip.image.sprite = sprButtonGray;
        }
        else
        {
            imaLock.color = Color.white;
            imaIcon.sprite = GUN_DATA.sprIcon_gray;
            buEquip.gameObject.SetActive(false);
        }

        //EQUIPED
        if (GUN_DATA.DATA.bEquiped)
        {
            buEquip.image.sprite = sprButtonEquiped;
        }

    }



    //SET BUTTON
    private void SetButton(Button _bu)
    { //for tutorial
        if (TheTutorialManager.Instance)
        {
            if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
        }

        if (_bu == buThis)
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_wood_board);//sound
            MainCode_Weapon.Instance.m_MainPanelWeapon.ShowTrack(this);
        }
        else if (_bu == buEquip)
        {
            MainCode_Weapon.Instance.m_MainPanelWeapon.ShowTrack(this);
            if (!GUN_DATA.DATA.bEquiped)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                // GUN_DATA.DATA.bEquiped = true;
                MainCode_Weapon.Instance.m_mainEquitpedWeapon.AddEquipedWeapon(GUN_DATA);
            }
            else
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
        }

    }

    public void UnlockNow()
    {
        int _price = GUN_DATA.iPriceToUnlock;
        if (TheDataManager.Instance.THE_DATA_PLAYER.iGem >= _price)
        {
            TheDataManager.Instance.THE_DATA_PLAYER.iGem -= _price;
            TheDataManager.Instance.SaveDataPlayer();//save
            TheEventManager.PostEvent_OnUpdatedBoard();

            GUN_DATA.bUNLOCKED = true;
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
           
        }
        else
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                                                                                    //khong du tien
            TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.note);
            Note.SetNote(Note.NOTE.no_gem.ToString());
        }




        Init(GUN_DATA);
        MainCode_Weapon.Instance.m_MainPanelWeapon.ShowTrack(this);
    }

    //EVENT
    private void HandleAddToEquipedWeaponList(GunData _gun)
    {
        if (_gun != GUN_DATA) return;
        GUN_DATA.DATA.bEquiped = true;
        if (!TheWeaponManager.Instance.LIST_EQUIPED_WEAPON.Contains(GUN_DATA))
            TheWeaponManager.Instance.LIST_EQUIPED_WEAPON.Add(GUN_DATA);

        Init(GUN_DATA);
    }
    private void HandleRemoveFromListEquipedList(GunData _gun)
    {
        if (_gun != GUN_DATA) return;

        if (TheWeaponManager.Instance.LIST_EQUIPED_WEAPON.Contains(GUN_DATA))
            TheWeaponManager.Instance.LIST_EQUIPED_WEAPON.Remove(GUN_DATA);
        GUN_DATA.DATA.bEquiped = false;

        Init(GUN_DATA);
    }

    private void OnEnable()
    {
        TheEventManager.OnAddToEquipedWeaponList += HandleAddToEquipedWeaponList;
        TheEventManager.OnRemoveFromEquipedWeaponList += HandleRemoveFromListEquipedList;
    }
    private void OnDisable()
    {
        TheEventManager.OnAddToEquipedWeaponList -= HandleAddToEquipedWeaponList;
        TheEventManager.OnRemoveFromEquipedWeaponList -= HandleRemoveFromListEquipedList;
    }

}
