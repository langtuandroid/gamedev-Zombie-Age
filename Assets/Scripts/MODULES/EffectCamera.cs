using UnityEngine;

namespace MODULES
{
    public class EffectCamera : MonoBehaviour
    {
        public static EffectCamera Instance;

        public enum LEVEL
        {
            level_1,
            level_2,
            level_3,
            level_4,
            level_5,
            level_6,
        }


        private Transform m_tranform;
        private Vector3 vOriginalPos;


  

        public bool bAllowshaking = false;
        // How long the object should shake for.
        private float shakeDuration = 0f;

        // Amplitude of the shake. A larger value shakes the camera harder.
        private float shakeAmount = 0.1f;
        private float decreaseFactor = 0.5f;
        void Awake()
        {
            m_tranform = GetComponent<Transform>();
 
            if (Instance == null)
                Instance = this;
        }



   

        // Update is called once per frame
        void Update()
        {
            //    if (!bPlayAnimationOnStartGameDone)
            //    {
            //        m_tranform.position = Vector3.Lerp(m_tranform.position, vOriginalPos, 0.22f);
            //        if (m_tranform.position.x - vOriginalPos.x < 0.01f)
            //        {
            //            m_tranform.position = vOriginalPos;
            //            bPlayAnimationOnStartGameDone = true;
            //            bAllowshaking = true;
            //        }
            //    }



            if (bAllowshaking)
            {
                if (shakeDuration > 0)
                {
                    m_tranform.localPosition = vOriginalPos + Random.insideUnitSphere * shakeAmount;

                    shakeDuration -= Time.deltaTime * decreaseFactor;
                }
                else
                {
                    shakeDuration = 0f;
                    m_tranform.localPosition = vOriginalPos;
                    bAllowshaking = false;
                }
            }

        }





        private void OnEnable()
        {
            vOriginalPos = m_tranform.position;
       
        }


        private void OnDisable()
        {

            Instance = null;
        }



        public void ShakingCamera(LEVEL _level)
        {

            ShakingCamera(0.03f * ((int)_level + 1));
            bAllowshaking = true;
        }

        private void ShakingCamera(float _shakeDuration)
        {
            shakeDuration = _shakeDuration;
        }


    }
}
