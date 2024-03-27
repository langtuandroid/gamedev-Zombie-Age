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
            if (MainCode_Gameplay.Instance.eGameStatus == MainCode_Gameplay.GAME_STATUS.victory
                || MainCode_Gameplay.Instance.eGameStatus == MainCode_Gameplay.GAME_STATUS.gameover) return;

            if (Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo <= 0) //het dan
            {
                TheEventManager.Weapon_OnWeaponNoBullet(null);//event
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
            TheEventManager.Weapon_OnWeaponShot(Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA);//event
            iAmmoInMagazine--;
            if (iAmmoInMagazine == 0)
            {
                bLOADING_MAGAZINE = true;
            }
            MainCode_Gameplay.Instance.m_WeaponShell.ShowBar(GetFactorBullet());//show shell



            #region SHOT
            objBeam.SetActive(true);
            m_animator.Play(aniGunShake.name, -1, 0f);//shake gun
       
            //ldier.Instance.PlayAnimator(TheEnumManager.SOLDIER_STATUS.shooting);//soldier shake
            TheSoundManager.Instance.PlayGunSound(TheEnumManager.WEAPON.shotgun);//sound


            vTargetOfBullet = vInputPos + Random.insideUnitCircle * 1.3f;
            fAngelZ = GetRoatateZ(objBeam.transform.position, vTargetOfBullet);

            _bullet = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.bullet_of_shotgun).GetObject();
            _bullet.GetComponent<Bullet>().SetBullet(TheEnumManager.WEAPON.shotgun, objBeam.transform.position, vTargetOfBullet, fAngelZ, fRangeOfGBullet, iDamageOfGun,sprBullet,vScaleOfBullet);
            _bullet.SetActive(true);

            #endregion

        }

    }
}
