using _4_Gameplay;
using MANAGERS;
using UnityEngine;
using UnityEngine.Serialization;

namespace MODULES.Soldiers
{
    public class Electric : HandWeapon
    {
        [FormerlySerializedAs("objElectricLine")] [SerializeField] private GameObject _electricLine;
        [FormerlySerializedAs("line_renderer")] [SerializeField] private ElectricLine _lineRenderer;

        private Vector3 _vPosStart, _targetOfBullet;

        protected override void Construct()
        {
            _electricLine = Instantiate(_electricLine);
            _lineRenderer = _electricLine.GetComponent<ElectricLine>();
        }

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
            //============ to reload 

            Soldier.Instance._weaponManager._gunData.DATA.iCurrentAmmo--;
            EventController.OnWeaponShotInvoke(Soldier.Instance._weaponManager._gunData);//event
            _ammoInMagazine--;
            if (_ammoInMagazine == 0) IsLoadingMagazine = true;

            GameplayController.Instance.weaponShell.Show(GetFactorBullet());
            #region SHOT

            _beam.SetActive(true);      

            _animator.Play(_gunShakeAnimation.name, -1, 0f);//shake gun
            SoundController.Instance.PlayGunSound(EnumController.WEAPON.stun_gun);//sound
            _targetOfBullet = _inputPosition + Random.insideUnitCircle * 1.3f;
            _vPosStart = _beam.transform.position;
            _vPosStart.z = -40.0f;
            _targetOfBullet.z = -40.0f;


            _lineRenderer.ShootElectric(_vPosStart,_targetOfBullet);

            EventController.OnBulletCompletedInvoke(EnumController.WEAPON.stun_gun, _targetOfBullet, _bulletRange, _damage);
            #endregion
            
            if (Soldier.Instance._weaponManager._gunData.DATA.iCurrentAmmo == 0)
                EventController.OnWeaponNoBulletInvoke(null);//event - thay sung
        }

    }
}
