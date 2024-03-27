using MANAGERS;
using UnityEngine;

namespace MODULES.Scriptobjectable
{
    [CreateAssetMenu(fileName = "Support Data")]
    [System.Serializable]
    public class SupportData : ScriptableObject
    {
        [System.Serializable]
        public class SuData
        {
            public TheEnumManager.SUPPORT _support;
            public int iCurrentValue;
        }



        [SerializeField]
        private SuData ORIGINAL_DATA;
        private SuData m_data;//data to save
        public SuData DATA
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


        public string strName;
        public string strContent;
        public int iPrice;
        public Sprite sprIcon;
        public int iMaxValue;//số lượng tối đa có thể mua.

        [Space(20)]
        public float fEffectTime; // thoi gian anh huong
        public float fRange; // pham vi

        [Tooltip("Poison: 10% tổng máu của zombie")]
        public int iDamage; // sat thuong: poision = 10% tổng máu của zombie


        //Init
        public void Init()
        {
            DATA = new SuData() ;
            DATA._support = ORIGINAL_DATA._support;
            DATA.iCurrentValue = ORIGINAL_DATA.iCurrentValue;


            if (TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(DATA._support) != null)
                DATA = TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(DATA._support);
            else
            {

                TheDataManager.Instance.THE_DATA_PLAYER.LIST_SUPPORT.Add(DATA);
                //TheDataManager.Instance.SaveDataPlayer();//save
            }
        }


        //time
        public float GetTime()
        {
            switch (DATA._support)
            {
                //freeze
                case TheEnumManager.SUPPORT.freeze:
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.freeze_time50).bEQUIPED) return fEffectTime * 1.5f;
                    break;

                //poison
                case TheEnumManager.SUPPORT.poison:
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.poision_time25).bEQUIPED) return fEffectTime * 1.25f;
                    break;
            }
            return fEffectTime;
        }


        //range
        public float GetRange()
        {

            switch (DATA._support)
            {
                case TheEnumManager.SUPPORT.grenade:
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.grenade_range25).bEQUIPED) return fRange * 1.25f;
                    break;
                case TheEnumManager.SUPPORT.freeze:
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.freeze_range25).bEQUIPED) return fRange * 1.25f;
                    break;
                case TheEnumManager.SUPPORT.poison:
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.posion_range25).bEQUIPED) return fRange * 1.25f;
                    break;
                case TheEnumManager.SUPPORT.big_bomb:
                    break;
            }
            return fRange;
        }


        //damage
        public int GetDamage()
        {
            switch (DATA._support)
            {
                case TheEnumManager.SUPPORT.grenade:
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.grenade_damage25).bEQUIPED) return (int)(iDamage * 1.25f);
                    break;
                case TheEnumManager.SUPPORT.freeze:
                    break;
                case TheEnumManager.SUPPORT.poison:
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.posion_damage50).bEQUIPED) return (int)(iDamage * 1.5f);
                    break;
                case TheEnumManager.SUPPORT.big_bomb:
                    if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.bigbom_damage25).bEQUIPED) return (int)(iDamage * 1.25f);
                    break;
            }
            return iDamage;
        }

    }
}
