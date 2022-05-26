using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class MovementAnimator : MonoBehaviour
    {
        [SerializeField] bool useStartingPositionAsA;
        [SerializeField] Vector3 positionA;
        [SerializeField] bool manuallySetTransformBPosition;
        [SerializeField] Vector3 positionB;
        [SerializeField] Vector3 manuallySetBPositionOffset;
        [SerializeField] AnimationCurve movementSpeedCurve;
        [SerializeField] UnityEvent onReachTargetPosition;
        [SerializeField] GameState activeGameState;

        bool _isMoving;
        float _currentTime;
        float _moveTime;
        Vector3 _startPosition;
        Vector3 _targetPosition;

        private void Awake()
        {
            _isMoving = false;
            _currentTime = 0f;
            if(useStartingPositionAsA)
            {
                positionA = transform.position;
            }

            if(manuallySetTransformBPosition)
            {
                positionB = transform.position;
                positionB += manuallySetBPositionOffset;
            }
        }

        private void Start()
        {
            //_startPosition = gameObject.transform.position; 5/24 - refactor out
            PhaseManager.INSTANCE.OnEndPhase.AddListener(Complete);
        }

        private void OnDestroy()
        {
            PhaseManager.INSTANCE.OnEndPhase.RemoveListener(Complete);
        }

        private void Update()
        {
            if (!_isMoving) return;
            if (!GameStateManager.IsInGameModeState()) return;

            _currentTime += Time.deltaTime;
            _currentTime = Mathf.Clamp(_currentTime, 0.0f, _moveTime);
            float normalizedTime = (_currentTime / _moveTime);
            Vector3 newPos = Vector3.Lerp(_startPosition, _targetPosition, movementSpeedCurve.Evaluate(normalizedTime));
            gameObject.transform.position = newPos;

            if (_currentTime >= _moveTime)
            {
                onReachTargetPosition.Invoke();
                _isMoving = false;
            }
        }

        // This will be called as an event in the inspector
        public void MoveFromAToB(float moveTime)
        {
            _currentTime = 0f;
            _isMoving = true;
            _moveTime = moveTime;
            _startPosition = positionA;
            _targetPosition = positionB;
        }

        public void MoveFromBToA(float moveTime)
        {
            _currentTime = 0f;
            _isMoving = true;
            _moveTime = moveTime;
            _startPosition = positionB;
            _targetPosition = positionA;
        }

        public void Complete()
        {
            if (!_isMoving) return;
            if (!GameStateManager.IsInGameModeState()) return;
            _currentTime = _moveTime;
        }
    }
}
