using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SCREENS
{
    public class Loading : MonoBehaviour
    {
        public enum FADE
        {
            fade_in,
            face_out,
        }

        public static Loading Instance;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }


        [FormerlySerializedAs("eFace")] public FADE _fadeType;
        [FormerlySerializedAs("imaLoading")] [SerializeField] Image _loadingImage;
        private bool _isAnimte;
        private float _setTime = 4.0f;
        private float _timeCount;
        private Color _thisColor = Color.black;


        public void Configure(FADE _fade)
        {
            Instance._fadeType = _fade;
            switch (_fade)
            {
                case FADE.fade_in:
                    _thisColor = Color.black;
                    _thisColor.a = 0.0f;

                    break;
                case FADE.face_out:
                    _thisColor = Color.black;
                    _thisColor.a = 1.0f;
                    break;
            }
            _loadingImage.color = _thisColor;
            _timeCount = _setTime;
            _isAnimte = true;
        }

        private void Update()
        {
            if (!_isAnimte) return;
            _loadingImage.color = _thisColor;
            switch (_fadeType)
            {
                case FADE.fade_in:
                    _thisColor.a += Time.deltaTime * _timeCount;
                    if (_thisColor.a >= 1)
                    {
                        _isAnimte = false;
                   
                    }
                    break;
                case FADE.face_out:
                    _thisColor.a -= Time.deltaTime * _timeCount;
                    if (_thisColor.a <= 0)
                    {
                        _isAnimte = false;
                   
                    }
                    break;
            }
        }
    }
}
