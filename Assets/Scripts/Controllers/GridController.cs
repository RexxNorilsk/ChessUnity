using UnityEngine;
using System.Collections.Generic;
using System;

namespace Controllers {

    public class GridController : MonoBehaviour {

        public static GridController Instance;
        [SerializeField] private GameObject _prefabPlace;
        [SerializeField] private Transform _gridTransform;
        [SerializeField] private int _gridSize = 8;
        [SerializeField] private float _gap = 0.1f;
        [SerializeField] private List<Material> _gridMaterials;

        // Настройки анимации
        [SerializeField, Range(0f, 1f)]
        private float _timelineSpread = 0.6f; // доля timeline, в которой растянут старт слоёв
        [SerializeField] private float _animationDuration = 2f;
        [SerializeField] private float _controlPointHeight = 0.85f; // высота control points для bezier (регулирует overshoot)
        [SerializeField] private float _waveAmplitude = 0.05f; // мелкие колебания сверху

        private float _animationProgress = 0f;
        private bool _isAnimating = true;

        private Action _callbackInit;

        private List<GameObject> _gridCells = new List<GameObject>();

        public void Awake() {
            if(Instance == null) Instance = this;
            else Destroy(gameObject);
            _animationProgress = 0f;
        }

        public Transform GetGridTransform() {
            return _gridTransform;
        }

        public Vector3 GetCellPosition(int x, int z) {
            return _gridCells[x * _gridSize + z].transform.localPosition;
        }

        public void Init(Action callback) {
            _gridCells.Clear();
            _callbackInit = callback;
            for(int i = 0; i < _gridSize; i++) {
                for(int j = 0; j < _gridSize; j++) {
                    GameObject cell = Instantiate(_prefabPlace, _gridTransform);
                    cell.transform.localPosition = new Vector3(i * _gap, -1f, j * _gap);
                    _gridCells.Add(cell);
                    int colorIndex = (i + j) % 2;
                    if (_gridMaterials != null && _gridMaterials.Count > 0)
                        cell.GetComponent<Renderer>().material = _gridMaterials[colorIndex % _gridMaterials.Count];
                }
            }
        }

        public void Update() {
            if (_isAnimating) AnimateGrid();
            else if (_callbackInit != null) {
                _callbackInit.Invoke();
                _callbackInit = null;
            }
        }

        private void SetAnimationStatus(bool isAnimating) {
            _isAnimating = isAnimating;
        }

        public void AnimateGrid() {
            _animationProgress += Time.deltaTime / Mathf.Max(0.0001f, _animationDuration);
            if (_animationProgress >= 1f) {
                _animationProgress = 1f;
                SetAnimationStatus(false);

                for (int i = 0; i < _gridCells.Count; i++) {
                    int x = i % _gridSize;
                    int z = i / _gridSize;
                    _gridCells[i].transform.localPosition = new Vector3(x * _gap, 0f, z * _gap);
                }
                return;
            }

            int maxLayer = (_gridSize - 1) * 2;
            if (maxLayer <= 0) maxLayer = 1;

            for (int i = 0; i < _gridCells.Count; i++) {
                int x = i % _gridSize;
                int z = i / _gridSize;

                int layerId = x + z;
                float normalizedLayer = (float)layerId / (float)maxLayer; 

                float layerStart = normalizedLayer * Mathf.Clamp01(_timelineSpread);
                float layerDuration = Mathf.Clamp01(1f - _timelineSpread); 

                float localProgress = Mathf.Clamp01((_animationProgress - layerStart) / Mathf.Max(0.0001f, layerDuration));

                float eased = Mathf.SmoothStep(0f, 1f, localProgress);

                float y = OvershootBezier(eased, -1f, _controlPointHeight, _controlPointHeight, 0f);
                float oscillation = Mathf.Sin(eased * Mathf.PI * 2f + normalizedLayer * 1.2f) * _waveAmplitude * (1f - eased);
                y += oscillation;

                Vector3 newPos = new Vector3(x * _gap, y, z * _gap);
                _gridCells[i].transform.localPosition = newPos;
            }
        }

        private float OvershootBezier(float t, float y0, float y1, float y2, float y3) {
            float inv = 1f - t;
            return inv * inv * inv * y0
                 + 3f * inv * inv * t * y1
                 + 3f * inv * t * t * y2
                 + t * t * t * y3;
        }
    }
}
