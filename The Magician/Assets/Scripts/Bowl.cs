using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    //[RequireComponent(typeof(Animator))]
    public class Bowl : MonoBehaviour
    {
        [SerializeField] UnityEvent onComplete;
        [SerializeField] UnityEvent onAddPebble;
        [SerializeField] Animator animator;

        int _numPebblesToDropTillCompletion = 1;
        int _currentNumPebbledDroppedInside = 0;

        private void Start()
        {
            GameStateManager.OnPause.AddListener(PauseAnimator);
            GameStateManager.OnUnpause.AddListener(UnpauseAnimator);
        }

        private void OnDestroy()
        {
            GameStateManager.OnPause.RemoveListener(PauseAnimator);
            GameStateManager.OnUnpause.RemoveListener(UnpauseAnimator);
        }

        public void AddedPebble()
        {
            _currentNumPebbledDroppedInside++;
            onAddPebble?.Invoke();
            animator.Play("Ripple");

            if(_currentNumPebbledDroppedInside >= _numPebblesToDropTillCompletion)
            {
                _currentNumPebbledDroppedInside = 0; // Reset
                PhaseManager.INSTANCE.StartNextPhase();
            }
        }

        public void SetPebbleAmountGoal(int val)
        {
            _numPebblesToDropTillCompletion = val;
            _currentNumPebbledDroppedInside = 0;
        }

        public void UnpauseAnimator()
        {
            animator.speed = 1f;
        }

        public void PauseAnimator()
        {
            animator.speed = 0f;
        }
    }
}
