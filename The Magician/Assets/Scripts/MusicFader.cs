using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMagician
{
    public class MusicFader : MonoBehaviour
    {
        [SerializeField] AudioSource track;
        [SerializeField] float fadeInAmount;
        [SerializeField] float fadeOutAmount;
        [SerializeField] float fadeTime;

        float _currentTime;
        bool _isFading;
        float _startVolume;
        float _targetVolume;

        private void Start()
        {
            _currentTime = 0f;
            _isFading = false;
        }

        private void Update()
        {
            if (!_isFading) return;
            if (!GameStateManager.IsInGameModeState()) return;

            _currentTime += Time.deltaTime;

            float progress = Mathf.Clamp01(_currentTime / fadeTime);
            track.volume = Mathf.Lerp(_startVolume, _targetVolume, progress);

            if (progress >= 1.0f)
            {
                _isFading = false;
                _currentTime = 0f;
            }
        }

        public void FadeOutByDecrement()
        {
            _currentTime = 0f;
            _isFading = true;
            _startVolume = track.volume;
            _targetVolume = Mathf.Clamp01(_startVolume - fadeOutAmount);
        }

        public void FadeInByIncrement()
        {
            _currentTime = 0f;
            _isFading = true;
            _startVolume = track.volume;
            _targetVolume = Mathf.Clamp01(_startVolume + fadeInAmount);
        }

        public void PlayTrack()
        {
            track.Play();
        }

        public void StopTrack()
        {
            track.Stop();
        }

        public void SetTrackVolume(float val)
        {
            track.volume = val;
        }

        public void FadeInCompletely()
        {
            _isFading = true;
            _startVolume = 0f;
            _targetVolume = 1f;
        }

        public void FadeOutCompletely()
        {
            _isFading = true;
            _startVolume = 1f;
            _targetVolume = 0f;
        }

        public void FadeInToTarget()
        {
            _isFading = true;
            _startVolume = track.volume;
        }

        public void SetFadeInAmount(float val)
        {
            fadeInAmount = val;
        }

        public void SetFadeOutAmount(float val)
        {
            fadeOutAmount = val;
        }

        public void SetFadeTime(float val)
        {
            fadeTime = val;
        }

        public void SetStartVolume(float val)
        {
            _startVolume = val;
        }

        public void SetTargetVolume(float val)
        {
            _targetVolume = val;
        }
    }
}
