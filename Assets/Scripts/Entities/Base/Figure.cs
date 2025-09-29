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


        public void SetType(FigureType type, Material material) {
            GetComponent<MeshFilter>().mesh = FiguresPrefabs.Find(x => x.type == type).prefab;
            GetComponent<Renderer>().material = material;
        }

    }
}