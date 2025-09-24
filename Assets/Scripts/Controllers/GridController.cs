using UnityEngine;
using System.Collections.Generic;

namespace Controllers {

    public class GridController : MonoBehaviour {

        public static GridController Instance;
        [SerializeField] private GameObject _prefab;
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

        private List<GameObject> _gridCells = new List<GameObject>();

        public void Awake() {
            if(Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void Start() {
            Init();
            _animationProgress = 0f;
        }

        public void Init() {
            _gridCells.Clear();

            for(int i = 0; i < _gridSize; i++) {
                for(int j = 0; j < _gridSize; j++) {
                    GameObject cell = Instantiate(_prefab, _gridTransform);
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
        }

        // ---------- Основной метод анимации (заменяет старый AnimateGrid) ----------
        public void AnimateGrid() {
            // прогресс (0..1)
            _animationProgress += Time.deltaTime / Mathf.Max(0.0001f, _animationDuration);
            if (_animationProgress >= 1f) {
                _animationProgress = 1f;
                _isAnimating = false;

                // финальные позиции
                for (int i = 0; i < _gridCells.Count; i++) {
                    int x = i % _gridSize;
                    int z = i / _gridSize;
                    _gridCells[i].transform.localPosition = new Vector3(x * _gap, 0f, z * _gap);
                }
                return;
            }

            // максимальный id слоя при диагональной разбивке (x + z)
            int maxLayer = (_gridSize - 1) * 2;
            if (maxLayer <= 0) maxLayer = 1;

            for (int i = 0; i < _gridCells.Count; i++) {
                int x = i % _gridSize;
                int z = i / _gridSize;

                // layer id: используем диагональную разбивку (x + z)
                int layerId = x + z;
                float normalizedLayer = (float)layerId / (float)maxLayer; // 0..1

                // вычисляем, когда начинается анимация этого слоя в пределах 0..1
                float layerStart = normalizedLayer * Mathf.Clamp01(_timelineSpread);
                float layerDuration = Mathf.Clamp01(1f - _timelineSpread); // оставшаяся часть timeline для завершения

                // локальный прогресс слоя (0..1)
                float localProgress = Mathf.Clamp01((_animationProgress - layerStart) / Mathf.Max(0.0001f, layerDuration));

                // сглаживаем локальный прогресс (можно менять тип easing)
                float eased = Mathf.SmoothStep(0f, 1f, localProgress);

                // непрерывная кривая y: кубическая bezier от -1 -> control -> control -> 0
                float y = OvershootBezier(eased, -1f, _controlPointHeight, _controlPointHeight, 0f);

                // небольшие убывающие колебания сверху для живости (гаснут к концу локальной анимации)
                float oscillation = Mathf.Sin(eased * Mathf.PI * 2f + normalizedLayer * 1.2f) * _waveAmplitude * (1f - eased);
                y += oscillation;

                Vector3 newPos = new Vector3(x * _gap, y, z * _gap);
                _gridCells[i].transform.localPosition = newPos;
            }
        }

        // Кубическая bezier для построения единой непрерывной траектории (даёт overshoot в середине)
        private float OvershootBezier(float t, float y0, float y1, float y2, float y3) {
            float inv = 1f - t;
            // B(t) = (1-t)^3*P0 + 3*(1-t)^2*t*P1 + 3*(1-t)*t^2*P2 + t^3*P3
            return inv * inv * inv * y0
                 + 3f * inv * inv * t * y1
                 + 3f * inv * t * t * y2
                 + t * t * t * y3;
        }
    }
}
