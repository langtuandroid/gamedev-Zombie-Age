using _4_Gameplay;
using UnityEngine;

namespace MODULES.Soldiers.Bullets
{
    public class BulletFire : MonoBehaviour
    {
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            _scale *= 0.05f;
        }

        
        Vector3 _scale = new (1, 1, 1);
        float _angle = -8.0f;
        Vector3 _euler;
        void Update()
        {
            if (Time.timeScale != 1.0f) return;//debug

            if (GameplayController.Instance.GameStatus != GameplayController.GAME_STATUS.playing) return;
            _euler.z += _angle;
            _transform.localScale += _scale;
            _transform.eulerAngles = _euler;
        }

        private void OnEnable()
        {
            _transform.localScale = Vector3.one;
        }
    }
}
