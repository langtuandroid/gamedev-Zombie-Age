using _4_Gameplay;
using MANAGERS;
using MODULES.Soldiers.Bullets;
using UnityEngine;

namespace MODULES.Soldiers
{
    public class Gun_Fire : HandOfWeapon
    {

        private GameObject _bullet;
        Vector2 vTargetOfBullet;

        public override void Shot()
        {
            if (GameplayController.Instance.GameStatus != GameplayController.GAME_STATUS.playing) return;
            if (Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo <= 0)
            {
                EventController.OnWeaponNoBulletInvoke(null);//event - thay sung
                return;
            }
            if (bLOADING_BULLET) return;
            if (bLOADING_MAGAZINE) return;


            bLOADING_BULLET = true;
            Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo--;
            iAmmoInMagazine--;
            if (iAmmoInMagazine == 0) bLOADING_MAGAZINE = true;



            EventController.OnWeaponShotInvoke(Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA);//event      
            GameplayController.Instance.weaponShell.Show(GetFactorBullet());//show shell




            #region  SHOT      
            //animation of body
            objBeam.SetActive(true);
       
            // Soldier.Instance.PlayAnimator(TheEnumManager.SOLDIER_STATUS.shooting);//soldier shake
            SoundController.Instance.PlayGunSound( EnumController.WEAPON.firegun);//sound

       
            m_animator.Play(0);//shake gun
      
            //bullet
            vTargetOfBullet = vInputPos + Random.insideUnitCircle * 1.3f;
            _bullet = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.bullet_fire).Get();
            _bullet.GetComponent<Bullet>().SetBullet( EnumController.WEAPON.firegun,objBeam.transform.position, vTargetOfBullet, 0.0f, fRangeOfGBullet, iDamageOfGun);
            _bullet.SetActive(true);

            #endregion
        
            //THAY SUNG
            if (Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo == 0)
                EventController.OnWeaponNoBulletInvoke(null);//event - thay sung

        }
    }
}
