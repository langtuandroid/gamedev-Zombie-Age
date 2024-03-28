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
            public EnumController.DEFENSE eDefense;
            public EnumController.ITEM_LEVEL eLevel;
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
                        case EnumController.DEFENSE.home:
                            break;
                        case EnumController.DEFENSE.metal:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_defense_metal);
                            break;
                        case EnumController.DEFENSE.thorn:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_defense_thorn);
                            break;

                    }
                    UIController.Instance.PopUpShow(UIController.POP_UP.reward);
                    WinReward.LoadRevardReward(_reward);
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
        public int GetDefense(EnumController.ITEM_LEVEL _level)
        {
            //Cong thuc: 6x+iBaseDefese
            //link do thi: https://www.desmos.com/calculator/wvnakbcsch
            float _defense = 0;
       

            switch (DATA.eDefense)
            {
                case EnumController.DEFENSE.home:
                    _defense = (20 * (int)_level) + iBaseDefense;
               
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.home_defense15).bEQUIPED)
                    {
                        _defense = _defense * 1.15f;
                    }
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.home_defense25).bEQUIPED)
                    {
                        _defense = _defense * 1.25f;
                    }
                    break;


                case EnumController.DEFENSE.metal:
                    _defense = 35 * (int)_level + iBaseDefense;
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.metal_defense15).bEQUIPED)
                    {
                        _defense = _defense * 1.15f;
                    }
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.metal_defense25).bEQUIPED)
                    {
                        _defense = _defense * 1.25f;
                    }
                    break;


                case EnumController.DEFENSE.thorn:
                    _defense = 30 * (int)_level + iBaseDefense;
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.thorn_defense15).bEQUIPED)
                    {
                        _defense = _defense * 1.15f;
                    }
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.thorn_defense25).bEQUIPED)
                    {
                        _defense = _defense * 1.25f;
                    }
                    break;
            }



            return (int)_defense;
        }

        //get price
        public int GetPriceToUpgrade(EnumController.ITEM_LEVEL _level)
        {
            if (_level != EnumController.ITEM_LEVEL.level_7)
                return (int)(GetDefense(_level) * DataController.Instance.PriceData.fUnitPriceGem_Defense);
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

            if (DataController.Instance.playerData.TakeDefense(DATA.eDefense) != null)
            {
                DATA = DataController.Instance.playerData.TakeDefense(DATA.eDefense);
            }
            else
            {
                DataController.Instance.playerData._defenseList.Add(DATA);
                // TheDataManager.Instance.SaveDataPlayer();//save
            }
        }



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
    }
}
