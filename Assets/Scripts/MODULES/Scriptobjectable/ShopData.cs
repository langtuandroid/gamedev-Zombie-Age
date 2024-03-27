using UnityEngine;

namespace MODULES.Scriptobjectable
{
    [CreateAssetMenu(fileName = "IAP Data")]
    [System.Serializable]
    public class ShopData : ScriptableObject
    {
        public string strProductName;
        public float fPriceDollar;
        public int iValueToAdd;
        public int iPrecentSale;
    }
}
