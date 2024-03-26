using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade data")]
[System.Serializable]
public class UpgradeData : ScriptableObject
{
    [System.Serializable]
    public class Updata
    {
        public TheEnumManager.KIND_OF_UPGRADE eUpgrade; //save
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
            return TheDataManager.Instance.THE_DATA_PLAYER.GetUpgarde(DATA.eUpgrade).bEquiped;
        }
        set
        {
            TheDataManager.Instance.THE_DATA_PLAYER.GetUpgarde(DATA.eUpgrade).bEquiped = value;
        }
    }


    public void Init()
    {
        DATA = new Updata();
        DATA.eUpgrade = ORIGINAL_DATA.eUpgrade;
        DATA.bEquiped = ORIGINAL_DATA.bEquiped;

        if (TheDataManager.Instance.THE_DATA_PLAYER.GetUpgarde((DATA.eUpgrade)) != null)
        {
            DATA.bEquiped = TheDataManager.Instance.THE_DATA_PLAYER.GetUpgarde(DATA.eUpgrade).bEquiped;
        }
        else
        {
            TheDataManager.Instance.THE_DATA_PLAYER.LIST_UPGRADE.Add(DATA);
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
