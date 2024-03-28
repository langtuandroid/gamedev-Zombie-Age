using MODULES;
using UnityEngine;
using Zenject;

namespace MANAGERS
{
    public class EffectController : MonoBehaviour
    {
        [Inject] private SoundController _soundController;
        [Inject] private ObjectPoolController _objectPoolController;
        private GameObject _bulletEffect;
        private void BulletEffect(EnumController.WEAPON _weapon, Vector2 _pos,float _range, int _damage)
        {
            switch (_weapon)
            {
                case EnumController.WEAPON.shotgun:
                //SHOT GUN
                case EnumController.WEAPON.shotgun2barrel:
                {
                    for (int i = 0; i < 5; i++)
                    {
                        _bulletEffect = _objectPoolController.GetObjectPool(EnumController.POOLING_OBJECT.exploison_bullet).Get();
                        _bulletEffect.transform.position = _pos + Random.insideUnitCircle;
                        _bulletEffect.SetActive(true);
                    }
                    return;
                }
                case EnumController.WEAPON.firegun:
                    return;
                case EnumController.WEAPON.bazoka:
                    CameraFX.Instance.CameraShake(CameraFX.LEVEL.level_1);//shaking camera
                    _soundController.Play(SoundController.SOUND.sfx_explosion_grenade);//sound
                    //effect
                    _bulletEffect = _objectPoolController.GetObjectPool(EnumController.POOLING_OBJECT.main_exploison).Get();
                    _bulletEffect.transform.position = _pos;
                    _bulletEffect.SetActive(true);
                    return;
            }


            _bulletEffect = _objectPoolController.GetObjectPool(EnumController.POOLING_OBJECT.exploison_bullet).Get();
            _bulletEffect.transform.position = _pos;
            _bulletEffect.SetActive(true);
        }

        private GameObject _supportEffect;
        private GameObject _freezeEffect;
        private void SupportEffect(EnumController.SUPPORT _support, Vector2 _pos)
        {
            switch (_support)
            {
                case EnumController.SUPPORT.grenade:
                    _supportEffect = _objectPoolController.GetObjectPool(EnumController.POOLING_OBJECT.main_exploison).Get();
                    _soundController.Play(SoundController.SOUND.sfx_explosion_grenade);//sound
                    CameraFX.Instance.CameraShake(CameraFX.LEVEL.level_3);//shaking camera
                    break;
                case EnumController.SUPPORT.freeze:
                    _supportEffect = _objectPoolController.GetObjectPool(EnumController.POOLING_OBJECT.main_exploison).Get();
                    _soundController.Play(SoundController.SOUND.sfx_break);//sound
                    _soundController.Play(SoundController.SOUND.sfx_explosion_freeze);//sound
                    CameraFX.Instance.CameraShake(CameraFX.LEVEL.level_1);//shaking camera

                    //freeze effect
                    _freezeEffect = _objectPoolController.GetObjectPool(EnumController.POOLING_OBJECT.freeze_range).Get();
                    _freezeEffect.transform.position = _pos;
                    _freezeEffect.SetActive(true);
                    break;
                case EnumController.SUPPORT.poison:
                    _supportEffect = _objectPoolController.GetObjectPool(EnumController.POOLING_OBJECT.main_exploison).Get();
                    _soundController.Play(SoundController.SOUND.sfx_break );//sound
                    CameraFX.Instance.CameraShake(CameraFX.LEVEL.level_1);//shaking camera
                    break;
                case EnumController.SUPPORT.big_bomb:
                    _supportEffect = _objectPoolController.GetObjectPool(EnumController.POOLING_OBJECT.main_exploison).Get();
                    _soundController.Play(SoundController.SOUND.sfx_explosion_bigbomb);//sound
                    CameraFX.Instance.CameraShake(CameraFX.LEVEL.level_6);//shaking camera
                    Instantiate(_objectPoolController._bigExplosionPrefab);//effect
                    break;
            }

            _supportEffect.transform.position = _pos;
            _supportEffect.SetActive(true);
        }
        
        private void BulletZombieEffect(EnumController.ZOMBIE _zombie, Vector2 _pos)
        {
            if (_zombie == EnumController.ZOMBIE.boss_mug 
                || _zombie == EnumController.ZOMBIE.boss_soldier 
                || _zombie == EnumController.ZOMBIE.boss_frog)
            {
                _soundController.Play(SoundController.SOUND.sfx_explosion_grenade);//sound
                _bulletEffect = _objectPoolController.GetObjectPool(EnumController.POOLING_OBJECT.main_exploison).Get();
            }
            else
            {
                if (_zombie == EnumController.ZOMBIE.ruoi) _soundController.Play(SoundController.SOUND.sfx_zom_bullet_explosion);//sound

                _bulletEffect = _objectPoolController.GetObjectPool(EnumController.POOLING_OBJECT.exploison_bullet).Get();
            }

            _bulletEffect.transform.position = _pos;
            _bulletEffect.SetActive(true);
        }

        private void OnEnable()
        {
            EventController.OnBulletCompleted += BulletEffect;
            EventController.OnSupportCompleted += SupportEffect;
            EventController.OnZombieBulletCompleted += BulletZombieEffect;
        }
        private void OnDisable()
        {
            EventController.OnBulletCompleted -= BulletEffect;
            EventController.OnSupportCompleted -= SupportEffect;
            EventController.OnZombieBulletCompleted -= BulletZombieEffect;
        }
    }
}
