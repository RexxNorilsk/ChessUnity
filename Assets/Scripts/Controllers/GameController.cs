using UnityEngine;
using System.Collections.Generic;
using Entities.Base;
using Controllers;

namespace Controllers {

    public class GameController : MonoBehaviour {
        public static GameController Instance;

        [SerializeField]
        private GameObject _figurePrefab;
        [SerializeField]
        private Vector3 _figureOffset = Vector3.zero;
        [SerializeField]
        private float _animationStep = 0.1f;
        [SerializeField]
        private Material _whiteMaterial;
        [SerializeField]
        private Material _blackMaterial;
        private bool _isAnimating = false;
        private float _animationProgress = 0f;
        private int _currentStep = 0;

        private List<Figure> _figures = new List<Figure>();

        public void Awake() {
            if(Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void Start() {
            GridController.Instance.Init(Init);
        }

        public void Update() {
            AnimateStep();
        }


        private void AnimateStep() {
            if(_isAnimating) {
                _animationProgress += Time.deltaTime;
                if(_animationProgress >= _animationStep) {
                    _animationProgress = 0;
                    GenerateFigure(_currentStep);
                    _currentStep++;
                    if(_currentStep >= 32) {
                        _isAnimating = false;
                    }
                }
            }
        }

        public void Init() {
            _isAnimating = true;
        }

        private void GenerateFigure(int figureIndex = 0) {
            bool white = figureIndex % 2 == 0;
            int teamIdx = figureIndex / 2;
            int i = teamIdx % 8;
            int j = white ? (0 + (teamIdx>=8? 1 : 0)) : (7 - (teamIdx>=8? 1 : 0));
            GameObject figure = Instantiate(_figurePrefab, Vector3.zero, Quaternion.identity, GridController.Instance.GetGridTransform());
            figure.transform.rotation = Quaternion.Euler(0, (white ? 90 : 270), 0);
            figure.transform.localPosition = GridController.Instance.GetCellPosition(i, j) + 
                (new Vector3(
                    _figureOffset.x* (white ? -1 : 1), 
                    _figureOffset.y,
                    _figureOffset.z * (white ? -1 : 1))
                );
            Figure figureComponent = figure.GetComponent<Figure>();
            _figures.Add(figureComponent);
            Material material = white ? _whiteMaterial : _blackMaterial;
            if(teamIdx >= 8) {
                figureComponent.SetType(Figure.FigureType.Pawn, material);
            }
            if(teamIdx == 0) {
                figureComponent.SetType(Figure.FigureType.Rook, material);
            }
            if(teamIdx == 1) {
                figureComponent.SetType(Figure.FigureType.Horse, material);
            }
            if(teamIdx == 2) {
                figureComponent.SetType(Figure.FigureType.Elephant, material);
            }
            if(teamIdx == 3) {
                figureComponent.SetType(Figure.FigureType.Queen, material);
            }
            if(teamIdx == 4) {
                figureComponent.SetType(Figure.FigureType.King, material);
            }
            if(teamIdx == 5) {
                figureComponent.SetType(Figure.FigureType.Elephant, material);
            }
            if(teamIdx == 6) {
                figureComponent.SetType(Figure.FigureType.Horse, material);
            }
            if(teamIdx == 7) {
                figureComponent.SetType(Figure.FigureType.Rook, material);
            }

            FXController.Instance.PlayFX(FXController.FXType.Smoke, figure.transform.position);
        }
    }
}
