using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] GameState startingState; // Probably irrelevant to set since PhaseManager will set the game state

        public static GameStateManager INSTANCE;

        private static GameState _currentGameState;
        private static GameState _previousGameState;
        public GameState CurrentGameState => _currentGameState;
        public GameState PreviousGameState => _previousGameState;

        public static UnityEvent OnPause;
        public static UnityEvent OnUnpause;

        private void Awake()
        {
            if (!INSTANCE) INSTANCE = this;
            _currentGameState = startingState;
            HubAPI.OnGamePaused += Pause;
            OnPause = new UnityEvent();
            OnUnpause = new UnityEvent();
        }

        private void OnDestroy()
        {
            HubAPI.OnGamePaused -= Pause;
        }
        private void Update()
        {
            Debug.Log("Current game state: " + _currentGameState);
        }

        public static void SetGameState(GameState gameState)
        {
            _previousGameState = _currentGameState;
            _currentGameState = gameState;
        }

        public static void Pause(bool shouldPause)
        {
            if (shouldPause)
            {
                SetGameState(GameState.PAUSED);
                Time.timeScale = 0.0f; // These will probably work just fine, especially for stopping yarnspinner text
                OnPause?.Invoke();
            }
            else
            {
                SetGameState(_previousGameState);
                Time.timeScale = 1.0f;
                OnUnpause?.Invoke();
            }
        }
    }
}
