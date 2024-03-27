using UnityEngine;

namespace MODULES.Soldiers.Bullets
{
    public class SmockOfBazoka : MonoBehaviour
    {
        private GameObject _gameobject;
        private SpriteRenderer _spriteRenderder;
        private Transform _transform;
        private Vector2 _currentPos;
        private Color _color;
        private Vector3 _startScale;
        private Vector3 _currentScale;
        private readonly Vector3 _temp = new Vector2(0.01f, 0.01f);

        private void Awake()
        {
            _spriteRenderder = GetComponent<SpriteRenderer>();
            _gameobject = gameObject;
            _transform = transform;
            _startScale = _transform.localScale;

        }

        private void Update()
        {
            //debug
            if (Time.timeScale != 1.0f) return;

            _color.a -= 0.008f;
            _spriteRenderder.color = _color;
            if (_color.a <= 0)
            {
                _gameobject.SetActive(false);
            }

            _currentPos.y += 0.01f;
            _transform.position = _currentPos;
            _currentScale -= _temp;
            _transform.localScale = _currentScale;
        }

        private void OnEnable()
        {
            _transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            _currentScale = _startScale;
            _currentPos = _transform.position;
            _color = Color.white;
        }

    }
}
