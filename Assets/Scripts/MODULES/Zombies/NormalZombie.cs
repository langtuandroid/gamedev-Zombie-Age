using MANAGERS;
using UnityEngine;
using UnityEngine.Serialization;

namespace MODULES.Zombies
{
    public class NormalZombie : Zombie
    {
        [FormerlySerializedAs("aniMove")]
        [Header("*** ANIMATION ****")]
        [Space(30)]
        [SerializeField] private AnimationClip _animator;
        [FormerlySerializedAs("aniAttack")] [SerializeField] private AnimationClip _attackAnimation;
        [FormerlySerializedAs("aniDie")] [SerializeField] private AnimationClip _dieAnimation;
        
        protected override void PlayAnimator(EnumController.ZOMBIE_STATUS _status)
        {
            switch (_status)
            {
                case EnumController.ZOMBIE_STATUS.moving:
                    if (_animator)
                    {
                        Animator.Play(_animator.name, -1, Random.Range(0f, 1f));
                    }
                    break;
                case EnumController.ZOMBIE_STATUS.die:
                    if (_dieAnimation)
                        Animator.Play(_dieAnimation.name);
                    break;
                case EnumController.ZOMBIE_STATUS.attack:
                    if (_attackAnimation)
                        Animator.Play(_attackAnimation.name);
                    break;

            }
        }



    }
}
