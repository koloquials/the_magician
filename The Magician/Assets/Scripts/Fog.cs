using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace TheMagician
{
    public class Fog : Interactable
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] float opacityDecreaseAmount;
        [SerializeField] float fogClearThreshold;
        [SerializeField] UnityEvent onFogClear;
        [SerializeField] UnityEvent onFogUpdate;

        float _currentOpacity;
        float _opacity = 1f;

        protected override void Awake()
        {
            base.Awake();

            _opacity = 1f;
            _currentOpacity = _opacity;
        }

        private void Update()
        {
            if (GameStateManager.INSTANCE.CurrentGameState != GameState.GAMEPLAY) return;
            if (!ShouldPickup) return;
        }

        // This lowers opacity
        public override bool PickUp()
        {
            if (GameStateManager.INSTANCE.CurrentGameState != GameState.GAMEPLAY) return false;
            if (!ShouldPickup) return false;

            _currentOpacity -= opacityDecreaseAmount;

            _currentOpacity = Mathf.Clamp(_currentOpacity, 0f, 1f);

            Color color = spriteRenderer.color;
            color.a = _currentOpacity;
            spriteRenderer.color = color;

            onFogUpdate?.Invoke();

            if(_currentOpacity <= fogClearThreshold)
            {
                onFogClear?.Invoke();
            }

            return false;
        }

        public override void Glow()
        {
            // Do nothing
        }

        public override void Unglow()
        {
            // Do nothing
        }
    }
}
