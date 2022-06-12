using System;
using UnityEngine;
using UnityEngine.Events;


namespace TheMagician
{     
    [RequireComponent(typeof(SpriteRenderer))]
    public class Pebble : Interactable
    {
        [SerializeField] LayerMask contactLayerMask;
        [SerializeField] UnityEvent onReturnToTable;
        [SerializeField] UnityEvent onDroppedIntoBowl;

        RaycastHit2D _contactHit;
        public Action<Pebble> OnDestroyed;
        private Vector2 _rippleFocalPoint;

        protected override void Awake()
        {
            base.Awake();
            _rippleFocalPoint = Vector2.right * 0.5f + Vector2.up * 0.5f;
        }

        public override bool DroppedSuccessfully()
        {
            State = State.DROPPED;
            OnDropped?.Invoke();

            Vector3 position = gameObject.transform.position;
            Vector2 size = Vector2.right * 1.0f + Vector2.up * 1.0f;
            float angle = 0.0f; // Might change this to be the pebble's rotation
            Vector2 direction = Vector2.zero;

            _contactHit = Physics2D.BoxCast(position, size, angle, direction, Mathf.Infinity, contactLayerMask);

            if(_contactHit)
            {
                Bowl bowl = _contactHit.collider.gameObject.GetComponent<Bowl>();

                if(bowl)
                {
                    bowl.AddedPebble();
                    onDroppedIntoBowl?.Invoke();
                    return true;
                }

                return false;
            }
            else
            {
                ResetPositionAndRotation();
                onReturnToTable?.Invoke();
            }

            return false;
        }

        public override void Success()
        {
            OnDestroyed?.Invoke(this);
            base.Success();
        }

        public override void Glow()
        {
            base.Glow();
            GlowMaterial.SetVector("_FocalPoint", _rippleFocalPoint);
        }

        public void SetRippleFocalPoint(Vector2 focalPoint)
        {
            _rippleFocalPoint = focalPoint;
        }
    }
}
