using UnityEngine;
using UnityEngine.Serialization;

namespace MANAGERS
{
    public class SupportItem : MonoBehaviour
    {
        [FormerlySerializedAs("eSupport")] [SerializeField] private EnumController.SUPPORT _supportType;
        private Transform _transform;
        private GameObject _gameobject;
        private Bezier _bezier = new ();

        [FormerlySerializedAs("fSpeed")] [SerializeField] private float _speed;
        [FormerlySerializedAs("fHigh")] [SerializeField] private float _high = 10.0f;

        private float _time;
        private bool _allowMove;
        private Vector2 _currentPosition;
        private Vector2 _startPos, _endPos;
        private Vector3 _euler;

        private void Awake()
        {
            _transform = transform;
            _gameobject = gameObject;
        }

        private void Update()
        {
            if (!_allowMove) return;
            _euler.z -= 5.0f;

            _time += Time.deltaTime * _speed;
            _currentPosition = _bezier.Get(_startPos, _endPos, _high, _time);
            _transform.position = _currentPosition;
            _transform.eulerAngles = _euler;
            if (_time >= 1)
            {
                _allowMove = false;
                EventController.OnSupportCompletedInvoke(_supportType, _currentPosition);//event
                _gameobject.SetActive(false);
            }
        }

        public void Move(Vector2 _from, Vector2 _to)
        {
            _startPos = _from;
            _endPos = _to;
            _time = 0.0f;
            _allowMove = true;
        }
    }
}
