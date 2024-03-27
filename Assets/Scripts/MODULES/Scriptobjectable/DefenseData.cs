using MANAGERS;
using SCREENS;
using UnityEngine;

namespace MODULES.Scriptobjectable
{
    [CreateAssetMenu(fileName = "Defense Data")]
    [System.Serializable]
    public class DefenseData : ScriptableObject
    {
        [System.Serializable]
        public class DeData
        {
            public TheEnumManager.DEFENSE eDefense;
            public TheEnumManager.ITEM_LEVEL eLevel;
            public bool bDefault;//is home
            public bool bUnlocked;
            public bool bEquiped;

        }


        [SerializeField]
        [Tooltip("Dữ liệu config ban đầu")]
        private DeData ORIGINAL_DATA;
        private DeData m_Data; //data to save
        public DeData DATA
        {
            get
            {
                return m_Data;
            }
            set
            {
                m_Data = value;
            }
        }



        public string strName;
        [Space(20)]
        public GameObject bjPrefab;
        public Sprite sprIcon;

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
                    switch (DATA.eDefense)
                    {
                        case TheEnumManager.DEFENSE.home:
                            break;
                        case TheEnumManager.DEFENSE.metal:
                            _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_defense_metal);
                            break;
                        case TheEnumManager.DEFENSE.thorn:
                            _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_defense_thorn);
                            break;

                    }
                    TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.reward);
                    VictoryReward.SetReward(_reward);
                }
                DATA.bUnlocked = value;
            }
        }

        [Header("Phần trăm DEFENSE được thêm vào")]
        [Space(30)]
        public int iBaseDefense;


        [Header("____ UNLOCK ___________")]
        [Space(30)]
        public bool bIsOnlyCoinUnlock;
        public int iLevelToUnlock;
        public int iPriteToUnlock;




        //DEFENSE
        public int GetDefense(TheEnumManager.ITEM_LEVEL _level)
        {
            //Cong thuc: 6x+iBaseDefese
            //link do thi: https://www.desmos.com/calculator/wvnakbcsch
            float _defense = 0;
       

            switch (DATA.eDefense)
            {
                case TheEnumManager.DEFENSE.home:
                    _defense = (20 * (int)_level) + iBaseDefense;
               
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.UpgradeType.home_defense15).bEQUIPED)
                    {
                        _defense = _defense * 1.15f;
                    }
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.UpgradeType.home_defense25).bEQUIPED)
                    {
                        _defense = _defense * 1.25f;
                    }
                    break;


                case TheEnumManager.DEFENSE.metal:
                    _defense = 35 * (int)_level + iBaseDefense;
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.UpgradeType.metal_defense15).bEQUIPED)
                    {
                        _defense = _defense * 1.15f;
                    }
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.UpgradeType.metal_defense25).bEQUIPED)
                    {
                        _defense = _defense * 1.25f;
                    }
                    break;


                case TheEnumManager.DEFENSE.thorn:
                    _defense = 30 * (int)_level + iBaseDefense;
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.UpgradeType.thorn_defense15).bEQUIPED)
                    {
                        _defense = _defense * 1.15f;
                    }
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.UpgradeType.thorn_defense25).bEQUIPED)
                    {
                        _defense = _defense * 1.25f;
                    }
                    break;
            }



            return (int)_defense;
        }

        //get price
        public int GetPriceToUpgrade(TheEnumManager.ITEM_LEVEL _level)
        {
            if (_level != TheEnumManager.ITEM_LEVEL.level_7)
                return (int)(GetDefense(_level) * TheDataManager.Instance.PRICE_CONFIG.fUnitPriceGem_Defense);
            else return 0;

        }


        public void Init()
        {
            //clone
            DATA = new DeData();
            DATA.eDefense = ORIGINAL_DATA.eDefense;
            DATA.eLevel = ORIGINAL_DATA.eLevel;
            DATA.bUnlocked = ORIGINAL_DATA.bUnlocked;
            DATA.bEquiped = ORIGINAL_DATA.bEquiped;
            DATA.bDefault = ORIGINAL_DATA.bDefault;
            //=======================

            if (TheDataManager.Instance.THE_DATA_PLAYER.GetDefense(DATA.eDefense) != null)
            {
                DATA = TheDataManager.Instance.THE_DATA_PLAYER.GetDefense(DATA.eDefense);
            }
            else
            {
                TheDataManager.Instance.THE_DATA_PLAYER.LIST_DEFENSE.Add(DATA);
                // TheDataManager.Instance.SaveDataPlayer();//save
            }
        }



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
    }
}
