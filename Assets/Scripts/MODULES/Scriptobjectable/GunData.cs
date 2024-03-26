using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Gun Data")]
[System.Serializable]
public class GunData : ScriptableObject
{
    [System.Serializable]
    public class WeData
    {
        public TheEnumManager.WEAPON eWeapon;
        public TheEnumManager.ITEM_LEVEL eLevel;


        [Space(10)]
        public bool bUnlocked;
        public bool bEquiped;
        public int iCurrentAmmo;// số đạn hiện tại
        public int iCurrentAmmoInMagazin; // so đạn có trong băng đạn.
        public bool bIsDefaultGun;//sung mặc định - free - ko giới hạn đạn


    }



    public WeData ORIGINAL_DATA;
    private WeData m_data;
    public WeData DATA
    {
        get
        {
            return m_data;
        }
        set
        {
            m_data = value;
        }
    }

    public string strNAME;
    public bool bUNLOCKED
    {
        get
        {
            return DATA.bUnlocked;
        }
        set
        {
            if (DATA.bUnlocked == false && value == true)
            {
                RewardData _reward = null;
                switch (DATA.eWeapon)
                {
                    case TheEnumManager.WEAPON.colt_python:
                        _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_gun_glock);
                        break;
                    case TheEnumManager.WEAPON.shotgun:
                        _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_gun_shotgun);
                        break;
                    case TheEnumManager.WEAPON.shotgun2barrel:
                        _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_gun_double_shotgun);
                        break;
                    case TheEnumManager.WEAPON.fn_p90:
                        _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_gun_ar15);
                        break;
                    case TheEnumManager.WEAPON.ak47:
                        _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_gun_ak47);
                        break;
                    case TheEnumManager.WEAPON.m16:
                        _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_gun_m16);
                        break;
                    case TheEnumManager.WEAPON.m134:
                        _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_gun_minigun);
                        break;
                    case TheEnumManager.WEAPON.firegun:
                        _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_gun_firegun);
                        break;
                    case TheEnumManager.WEAPON.bazoka:
                        _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_gun_bazoka);
                        break;
                    case TheEnumManager.WEAPON.stun_gun:
                        _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_gun_electric);
                        break;

                }

                TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.reward);
                VictoryReward.SetReward(_reward);

            }
            DATA.bUnlocked = value;
        }
    }



    [Space(30)]
    public GameObject objPrefabHand;
    public Sprite sprIcon;
    public Sprite sprIcon_gray;
    public Sprite spriteOfWeaponShell;
    public Sprite sprBullet;
    public Vector2 vScaleOfBullet;


    #region CONFIG AMMO 
    [Header("_____ CONFIG ____________")]

    [Space(30)]

    [Tooltip("Phạm vi để gây sát thương")]
    public float fRangeOfBullet;

    [Tooltip("thoi gian load giữa 2 viên đạn")]
    public float fTimeloadOrBullet; // 
   
    public List<int> LIST_DAMAGE;


    //Get reload time

    //Get damage

    public int GetDamage(TheEnumManager.ITEM_LEVEL _level)
    {
        //    //Cong thuc: Damage = 4*level+iBaseDamage;
        //    // int _damage = 4 * (int)_level + iBaseDamage;
        //    int _damage = iBaseDamage;    
        //    for (int i = 0; i < (int)_level+1; i++)
        //    {
        //        _damage = (int)(_damage * 1.1f);
        //    }

        int _damage = LIST_DAMAGE[(int)_level];

        if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.weapon_damage5).bEQUIPED)
        {
            _damage = (int)(_damage * 1.05f);
        }
        if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.weapon_damage_10).bEQUIPED)
        {
            _damage = (int)(_damage * 1.1f);
        }

        Debug.Log("DAMAGE: " + _damage);
        return _damage;
    }

    [Space(20)]
    [Tooltip("số đạn có trong 1 băng")]
    public int iBulletInEachGroup;

    [Tooltip("số đạn tối đa của súng")]
    public int iMaxAmmo;

    [Tooltip("số đạn có thể mua 1 lúc (trong cửa hàng)")]
    public int iAmmoToBuy;


    [Tooltip("số đạn trong 1 băng ")]
    public int iAmmoInMagazine;


    [Space(20)]
    [Tooltip("Giá mua 1 nhóm đạn")]
    public int iPriceToBuyAmmo;
   

    [Header("_____ UNLOCK ____________")]
    [Space(30)]

    public bool bIsOnlyCoinUnlock;//chi unlock bang coin
    public int iLevelToUnlock;
    public int iPriceToUnlock;



    [Header("_____ PRICE TO UPGRADE ____________")]
    [Space(30)]
    public List<int> LIST_UPGRADE_PRICE;
    public int GetPriceToUpgrade(TheEnumManager.ITEM_LEVEL _level)
    {
        if (_level == TheEnumManager.ITEM_LEVEL.level_7) return 0;
        return LIST_UPGRADE_PRICE[(int)_level];


        //if (data.elevel != theenummanager.item_level.level_7)
        //{
        //    return (int)(getdamage(data.elevel + 1) * thedatamanager.instance.price_config.funitpricegem_damage);
        //}
        //return 0;
        
    }




    #endregion








    //CHECK UNLOCK WITH LEVEL
    public void CheckUnlockWithLevel()
    {
        int _currentlevel = TheDataManager.Instance.THE_DATA_PLAYER.GetTotalPlayerLevel();
        if (!bUNLOCKED && !bIsOnlyCoinUnlock && _currentlevel >= iLevelToUnlock)
        {
            bUNLOCKED = true;
            TheDataManager.Instance.SaveDataPlayer();//save
        }
    }

    //INIT
    public void Init()
    {
        //clone
        m_data = new WeData();
        m_data.eWeapon = ORIGINAL_DATA.eWeapon;
        m_data.eLevel = ORIGINAL_DATA.eLevel;
        m_data.bUnlocked = ORIGINAL_DATA.bUnlocked;
        m_data.bEquiped = ORIGINAL_DATA.bEquiped;
        m_data.iCurrentAmmo = ORIGINAL_DATA.iCurrentAmmo;
        m_data.bIsDefaultGun = ORIGINAL_DATA.bIsDefaultGun;

        //=============================

        if (TheDataManager.Instance.THE_DATA_PLAYER.GetWeapon(DATA.eWeapon) != null)
        {
            DATA = TheDataManager.Instance.THE_DATA_PLAYER.GetWeapon(DATA.eWeapon);
        }
        else
        {
            TheDataManager.Instance.THE_DATA_PLAYER.LIST_WEAPON.Add(DATA);
            // TheDataManager.Instance.SaveDataPlayer();//save
        }



        if (DATA.bEquiped && !TheWeaponManager.Instance.LIST_EQUIPED_WEAPON.Contains(this))
            TheWeaponManager.Instance.LIST_EQUIPED_WEAPON.Add(this);

    }


}
