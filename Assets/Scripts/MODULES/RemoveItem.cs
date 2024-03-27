using UnityEngine;

namespace MODULES
{
    public class RemoveItem : MonoBehaviour
    {
        [SerializeField] SpriteRenderer sprItems;
    
        public void SetItem(Sprite _sprite)
        {
            sprItems.sprite = _sprite;
        }

    }
}
