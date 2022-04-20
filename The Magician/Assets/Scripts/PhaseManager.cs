using System.Collections.Generic;
using UnityEngine;

namespace TheMagician
{
    public class PhaseManager : MonoBehaviour
    {
        [SerializeField] List<Phase> phases;

        public static PhaseManager INSTANCE;

        int _currentPhaseIndex;

        private void Awake()
        {
            if (!INSTANCE) INSTANCE = this;
        }

        private void Start()
        {
            if(phases.Count > 0)
            {
                _currentPhaseIndex = 0;
                phases[_currentPhaseIndex].Start();
                Debug.Log("Phase: 0");
            }
        }

        /*public void StartNextPhase()
        {
            _currentPhaseIndex++;

            if(_currentPhaseIndex < phases.Count)
            {
                phases[_currentPhaseIndex].Start();
            }
        }*/

        public void StartNextPhase()
        {
            /*if(!phases[_currentPhaseIndex].Advance())
            {
                 StartNextPhase();
            }*/

            phases[_currentPhaseIndex].End();

            _currentPhaseIndex++;

            if (_currentPhaseIndex < phases.Count)
            {
                phases[_currentPhaseIndex].Start();
                Debug.Log("Phase: " + _currentPhaseIndex);
            }
        }
    }
}
