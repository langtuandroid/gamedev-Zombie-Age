using UnityEngine;
using UnityEngine.Serialization;

namespace MODULES
{
    public class ElectricLine : MonoBehaviour
    {
        [FormerlySerializedAs("line_renderer")] public LineRenderer _lineRenderer;
        [FormerlySerializedAs("maThunder1")] [SerializeField] private Material _thunderMaterial1;
        [FormerlySerializedAs("maThunder2")] [SerializeField] private Material _thunderMaterial2;

        public  void ShootElectric(Vector3 _start,Vector3 _target)
        {
            ThunderStart();
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, _start);
            _lineRenderer.SetPosition(1, _target);
            Invoke("ThunderRemove", 0.05f);
        }
        
        private int _materialID = 1;

        private void ThunderStart()
        {
            if (_materialID == 1)
            {
                _lineRenderer.material = _thunderMaterial1;
                _materialID = 2;
            }
            else
            {
                _lineRenderer.material = _thunderMaterial2;
                _materialID = 1;
            }
            _lineRenderer.enabled = true;
        }

        public void ThunderRemove()
        {
            _lineRenderer.enabled = false;
        }

        private void OnEnable()
        {
            ThunderRemove();
        }
    }
}
