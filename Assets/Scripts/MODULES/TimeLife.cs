using UnityEngine;

namespace MODULES
{
    public enum TYPE
    {
        Destroy,
        Active,
    }
    public class TimeLife : MonoBehaviour
    {
        public TYPE eType;
        public float fTimeLife;
        private float fCountTime;
        private GameObject _gameobject;

        private void Awake()
        {
            _gameobject = gameObject;
        }
        // Update is called once per frame
        void Update()
        {
            fCountTime -= Time.deltaTime;
            if(fCountTime<=0)
            {
                fCountTime = fTimeLife;
                switch(eType)
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
            fCountTime = fTimeLife;
        }

    }
}