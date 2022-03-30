using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private void Awake()
        {
            if (!INSTANCE) INSTANCE = this;
            _currentGameState = startingState;
            HubAPI.OnGamePaused += Pause;
        }

        private void OnDestroy()
        {
            HubAPI.OnGamePaused -= Pause;
        }

        public static void SetGameState(GameState gameState)
        {
            _previousGameState = _currentGameState;
            _currentGameState = gameState;
        }

        public static void Pause(bool shouldPause)
        {
            if (shouldPause)
                SetGameState(GameState.PAUSED);
            else
                SetGameState(_previousGameState);
        }
    }
}
