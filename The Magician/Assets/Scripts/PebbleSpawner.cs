using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMagician
{
    public class PebbleSpawner : MonoBehaviour
    {
        [Tooltip("Assign the pebbles that will be replaced by randomly generated sprites, rotations, etc. at their locations")]
        [SerializeField] List<Pebble> pebbles;

        [SerializeField] List<Sprite> spriteVariations;

        [Tooltip("X = min angle and Y = max angle for generating random rotations inbetween those values")]
        [SerializeField] Vector2 varyingRotationAngleMinMax;

        [System.Serializable]
        struct PebbleSprite
        {
            Sprite sprite;
            Vector2 rippleFocalPoint;
        }

        public void GeneratePebbles()
        {
            List<Sprite> spriteVariationCopy = new List<Sprite>(spriteVariations);

            foreach (Pebble pebble in pebbles)
            {
                SpriteRenderer spriteRenderer = pebble.gameObject.GetComponent<SpriteRenderer>();

                if (spriteRenderer) // Probably don't need to check for this since Pebble will require the SpriteRenderer component so that it will always exist
                {
                    if (spriteVariationCopy.Count == 0)
                    {
                        spriteVariationCopy.Clear();
                        spriteVariationCopy = new List<Sprite>(spriteVariations);
                    }

                    int index = Random.Range(0, spriteVariationCopy.Count);
                    spriteRenderer.sprite = spriteVariationCopy[index];
                    spriteVariationCopy.Remove(spriteVariationCopy[index]);
                }

                pebble.gameObject.transform.Rotate(Vector3.forward, Random.Range(varyingRotationAngleMinMax.x, varyingRotationAngleMinMax.y));

                // Set gameobjects false at the beginning until requested to turn them on
                if (pebble.gameObject.activeInHierarchy)
                {
                    pebble.gameObject.SetActive(false);
                }

                pebble.OnDestroyed += DestroyPebble;
            }

            spriteVariationCopy.Clear();
        }

        // Call this or just manually set the pebbles
        public void SetPebblesActive(bool val)
        {
            foreach(Pebble pebble in pebbles)
            {
                pebble.gameObject.SetActive(val);
            }
        }

        public void DestroyPebble(Pebble pebble)
        {
            pebble.OnDestroyed -= DestroyPebble;
            pebbles.Remove(pebble);
        }

        public void FadeInPebbles(float fadeTime)
        {
            foreach (Pebble pebble in pebbles)
            {
                SpriteAnimator animator = pebble.gameObject.GetComponent<SpriteAnimator>();

                if (animator != null)
                {
                    animator.FadeIn(fadeTime);
                }
            }
        }

        public void FadeOutPebbles(float fadeTime)
        {
            foreach(Pebble pebble in pebbles)
            {
                SpriteAnimator animator = pebble.gameObject.GetComponent<SpriteAnimator>();

                if(animator != null)
                {
                    animator.FadeOut(fadeTime);
                }
            }
        }

        public void MovePebblesFromAToB(float moveTime)
        {
            foreach(Pebble pebble in pebbles)
            {
                MovementAnimator movementAnimator = pebble.gameObject.GetComponent<MovementAnimator>();

                if(movementAnimator != null)
                {
                    movementAnimator.MoveFromAToB(moveTime);
                }
            }
        }

        public void ShouldPickUpPebbles(bool val)
        {
            foreach(Pebble pebble in pebbles)
            {
                pebble.SetShouldPickup(val);
            }
        }
    }

}