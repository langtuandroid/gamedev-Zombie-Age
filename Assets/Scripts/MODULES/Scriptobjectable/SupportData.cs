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
            public EnumController.SUPPORT _support;
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


            if (DataController.Instance.playerData.TakeSupport(DATA._support) != null)
                DATA = DataController.Instance.playerData.TakeSupport(DATA._support);
            else
            {

                DataController.Instance.playerData._supportList.Add(DATA);
                //TheDataManager.Instance.SaveDataPlayer();//save
            }
        }


        //time
        public float GetTime()
        {
            switch (DATA._support)
            {
                //freeze
                case EnumController.SUPPORT.freeze:
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.freeze_time50).bEQUIPED) return fEffectTime * 1.5f;
                    break;

                //poison
                case EnumController.SUPPORT.poison:
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.poision_time25).bEQUIPED) return fEffectTime * 1.25f;
                    break;
            }
            return fEffectTime;
        }


        //range
        public float GetRange()
        {

            switch (DATA._support)
            {
                case EnumController.SUPPORT.grenade:
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.grenade_range25).bEQUIPED) return fRange * 1.25f;
                    break;
                case EnumController.SUPPORT.freeze:
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.freeze_range25).bEQUIPED) return fRange * 1.25f;
                    break;
                case EnumController.SUPPORT.poison:
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.posion_range25).bEQUIPED) return fRange * 1.25f;
                    break;
                case EnumController.SUPPORT.big_bomb:
                    break;
            }
            return fRange;
        }


        //damage
        public int GetDamage()
        {
            switch (DATA._support)
            {
                case EnumController.SUPPORT.grenade:
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.grenade_damage25).bEQUIPED) return (int)(iDamage * 1.25f);
                    break;
                case EnumController.SUPPORT.freeze:
                    break;
                case EnumController.SUPPORT.poison:
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.posion_damage50).bEQUIPED) return (int)(iDamage * 1.5f);
                    break;
                case EnumController.SUPPORT.big_bomb:
                    if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.bigbom_damage25).bEQUIPED) return (int)(iDamage * 1.25f);
                    break;
            }
            return iDamage;
        }

    }
}
