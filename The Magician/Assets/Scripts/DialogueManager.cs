using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn;
using Yarn.Unity;

namespace TheMagician
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] DialogueRunner dialogueRunner;
        [SerializeField] LineView lineView;

        public static DialogueManager INSTANCE;

        private void Awake()
        {
            if(!INSTANCE) INSTANCE = this;
        }

        private void Start()
        {
            
        }

        // This is set under MouseController's OnClickDuringDialogue event
        public void ContinueDialogue()
        {
            lineView.OnContinueClicked();
        }

        public bool NodeExists(string nodeName)
        {
            return dialogueRunner.NodeExists(nodeName);
        }

        public void StopDialogue()
        {
            lineView.StopAllCoroutines();
            dialogueRunner.Stop();
        }

        public void StartDialogue(string startNode)
        {
            dialogueRunner.StartDialogue(startNode);
        }
    }
}
