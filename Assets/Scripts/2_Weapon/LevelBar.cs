using UnityEngine;
using UnityEngine.UI;

namespace _2_Weapon
{
    public class LevelBar : MonoBehaviour
    {
        [SerializeField] private Image[] _bars;

        public void SetLevel(int level)
        {
            for (int i = 0; i < _bars.Length; i++)
            {
                _bars[i].gameObject.SetActive(i < level);
            }
        }
    }
}
