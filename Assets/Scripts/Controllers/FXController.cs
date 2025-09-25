

using UnityEngine;
using System.Collections.Generic;

namespace Controllers {

    

    public class FXController : MonoBehaviour {
        public static FXController Instance;

        [SerializeField]
        private List<FX> _fxPrefabs = new List<FX>();
    
        [System.Serializable]
        public class FX {
            public FXType type;
            public GameObject prefab;
        }

        [System.Serializable]
        public enum FXType {
            Smoke,
        }

        public void Awake() {
            if(Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void PlayFX(FXType type, Vector3 position) {
            GameObject fx = Instantiate(_fxPrefabs.Find(x => x.type == type).prefab, position, Quaternion.identity);
        }


    }

}