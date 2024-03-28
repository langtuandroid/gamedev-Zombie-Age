using _4_Gameplay;
using MANAGERS;
using MODULES.Soldiers.Bullets;
using UnityEngine;

namespace MODULES.Soldiers
{
    public class Fire : HandWeapon
    {
        private GameObject _bulletPrefab;
        private Vector2 _targetOfBullet;

        protected override void Shoot()
        {
            if (_gameplayController.GameStatus != GameplayController.GAME_STATUS.playing) return;
            if (Soldier.Instance._weaponManager._gunData.DATA.iCurrentAmmo <= 0)
            {
                EventController.OnWeaponNoBulletInvoke(null);//event - thay sung
                return;
            }
            if (IsLoadingBullet) return;
            if (IsLoadingMagazine) return;


            IsLoadingBullet = true;
            Soldier.Instance._weaponManager._gunData.DATA.iCurrentAmmo--;
            _ammoInMagazine--;
            if (_ammoInMagazine == 0) IsLoadingMagazine = true;
            
            EventController.OnWeaponShotInvoke(Soldier.Instance._weaponManager._gunData);//event      
            _gameplayController.weaponShell.Show(GetFactorBullet());//show shell
            
            #region  SHOT      

            _beam.SetActive(true);

            SoundController.PlayGunSound( EnumController.WEAPON.firegun);//sound
            
            _animator.Play(0);//shake gun
      
            //bullet
            _targetOfBullet = _inputPosition + Random.insideUnitCircle * 1.3f;
            _bulletPrefab = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.bullet_fire).Get();
            _bulletPrefab.GetComponent<Bullet>().ConstructBullet( EnumController.WEAPON.firegun,_beam.transform.position, _targetOfBullet, 0.0f, _bulletRange, _damage);
            _bulletPrefab.SetActive(true);

            #endregion
        
            //THAY SUNG
            if (Soldier.Instance._weaponManager._gunData.DATA.iCurrentAmmo == 0)
                EventController.OnWeaponNoBulletInvoke(null);//event - thay sung

        }
    }
}
