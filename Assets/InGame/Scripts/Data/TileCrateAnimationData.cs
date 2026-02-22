using DG.Tweening;
using UnityEngine;

namespace TileMatching.Data {
    [CreateAssetMenu(fileName = "TileCreateAnimationData", menuName = "GameData/AnimationData/TileCrateAnimationData", order = 0)]
    public class TileCrateAnimationData : ScriptableObject {    
        public float moveAnimationOffset = 1;
        public float moveAnimationTime = .2f;
        [Space]
        public float scaleAnimationTime = .25f;
        public Vector3 scaleAnimationFinalValue = Vector3.zero;
        public Ease scaleAnimationEase = Ease.InOutBounce;
    }
}