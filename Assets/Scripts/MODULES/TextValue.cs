using UnityEngine;

namespace MODULES
{
    public class TextValue : MonoBehaviour
    {
        [SerializeField] TextMesh m_TextMesh;

        public  void SetValue(int _value)
        {
            m_TextMesh.text = "-" + _value.ToString();
        }
    }
}
