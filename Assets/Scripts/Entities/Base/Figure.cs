using UnityEngine;
using System.Collections.Generic;

namespace Entities.Base {
    public class Figure : MonoBehaviour {
        public PositionOnGrid Position;
        [SerializeField]
        public List<FigurePrefab> FiguresPrefabs = new List<FigurePrefab>();

        [System.Serializable]
        public class FigurePrefab {
            public FigureType type;
            public Mesh prefab;
        }

        [System.Serializable]
        public enum FigureType {
            Pawn,
            Rook,
            Queen,
            Elephant,
            King,
            Horse,
        }
    }
}