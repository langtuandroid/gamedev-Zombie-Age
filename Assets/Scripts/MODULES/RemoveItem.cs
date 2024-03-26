using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprItems;
    
    public void SetItem(Sprite _sprite)
    {
        sprItems.sprite = _sprite;
    }

}
