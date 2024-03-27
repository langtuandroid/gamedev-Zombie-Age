using MANAGERS;
using UnityEngine;

namespace MODULES.Scriptobjectable
{
    [CreateAssetMenu(fileName = "Upgrade data")]
    [System.Serializable]
    public class UpgradeData : ScriptableObject
    {
        [System.Serializable]
        public class Updata
        {
            public EnumController.UpgradeType eUpgrade; //save
            public bool bEquiped; //save
        }

        public Updata ORIGINAL_DATA;
        private Updata m_Data; //data to save
        public Updata DATA
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


   
        public int iStar;

        [Space(20)]
        public string strName;
        public string strContent;

        [Space(20)]
        public Sprite sprIcon;
        public Sprite sprIcon_gray;

    
        public bool bEQUIPED
        {
            get
            {
                return DataController.Instance.playerData.TakeUpgarde(DATA.eUpgrade).bEquiped;
            }
            set
            {
                DataController.Instance.playerData.TakeUpgarde(DATA.eUpgrade).bEquiped = value;
            }
        }


        public void Init()
        {
            DATA = new Updata();
            DATA.eUpgrade = ORIGINAL_DATA.eUpgrade;
            DATA.bEquiped = ORIGINAL_DATA.bEquiped;

            if (DataController.Instance.playerData.TakeUpgarde((DATA.eUpgrade)) != null)
            {
                DATA.bEquiped = DataController.Instance.playerData.TakeUpgarde(DATA.eUpgrade).bEquiped;
            }
            else
            {
                DataController.Instance.playerData._upgradeList.Add(DATA);
            }
        }


        public void Upgrade()
        {
            bEQUIPED = true ;
        }

        public void Remove()
        {
            bEQUIPED = false;
        }
    }
}
