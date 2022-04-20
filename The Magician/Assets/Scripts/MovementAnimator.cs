using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class MovementAnimator : MonoBehaviour
    {
        [SerializeField] Vector3 targetPosition;
        [SerializeField] float moveToTargetTime;
        [SerializeField] AnimationCurve movementSpeedCurve;
        [SerializeField] UnityEvent onReachTargetPosition;
        [SerializeField] GameState activeGameState;

        bool _isMoving;
        float _currentTime;
        Vector3 _startPosition;

        private void Start()
        {
            _currentTime = 0.0f;
            _isMoving = false;
            _startPosition = gameObject.transform.position;
        }

        private void Update()
        {
            if (!_isMoving) return;
            if (!GameStateManager.IsInGameModeState()) return;

            _currentTime += Time.deltaTime;
            _currentTime = Mathf.Clamp(_currentTime, 0.0f, moveToTargetTime);
            float normalizedTime = (_currentTime / moveToTargetTime);
            Vector3 newPos = Vector3.Lerp(_startPosition, targetPosition, movementSpeedCurve.Evaluate(normalizedTime));
            gameObject.transform.position = newPos;

            if (_currentTime >= moveToTargetTime)
            {
                onReachTargetPosition.Invoke();
                _isMoving = false;
            }
        }

        // This will be called as an event in the inspector
        public void MoveToTarget()
        {
            _isMoving = true;
        }
    }
}
