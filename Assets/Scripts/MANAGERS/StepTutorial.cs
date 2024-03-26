using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTutorial : MonoBehaviour
{
    public TheTutorialManager.TUTORIAL eTutorial;
    
    public float fTimeDelay;

    RaycastHit2D _ray ;
    // Start is called before the first frame update


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
