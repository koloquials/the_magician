using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class FogSpriteAnimator : MonoBehaviour
    {
        [SerializeField] SpriteRenderer[] spriteRenderers;
        [SerializeField] AnimationCurve fadeInCurve;
        [SerializeField] AnimationCurve fadeOutCurve;
        [SerializeField] AnimationCurve fadeInOutCurve;
        [SerializeField] UnityEvent onFadeInComplete;
        [SerializeField] UnityEvent onFadeOutComplete;
        [SerializeField] UnityEvent onFadeInOutComplete;

        bool _isAnimating;
        float _currentTime;
        float _targetFadeTime;
        AnimationCurve _currentAnimationCurve;
        bool _gameobjectActiveAfterFade;
        UnityEvent _fadeComplete;

        private void Awake()
        {
            _currentTime = 0.0f;
            _isAnimating = false;
        }

        private void Start()
        {
            PhaseManager.INSTANCE.OnEndPhase.AddListener(Complete);
        }

        private void OnDestroy()
        {
            PhaseManager.INSTANCE.OnEndPhase.RemoveListener(Complete);
        }

        private void Update()
        {
            if (!_isAnimating) return;
            if (!GameStateManager.IsInGameModeState()) return;

            _currentTime += Time.deltaTime;
            _currentTime = Mathf.Clamp(_currentTime, 0.0f, _targetFadeTime);

            float normalizedTime = (_currentTime / _targetFadeTime);

            foreach(SpriteRenderer spriteRenderer in spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = _currentAnimationCurve.Evaluate(normalizedTime);
                spriteRenderer.color = color;
            }

            if (_currentTime >= _targetFadeTime)
            {
                _isAnimating = false;
                gameObject.SetActive(_gameobjectActiveAfterFade);
                _fadeComplete.Invoke();

            }
        }

        // The bottom two functions will be called via events
        public void FadeIn(float fadeTime)
        {
            gameObject.SetActive(true);
            _gameobjectActiveAfterFade = true;
            _isAnimating = true;

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = 0.0f;
                spriteRenderer.color = color;
            }
   
            _currentTime = 0.0f;
            _targetFadeTime = fadeTime;
            _currentAnimationCurve = fadeInCurve;
            _fadeComplete = onFadeInComplete;
        }

        public void FadeOut(float fadeTime)
        {
            gameObject.SetActive(true);
            _gameobjectActiveAfterFade = false;
            _isAnimating = true;

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = 1.0f;
                spriteRenderer.color = color;
            }

            _currentTime = 0.0f;
            _targetFadeTime = fadeTime;
            _currentAnimationCurve = fadeOutCurve;
            _fadeComplete = onFadeOutComplete;
        }

        public void FadeInOut(float fadeTime)
        {
            gameObject.SetActive(true);
            _gameobjectActiveAfterFade = false;
            _isAnimating = true;
            _currentTime = 0.0f;
            _targetFadeTime = fadeTime;
            _currentAnimationCurve = fadeInOutCurve;
            _fadeComplete = onFadeInOutComplete;

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = fadeInOutCurve.Evaluate(0.0f);
                spriteRenderer.color = color;
            }
        }

        public void Complete()
        {
            if (!_isAnimating) return;
            if (!GameStateManager.IsInGameModeState()) return;
            _currentTime = _targetFadeTime;
        }
    }
}
