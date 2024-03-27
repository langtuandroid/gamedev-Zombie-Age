using MANAGERS;
using UnityEngine;

namespace MODULES.Zombies
{
    public class NormalZombie : Zombie
    {
        [Header("*** ANIMATION ****")]
        [Space(30)]
        [SerializeField]
        private AnimationClip aniMove;

        [SerializeField]
        private AnimationClip aniAttack;
        [SerializeField]
        private AnimationClip aniDie;



        protected override void AnimatorPlay(TheEnumManager.ZOMBIE_STATUS _status)
        {

            switch (_status)
            {
                case TheEnumManager.ZOMBIE_STATUS.moving:
                    if (aniMove)
                    {
                        m_animator.Play(aniMove.name, -1, Random.Range(0f, 1f));
                    }
                    break;
                case TheEnumManager.ZOMBIE_STATUS.die:
                    if (aniDie)
                        m_animator.Play(aniDie.name);
                    break;
                case TheEnumManager.ZOMBIE_STATUS.attack:
                    if (aniAttack)
                        m_animator.Play(aniAttack.name);
                    break;

            }
        }



    }
}
