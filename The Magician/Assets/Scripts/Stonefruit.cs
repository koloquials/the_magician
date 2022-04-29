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
        float _currentRotationAngle;
        float _desiredRotationAngle;
        bool _hasSlipped;

        protected override void Awake()
        {
            base.Awake();
            rigidBody.gravityScale = 0f;
            rigidBody.velocity = Vector2.zero;
            _hasSlipped = false;
        }

        protected override void Start()
        {
            base.Start();
            _currentTimeHeld = 0f;
        }

        private void Update()
        {
            if (GameStateManager.INSTANCE.CurrentGameState != GameState.GAMEPLAY) return;
            if (!ShouldPickup) return;

            if(State == State.PICKED_UP)
            {
                _currentTimeHeld += Time.deltaTime;
                _currentRotationAngle += Time.deltaTime;
                float rotationProgress = Mathf.Clamp(_currentRotationAngle, 0f, rotationTimeAfterPickup);
                transform.rotation = Quaternion.Slerp(StartingRotation, Quaternion.AngleAxis(_desiredRotationAngle, Vector3.forward), rotationProgress / rotationTimeAfterPickup);

                if(_currentTimeHeld >= holdTimeTillSlip)
                {
                    State = State.DROPPED;
                    onSlipped?.Invoke();
                    rigidBody.gravityScale = gravityScaleAfterSlipping;
                    _currentTimeHeld = 0f;
                    _hasSlipped = true;
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
            _hasSlipped = false;
            ResetRigidbody();
            return false;
        }

        public override void SetShouldPickup(bool val)
        {
            ShouldPickup = val;
            if (val) _hasSlipped = false;
        }

        public void ResetRigidbody()
        {
            rigidBody.gravityScale = 0f;
            rigidBody.velocity = Vector2.zero;
        }

        public override void Glow()
        {
            if (!ShouldPickup || _hasSlipped) return;
            if (GlowMaterial) SpriteRenderer.material = GlowMaterial; // if check just in case we don't assign anything
        }
    }
}
