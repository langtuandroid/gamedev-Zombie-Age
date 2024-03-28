using UnityEngine;
using UnityEngine.Serialization;

namespace MODULES
{
    public enum TYPE
    {
        Destroy,
        Active,
    }
    public class TimeLife : MonoBehaviour
    {
        [FormerlySerializedAs("eType")] public TYPE _timeLifeType;
        [FormerlySerializedAs("fTimeLife")] public float _timeLife;
        private float _timeCount;
        private GameObject _gameobject;

        private void Awake()
        {
            _gameobject = gameObject;
        }
        private void Update()
        {
            _timeCount -= Time.deltaTime;
            if(_timeCount<=0)
            {
                _timeCount = _timeLife;
                switch(_timeLifeType)
                {
                    case TYPE.Active:
                        _gameobject.SetActive(false);
                        break;
                    case TYPE.Destroy:
                        Destroy(_gameobject);
                        break;
                }
            }
        }

        private void OnEnable()
        {
            _timeCount = _timeLife;
        }

    }
}