using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class JustTime : MonoBehaviour
    {
        [SerializeField] UnityEvent onFinish;
        float _currentTime;
        float _totalTime;
        bool _isRunning;

        private void Awake()
        {
            _currentTime = 0f;
            _isRunning = false;
        }

        private void Update()
        {
            if (!_isRunning) return;
            if (!GameStateManager.IsInGameModeState()) return;

            _currentTime += Time.deltaTime;
            
            if(_currentTime >= _totalTime)
            {
                _isRunning = false;
                _currentTime = 0f;
                DialogueManager.INSTANCE.ContinueDialogue();
                onFinish?.Invoke();
            }
        }

        public void StartTicking(float totalTime)
        {
            _isRunning = true;
            _currentTime = 0f;
            _totalTime = totalTime;
        }
    }
}
