using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheSpriteManager : MonoBehaviour
{
    public static TheSpriteManager Instance;

    #region CLASS SPRITE ITEMS FOR ZOMBIE
    [System.Serializable]
    public class ItemsForZombie
    {

        [System.Serializable]
        public class HatItemForZombie
        {
            public TheEnumManager.HAT_OF_ZOMBIE eHat;
            public Sprite sprSprite;
        }
        [System.Serializable]
        public class WeaponItemForZombie
        {
            public TheEnumManager.WEAPON_OF_ZOMBIE eWeapon;
            public Sprite sprSprite;
        }
        [System.Serializable]
        public class ShieldItemForZombie
        {
            public TheEnumManager.SHIELD_OF_ZOMBIE eShield;
            public Sprite sprSprite;
        }


        public List<HatItemForZombie> LIST_HAT_FOR_ZOMBIE;
        public List<WeaponItemForZombie> LIST_WEAPON_FOR_ZOMBIE;
        public List<ShieldItemForZombie> LIST_SHIELD_FOR_ZOMBIE;


        public HatItemForZombie GetHat(TheEnumManager.HAT_OF_ZOMBIE _hat)
        {
            int _total = LIST_HAT_FOR_ZOMBIE.Count;
            for (int i = 0; i < _total; i++)
            {
                if (LIST_HAT_FOR_ZOMBIE[i].eHat == _hat)
                    return LIST_HAT_FOR_ZOMBIE[i];
            }
            return null;
        }
        public WeaponItemForZombie GetWeapon(TheEnumManager.WEAPON_OF_ZOMBIE _weapon)
        {

            int _total = LIST_WEAPON_FOR_ZOMBIE.Count;
            for (int i = 0; i < _total; i++)
            {
                if (LIST_WEAPON_FOR_ZOMBIE[i].eWeapon == _weapon)
                    return LIST_WEAPON_FOR_ZOMBIE[i];
            }
            return null;
        }
        public ShieldItemForZombie GetShield(TheEnumManager.SHIELD_OF_ZOMBIE _shield)
        {

            int _total = LIST_SHIELD_FOR_ZOMBIE.Count;
            for (int i = 0; i < _total; i++)
            {
                if (LIST_SHIELD_FOR_ZOMBIE[i].eShield == _shield)
                    return LIST_SHIELD_FOR_ZOMBIE[i];
            }
            return null;
        }
    }
    #endregion

    #region SUPPORT
    [System.Serializable]
    public class SpriteOfSupport
    {
        public TheEnumManager.SUPPORT eSupport;
        public Sprite spriSprite;
    }
    public SpriteOfSupport GetSpriteOfSupport(TheEnumManager.SUPPORT _support)
    {
        int _total = SPRITE_OF_SUPPORT.Count;
        for (int i = 0; i < _total; i++)
        {
            if (SPRITE_OF_SUPPORT[i].eSupport == _support)
                return SPRITE_OF_SUPPORT[i];
        }
        return null;
    }
    #endregion



    public ItemsForZombie ITEM_FOR_ZOMBIE;

    [Space(30)]
    public List<SpriteOfSupport> SPRITE_OF_SUPPORT;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

}
