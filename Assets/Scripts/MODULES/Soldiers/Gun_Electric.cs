using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Electric : HandOfWeapon
{
    [SerializeField] GameObject objElectricLine;
    public ElectricLine line_renderer;

    private Vector3 vPosStart, vTargetOfBullet;



    public override void Init()
    {
        objElectricLine = Instantiate(objElectricLine);
        line_renderer = objElectricLine.GetComponent<ElectricLine>();
    }


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
        //============ to reload 

        Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo--;
        TheEventManager.Weapon_OnWeaponShot(Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA);//event
        iAmmoInMagazine--;
        if (iAmmoInMagazine == 0) bLOADING_MAGAZINE = true;

        MainCode_Gameplay.Instance.m_WeaponShell.ShowBar(GetFactorBullet());//show shell


        #region SHOT
        //animation of body
        objBeam.SetActive(true);      
       // Soldier.Instance.PlayAnimator(TheEnumManager.SOLDIER_STATUS.shooting);//soldier shake
        m_animator.Play(aniGunShake.name, -1, 0f);//shake gun
        TheSoundManager.Instance.PlayGunSound(TheEnumManager.WEAPON.stun_gun);//sound
        //--------------------
        vTargetOfBullet = vInputPos + Random.insideUnitCircle * 1.3f;
        vPosStart = objBeam.transform.position;
        vPosStart.z = -40.0f;
        vTargetOfBullet.z = -40.0f;


        line_renderer.Shot(vPosStart,vTargetOfBullet);

        TheEventManager.PostEvent_OnBulletCompleted(TheEnumManager.WEAPON.stun_gun, vTargetOfBullet, fRangeOfGBullet, iDamageOfGun);
        #endregion

        //THAY SUNG
        if (Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo == 0)
            TheEventManager.Weapon_OnWeaponNoBullet(null);//event - thay sung
    }

}
