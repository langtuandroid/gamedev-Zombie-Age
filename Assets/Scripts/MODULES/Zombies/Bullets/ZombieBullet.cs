using MANAGERS;
using UnityEngine;

namespace MODULES.Zombies.Bullets
{
    public class ZombieBullet : MonoBehaviour
    {
        private EnumController.ZOMBIE _zombieType;

        private SpriteRenderer _spriteRenderer;
        private Transform _transform;
        private GameObject _gameobject;

        private Vector2 _targetPos;
        private Vector2 _currentPos;
        private float _speed = 15;

        private float _damage;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transform = transform;
            _gameobject = gameObject;
        }

        public virtual void Update() 
        {
            _currentPos = _transform.position;
            _currentPos = Vector2.MoveTowards(_currentPos, _targetPos, Time.deltaTime * _speed);
            if (_currentPos == _targetPos)
            {
                BulletDone();
                _gameobject.SetActive(false);
            }
            _transform.position = _currentPos;
        }

        
        public void ConstructBullet(EnumController.ZOMBIE _zombie, float  _damage, Vector2 _to, Vector3 _scale, Sprite _sprite)
        {
            _spriteRenderer.sprite = _sprite;
            _targetPos = _to;
            _zombieType = _zombie;
            this._damage = _damage;
            _transform.localScale = _scale;
        }

        protected virtual void BulletDone()
        {

            EventController.OnZombieBulletCompletedInvoke(_zombieType, _targetPos);
            EventController.ZombieEvent_OnZombieAttack(_damage);//attack

        }


        private void OnDisable()
        {
            _transform.position = new Vector2(100, 100);
        }
    }
}
