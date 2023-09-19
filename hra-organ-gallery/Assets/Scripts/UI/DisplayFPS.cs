using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.UI
{
    public class DisplayFPS : MonoBehaviour
    {
        public float updateDelay = 0f;

        private float _targetFPS = 72f;
        private float _midFPS = 60f;
        private float _currentFPS = 0f;
        private float _deltaTime = 0f;
        [SerializeField] private Color _highColor = new Color32(0, 177, 215, 255);
        [SerializeField] private Color _midColor = Color.white;
        [SerializeField] private Color _low_color = new Color32(200, 68, 124, 255);

        private TextMeshProUGUI _textFPS;
        // Start is called before the first frame update
        void Start()
        {
            _textFPS = GetComponent<TextMeshProUGUI>();
            StartCoroutine(DisplayFramesPerSecond());
        }

        // Update is called once per frame
        void Update()
        {

            GenerateFramesPerSecond();
        }

        private void GenerateFramesPerSecond()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * .1f;
            _currentFPS = 1.0f / _deltaTime;
        }

        private IEnumerator DisplayFramesPerSecond()
        {
            while (true)
            {
                if (_currentFPS >= _targetFPS)
                {
                    _textFPS.color = _highColor;
                }
                else if(_currentFPS < _targetFPS && _currentFPS >= _midFPS)
                {
                    _textFPS.color = _midColor;
                }
                else
                {
                    _textFPS.color = _low_color;
                }
                _textFPS.text = "FPS: " + _currentFPS.ToString(".0");
                yield return new WaitForSeconds(updateDelay);
            }

        }
    }
}