using UnityEngine;
using UnityEngine.Serialization;

namespace MODULES
{
    public class TextValue : MonoBehaviour
    {
        [FormerlySerializedAs("m_TextMesh")] [SerializeField]
        private TextMesh _textMesh;

        public  void SetText(int _value)
        {
            _textMesh.text = "-" + _value.ToString();
        }
    }
}
