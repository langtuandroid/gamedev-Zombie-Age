using MANAGERS;
using UnityEngine;
using UnityEngine.Serialization;

namespace MODULES.Soldiers.Bullets
{
    public class Bullet : MonoBehaviour
    {
        private EnumController.WEAPON _weaponType;
        private EnumController.ZOMBIE _zombieType;
        private bool _playerBullets; //fasle is mean of zombie
        private SpriteRenderer _spriteRenderer;


        private Transform _transform;
        private GameObject _gameobject;

        private Vector2 _targetPos;
        private Vector2 _currentPos;
        [FormerlySerializedAs("fSpeed")] public float _speed;

        private int _damage;
        private float _range;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transform = transform;
            _gameobject = gameObject;
        }
        
        void Update()
        {
            _currentPos = _transform.position;
            _currentPos = Vector2.MoveTowards(_currentPos, _targetPos, Time.deltaTime * _speed);
            if (_currentPos == _targetPos)
            {
                BulletComplete();
                _gameobject.SetActive(false);
            }
            _transform.position = _currentPos;
        }
        
        public void ConstructBullet(EnumController.WEAPON _weapon, Vector2 _startPos, Vector2 _targetPos,  float _angle, float _range, int _damage,Sprite _spriteOfBullet,Vector3 _scaleOfBullet)
        {
            _playerBullets = true; //bullet of player
            _weaponType = _weapon;
            _currentPos = _startPos;
            _transform.position = _currentPos;
            _transform.eulerAngles = new Vector3(0, 0, _angle);
            this._targetPos = _targetPos;
            this._damage = _damage;
            this._range = _range;
            _spriteRenderer.sprite = _spriteOfBullet;
            _transform.localScale = _scaleOfBullet;
        }

        //Set bullet of player
        public void ConstructBullet(EnumController.WEAPON _weapon, Vector2 _startPos, Vector2 _targetPos, float _angle, float _range, int _damage)
        {
            _playerBullets = true; //bullet of player
            _weaponType = _weapon;
            _currentPos = _startPos;
            _transform.position = _currentPos;
            _transform.eulerAngles = new Vector3(0, 0, _angle);
            this._targetPos = _targetPos;
            this._damage = _damage;
            this._range = _range;
       
        }
        
        protected virtual void BulletComplete()
        {
            if (_playerBullets)
                EventController.OnBulletCompletedInvoke(_weaponType, _targetPos,_range, _damage);
            else
            {
                EventController.OnZombieBulletCompletedInvoke(_zombieType, _targetPos);
                EventController.ZombieEvent_OnZombieAttack(_damage);//attack
            }
        }


        private void OnDisable()
        {
            _transform.position = new Vector2(100, 100);
        }
    }
}
