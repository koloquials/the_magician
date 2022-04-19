using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMagician
{
    public class SpriteChanger : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        
        public void ChangeSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}

