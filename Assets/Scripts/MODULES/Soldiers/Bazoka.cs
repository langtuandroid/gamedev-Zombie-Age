using System.Collections.Generic;
using _4_Gameplay;
using MANAGERS;
using MODULES.Soldiers.Bullets;
using UnityEngine;
using UnityEngine.Serialization;

namespace MODULES.Soldiers
{
    public class Bazoka : HandWeapon
    {
        private GameObject _bullet;
        [FormerlySerializedAs("GROUP_PATH_FAR")] [SerializeField] private Transform _groupPathFar;
        [FormerlySerializedAs("GROUP_PATH_NEAR")] [SerializeField] private Transform _groupPathNear;
        [FormerlySerializedAs("GROUP_PATH_CLOSER")] [SerializeField] private Transform _groupPathCloser;
        private List<Vector2> _listPath = new List<Vector2>();
        private int _totalFar, _iTotalNear, _iTotalCloser;

        protected override void Construct()
        {
            _totalFar = _groupPathFar.childCount;
            _iTotalNear = _groupPathNear.childCount;
            _iTotalCloser = _groupPathCloser.childCount;
        }

        private void UpdateRoad(Vector2 _target)
        {
            _listPath.Clear();
            if (_target.x > 5.7f)
            {
                for (int i = 0; i < _totalFar; i++)
                {
                    _listPath.Add(_groupPathFar.GetChild(i).position);
                }
                _listPath[_totalFar - 1] = _target;
            }
            else if (_target.x < 0.5f)
            {
                for (int i = 0; i < _iTotalCloser; i++)
                {
                    _listPath.Add(_groupPathCloser.GetChild(i).position);
                }
                _listPath[_iTotalCloser - 1] = _target;
            }
            else
            {
                for (int i = 0; i < _iTotalNear; i++)
                {
                    _listPath.Add(_groupPathNear.GetChild(i).position);
                }
                _listPath[_iTotalNear - 1] = _target;
            }
        }


        protected override void Shoot()
        {
            Invoke("NoShoot", 0.05f);
        }

        private void NoShoot()
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

            EventController.OnWeaponShotInvoke(Soldier.Instance._weaponManager._gunData);//event
            _ammoInMagazine--;
            if (_ammoInMagazine == 0 ) IsLoadingMagazine = true;
            
            _gameplayController.weaponShell.Show(GetFactorBullet());//show shell
            
            _beam.SetActive(true);

            SoundController.PlayGunSound( EnumController.WEAPON.bazoka);//sound

            #region SHOT
            _animator.Play(_gunShakeAnimation.name, -1, 0f);//shake gun 
            UpdateRoad(_inputPosition);


            //EFFECT---------------
            _bullet = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.bullet_of_bazoka).Get();
            _bullet.GetComponent<BulletBazoka>().SetupBullet(_listPath, _bulletRange, _damage);
            _bullet.transform.position = _listPath[0];
            _bullet.transform.eulerAngles = Transform.eulerAngles;
            _bullet.SetActive(true);
            #endregion

            //THAY SUNG
            if (Soldier.Instance._weaponManager._gunData.DATA.iCurrentAmmo == 0)
                EventController.OnWeaponNoBulletInvoke(null);//event - thay sung

        }
    }
}
