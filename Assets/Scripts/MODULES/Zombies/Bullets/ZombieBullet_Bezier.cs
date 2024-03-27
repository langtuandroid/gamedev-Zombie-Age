using MANAGERS;
using UnityEngine;

namespace MODULES.Zombies.Bullets
{
    public class ZombieBullet_Bezier : MonoBehaviour
    {
        public EnumController.ZOMBIE eZombie;
        public float fDamage;


        private Bezier m_Bezier = new Bezier();
        private Transform m_tranform;
        private GameObject m_gameobject;

        private float fSpeed = 1.0f;

        private Vector2 vTagetPos = new Vector2();
        private Vector2 vCurrentPos;
        private Vector2 vOriginalPos;


        private float fTimeBezier = 0.0f;

        private void Awake()
        {
            m_gameobject = gameObject;
            m_tranform = transform;

        }
        // Start is called before the first frame update
        void OnEnable()
        {
      
            vTagetPos.x = Random.Range(-8.5f, -6.6f);
            vTagetPos.y = Random.Range(-1.0f, 4.0f);

            // vCurrentPos = m_tranform.position;
            vOriginalPos = m_tranform.position;
        }


        //setup
        public void SetUp(EnumController.ZOMBIE _zombie,float _Damage)
        {
            fDamage = _Damage;
            eZombie = _zombie;
        }


        GameObject _effect = null;
        private void Update()
        {
            fTimeBezier += Time.deltaTime * fSpeed;
            vCurrentPos = m_Bezier.Get(vOriginalPos, vTagetPos, 5.0f, fTimeBezier);
            m_tranform.position = vCurrentPos;


            if (fTimeBezier >= 0.95f)
            {

                EventController.ZombieEvent_OnZombieAttack(fDamage);//attack
                //effect
                _effect = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.main_exploison).Get();
                _effect.transform.position = vCurrentPos;
                _effect.SetActive(true);

                //sound
                SoundController.Instance.Play(SoundController.SOUND.sfx_explosion_grenade);

                fTimeBezier = 0.0f;
                m_gameobject.SetActive(false);
            }
        }
    }
}
