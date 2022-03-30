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
        [SerializeField] UnityEvent onPickUp;
        [SerializeField] UnityEvent onDropped;

        protected State State;

        protected void Awake()
        {
            State = State.NOT_PICKED_UP_YET;
        }

        public virtual bool PickUp()
        {
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
    }
}
