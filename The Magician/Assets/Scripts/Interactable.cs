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
        [SerializeField] protected UnityEvent onPickUp;
        [SerializeField] protected UnityEvent onDropped;
        [SerializeField] protected Label optionalLabel;

        protected State State;
        protected Vector3 StartingPosition;
        protected Quaternion StartingRotation;

        protected virtual void Awake()
        {
            State = State.NOT_PICKED_UP_YET;
            StartingPosition = transform.position;
            StartingRotation = transform.rotation;
        }

        public virtual bool PickUp()
        {
            if (State == State.NOT_PICKED_UP_YET)
            {
                if (optionalLabel) optionalLabel.gameObject.SetActive(false);
                OnFirstPickUp?.Invoke();
            }

            State = State.PICKED_UP;
            onPickUp.Invoke();
            return true;
        }

        public virtual bool Dropped()
        {
            State = State.DROPPED;
            onDropped.Invoke();
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
    }
}
