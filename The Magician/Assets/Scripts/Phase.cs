using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Yarn.Unity;

namespace TheMagician
{
    [System.Serializable]
    public class Phase
    {
        /*[System.Serializable]
        struct Subphase
        {
            [SerializeField] public bool isDialogue;
            [SerializeField] public string dialogueNodeName;
            [SerializeField] public UnityEvent onStartSubphase;
            [SerializeField] public UnityEvent onEndSubphase;
        }

        [SerializeField] List<Subphase> subphases;

        int _currentSubphaseIndex = 0;*/

        [Tooltip("If isDialogue is set to false, gameplay (i.e. interaction phase) will be assumed")]
        [SerializeField] public bool isDialogue;
        [SerializeField] public string dialogueNodeName;

        [Tooltip("Just like it's named, it's just a note for seeing a description of what this phase is supposed to do. Only meant to be readable in inspector")]
        [SerializeField, TextArea] public string phaseDescription;
        [SerializeField] public UnityEvent onStartPhase;
        [SerializeField] public UnityEvent onEndPhase;

        public void Start()
        {
            /*if(subphases.Count > 0)
            {
                _currentSubphaseIndex = 0;
                BeginSubphase(subphases[_currentSubphaseIndex]);
            }*/

            onStartPhase?.Invoke();

            if (isDialogue)
            {
                if (DialogueManager.INSTANCE.NodeExists(dialogueNodeName))
                {
                    //GameStateManager.SetGameState(GameState.DIALOGUE); 5/23 - refactoring out
                    DialogueManager.INSTANCE.StartDialogue(dialogueNodeName);
                }
                else
                {
                    Debug.LogError("ERROR: " + dialogueNodeName + " does not exist as a node title in Yarn script.");
                }
            }
            //else GameStateManager.SetGameState(GameState.GAMEPLAY); 5/23 - refactoring out

            GameStateManager.SetGameState(GameState.GAMEPLAY);
        }

        // Returns true if we can advance to next subphase, otherwise false
        /*public bool Advance()
        {
            EndSubphase(subphases[_currentSubphaseIndex]);
            _currentSubphaseIndex++;

            if (_currentSubphaseIndex >= subphases.Count) return false;
            else
            {
                BeginSubphase(subphases[_currentSubphaseIndex]);
                return true;
            }
        }*/

        /*private void BeginSubphase(Subphase subphase)
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
        }*/

        /*private void EndSubphase(Subphase subphase)
        {
            subphase.onEndSubphase.Invoke();

            if (subphase.isDialogue)
            {
                DialogueManager.INSTANCE.StopDialogue();
            }
        }*/

        public void End()
        {
            onEndPhase?.Invoke();

            if(isDialogue)
            {
                DialogueManager.INSTANCE.StopDialogue();
            }
        }
    }
}
