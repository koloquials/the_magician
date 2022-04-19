using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMagician
{
    public class Label : MonoBehaviour
    {
        [SerializeField] float floatSpeedMultiplier;
        [SerializeField] float floatAmplitude;

        float _angle;
        Vector3 _startingPosition;

        private void Start()
        {
            _angle = 0f;
            _startingPosition = transform.position;
        }

        private void Update()
        {
            if (GameStateManager.INSTANCE.CurrentGameState != GameState.GAMEPLAY) return;

            _angle += Time.deltaTime * floatSpeedMultiplier;

            if(_angle >= 360.0f) // Don't just let _angle increase on forever, loop it back starting at 0
            {
                _angle -= 360.0f;
            }

            Vector3 position = _startingPosition + Vector3.up * Mathf.Sin(_angle) * floatAmplitude;
            transform.position = position;
            transform.rotation = Quaternion.identity;
        }
    }
}
