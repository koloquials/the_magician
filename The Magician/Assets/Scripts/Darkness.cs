using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheMagician
{
    public class Darkness : MonoBehaviour
    {

        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Color targetColor;
        [SerializeField] float fadeTime;
        [SerializeField] int timesUntilDark;
        [SerializeField] AnimationCurve darknessCurve;
        [SerializeField] UnityEvent onDarknessComplete;

        bool _isAnimating;
        float _currentTime;
        float _targetFadeTime;
        Color _targetColor;
        Color _startColor;
        int _timesUntilDark;
        int _currentTimesUntilDark;

        float _epsilon = 0.01f;
        bool IsBlack(Color color) => color == Color.black || (color.r < (0.0f + _epsilon) && color.g < (0.0f + _epsilon) && color.b < (0.0f + _epsilon));

        private void Awake()
        {
            _currentTime = 0.0f;
            _isAnimating = false;
            _targetColor = targetColor;
            _targetFadeTime = fadeTime;
            _startColor = spriteRenderer.color;
            _timesUntilDark = timesUntilDark;
            _currentTimesUntilDark = 0;
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
            color = Color.Lerp(_startColor, _targetColor, darknessCurve.Evaluate(normalizedTime));
            spriteRenderer.color = color;

            if (IsBlack(color))
            {
                spriteRenderer.color = _targetColor;
                _isAnimating = false;
                onDarknessComplete?.Invoke();
            }
        }

        // The bottom two functions will be called via events
        public void FadeToBlackByAlreadySetAmount()
        {
            _isAnimating = true;
            _currentTime = 0.0f;
            _targetFadeTime = fadeTime;
            _startColor = spriteRenderer.color;
            _currentTimesUntilDark++;
            _targetColor = Color.white;
            _targetColor.r *= (_timesUntilDark - _currentTimesUntilDark) / (float)_timesUntilDark;
            _targetColor.g *= (_timesUntilDark - _currentTimesUntilDark) / (float)_timesUntilDark;
            _targetColor.b *= (_timesUntilDark - _currentTimesUntilDark) / (float)_timesUntilDark;
            Debug.Log("target color: " + _targetColor);
        }

        public void SetTargetFadeTime(float fadeTime)
        {
            _targetFadeTime = fadeTime;
        }

        public void SetClickTimesUntilDark(int val)
        {
            _timesUntilDark = val;
            spriteRenderer.color = Color.white;
        }

        public void Complete()
        {
            if (!_isAnimating) return;
            if (!GameStateManager.IsInGameModeState()) return;
            _currentTime = _targetFadeTime;
        }
    }

}