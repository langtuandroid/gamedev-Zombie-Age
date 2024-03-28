using System.Collections;
using MANAGERS;
using UnityEngine;
using Zenject;

namespace MODULES
{
    public class BloodBigFX : MonoBehaviour
    {
        [Inject] private SoundController _soundController;
        private Vector2 _thisPos;
        private Vector2 _tempPos;
        private int _bloodNum = 15;
        private WaitForSeconds _timeBetween = new WaitForSeconds(0.2f);

        private void Awake()
        {
            _thisPos = transform.position;
        }


        private void Start()
        {
            StartCoroutine(PlayEffect(_bloodNum));
        }


        private GameObject _bloodPrefab;
        private IEnumerator PlayEffect(int _loop)
        {
            for (int i = 0; i < _loop; i++)
            {
                _bloodPrefab = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.zombie_blood_exploison).Get();
                if(_bloodPrefab)
                {
                    _tempPos = _thisPos + Random.insideUnitCircle*2.0f;
                    _bloodPrefab.transform.position = _tempPos;
                    _bloodPrefab.SetActive(true);
                    _soundController.ZombieExplosion();//sound
                    yield return _timeBetween;
                }
            }

            Destroy(gameObject);
        }

    
    }
}
