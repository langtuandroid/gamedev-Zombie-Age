using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEventManager
{
    #region EVENT OF ZOMBIE
    public delegate void ZombieEvent(Zombie _zombie);
    public delegate void ZombieAttack(float  _damage);
    //die
    public static event ZombieEvent OnZombieDie;
    public static void ZombieEvent_Die(Zombie _zombie)
    {
        if (OnZombieDie != null)
            OnZombieDie(_zombie);
    }

    //born
    public static event ZombieEvent OnZombieBorn;
    public static void ZombieEvent_Born(Zombie _zombie)
    {
        if (OnZombieBorn != null)
            OnZombieBorn(_zombie);
    }

    //Zombie attack
    public static event ZombieAttack OnZombieAttack;
    public static void ZombieEvent_OnZombieAttack(float  _dama)
    {
        if (OnZombieAttack != null)
            OnZombieAttack(_dama);
    }

    #endregion

    #region EVNET OF GAME
    public delegate void GameEvent();

    //win
    public static event GameEvent OnGameWin;
    public static event GameEvent OnGameover;
    public static event GameEvent OnStartLevel;
    public static event GameEvent OnStartNewWave;
    public static event GameEvent OnStartNewScene;
    public static event GameEvent OnUpdatedBoard;
    public static event GameEvent OnResetMagazinBullet;
    public static event GameEvent OnAdsOpening;
    public static event GameEvent OnAdsClosed;


    public static void PostEvent_OnGameWin()
    {
        if (OnGameWin != null)
            OnGameWin();
    }
    public static void PostEvent_OnGameover()
    {
        if (OnGameover != null)
            OnGameover();
    }
    public static void PostEvent_OnStartLevel()
    {
        if (OnStartLevel != null)
            OnStartLevel();
    }
    public static void PostEvent_OnStartNewWave()
    {
        if (OnStartNewWave != null)
            OnStartNewWave();
    }
    public static void PostEvent_OnStartNewScene()
    {
        if (OnStartNewScene != null)
            OnStartNewScene();
    }
    public static void PostEvent_OnUpdatedBoard()
    {
        if (OnUpdatedBoard != null)
            OnUpdatedBoard();
    }
    public static void PostEvent_OnResetMagazinBullet()
    {
        if (OnResetMagazinBullet != null)
            OnResetMagazinBullet();
    }
    public static void PostEvent_OnAdsOpening()
    {
        if (OnAdsOpening != null)
            OnAdsOpening();
    }
    public static void PostEvent_OnAdsClosed()
    {
        if (OnAdsClosed != null)
            OnAdsClosed();
    }


    #endregion

    #region EVENT OF PLAYER BULLET
    public delegate void BulletEvent(TheEnumManager.WEAPON eWeapon, Vector2 _pos,float _range, int _damage);

    //Bullet complete
    public static event BulletEvent OnBulletCompleted; // bullet tới điểm player bán
    public static void PostEvent_OnBulletCompleted(TheEnumManager.WEAPON eWeapon, Vector2 _pos, float _range, int _damage)
    {
        if (OnBulletCompleted != null)
            OnBulletCompleted(eWeapon, _pos, _range, _damage);
    }



   
    #endregion

    #region EVENT OF ZOMBIE BULLET
    public delegate void ZombieBulletEvent(TheEnumManager.ZOMBIE _zombie, Vector2 _pos);

    //Bullet complete
    public static event ZombieBulletEvent OnZombieBulletCompleted; // bullet tới điểm player bán
    public static void PostEvent_OnZombieBulletCompleted(TheEnumManager.ZOMBIE _zombie, Vector2 _pos)
    {
        if (OnZombieBulletCompleted != null)
            OnZombieBulletCompleted(_zombie, _pos);
    }
    #endregion

    #region EVENT OF GUN
    public delegate void WeaponEven(GunData _gundata);

    //Het dan
    public static event WeaponEven OnWeaponNoBullet;
    public static void Weapon_OnWeaponNoBullet(GunData _gun)
    {
        if (OnWeaponNoBullet != null)
            OnWeaponNoBullet(_gun);
    }

    //shot
    public static event WeaponEven OnWeaponShot;
    public static void Weapon_OnWeaponShot(GunData _gun)
    {
        if (OnWeaponShot != null)
            OnWeaponShot(_gun);
    }

    //Add to list equiped weapon list
    public static event WeaponEven OnAddToEquipedWeaponList;
    public static void Weapon_OnAddToEquipedWeaponList(GunData _gun)
    {
        if (OnAddToEquipedWeaponList != null)
            OnAddToEquipedWeaponList(_gun);
    }


    //Remove to list equiped weapon list
    public static event WeaponEven OnRemoveFromEquipedWeaponList;
    public static void Weapon_OnRemoveFromEquipedWeaponList(GunData _gun)
    {
        if (OnRemoveFromEquipedWeaponList != null)
            OnRemoveFromEquipedWeaponList(_gun);
    }


    //CHANGED WEAPON FOR SOLDIER
    public static event WeaponEven OnChangedWeapon;
    public static void Weapon_OnChangedWeapon(GunData _gun)
    {
        if (OnChangedWeapon != null)
            OnChangedWeapon(_gun);
    }

    #endregion

    #region EVENT OF DEFENSE
    public delegate void DefenseEven(DefenseData  _defense);
    //Add to list equiped weapon list
    public static event DefenseEven OnAddToEquipedDefenseList;
    public static void Defense_OnAddToEquipedDefenseList(DefenseData _defense)
    {
        if (OnAddToEquipedDefenseList != null)
            OnAddToEquipedDefenseList(_defense);
    }


    //Remove to list equiped weapon list
    public static event DefenseEven OnRemoveToEquipedDefenseList;
    public static void Defense_OnRemoveToEquipedDefenseList(DefenseData _defense)
    {
        if (OnRemoveToEquipedDefenseList != null)
            OnRemoveToEquipedDefenseList(_defense);
    }
    #endregion

    #region EVENT OF REWARD
    public delegate void EventReward(TheEnumManager.REWARD _reward);

    //get reward
    public static event EventReward OnGetReward;
    public static void PostEvent_OnGetReward(TheEnumManager.REWARD _reward)
    {
        if (OnGetReward != null)
            OnGetReward(_reward);
    }
    #endregion

    #region EVENT OF POPUP
    public delegate void EventOfPopup(TheUiManager.POP_UP _popup);

    //show popup
    public static event EventOfPopup OnShowPopup;
    public static void  PostEvent_OnShowPopup(TheUiManager.POP_UP _popup)
    {
        if(OnShowPopup!=null)
        {
            OnShowPopup(_popup);
        }
    }

    public static event EventOfPopup OnHidePopup;
    public static void PostEvent_OnHidePopup(TheUiManager.POP_UP _popup)
    {
        if (OnHidePopup != null)
        {
            OnHidePopup(_popup);
        }
    }



    #endregion

    #region  EVENT OF SUPPORT
    public delegate void SupportEvent(TheEnumManager.SUPPORT _support, Vector2 _pos);

    //Bullet complete
    public static event SupportEvent OnSupportCompleted; // bullet tới điểm player bán
    public static void PostEvent_OnSupportCompleted(TheEnumManager.SUPPORT _support, Vector2 _pos)
    {
        if (OnSupportCompleted != null)
            OnSupportCompleted(_support, _pos);
    }
    #endregion
}
