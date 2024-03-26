using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Level data config")]
[System.Serializable]
public class LevelData : ScriptableObject
{



    [System.Serializable]
    public class Zombie
    {
        public TheEnumManager.ZOMBIE ZOMBIE;

        [Space(20)]
        public TheEnumManager.HAT_OF_ZOMBIE eHat;


       
        [Tooltip("Tỉ lệ đội mũ")]
        [Range(0,1)]
        public float fPercent_HasHat;



        [Space(10)]
        public TheEnumManager.WEAPON_OF_ZOMBIE eWeapon;
        [Tooltip("Tỉ lệ cầm vũ khí")]
        [Range(0, 1)]
        public float fPercent_HasWeapon;



        [Space(10)]
        public TheEnumManager.SHIELD_OF_ZOMBIE eShield;
        [Tooltip("Tỉ lệ có khiêng")]
        [Range(0, 1)]
        public float fPercent_HasShield;

        //======================================
        //Xem xem có đủ điều kiện để có mũ hay không
        float _randomValue = 0;
        public bool IsHat()
        {
            if (eHat == TheEnumManager.HAT_OF_ZOMBIE.NO_HAT) return false;
            if (fPercent_HasHat ==1.0f) return true;
            _randomValue = Random.Range(0, 1.0f);
            if (_randomValue <= fPercent_HasHat) return true;
            else return false;

        }
        public bool IsWeapon()
        {
            if (eWeapon== TheEnumManager.WEAPON_OF_ZOMBIE.NO_WEAPON) return false;
            if (fPercent_HasWeapon == 1.0f) return true;
            _randomValue = Random.Range(0, 1.0f);
            if (_randomValue <= fPercent_HasWeapon) return true;
            else return false;

        }
        public bool IsShield()
        {
            if (eShield == TheEnumManager.SHIELD_OF_ZOMBIE.NO_SHIELD) return false;
            if (fPercent_HasShield == 1.0f) return true;
            _randomValue = Random.Range(0, 1.0f);
            if (_randomValue <= fPercent_HasShield) return true;
            else return false;

        }



        [Header("-----------------------------------")]
        [Space(60)]
        public char  bSpace;
    }

    [Header("Background config")]
    public Sprite sprBackgroundFrame;
    public Sprite sprBackground;



    [Header("TOTAL WAVE")]
    public int iTotalWave;

    [Header("BOSS")]
    [Space(30)]
    public GameObject prefabBoss;

    [Header("CONFIF ZOMBIE")]
    [Space(30)]
    public List<Zombie> LIST_ZOMBIE_IN_LEVEL;


    [Header("TỈ LỆ  ZOMBIE Ở TRẠNG THÁI ĐẶC BIỆT")]
    [Range(0,100)]
    [Space(20)]
    public float iConfigSpecialStatus;


    public Zombie GetZombie(TheEnumManager.ZOMBIE _zombie)
    {
        int length = LIST_ZOMBIE_IN_LEVEL.Count;
        for (int i = 0; i < length; i++)
        {
            if (LIST_ZOMBIE_IN_LEVEL[i].ZOMBIE == _zombie) return LIST_ZOMBIE_IN_LEVEL[i];
        }
        return null;
    }
    
   
}
