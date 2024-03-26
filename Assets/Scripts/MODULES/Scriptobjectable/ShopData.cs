using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IAP Data")]
[System.Serializable]
public class ShopData : ScriptableObject
{
    public TheEnumManager.KIND_OF_IAP eKindOfIap;
    public string strProductName;
    public string strIdOfStore;
    public float fPriceDollar;
    public int iValueToAdd;
    public int iPrecentSale;


}
