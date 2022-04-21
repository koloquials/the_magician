using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        /*[SerializeField] Color colorA;
        [SerializeField] Color colorB;*/
        //[SerializeField] float fadeTimeFromColorAToB;
        //[SerializeField] float fadeTimeFromColorBToA;
        [SerializeField] float fadeInTime;
        [SerializeField] float fadeOutTime;
        [SerializeField] AnimationCurve fadeInCurve;
        [SerializeField] AnimationCurve fadeOutCurve;
        [SerializeField] UnityEvent onFadeInComplete;
        [SerializeField] UnityEvent onFadeOutComplete;

        bool _isAnimating;
        float _currentTime;
        float _targetFadeTime;
        //Color _startColor;
        //Color _targetColor;
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

        private void Update()
        {
            if (!_isAnimating) return;
            if (!GameStateManager.IsInGameModeState()) return;

            _currentTime += Time.deltaTime;
            _currentTime = Mathf.Clamp(_currentTime, 0.0f, _targetFadeTime);

            float normalizedTime = (_currentTime / _targetFadeTime);

                Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(_startAlpha, _targetAlpha, _currentAnimationCurve.Evaluate(normalizedTime));
            spriteRenderer.color = color;

            if(_currentTime >= _targetFadeTime)
            {
                _fadeComplete.Invoke();
                _isAnimating = false;
                gameObject.SetActive(_gameobjectActiveAfterFade);
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
            _startAlpha = 0.0f;
            _targetAlpha = 1.0f;
            _currentTime = 0.0f;
            _targetFadeTime = fadeInTime;
            _currentAnimationCurve = fadeInCurve;
            _fadeComplete = onFadeInComplete;
            Stonefruit sf = GetComponent<Stonefruit>();
        }

        public void FadeOut()
        {
            gameObject.SetActive(true);
            _gameobjectActiveAfterFade = false;
            _isAnimating = true;
            Color color = spriteRenderer.color;
            color.a = 1.0f;
            spriteRenderer.color = color;
            _startAlpha = 1.0f;
            _targetAlpha = 0.0f;
            _currentTime = 0.0f;
            _targetFadeTime = fadeOutTime;
            _currentAnimationCurve = fadeOutCurve;
            _fadeComplete = onFadeOutComplete;
            Stonefruit sf = GetComponent<Stonefruit>();
        }
    }
}
