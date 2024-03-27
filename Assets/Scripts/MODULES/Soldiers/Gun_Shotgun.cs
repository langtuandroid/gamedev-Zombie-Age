using _4_Gameplay;
using MANAGERS;
using MODULES.Soldiers.Bullets;
using UnityEngine;

namespace MODULES.Soldiers
{
    public class Gun_Shotgun : HandOfWeapon
    {
        private GameObject _bullet;
        Vector2 vTargetOfBullet;
        float fAngelZ;

        public override void Shot()
        {
            if (GameplayController.Instance.GameStatus == GameplayController.GAME_STATUS.victory
                || GameplayController.Instance.GameStatus == GameplayController.GAME_STATUS.gameover) return;

            if (Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo <= 0) //het dan
            {
                EventController.OnWeaponNoBulletInvoke(null);//event
                return;
            }
            if (bLOADING_BULLET)
            {
           
                return;
            }
            if (bLOADING_MAGAZINE)
            {
                //audio here
                return;
            }


            bLOADING_BULLET = true;
            //============= to reload 

            if (!Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.bIsDefaultGun)
                Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo--;
            EventController.OnWeaponShotInvoke(Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA);//event
            iAmmoInMagazine--;
            if (iAmmoInMagazine == 0)
            {
                bLOADING_MAGAZINE = true;
            }
            GameplayController.Instance.weaponShell.Show(GetFactorBullet());//show shell



            #region SHOT
            objBeam.SetActive(true);
            m_animator.Play(aniGunShake.name, -1, 0f);//shake gun
       
            //ldier.Instance.PlayAnimator(TheEnumManager.SOLDIER_STATUS.shooting);//soldier shake
            SoundController.Instance.PlayGunSound(EnumController.WEAPON.shotgun);//sound


            vTargetOfBullet = vInputPos + Random.insideUnitCircle * 1.3f;
            fAngelZ = GetRoatateZ(objBeam.transform.position, vTargetOfBullet);

            _bullet = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.bullet_of_shotgun).Get();
            _bullet.GetComponent<Bullet>().SetBullet(EnumController.WEAPON.shotgun, objBeam.transform.position, vTargetOfBullet, fAngelZ, fRangeOfGBullet, iDamageOfGun,sprBullet,vScaleOfBullet);
            _bullet.SetActive(true);

            #endregion

        }

    }
}
