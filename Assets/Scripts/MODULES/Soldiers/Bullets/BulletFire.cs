using _4_Gameplay;
using UnityEngine;

namespace MODULES.Soldiers.Bullets
{
    public class BulletFire : MonoBehaviour
    {
        private Transform m_tranform;

        private void Awake()
        {
       
            m_tranform = transform;
            _scale *= 0.05f;
        }


        // Update is called once per frame
        Vector3 _scale = new Vector3(1, 1, 1);
        float _angle = -8.0f;
        Vector3 _euler = new Vector3();
        void Update()
        {
            if (Time.timeScale != 1.0f) return;//debug

            if (MainCode_Gameplay.Instance.eGameStatus != MainCode_Gameplay.GAME_STATUS.playing) return;
            _euler.z += _angle;
            m_tranform.localScale += _scale;
            m_tranform.eulerAngles = _euler;
        }

        private void OnEnable()
        {
            m_tranform.localScale = Vector3.one;
        }
    }
}
