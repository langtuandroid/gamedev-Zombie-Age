using UnityEngine;

namespace MODULES
{
    public class ElectricLine : MonoBehaviour
    {
        public LineRenderer line_renderer;
        [SerializeField] Material maThunder1, maThunder2;
   

        public  void Shot(Vector3 _start,Vector3 _target)
        {

            #region SHOT
            //animation of body

            //--------------------
      

            ShowThunder();
            line_renderer.positionCount = 2;
            line_renderer.SetPosition(0, _start);
            line_renderer.SetPosition(1, _target);
            Invoke("HideThunder", 0.05f);
            #endregion

        }


        private int iIndexOfMaterial = 1;
        public void ShowThunder()
        {
            if (iIndexOfMaterial == 1)
            {
                line_renderer.material = maThunder1;
                iIndexOfMaterial = 2;
            }
            else
            {
                line_renderer.material = maThunder2;
                iIndexOfMaterial = 1;
            }
            line_renderer.enabled = true;
        }

        public void HideThunder()
        {
            line_renderer.enabled = false;
        }

        private void OnEnable()
        {
            HideThunder();
        }
    }
}
