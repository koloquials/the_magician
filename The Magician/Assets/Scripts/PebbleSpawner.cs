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

        private void Awake()
        {
            foreach(Pebble pebble in pebbles)
            {
                SpriteRenderer sr = pebble.gameObject.GetComponent<SpriteRenderer>();
                if(sr) // Probably don't need to check for this since Pebble will require the SpriteRenderer component so that it will always exist
                {
                    // TODO: Don't repeat variations if already set
                    sr.sprite = spriteVariations[Random.Range(0, spriteVariations.Count)];
                }

                pebble.gameObject.transform.Rotate(Vector3.forward, Random.Range(varyingRotationAngleMinMax.x, varyingRotationAngleMinMax.y));

                // Set gameobjects false at the beginning until requested to turn them on
                if(pebble.gameObject.activeInHierarchy)
                {
                    pebble.gameObject.SetActive(false);
                }
            }
        }

        // Call this or just manually set the pebbles
        public void SetPebblesActive(bool val)
        {
            foreach(Pebble pebble in pebbles)
            {
                pebble.gameObject.SetActive(val);
            }
        }
    }

}