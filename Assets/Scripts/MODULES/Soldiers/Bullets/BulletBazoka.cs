using System.Collections.Generic;
using _4_Gameplay;
using MANAGERS;
using UnityEngine;
using Zenject;

namespace MODULES.Soldiers.Bullets
{
    public class BulletBazoka : MonoBehaviour
    {
        [Inject] private GameplayController _gameplayController;
        private GameObject _gameobject;
        private Transform _transform;
        private List<Vector2> _path = new();
        private int _pointOnPath;

        private int _pathIndex;

        private bool _allowMove = false;
        private int _damage;
        private float _rang;

        float _timeToMove;
        Vector2 _currentPos;
        Vector2 _targetPos;

        
        private void Awake()
        {
            _gameobject = gameObject;
            _transform = transform;
        }
        public void SetupBullet(List<Vector2> _list,float _range, int _damage)
        {      
            CopyList(_list, _path);
            this._damage = _damage;
            _pathIndex = 0;
            _currentPos = _path[0];
            _targetPos = _currentPos;
            _transform.position = _currentPos;
            _rang = _range;
            _allowMove = true;
        }


        private void Update()
        {
            if (Time.timeScale != 1.0f) return; //for debug
            if (_gameplayController.GameStatus != GameplayController.GAME_STATUS.playing) return;
            if (!_allowMove) return;

            Smock();//smock
            _currentPos = Vector2.MoveTowards(_currentPos, _targetPos, 0.5f);
            _transform.position = _currentPos;
            if (_currentPos==_targetPos)
            {
                _pathIndex ++;
                if(_pathIndex>=_pointOnPath)
                {
                    _allowMove = false;
                    EventController.OnBulletCompletedInvoke(EnumController.WEAPON.bazoka, _currentPos,_rang, _damage);//event
                    _gameobject.SetActive(false);
                }
                else
                {
                    _targetPos = _path[_pathIndex];
                    Rotation(_targetPos);
                }
            }
       
        }




        //ROTATION
        private Quaternion m_quaGoalDirection;
        protected Vector2 moveDirection;
        float angle;
        private void Rotation(Vector2 _targetpos)
        {
            moveDirection = _targetpos - _currentPos;
            angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            m_quaGoalDirection = Quaternion.AngleAxis(angle, Vector3.forward);
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, m_quaGoalDirection, 30.0f);

        }


        //copy path
        private void CopyList(List<Vector2> _from, List<Vector2> _to)
        {
            _to.Clear();
            _pointOnPath = _from.Count;
            for (int i = 0; i < _pointOnPath; i++)
            {
                _to.Add(_from[i]);
            }
        }


        //SMOCK
        GameObject _smock;
        private float _time = 0.03f;
        private void Smock()
        {
            if (_time <= 0)
            {
                _smock = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.smock_of_bazoka).Get();
                _smock.transform.position = _currentPos;
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
