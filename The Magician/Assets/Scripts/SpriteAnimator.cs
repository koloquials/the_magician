using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Color colorA;
        [SerializeField] Color colorB;
        [SerializeField] float fadeTimeFromColorAToB;
        [SerializeField] AnimationCurve fadeCurveFromAToB;
        [SerializeField] float fadeTimeFromColorBToA;
        [SerializeField] AnimationCurve fadeCurveFromBToA;
        [SerializeField] UnityEvent onFadeComplete;

        bool _isAnimating;
        float _currentTime;
        float _targetFadeTime;
        Color _startColor;
        Color _targetColor;
        AnimationCurve _currentAnimationCurve;

        private void Start()
        {
            _currentTime = 0.0f;
            _isAnimating = false;
        }

        private void Update()
        {
            if (!_isAnimating) return;
            Debug.Log("current time: " + _currentTime);
            _currentTime += Time.deltaTime;
            _currentTime = Mathf.Clamp(_currentTime, 0, _targetFadeTime);
            float normalizedTime = (_currentTime / _targetFadeTime);
            spriteRenderer.color = Color.Lerp(_startColor, _targetColor, _currentAnimationCurve.Evaluate(normalizedTime));

            if(_currentTime >= _targetFadeTime)
            {
                onFadeComplete.Invoke();
                _isAnimating = false;
            }
        }

        // The bottom two functions will be called via events
        public void FadeFromColorAToB()
        {
            _isAnimating = true;
            spriteRenderer.color = colorA;
            _startColor = colorA;
            _targetColor = colorB;
            _currentTime = 0.0f;
            _targetFadeTime = fadeTimeFromColorAToB;
            _currentAnimationCurve = fadeCurveFromAToB;
        }

        public void FadeFromColorBToA()
        {
            _isAnimating = true;
            spriteRenderer.color = colorB;
            _startColor = colorB;
            _targetColor = colorA;
            _currentTime = 0.0f;
            _targetFadeTime = fadeTimeFromColorBToA;
            _currentAnimationCurve = fadeCurveFromBToA;
        }
    }
}
