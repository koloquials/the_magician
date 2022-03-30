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
            }
        }

        public void StartNextPhase()
        {
            _currentPhaseIndex++;

            if(_currentPhaseIndex < phases.Count)
            {
                phases[_currentPhaseIndex].Start();
            }
        }

        public void Advance()
        {
           if(!phases[_currentPhaseIndex].Advance())
           {
                StartNextPhase();
           }
        }
    }
}
