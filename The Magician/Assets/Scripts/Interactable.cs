using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace TheMagician
{
    public enum State
    {
        NOT_PICKED_UP_YET,
        PICKED_UP,
        DROPPED
    }

    public class Interactable : MonoBehaviour
    {
        [SerializeField] protected UnityEvent OnFirstPickUp;
        [SerializeField] protected UnityEvent OnPickUp;
        [SerializeField] protected UnityEvent OnDropped;
        [SerializeField] protected Label OptionalLabel;
        [SerializeField] protected Material GlowMaterial;

        protected State State;
        protected Vector3 StartingPosition;
        protected Quaternion StartingRotation;

        protected bool ShouldPickup;
        protected SpriteRenderer SpriteRenderer;
        protected Material OriginalMaterial;

        protected virtual void Awake()
        {
            State = State.NOT_PICKED_UP_YET;
            StartingPosition = transform.position;
            StartingRotation = transform.rotation;
            ShouldPickup = true;
            SpriteRenderer = GetComponent<SpriteRenderer>();
            OriginalMaterial = SpriteRenderer.material;
        }

        public virtual bool PickUp()
        {
            if (State == State.NOT_PICKED_UP_YET)
            {
                if (OptionalLabel) OptionalLabel.gameObject.SetActive(false);
                OnFirstPickUp?.Invoke();
            }

            State = State.PICKED_UP;
            OnPickUp.Invoke();
            return true;
        }

        public virtual bool Dropped()
        {
            State = State.DROPPED;
            OnDropped.Invoke();
            return true;
        }

        public virtual void Destroy()
        {
            Destroy(gameObject);
        }

        public void ResetPosition()
        {
            transform.position = StartingPosition;
        }

        public void ResetRotation()
        {
            transform.rotation = StartingRotation;
        }

        public void ResetPositionAndRotation()
        {
            transform.position = StartingPosition;
            transform.rotation = StartingRotation;
        }

        public void SetShouldPickup(bool val)
        {
            ShouldPickup = val;
        }

        public void ResetState()
        {
            State = State.NOT_PICKED_UP_YET;
        }

        public void Glow()
        {
            if(GlowMaterial) SpriteRenderer.material = GlowMaterial; // if check just in case we don't assign anything
        }

        public void Unglow()
        {
            SpriteRenderer.material = OriginalMaterial;
        }
    }
}
