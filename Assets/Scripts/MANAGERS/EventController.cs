using MODULES.Scriptobjectable;
using MODULES.Zombies;
using UnityEngine;

namespace MANAGERS
{
    public class EventController
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


        public static void OnGameWinInvoke()
        {
            if (OnGameWin != null)
                OnGameWin();
        }
        public static void OnGameoverInvoke()
        {
            if (OnGameover != null)
                OnGameover();
        }
        public static void OnStartLevelInvoke()
        {
            if (OnStartLevel != null)
                OnStartLevel();
        }
        public static void OnStartNewWaveInvoke()
        {
            if (OnStartNewWave != null)
                OnStartNewWave();
        }
        public static void OnStartNewSceneInvoke()
        {
            if (OnStartNewScene != null)
                OnStartNewScene();
        }
        public static void OnUpdatedBoardInvoke()
        {
            if (OnUpdatedBoard != null)
                OnUpdatedBoard();
        }
        public static void OnResetMagazinBulletInvoke()
        {
            if (OnResetMagazinBullet != null)
                OnResetMagazinBullet();
        }



        #endregion

        #region EVENT OF PLAYER BULLET
        public delegate void BulletEvent(EnumController.WEAPON eWeapon, Vector2 _pos,float _range, int _damage);

        //Bullet complete
        public static event BulletEvent OnBulletCompleted; // bullet tới điểm player bán
        public static void OnBulletCompletedInvoke(EnumController.WEAPON eWeapon, Vector2 _pos, float _range, int _damage)
        {
            if (OnBulletCompleted != null)
                OnBulletCompleted(eWeapon, _pos, _range, _damage);
        }
        
   
        #endregion

        #region EVENT OF ZOMBIE BULLET
        public delegate void ZombieBulletEvent(EnumController.ZOMBIE _zombie, Vector2 _pos);

        //Bullet complete
        public static event ZombieBulletEvent OnZombieBulletCompleted; // bullet tới điểm player bán
        public static void OnZombieBulletCompletedInvoke(EnumController.ZOMBIE _zombie, Vector2 _pos)
        {
            if (OnZombieBulletCompleted != null)
                OnZombieBulletCompleted(_zombie, _pos);
        }
        #endregion

        #region EVENT OF GUN
        public delegate void WeaponEven(GunData _gundata);

        //Het dan
        public static event WeaponEven OnWeaponNoBullet;
        public static void OnWeaponNoBulletInvoke(GunData _gun)
        {
            if (OnWeaponNoBullet != null)
                OnWeaponNoBullet(_gun);
        }

        //shot
        public static event WeaponEven OnWeaponShot;
        public static void OnWeaponShotInvoke(GunData _gun)
        {
            if (OnWeaponShot != null)
                OnWeaponShot(_gun);
        }

        //Add to list equiped weapon list
        public static event WeaponEven OnAddToEquipedWeaponList;
        public static void OnoEquipedWeaponInvoke(GunData _gun)
        {
            if (OnAddToEquipedWeaponList != null)
                OnAddToEquipedWeaponList(_gun);
        }


        //Remove to list equiped weapon list
        public static event WeaponEven OnRemoveFromEquipedWeaponList;
        public static void OnUnEquipedWeaponInvoke(GunData _gun)
        {
            if (OnRemoveFromEquipedWeaponList != null)
                OnRemoveFromEquipedWeaponList(_gun);
        }


        //CHANGED WEAPON FOR SOLDIER
        public static event WeaponEven OnChangedWeapon;
        public static void OnChangedWeaponInvoke(GunData _gun)
        {
            if (OnChangedWeapon != null)
                OnChangedWeapon(_gun);
        }

        #endregion

        #region EVENT OF DEFENSE
        public delegate void DefenseEven(DefenseData  _defense);
        //Add to list equiped weapon list
        public static event DefenseEven OnAddToEquipedDefenseList;
        public static void OnEquipeDefenseInvoke(DefenseData _defense)
        {
            if (OnAddToEquipedDefenseList != null)
                OnAddToEquipedDefenseList(_defense);
        }


        //Remove to list equiped weapon list
        public static event DefenseEven OnRemoveToEquipedDefenseList;
        public static void OnRemoveDefenseInvoke(DefenseData _defense)
        {
            if (OnRemoveToEquipedDefenseList != null)
                OnRemoveToEquipedDefenseList(_defense);
        }
        #endregion

        #region EVENT OF REWARD
        public delegate void EventReward(EnumController.REWARD _reward);

        //get reward
        public static event EventReward OnGetReward;
        public static void OnGetRewardInvoke(EnumController.REWARD _reward)
        {
            if (OnGetReward != null)
                OnGetReward(_reward);
        }
        #endregion

        #region EVENT OF POPUP
        public delegate void EventOfPopup(UIController.POP_UP _popup);

        //show popup
        public static event EventOfPopup OnShowPopup;
        public static void  OnShowPopupInvoke(UIController.POP_UP _popup)
        {
            if(OnShowPopup!=null)
            {
                OnShowPopup(_popup);
            }
        }

        public static event EventOfPopup OnHidePopup;
        public static void OnHidePopupInvoke(UIController.POP_UP _popup)
        {
            if (OnHidePopup != null)
            {
                OnHidePopup(_popup);
            }
        }



        #endregion

        #region  EVENT OF SUPPORT
        public delegate void SupportEvent(EnumController.SUPPORT _support, Vector2 _pos);

        //Bullet complete
        public static event SupportEvent OnSupportCompleted; // bullet tới điểm player bán
        public static void OnSupportCompletedInvoke(EnumController.SUPPORT _support, Vector2 _pos)
        {
            if (OnSupportCompleted != null)
                OnSupportCompleted(_support, _pos);
        }
        #endregion
    }
}
