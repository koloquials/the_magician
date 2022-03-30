using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheMagician
{     
    [RequireComponent(typeof(SpriteRenderer))]
    public class Pebble : Interactable
    {
        [SerializeField] LayerMask contactLayerMask;

        RaycastHit2D _contactHit;

        public override bool Dropped()
        {
            State = State.DROPPED;

            // Check if dropped onto bowl
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
                }

                return true;
            }

            return false;
        }
    }
}
