using MANAGERS;
using UnityEngine;

namespace MODULES.Zombies.Bullets
{
    public class BulletBoss_Soldier : MonoBehaviour
    {

        private TheEnumManager.ZOMBIE eZombie;

        private Transform _tranOfThis;
        private GameObject _gameobject;

        private Vector2 vTargetPos;
        private Vector2 vCurrentPos;
        public float fSpeed;
        public int iDamage;

        // Start is called before the first frame update
        void Awake()
        {
            _tranOfThis = transform;
            _gameobject = gameObject;
            vCurrentPos = _tranOfThis.position;
        }

        private void Start()
        {
            vTargetPos = vCurrentPos;
            vTargetPos.x = Random.Range(-8.5f, -6.5f);
        }

        // Update is called once per frame
        GameObject _effect = null;
        public virtual void Update() //Bay thang
        {
            vCurrentPos = _tranOfThis.position;
            vCurrentPos = Vector2.MoveTowards(vCurrentPos, vTargetPos, Time.deltaTime * fSpeed);
            Smock();
            if (vCurrentPos == vTargetPos)
            {
                TheEventManager.ZombieEvent_OnZombieAttack(iDamage);//attack
                //effect
                _effect = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.main_exploison).GetObject();
                _effect.transform.position = vTargetPos;
                _effect.SetActive(true);

                //sound
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_explosion_grenade);


                Destroy(_gameobject);
            }
            _tranOfThis.position = vCurrentPos;
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
