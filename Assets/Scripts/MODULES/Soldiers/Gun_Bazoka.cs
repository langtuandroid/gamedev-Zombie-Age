using System.Collections.Generic;
using _4_Gameplay;
using MANAGERS;
using MODULES.Soldiers.Bullets;
using UnityEngine;

namespace MODULES.Soldiers
{
    public class Gun_Bazoka : HandOfWeapon
    {
        private GameObject objBullet;
        public Transform GROUP_PATH_FAR, GROUP_PATH_NEAR, GROUP_PATH_CLOSER;
        private List<Vector2> LIST_PATH = new List<Vector2>();
        private int iTotalFar, iTotalNear, iTotalCloser;

        public override void Init()
        {
            iTotalFar = GROUP_PATH_FAR.childCount;
            iTotalNear = GROUP_PATH_NEAR.childCount;
            iTotalCloser = GROUP_PATH_CLOSER.childCount;
        }

        private void UpdateRoad(Vector2 _target)
        {
            LIST_PATH.Clear();
            if (_target.x > 5.7f)
            {
                for (int i = 0; i < iTotalFar; i++)
                {
                    LIST_PATH.Add(GROUP_PATH_FAR.GetChild(i).position);
                }
                LIST_PATH[iTotalFar - 1] = _target;
            }
            else if (_target.x < 0.5f)
            {
                for (int i = 0; i < iTotalCloser; i++)
                {
                    LIST_PATH.Add(GROUP_PATH_CLOSER.GetChild(i).position);
                }
                LIST_PATH[iTotalCloser - 1] = _target;
            }
            else
            {
                for (int i = 0; i < iTotalNear; i++)
                {
                    LIST_PATH.Add(GROUP_PATH_NEAR.GetChild(i).position);
                }
                LIST_PATH[iTotalNear - 1] = _target;
            }



        }


        public override void Shot()
        {
            Invoke("Deshot", 0.05f);
        }

        private void Deshot()
        {
            if (GameplayController.Instance.GameStatus != GameplayController.GAME_STATUS.playing) return;
            if (Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo <= 0)
            {
                TheEventManager.Weapon_OnWeaponNoBullet(null);//event - thay sung
                return;
            }
            if (bLOADING_BULLET) return;
            if (bLOADING_MAGAZINE) return;


            bLOADING_BULLET = true;
            //=== to reload

            Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo--;

            TheEventManager.Weapon_OnWeaponShot(Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA);//event
            iAmmoInMagazine--;
            if (iAmmoInMagazine == 0 ) bLOADING_MAGAZINE = true;
       

            GameplayController.Instance.weaponShell.Show(GetFactorBullet());//show shell

            //animation of body
            objBeam.SetActive(true);
       
            // Soldier.Instance.PlayAnimator(TheEnumManager.SOLDIER_STATUS.shooting);//soldier shake

            TheSoundManager.Instance.PlayGunSound( TheEnumManager.WEAPON.bazoka);//sound




            #region SHOT
            m_animator.Play(aniGunShake.name, -1, 0f);//shake gun 
            UpdateRoad(vInputPos);


            //EFFECT---------------
            objBullet = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.bullet_of_bazoka).GetObject();
            objBullet.GetComponent<BulletBazoka>().Setup(LIST_PATH, fRangeOfGBullet, iDamageOfGun);
            objBullet.transform.position = LIST_PATH[0];
            objBullet.transform.eulerAngles = m_tranform.eulerAngles;
            objBullet.SetActive(true);
            #endregion

            //THAY SUNG
            if (Soldier.Instance.WEAPON_MANAGER.CURRENT_GUN_DATA.DATA.iCurrentAmmo == 0)
                TheEventManager.Weapon_OnWeaponNoBullet(null);//event - thay sung

        }
    }
}
