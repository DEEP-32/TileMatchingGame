using Dreamteck.Splines;
using TileMatching.Data;
using TileMatching.Utils;
using UnityEngine;

namespace TileMatching {
    public class Tile : MonoBehaviour {
        [SerializeField] SplineFollower splineFollower;
        [SerializeField] MeshRenderer meshRenderer;

        public TileColorKey CurrentColorKey {
            get;
            private set;
        }

        public void Initialize(SplineComputer spline,TileColorKey tileColorKey = TileColorKey.None,bool shouldAnimate = false) {
            splineFollower.spline = spline;

            if (tileColorKey != TileColorKey.None) {
                CurrentColorKey = tileColorKey;
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

        public void ReturnToPool() {
            ObjectPooler.Instance.ReturnToPool(gameObject);
        }

        public void ReparentToPool() {
            transform.SetParent(ObjectPooler.Instance.transform);
        }
    }
}