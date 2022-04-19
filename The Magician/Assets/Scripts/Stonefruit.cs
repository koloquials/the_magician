using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class Stonefruit : Interactable
    {
        [SerializeField] Rigidbody2D rigidBody;
        [SerializeField] float holdTimeTillSlip;
        [SerializeField] float gravityScaleAfterSlipping;
        [SerializeField] float rotationTimeAfterPickup;
        [SerializeField] Vector2 varyingRotationAngleMinMax;
        [SerializeField] UnityEvent onSlipped;

        float _currentTimeHeld;

        protected override void Awake()
        {
            base.Awake();
            rigidBody.gravityScale = 0f;
            rigidBody.velocity = Vector2.zero;
        }

        private void Start()
        {
            _currentTimeHeld = 0f;
        }

        private void Update()
        {
            if (GameStateManager.INSTANCE.CurrentGameState != GameState.GAMEPLAY) return;

            if(State == State.PICKED_UP)
            {
                _currentTimeHeld += Time.deltaTime;

                if(_currentTimeHeld >= holdTimeTillSlip)
                {
                    State = State.DROPPED;
                    onSlipped?.Invoke();
                    rigidBody.gravityScale = gravityScaleAfterSlipping;
                }
            }
        }

        public override bool PickUp()
        {
            if (State == State.NOT_PICKED_UP_YET)
            {
                if (optionalLabel) optionalLabel.gameObject.SetActive(false);
                OnFirstPickUp?.Invoke();
            }

            State = State.PICKED_UP;
            onPickUp.Invoke();

            rigidBody.gravityScale = 0f;

            return true;
        }

        public override bool Dropped()
        {
            State = State.DROPPED;
            _currentTimeHeld = 0f; // Reset time
            rigidBody.gravityScale = 0f;
            rigidBody.velocity = Vector2.zero;
            return false;
        }

        public void ResetRigidbody()
        {
            rigidBody.gravityScale = 0f;
            rigidBody.velocity = Vector2.zero;
        }
    }
}
