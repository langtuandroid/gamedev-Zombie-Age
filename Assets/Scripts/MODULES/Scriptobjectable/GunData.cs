using System.Collections.Generic;
using MANAGERS;
using SCREENS;
using UnityEngine;

namespace MODULES.Scriptobjectable
{
    [CreateAssetMenu(fileName = "Gun Data")]
    [System.Serializable]
    public class GunData : ScriptableObject
    {
        [System.Serializable]
        public class WeData
        {
            public EnumController.WEAPON eWeapon;
            public EnumController.ITEM_LEVEL eLevel;


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
                        case EnumController.WEAPON.colt_python:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_gun_glock);
                            break;
                        case EnumController.WEAPON.shotgun:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_gun_shotgun);
                            break;
                        case EnumController.WEAPON.shotgun2barrel:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_gun_double_shotgun);
                            break;
                        case EnumController.WEAPON.fn_p90:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_gun_ar15);
                            break;
                        case EnumController.WEAPON.ak47:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_gun_ak47);
                            break;
                        case EnumController.WEAPON.m16:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_gun_m16);
                            break;
                        case EnumController.WEAPON.m134:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_gun_minigun);
                            break;
                        case EnumController.WEAPON.firegun:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_gun_firegun);
                            break;
                        case EnumController.WEAPON.bazoka:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_gun_bazoka);
                            break;
                        case EnumController.WEAPON.stun_gun:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_gun_electric);
                            break;

                    }

                    UIController.Instance.PopUpShow(UIController.POP_UP.reward);
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

        public int GetDamage(EnumController.ITEM_LEVEL _level)
        {
            //    //Cong thuc: Damage = 4*level+iBaseDamage;
            //    // int _damage = 4 * (int)_level + iBaseDamage;
            //    int _damage = iBaseDamage;    
            //    for (int i = 0; i < (int)_level+1; i++)
            //    {
            //        _damage = (int)(_damage * 1.1f);
            //    }

            int _damage = LIST_DAMAGE[(int)_level];

            if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.weapon_damage5).bEQUIPED)
            {
                _damage = (int)(_damage * 1.05f);
            }
            if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.weapon_damage_10).bEQUIPED)
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
        public int GetPriceToUpgrade(EnumController.ITEM_LEVEL _level)
        {
            if (_level == EnumController.ITEM_LEVEL.level_7) return 0;
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
            int _currentlevel = DataController.Instance.playerData.CalculatePlayerLevel();
            if (!bUNLOCKED && !bIsOnlyCoinUnlock && _currentlevel >= iLevelToUnlock)
            {
                bUNLOCKED = true;
                DataController.Instance.SaveData();//save
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

            if (DataController.Instance.playerData.TakeWeapon(DATA.eWeapon) != null)
            {
                DATA = DataController.Instance.playerData.TakeWeapon(DATA.eWeapon);
            }
            else
            {
                DataController.Instance.playerData._weaponList.Add(DATA);
                // TheDataManager.Instance.SaveDataPlayer();//save
            }



            if (DATA.bEquiped && !WeaponController.Instance.equipedWeaponList.Contains(this))
                WeaponController.Instance.equipedWeaponList.Add(this);

        }


    }
}
