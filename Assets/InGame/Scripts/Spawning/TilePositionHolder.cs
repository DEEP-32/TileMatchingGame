using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TileMatching.Spawning {
    public class TilePositionHolder : MonoBehaviour {
        [FormerlySerializedAs("positions")] [SerializeField] List<Transform> points;
        public List<Transform> Points => points;
    }
}