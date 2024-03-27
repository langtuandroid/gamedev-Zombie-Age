using MANAGERS;
using UnityEngine;

namespace MODULES.Zombies.Bullets
{
    public class ZombieBullet_Bezier : MonoBehaviour
    {
        public TheEnumManager.ZOMBIE eZombie;
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
        public void SetUp(TheEnumManager.ZOMBIE _zombie,float _Damage)
        {
            fDamage = _Damage;
            eZombie = _zombie;
        }


        GameObject _effect = null;
        private void Update()
        {
            fTimeBezier += Time.deltaTime * fSpeed;
            vCurrentPos = m_Bezier.GetBezier(vOriginalPos, vTagetPos, 5.0f, fTimeBezier);
            m_tranform.position = vCurrentPos;


            if (fTimeBezier >= 0.95f)
            {

                TheEventManager.ZombieEvent_OnZombieAttack(fDamage);//attack
                //effect
                _effect = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.main_exploison).GetObject();
                _effect.transform.position = vCurrentPos;
                _effect.SetActive(true);

                //sound
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_explosion_grenade);

                fTimeBezier = 0.0f;
                m_gameobject.SetActive(false);
            }
        }
    }
}
