using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMagician
{
    public class WaterRipple : MonoBehaviour
    {
        [SerializeField] Animator animator;

        private void Start()
        {
            GameStateManager.OnPause.AddListener(PauseAnimator);
            GameStateManager.OnUnpause.AddListener(UnpauseAnimator);
        }

        private void OnDestroy()
        {
            GameStateManager.OnPause.RemoveListener(PauseAnimator);
            GameStateManager.OnUnpause.RemoveListener(UnpauseAnimator);
        }

        public void UnpauseAnimator()
        {
            animator.speed = 1f;
        }

        public void PauseAnimator()
        {
            animator.speed = 0f;
        }
    }
}
