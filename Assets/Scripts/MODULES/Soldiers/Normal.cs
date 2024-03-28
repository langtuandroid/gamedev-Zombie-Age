using _4_Gameplay;
using MANAGERS;
using MODULES.Soldiers.Bullets;
using UnityEngine;

namespace MODULES.Soldiers
{
    public class Normal : HandWeapon
    {
        private GameObject _bulletPrefab;
        private Vector2 _targetOfBullet;
        private float _anglee;

        protected override void Shoot()
        {
            if (GameplayController.Instance.GameStatus != GameplayController.GAME_STATUS.playing) return;
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
            GameplayController.Instance.weaponShell.Show(GetFactorBullet());//show shell
            
            #region SHOT  
            _beam.SetActive(true);

            _animator.Play(_gunShakeAnimation.name, -1, 0f);//shake gun
            SoundController.PlayGunSound(Soldier.Instance._weaponManager._gunData.DATA.eWeapon);//sound

            _targetOfBullet = _inputPosition + Random.insideUnitCircle * 1.3f;
            _anglee = GetRoatateZ(_beam.transform.position, _targetOfBullet);

            _bulletPrefab = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.bullet).Get();
            _bulletPrefab.GetComponent<Bullet>().ConstructBullet(Soldier.Instance._weaponManager._gunData.DATA.eWeapon, _beam.transform.position, _targetOfBullet, _anglee, _bulletRange, _damage, _bulletSprite, _bulletScale);
            _bulletPrefab.SetActive(true);
            #endregion

            //THAY SUNG
            if (Soldier.Instance._weaponManager._gunData.DATA.iCurrentAmmo == 0)
                EventController.OnWeaponNoBulletInvoke(null);//event - thay sung
        }

    }
}
