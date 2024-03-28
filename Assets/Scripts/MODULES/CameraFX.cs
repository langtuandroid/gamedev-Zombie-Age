using UnityEngine;
using UnityEngine.Serialization;

namespace MODULES
{
    public class CameraFX : MonoBehaviour
    {
        public static CameraFX Instance;

        public enum LEVEL
        {
            level_1,
            level_2,
            level_3,
            level_4,
            level_5,
            level_6,
        }


        private Transform _trasf;
        private Vector3 _originalPos;

        private bool _isShake;
        private float _shakeTime;
        
        private readonly float _shakeTimes = 0.1f;
        private readonly float _decreaseFactor = 0.5f;

        private void Awake()
        {
            _trasf = GetComponent<Transform>();
 
            if (Instance == null)
                Instance = this;
        }

        private void Update()
        {
            if (_isShake)
            {
                if (_shakeTime > 0)
                {
                    _trasf.localPosition = _originalPos + Random.insideUnitSphere * _shakeTimes;

                    _shakeTime -= Time.deltaTime * _decreaseFactor;
                }
                else
                {
                    _shakeTime = 0f;
                    _trasf.localPosition = _originalPos;
                    _isShake = false;
                }
            }

        }

        private void OnEnable()
        {
            _originalPos = _trasf.position;
       
        }

        private void OnDisable()
        {

            Instance = null;
        }
        
        public void CameraShake(LEVEL _level)
        {

            CameraShake(0.03f * ((int)_level + 1));
            _isShake = true;
        }

        private void CameraShake(float _shakeDuration)
        {
            _shakeTime = _shakeDuration;
        }
    }
}
