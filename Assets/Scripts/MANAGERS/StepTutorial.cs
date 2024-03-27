using UnityEngine;

namespace MANAGERS
{
    public class StepTutorial : MonoBehaviour
    {
        public TheTutorialManager.TUTORIAL eTutorial;
    
        public float fTimeDelay;

        private RaycastHit2D _ray;
    
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
            TheTutorialManager.Instance.CURRENT_STEP_TUTORIAL = this;
        }
    }
}
