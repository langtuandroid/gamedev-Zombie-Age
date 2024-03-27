using MANAGERS;
using UnityEngine;
using UnityEngine.Serialization;

namespace MODULES.Zombies.Bullets
{
    public class BulletBossFrog : MonoBehaviour
    {
        [FormerlySerializedAs("iDamage")] public int _damage;
        private Bezier _bezier = new ();
        private Transform _transform;
        private GameObject _gameobject;

        private float _speed = 1.0f;

        private Vector2 _tagetPos;
        private Vector2 _currentPos;
        private Vector2 _originalPos;
        
        private float _timeBezier;

        void Start()
        {
            _gameobject = gameObject;
            _transform = transform;

            _tagetPos.x = Random.Range(-8.5f, -6.6f);
            _tagetPos.y = Random.Range(-1.0f, 4.0f);
            
            _originalPos = _transform.position;
        }

        private GameObject _effect;
        private void Update()
        {
            _timeBezier += Time.deltaTime * _speed;
            _currentPos = _bezier.Get(_originalPos, _tagetPos, 5.0f, _timeBezier);
            _transform.position = _currentPos;

            Smock();

            if (_timeBezier >= 0.95f)
            {

                EventController.ZombieEvent_OnZombieAttack(_damage);//attack
                _effect = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.main_exploison).Get();
                _effect.transform.position = _currentPos;
                _effect.SetActive(true);
                
                SoundController.Instance.Play(SoundController.SOUND.sfx_explosion_grenade);


                Destroy(_gameobject);
            }
        }


        private GameObject _smockEffect;
        private float _time = 0.03f;
        private void Smock()
        {
            if (_time <= 0)
            {
                _smockEffect = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.smock_of_bazoka).Get();
                _smockEffect.transform.position = _currentPos;
                _smockEffect.SetActive(true);
                _time = 0.03f;
            }
            else
            {
                _time -= Time.deltaTime;
            }
        }
    }
}
