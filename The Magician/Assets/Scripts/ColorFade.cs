using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMagician
{
    //TODO: finish this so we can make bg's black
    public class ColorFade : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;

        float _currentFadeTime;

        void Start()
        {
            _currentFadeTime = 0f;
        }

        void Update()
        {

        }
    }
}
