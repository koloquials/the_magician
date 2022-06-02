using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class TwoStepBowl : Bowl
    {
        [SerializeField] UnityEvent onAddFirstPebble;
        //[SerializeField] UnityEvent onAddSecondPebble;

        public override void AddedPebble()
        {
            _currentNumPebbledDroppedInside++;
            onAddPebble?.Invoke();
            animator.Play("Ripple");

            switch(_currentNumPebbledDroppedInside)
            {
                case 1:
                    onAddFirstPebble?.Invoke();
                    break;
                case 2:
                    PhaseManager.INSTANCE.StartNextPhase();
                break;
                default:
                break;
            }
        }

        public void ResetPebbleInBowlCount()
        {
            _currentNumPebbledDroppedInside = 0;
        }
    }
}
