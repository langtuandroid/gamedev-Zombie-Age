using _4_Gameplay;
using MANAGERS;
using MODULES.Soldiers.Bullets;
using UnityEngine;

namespace MODULES.Soldiers
{
    public class Shotgun : HandWeapon
    {
        private GameObject _bulletPrefab;
        private Vector2 _targetOfBullet;
        private float _angel;

        protected override void Shoot()
        {
            if (GameplayController.Instance.GameStatus == GameplayController.GAME_STATUS.victory
                || GameplayController.Instance.GameStatus == GameplayController.GAME_STATUS.gameover) return;

            if (Soldier.Instance._weaponManager._gunData.DATA.iCurrentAmmo <= 0) //het dan
            {
                EventController.OnWeaponNoBulletInvoke(null);//event
                return;
            }
            if (IsLoadingBullet)
            {
           
                return;
            }
            if (IsLoadingMagazine)
            {
                return;
            }
            
            IsLoadingBullet = true;

            if (!Soldier.Instance._weaponManager._gunData.DATA.bIsDefaultGun)
                Soldier.Instance._weaponManager._gunData.DATA.iCurrentAmmo--;
            EventController.OnWeaponShotInvoke(Soldier.Instance._weaponManager._gunData);//event
            _ammoInMagazine--;
            if (_ammoInMagazine == 0)
            {
                IsLoadingMagazine = true;
            }
            GameplayController.Instance.weaponShell.Show(GetFactorBullet());//show shell



            #region SHOT
            _beam.SetActive(true);
            _animator.Play(_gunShakeAnimation.name, -1, 0f);//shake gun
            
            SoundController.PlayGunSound(EnumController.WEAPON.shotgun);//sound


            _targetOfBullet = _inputPosition + Random.insideUnitCircle * 1.3f;
            _angel = GetRoatateZ(_beam.transform.position, _targetOfBullet);

            _bulletPrefab = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.bullet_of_shotgun).Get();
            _bulletPrefab.GetComponent<Bullet>().ConstructBullet(EnumController.WEAPON.shotgun, _beam.transform.position, _targetOfBullet, _angel, _bulletRange, _damage,_bulletSprite,_bulletScale);
            _bulletPrefab.SetActive(true);

            #endregion

        }

    }
}
