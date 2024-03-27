using UnityEngine;

namespace MODULES.Scriptobjectable
{
    [CreateAssetMenu(fileName ="Unit price")]
    public class PriceUnitConfig : ScriptableObject
    {
        [Header("Price cho weapon, trên 1 đơn vị sát thương")]
        public float fUnitPriceGem_Damage; // for weapon


        [Header("Price cho weapon, trên 1 đơn vị defense")]
        [Space(30)]
        public float fUnitPriceGem_Defense; //defense
    }
}
