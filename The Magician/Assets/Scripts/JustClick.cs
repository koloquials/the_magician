using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class JustClick : Interactable
    {
        [SerializeField] UnityEvent onClicked;

        protected override void Awake()
        {
            base.Awake();
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

            onClicked?.Invoke();

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
