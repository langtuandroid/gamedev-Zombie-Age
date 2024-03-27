using MANAGERS;
using UnityEngine;

namespace MODULES.Zombies.Bullets
{
    public class BulletBossFrog : MonoBehaviour
    {
        public TheEnumManager.ZOMBIE eZombie;
        public int iDamage;


        private Bezier m_Bezier = new Bezier();
        private Transform m_tranform;
        private GameObject m_gameobject;

        private float fSpeed = 1.0f;

        private Vector2 vTagetPos = new Vector2();
        private Vector2 vCurrentPos;
        private Vector2 vOriginalPos;


        private float fTimeBezier = 0.0f;
        // Start is called before the first frame update
        void Start()
        {
            m_gameobject = gameObject;
            m_tranform = transform;

            vTagetPos.x = Random.Range(-8.5f, -6.6f);
            vTagetPos.y = Random.Range(-1.0f, 4.0f);

            // vCurrentPos = m_tranform.position;
            vOriginalPos = m_tranform.position;
        }


        GameObject _effect = null;
        private void Update()
        {
            fTimeBezier += Time.deltaTime * fSpeed;
            vCurrentPos = m_Bezier.Get(vOriginalPos, vTagetPos, 5.0f, fTimeBezier);
            m_tranform.position = vCurrentPos;

            Smock();//smock


            if (fTimeBezier >= 0.95f)
            {

                TheEventManager.ZombieEvent_OnZombieAttack(iDamage);//attack
                //effect
                _effect = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.main_exploison).GetObject();
                _effect.transform.position = vCurrentPos;
                _effect.SetActive(true);

                //sound
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_explosion_grenade);


                Destroy(m_gameobject);
            }
        }



        //SMOCK
        GameObject _smock;
        private float _time = 0.03f;
        private void Smock()
        {
            if (_time <= 0)
            {
                _smock = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.smock_of_bazoka).GetObject();
                _smock.transform.position = vCurrentPos;
                _smock.SetActive(true);
                _time = 0.03f;
            }
            else
            {
                _time -= Time.deltaTime;
            }
        }
    }
}
