using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class Flute : Interactable
    {
        [SerializeField] Rigidbody2D rigidBody;
        [SerializeField] float holdTimeTillSlip;
        [SerializeField] float gravityScaleAfterSlipping;
        [SerializeField] float rotationTimeAfterPickup;
        [SerializeField] Vector2 varyingRotationAngleMinMax;
        [SerializeField] UnityEvent onSlipped;

        float _currentTimeHeld;
        float _currentRotationAngle;
        float _desiredRotationAngle;

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
            if (!ShouldPickup) return;

            if (State == State.PICKED_UP)
            {
                _currentTimeHeld += Time.deltaTime;
                _currentRotationAngle += Time.deltaTime;
                float rotationProgress = Mathf.Clamp(_currentRotationAngle, 0f, rotationTimeAfterPickup);
                transform.rotation = Quaternion.Slerp(StartingRotation, Quaternion.AngleAxis(_desiredRotationAngle, Vector3.forward), rotationProgress / rotationTimeAfterPickup);

                if (_currentTimeHeld >= holdTimeTillSlip)
                {
                    State = State.DROPPED;
                    onSlipped?.Invoke();
                    rigidBody.gravityScale = gravityScaleAfterSlipping;
                    _currentTimeHeld = 0f;
                }
            }
        }

        public override bool PickUp()
        {
            if (!ShouldPickup) return false;

            if (State == State.NOT_PICKED_UP_YET)
            {
                if (OptionalLabel) OptionalLabel.gameObject.SetActive(false);
                OnFirstPickUp?.Invoke();
            }

            State = State.PICKED_UP;
            OnPickUp.Invoke();

            rigidBody.gravityScale = 0f;
            _desiredRotationAngle = Random.Range(varyingRotationAngleMinMax.x, varyingRotationAngleMinMax.y);
            _currentRotationAngle = 0f;
            return true;
        }

        public override bool Dropped()
        {
            State = State.DROPPED;
            _currentTimeHeld = 0f; // Reset time
            ResetRigidbody();
            return false;
        }

        public void ResetRigidbody()
        {
            rigidBody.gravityScale = 0f;
            rigidBody.velocity = Vector2.zero;
        }
    }
}
