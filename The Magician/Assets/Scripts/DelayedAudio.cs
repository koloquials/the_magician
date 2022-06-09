using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMagician
{
    public class DelayedAudio : MonoBehaviour
    {
        [SerializeField] float secondAudioDelayTime;
        [SerializeField] AudioSource firstAudio;
        [SerializeField] AudioSource secondAudio;

        bool _isPlaying;
        float _currentTime;

        private void Start()
        {
            _isPlaying = false;
            _currentTime = 0f;
            PhaseManager.INSTANCE.OnEndPhase.AddListener(Complete);
        }

        private void OnDestroy()
        {
            PhaseManager.INSTANCE.OnEndPhase.RemoveListener(Complete);
        }

        private void Update()
        {
            if (!_isPlaying) return;
            if (!GameStateManager.IsInGameModeState()) return;

            _currentTime += Time.deltaTime;

            if (_currentTime >= secondAudioDelayTime)
            {
                _isPlaying = false;
                _currentTime = 0f;
                if(secondAudio != null)
                    secondAudio.Play();
            }
        }

        public void PlayAudioSequence()
        {
            _isPlaying = true;
            _currentTime = 0f;

            if(firstAudio != null)
                firstAudio.Play();
        }

        public void Complete()
        {
            if (!_isPlaying) return;
            if (!GameStateManager.IsInGameModeState()) return;

            _isPlaying = false;
            _currentTime = 0f;
        }
    }
}

