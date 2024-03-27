using System.Collections;
using MANAGERS;
using UnityEngine;
using UnityEngine.Serialization;

namespace MODULES.Zombies
{
    public class ZombieBoss : Zombie
    {
        private enum Status
        {
            walk,
            idie,
            attack,
        }
        [FormerlySerializedAs("ebossStatus")] [SerializeField] Status _boosState = Status.walk;

        [FormerlySerializedAs("prefabBossBullet")] [SerializeField] GameObject _bulletPrefab;
        [FormerlySerializedAs("m_TranOfOriginalBulletPos")] [SerializeField] Transform _originalBulletPos;
        
        [Space(30)]
        [FormerlySerializedAs("aniIdie")] [SerializeField] AnimationClip _idleAnimation;
        [FormerlySerializedAs("aniAttack")] [SerializeField] AnimationClip _atackAnimation;
        [FormerlySerializedAs("aniWalk")] [SerializeField] AnimationClip _walkAnimation;

        protected override void Construct()
        {        
            vCurrentPos = _Transform.position;
            vTargetPos = GetTargetPos();
            Animator.Play(_walkAnimation.name);
        }

        public override void Move()
        {
            if (_boosState != Status.walk) return;
            if (!IsFreezing)
                Animator.Play(_walkAnimation.name);

            vCurrentPos = Vector2.MoveTowards(vCurrentPos, vTargetPos, Speed * Time.deltaTime);
            if (vCurrentPos == vTargetPos)
            {
                vTargetPos = GetTargetPos();
                _boosState = Status.idie;
                Animator.Play(_idleAnimation.name);
                StartCoroutine(ShootCoroutine());
            }
            vCurrentPos.z = vCurrentPos.y;
            _Transform.position = vCurrentPos;
        }


        private float _targetXmin = 3.0f, _targetXmax = 8.0f, _targetYmin = -4.0f, _targetYmax = 0.0f;
        private Vector3 GetTargetPos()
        {
            return new Vector2(Random.Range(_targetXmin, _targetXmax), Random.Range(_targetYmin, _targetYmax));
        }


        //SHOT
        GameObject _bullet = null;

        private WaitForSeconds _attackTime = new(2.0f);
        private WaitForSeconds _waitToShootTime = new(0.3f);
        private WaitForSeconds _shootCooldown = new(0.1f);
        private WaitForSeconds _idleCooldown = new(0.5f);
        private WaitForSeconds _waitToWalk = new(2.0f);


        private IEnumerator ShootCoroutine()
        {
            if (!_isAlive) yield break;
            yield return _attackTime;
            if (IsFreezing) goto RESET;

            _boosState = Status.attack;
            Animator.Play(_atackAnimation.name);
    
            switch (_zombieData.eZombie)
            {
                case EnumController.ZOMBIE.boss_mug:
                    SoundController.Instance.Play(SoundController.SOUND.sfx_zom_bullet_explosion);
                    break;
                case EnumController.ZOMBIE.boss_soldier:
                    SoundController.Instance.Play(SoundController.SOUND.sfx_boss_shot_fire);
                    break;
                case EnumController.ZOMBIE.boss_frog:
                    SoundController.Instance.Play(SoundController.SOUND.sfx_boss_shot_stone);
                    break;
            }


            yield return _waitToShootTime;
            if (IsFreezing) goto RESET;

       
            switch (_zombieData.eZombie)
            {
                case EnumController.ZOMBIE.boss_frog:
                {
                    if (IsFreezing) goto RESET;
                    Instantiate(_bulletPrefab, _originalBulletPos.position, Quaternion.identity);

                    yield return _shootCooldown;
                    if (IsFreezing) goto RESET;
                    Instantiate(_bulletPrefab, _originalBulletPos.position, Quaternion.identity);

                    yield return _shootCooldown;
                    if (IsFreezing) goto RESET;
                    Instantiate(_bulletPrefab, _originalBulletPos.position, Quaternion.identity);
                    break;
                }
                case EnumController.ZOMBIE.boss_mug:
                {
                    // yield return _waitToShot;
                    if (IsFreezing) goto RESET;
                    Instantiate(_bulletPrefab, _originalBulletPos.position, Quaternion.identity);

                    yield return _shootCooldown;
                    if (IsFreezing) goto RESET;
                    Instantiate(_bulletPrefab, _originalBulletPos.position, Quaternion.identity);
                    break;
                }
                case EnumController.ZOMBIE.boss_soldier:
                {
                    //yield return _waitToShot;
                    if (IsFreezing) goto RESET;
                    Instantiate(_bulletPrefab, _originalBulletPos.position, Quaternion.identity);
                    break;
                }
            }


            yield return _idleCooldown;
            if (IsFreezing) goto RESET;
            Animator.Play(_idleAnimation.name);//player animation;

            yield return _waitToWalk;
            RESET:
            _boosState = Status.walk;

        }
    }
}
