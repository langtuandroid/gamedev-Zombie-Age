using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MANAGERS
{
    public class SpriteController : MonoBehaviour
    {
        #region CLASS SPRITE ITEMS FOR ZOMBIE
        [System.Serializable]
        public class ItemsForZombie
        {

            [System.Serializable]
            public class HatZombieItem
            {
                [FormerlySerializedAs("eHat")] public EnumController.HAT_OF_ZOMBIE _hatType;
                [FormerlySerializedAs("sprSprite")] public Sprite _sprite;
            }
            [System.Serializable]
            public class WeaponZombieItem
            {
                [FormerlySerializedAs("eWeapon")] public EnumController.WEAPON_OF_ZOMBIE _zomieWeaponType;
                [FormerlySerializedAs("sprSprite")] public Sprite _sprite;
            }
            [System.Serializable]
            public class ShieldZombieItem
            {
                [FormerlySerializedAs("eShield")] public EnumController.SHIELD_OF_ZOMBIE _sombieShield;
                [FormerlySerializedAs("sprSprite")] public Sprite _sprite;
            }
            
            [FormerlySerializedAs("LIST_HAT_FOR_ZOMBIE")] public List<HatZombieItem> _hatList;
            [FormerlySerializedAs("LIST_WEAPON_FOR_ZOMBIE")] public List<WeaponZombieItem> _weaponList;
            [FormerlySerializedAs("LIST_SHIELD_FOR_ZOMBIE")] public List<ShieldZombieItem> _shieldList;
            
            public HatZombieItem TakeHat(EnumController.HAT_OF_ZOMBIE _hat)
            {
                int _total = _hatList.Count;
                for (int i = 0; i < _total; i++)
                {
                    if (_hatList[i]._hatType == _hat)
                        return _hatList[i];
                }
                return null;
            }
            public WeaponZombieItem TakeWeapon(EnumController.WEAPON_OF_ZOMBIE _weapon)
            {

                int _total = _weaponList.Count;
                for (int i = 0; i < _total; i++)
                {
                    if (_weaponList[i]._zomieWeaponType == _weapon)
                        return _weaponList[i];
                }
                return null;
            }
            public ShieldZombieItem TakeShield(EnumController.SHIELD_OF_ZOMBIE _shield)
            {

                int _total = _shieldList.Count;
                for (int i = 0; i < _total; i++)
                {
                    if (_shieldList[i]._sombieShield == _shield)
                        return _shieldList[i];
                }
                return null;
            }
        }
        #endregion

        #region SUPPORT
        [System.Serializable]
        public class SpriteOfSupport
        {
            [FormerlySerializedAs("eSupport")] public EnumController.SUPPORT _supportType;
            [FormerlySerializedAs("spriSprite")] public Sprite _sprite;
        }
        public SpriteOfSupport GetSupportSprite(EnumController.SUPPORT _support)
        {
            int _total = _supporSprites.Count;
            for (int i = 0; i < _total; i++)
            {
                if (_supporSprites[i]._supportType == _support)
                    return _supporSprites[i];
            }
            return null;
        }
        #endregion



        [FormerlySerializedAs("ITEM_FOR_ZOMBIE")] public ItemsForZombie _zombieItem;

        [Space(30)]
        [FormerlySerializedAs("SPRITE_OF_SUPPORT")] public List<SpriteOfSupport> _supporSprites;

    }
}
