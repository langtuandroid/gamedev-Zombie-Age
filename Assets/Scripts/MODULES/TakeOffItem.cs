using UnityEngine;
using UnityEngine.Serialization;

namespace MODULES
{
    public class TakeOffItem : MonoBehaviour
    {
        [FormerlySerializedAs("sprItems")] [SerializeField] SpriteRenderer _sprite;
    
        public void SetSprite(Sprite _sprite)
        {
            this._sprite.sprite = _sprite;
        }

    }
}
