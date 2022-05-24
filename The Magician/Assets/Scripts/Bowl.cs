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

        public void AddedPebble()
        {
            _currentNumPebbledDroppedInside++;
            onAddPebble?.Invoke();

            if(_currentNumPebbledDroppedInside >= _numPebblesToDropTillCompletion)
            {
                onComplete.Invoke();
                _currentNumPebbledDroppedInside = 0; // Reset
            }
        }

        public void SetPebbleAmountGoal(int val)
        {
            _numPebblesToDropTillCompletion = val;
            _currentNumPebbledDroppedInside = 0;
        }

        /*public void PlayWaterRipple()
        {
            animator.Play("WaterRipple");
        }*/
    }
}
