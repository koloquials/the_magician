using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class Bowl : MonoBehaviour
    {
        [SerializeField] UnityEvent onComplete;

        int _numPebblesToDropTillCompletion = 1;
        int _currentNumPebbledDroppedInside = 0;
        public void AddedPebble()
        {
            _currentNumPebbledDroppedInside++;

            if(_currentNumPebbledDroppedInside >= _numPebblesToDropTillCompletion)
            {
                onComplete.Invoke();
            }
        }

        public void SetPebbleAmountGoal(int val)
        {
            _numPebblesToDropTillCompletion = val;
            _currentNumPebbledDroppedInside = 0;
        }
    }
}
