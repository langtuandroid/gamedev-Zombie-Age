using MANAGERS;
using UnityEngine;

namespace MODULES.Zombies.Bullets
{
    public class BulletBoss_Soldier : MonoBehaviour
    {

        private EnumController.ZOMBIE eZombie;

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
                EventController.ZombieEvent_OnZombieAttack(iDamage);//attack
                //effect
                _effect = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.main_exploison).Get();
                _effect.transform.position = vTargetPos;
                _effect.SetActive(true);

                //sound
                SoundController.Instance.Play(SoundController.SOUND.sfx_explosion_grenade);


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
                _smock = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.smock_of_bazoka).Get();
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
