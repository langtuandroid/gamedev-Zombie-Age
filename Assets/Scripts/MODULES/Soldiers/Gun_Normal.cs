using _4_Gameplay;
using MANAGERS;
using MODULES.Soldiers.Bullets;
using UnityEngine;

namespace MODULES.Soldiers
{
    public class Gun_Normal : HandOfWeapon
    {
        private GameObject _bullet;
        Vector2 vTargetOfBullet;
        float fAngelZ;

        public override void Shot()
        {
            if (MainCode_Gameplay.Instance.eGameStatus != MainCode_Gameplay.GAME_STATUS.playing) return;
            if (Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo <= 0)
            {
                TheEventManager.Weapon_OnWeaponNoBullet(null);//event - thay sung
                return;
            }
            if (bLOADING_BULLET) return;
            if (bLOADING_MAGAZINE) return;

            bLOADING_BULLET = true;

            //------------- to reload
            Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo--;
            iAmmoInMagazine--;
            if (iAmmoInMagazine == 0) bLOADING_MAGAZINE = true;

            TheEventManager.Weapon_OnWeaponShot(Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA);//event     
            MainCode_Gameplay.Instance.m_WeaponShell.ShowBar(GetFactorBullet());//show shell



            #region SHOT  
            objBeam.SetActive(true);
       
            //Soldier.Instance.PlayAnimator(TheEnumManager.SOLDIER_STATUS.shooting);//soldier shake
            m_animator.Play(aniGunShake.name, -1, 0f);//shake gun
            TheSoundManager.Instance.PlayGunSound(Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.eWeapon);//sound

            //bullet
            vTargetOfBullet = vInputPos + Random.insideUnitCircle * 1.3f;
            fAngelZ = GetRoatateZ(objBeam.transform.position, vTargetOfBullet);

            _bullet = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.bullet).GetObject();
            _bullet.GetComponent<Bullet>().SetBullet(Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.eWeapon, objBeam.transform.position, vTargetOfBullet, fAngelZ, fRangeOfGBullet, iDamageOfGun, sprBullet, vScaleOfBullet);
            _bullet.SetActive(true);
            #endregion

            //THAY SUNG
            if (Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo == 0)
                TheEventManager.Weapon_OnWeaponNoBullet(null);//event - thay sung
        }

    }
}
