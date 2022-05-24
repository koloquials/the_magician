using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] float fadeInTime;
        [SerializeField] float fadeOutTime;
        [SerializeField] float fadeInOutTime;
        [SerializeField] AnimationCurve fadeInCurve;
        [SerializeField] AnimationCurve fadeOutCurve;
        [SerializeField] AnimationCurve fadeInOutCurve;
        [SerializeField] UnityEvent onFadeInComplete;
        [SerializeField] UnityEvent onFadeOutComplete; 
        [SerializeField] UnityEvent onFadeInOutComplete;

        bool _isAnimating;
        float _currentTime;
        float _targetFadeTime;
        float _startAlpha;
        float _targetAlpha;
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

                Color color = spriteRenderer.color;
            //color.a = Mathf.Lerp(_startAlpha, _targetAlpha, _currentAnimationCurve.Evaluate(normalizedTime));
            Debug.Log("current alpha val; " + _currentAnimationCurve.Evaluate(normalizedTime));
            color.a = _currentAnimationCurve.Evaluate(normalizedTime);
            spriteRenderer.color = color;

            if(_currentTime >= _targetFadeTime)
            {
                _isAnimating = false;
                gameObject.SetActive(_gameobjectActiveAfterFade);
                _fadeComplete.Invoke();

            }
        }

        // The bottom two functions will be called via events
        public void FadeIn()
        {
            gameObject.SetActive(true);
            _gameobjectActiveAfterFade = true;
            _isAnimating = true;
            Color color = spriteRenderer.color;
            color.a = 0.0f;
            spriteRenderer.color = color;
            /*_startAlpha = 0.0f;
            _targetAlpha = 1.0f;*/
            _currentTime = 0.0f;
            _targetFadeTime = fadeInTime;
            _currentAnimationCurve = fadeInCurve;
            _fadeComplete = onFadeInComplete;
        }

        public void FadeOut()
        {
            gameObject.SetActive(true);
            _gameobjectActiveAfterFade = false;
            _isAnimating = true;
            Color color = spriteRenderer.color;
            color.a = 1.0f;
            spriteRenderer.color = color;
            /*_startAlpha = 1.0f;
            _targetAlpha = 0.0f;*/
            _currentTime = 0.0f;
            _targetFadeTime = fadeOutTime;
            _currentAnimationCurve = fadeOutCurve;
            _fadeComplete = onFadeOutComplete;
        }

        public void FadeInOut()
        {
            gameObject.SetActive(true);
            _gameobjectActiveAfterFade = false;
            _isAnimating = true;
            _startAlpha = fadeInOutCurve.Evaluate(0.0f);
            _targetAlpha = fadeInOutCurve.Evaluate(1.0f);
            _currentTime = 0.0f;
            _targetFadeTime = fadeInOutTime;
            _currentAnimationCurve = fadeInOutCurve;
            _fadeComplete = onFadeInOutComplete;
            Color color = spriteRenderer.color;
            color.a = _startAlpha;
            spriteRenderer.color = color;
        }

        public void Complete()
        {
            if (!_isAnimating) return;
            if (!GameStateManager.IsInGameModeState()) return;
            _currentTime = _targetFadeTime;
        }
    }
}
