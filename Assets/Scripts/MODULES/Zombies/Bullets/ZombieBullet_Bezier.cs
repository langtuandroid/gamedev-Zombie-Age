using MANAGERS;
using UnityEngine;

namespace MODULES.Zombies.Bullets
{
    public class ZombieBullet_Bezier : MonoBehaviour
    {
        private float _damage;
        private Bezier _bezier = new ();
        private Transform _transform;
        private GameObject _gameobject;

        private float _speed = 1.0f;

        private Vector2 _targetPos;
        private Vector2 _currentPos;
        private Vector2 _originalPos;
        
        private float _timeBezier;

        private void Awake()
        {
            _gameobject = gameObject;
            _transform = transform;

        }
        void OnEnable()
        {
            _targetPos.x = Random.Range(-8.5f, -6.6f);
            _targetPos.y = Random.Range(-1.0f, 4.0f);

            _originalPos = _transform.position;
        }

        private GameObject _effect;
        private void Update()
        {
            _timeBezier += Time.deltaTime * _speed;
            _currentPos = _bezier.Get(_originalPos, _targetPos, 5.0f, _timeBezier);
            _transform.position = _currentPos;


            if (_timeBezier >= 0.95f)
            {

                EventController.ZombieEvent_OnZombieAttack(_damage);//attack
                _effect = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.main_exploison).Get();
                _effect.transform.position = _currentPos;
                _effect.SetActive(true);

                //sound
                SoundController.Instance.Play(SoundController.SOUND.sfx_explosion_grenade);

                _timeBezier = 0.0f;
                _gameobject.SetActive(false);
            }
        }
    }
}
