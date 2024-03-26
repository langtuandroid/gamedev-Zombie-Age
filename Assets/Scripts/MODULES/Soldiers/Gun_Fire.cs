using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Fire : HandOfWeapon
{

    private GameObject _bullet;
    Vector2 vTargetOfBullet;

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
        Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo--;
        iAmmoInMagazine--;
        if (iAmmoInMagazine == 0) bLOADING_MAGAZINE = true;



        TheEventManager.Weapon_OnWeaponShot(Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA);//event      
        MainCode_Gameplay.Instance.m_WeaponShell.ShowBar(GetFactorBullet());//show shell




        #region  SHOT      
        //animation of body
        objBeam.SetActive(true);
       
       // Soldier.Instance.PlayAnimator(TheEnumManager.SOLDIER_STATUS.shooting);//soldier shake
        TheSoundManager.Instance.PlayGunSound( TheEnumManager.WEAPON.firegun);//sound

       
        m_animator.Play(0);//shake gun
      
        //bullet
        vTargetOfBullet = vInputPos + Random.insideUnitCircle * 1.3f;
        _bullet = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.bullet_fire).GetObject();
        _bullet.GetComponent<Bullet>().SetBullet( TheEnumManager.WEAPON.firegun,objBeam.transform.position, vTargetOfBullet, 0.0f, fRangeOfGBullet, iDamageOfGun);
        _bullet.SetActive(true);

        #endregion
        
        //THAY SUNG
        if (Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo == 0)
            TheEventManager.Weapon_OnWeaponNoBullet(null);//event - thay sung

    }
}
