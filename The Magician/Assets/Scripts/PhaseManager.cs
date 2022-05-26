using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class PhaseManager : MonoBehaviour
    {
        [SerializeField] List<Phase> phases;
        [SerializeField] public UnityEvent OnContinuePhase;
        [SerializeField] public UnityEvent OnEndPhase;

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

            if(_currentPhaseIndex < phases.Count)
            {
                OnEndPhase?.Invoke();
                phases[_currentPhaseIndex].End();
            }

            _currentPhaseIndex++;

            if (_currentPhaseIndex < phases.Count)
            {
                Debug.Log("ENTERING PHASE: " + _currentPhaseIndex);
                phases[_currentPhaseIndex].Start();
                OnContinuePhase?.Invoke();
            }
        }
    }
}
