﻿using MANAGERS;
using UnityEngine;
using Zenject;

namespace MODULES.Zombies.Bullets
{
    public class BulletBoss_Soldier : MonoBehaviour
    {
        [Inject] private ObjectPoolController _objectPoolController;
        [Inject] private SoundController _soundController;
        private EnumController.ZOMBIE _zombieType;

        private Transform _transform;
        private GameObject _gameobject;

        private Vector2 _targetPos;
        private Vector2 _currentPos;
        private readonly float _speed = 20;
        private readonly int _damage = 20;

        private void Awake()
        {
            _transform = transform;
            _gameobject = gameObject;
            _currentPos = _transform.position;
        }

        private void Start()
        {
            _targetPos = _currentPos;
            _targetPos.x = Random.Range(-8.5f, -6.5f);
        }

        private GameObject _effect;
        public virtual void Update() //Bay thang
        {
            _currentPos = _transform.position;
            _currentPos = Vector2.MoveTowards(_currentPos, _targetPos, Time.deltaTime * _speed);
            Smock();
            if (_currentPos == _targetPos)
            {
                EventController.ZombieEvent_OnZombieAttack(_damage);//attack
                //effect
                _effect = _objectPoolController.GetObjectPool(EnumController.POOLING_OBJECT.main_exploison).Get();
                _effect.transform.position = _targetPos;
                _effect.SetActive(true);

                //sound
                _soundController.Play(SoundController.SOUND.sfx_explosion_grenade);


                Destroy(_gameobject);
            }
            _transform.position = _currentPos;
        }

        private GameObject _smockEffect;
        private float _timeDelay = 0.03f;
        private void Smock()
        {
            if (_timeDelay <= 0)
            {
                _smockEffect = _objectPoolController.GetObjectPool(EnumController.POOLING_OBJECT.smock_of_bazoka).Get();
                _smockEffect.transform.position = _currentPos;
                _smockEffect.SetActive(true);
                _timeDelay = 0.03f;
            }
            else
            {
                _timeDelay -= Time.deltaTime;
            }
        }


    }
}
