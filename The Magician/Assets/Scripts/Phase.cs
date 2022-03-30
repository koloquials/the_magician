using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace TheMagician
{
    [System.Serializable]
    public class Phase
    {
        [System.Serializable]
        struct Subphase
        {
            [SerializeField] public bool isDialogue;
            [SerializeField] public string dialogueNodeName;
            [SerializeField] public UnityEvent onStartSubphase;
            [SerializeField] public UnityEvent onEndSubphase;
        }

        [SerializeField] List<Subphase> subphases;

        int _currentSubphaseIndex = 0;

        public void Start()
        {
            if(subphases.Count > 0)
            {
                _currentSubphaseIndex = 0;
                BeginSubphase(subphases[_currentSubphaseIndex]);
            }
        }

        // Returns true if we can advance to next subphase, otherwise false
        public bool Advance()
        {
            EndSubphase(subphases[_currentSubphaseIndex]);
            _currentSubphaseIndex++;

            if (_currentSubphaseIndex >= subphases.Count) return false;
            else
            {
                BeginSubphase(subphases[_currentSubphaseIndex]);
                return true;
            }
        }

        private void BeginSubphase(Subphase subphase)
        {
            subphase.onStartSubphase.Invoke();

            if (subphase.isDialogue)
            {
                if(DialogueManager.INSTANCE.NodeExists(subphase.dialogueNodeName))
                {
                    GameStateManager.SetGameState(GameState.DIALOGUE);
                    DialogueManager.INSTANCE.StartDialogue(subphase.dialogueNodeName);
                }
                else
                {
                    Debug.LogError("ERROR: " + subphase.dialogueNodeName + " does not exist as a node title in Yarn script.");
                }
            }
            else GameStateManager.SetGameState(GameState.GAMEPLAY);
        }

        private void EndSubphase(Subphase subphase)
        {
            subphase.onEndSubphase.Invoke();

            if (subphase.isDialogue)
            {
                DialogueManager.INSTANCE.StopDialogue();
            }
        }
    }
}
