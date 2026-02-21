using Dreamteck.Splines;
using TileMatching.Data;
using UnityEngine;

namespace TileMatching {
    public class Tile : MonoBehaviour {
        [SerializeField] SplineFollower splineFollower;
        [SerializeField] MeshRenderer meshRenderer;

        public void Initialize(SplineComputer spline,TileColorKey tileColorKey = TileColorKey.None) {
            splineFollower.spline = spline;

            if (tileColorKey != TileColorKey.None) {
                meshRenderer.material = GameManager.Instance.GameConfig.GetTimeMatcherDataFor(tileColorKey).Material;
            }
        }
        
        public void AllowSplineMovement(bool shouldMove,double startPointRatio = 0) {
            splineFollower.enabled = shouldMove;
            if (shouldMove) {
                splineFollower.SetPercent(startPointRatio);
                Debug.Log("Set percent to " + startPointRatio + " for " + splineFollower.name);
                splineFollower.follow = true;
            }
            
        }
    }
}