using MANAGERS;
using MODULES.Zombies.Bullets;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace MODULES.Zombies
{
    public class FlyingZombie : Zombie
    {
       
        [FormerlySerializedAs("m_TranOfOriginalBulletPos")] [SerializeField] Transform _bulletPos;
        [FormerlySerializedAs("vScaleOfBullet")] [SerializeField] Vector3 _bulletScale;
        [FormerlySerializedAs("sprBullet")] [Space(20)]
        public Sprite _bulletSprite;

        protected override void Construct()
        {
            vCurrentPos = _Transform.position;
            vTargetPos = GetTargetPosition();

            InvokeRepeating("Shoot", Random.Range(2.0f, 4.0f), Random.Range(3.0f, 5.0f));
        }

        public override void Move()
        {
            vCurrentPos = Vector2.MoveTowards(vCurrentPos, vTargetPos, Speed * Time.deltaTime);
            if (vCurrentPos == vTargetPos)
            {
                vTargetPos = GetTargetPosition();
            }
            vCurrentPos.z = vCurrentPos.y;
            _Transform.position = vCurrentPos;
        }


        private float _targetXMin = -0.9f, _targetXMax = 8.0f, _targetYMin = -5.0f, _targetYMax = 2.7f;
        private Vector3 GetTargetPosition()
        {
            return new Vector2(Random.Range(_targetXMin, _targetXMax), Random.Range(_targetYMin, _targetYMax));
        }


        private GameObject _bulletPrefab;
        private Vector2 _targetBulletPos;

        private void Shoot()
        {
            if (Speed == 0) return;

            if (_zombieData.eZombie == EnumController.ZOMBIE.ruoi) SoundController.Play(SoundController.SOUND.sfx_zombie_ruoi_shot);//sound
            if (_zombieData.eZombie == EnumController.ZOMBIE.muoi) SoundController.Play(SoundController.SOUND.sfx_zombie_ruoi_shot);//sound

            _targetBulletPos.x = Random.Range(-8.0f, -5.0f);
            _targetBulletPos.y = _bulletPos.position.y;

            _bulletPrefab = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.bullet_of_zombie).Get();
            _bulletPrefab.transform.position = _bulletPos.position;
            _bulletPrefab.GetComponent<ZombieBullet>().ConstructBullet(_zombieData.eZombie, _zombieData.GetDamage(), _targetBulletPos, _bulletScale,_bulletSprite);     
            _bulletPrefab.SetActive(true);
        }
    }
}
