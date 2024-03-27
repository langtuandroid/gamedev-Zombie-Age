using System.Collections;
using MANAGERS;
using UnityEngine;

namespace MODULES
{
    public class EffectBigBlood : MonoBehaviour
    {
   
        private Vector2 vCurrentPos;
        private Vector2 vTempPos;
        public int NumberOfBlood;
        private WaitForSeconds _time = new WaitForSeconds(0.2f);

        private void Awake()
        {
            vCurrentPos = transform.position;
        }


        private void Start()
        {
            StartCoroutine(Effect(NumberOfBlood));
        }


        private GameObject _blood;
        private IEnumerator Effect(int _loop)
        {
            for (int i = 0; i < _loop; i++)
            {
                _blood = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.zombie_blood_exploison).GetObject();
                if(_blood)
                {
                    vTempPos = vCurrentPos + Random.insideUnitCircle*2.0f;
                    _blood.transform.position = vTempPos;
                    _blood.SetActive(true);
                    TheSoundManager.Instance.ZombieExplosion();//sound
                    yield return _time;
                }
            }

            Destroy(gameObject);
        }

    
    }
}
