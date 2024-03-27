using UnityEngine;

namespace MANAGERS
{
    public class TutorialPage : MonoBehaviour
    {
       
        [SerializeField] private TutorialController.TUTORIAL eTutorial;
        [SerializeField] private float fTimeDelay;
        private RaycastHit2D _ray;
        public TutorialController.TUTORIAL TutorialState => eTutorial;
        public float Delay => fTimeDelay;
        public bool IsRightInputPos()
        {
            _ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (_ray.collider != null && _ray.collider.CompareTag("isTutorial"))
            {
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            TutorialController.Instance._currentStep = this;
        }
    }
}
