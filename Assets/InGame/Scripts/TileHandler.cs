using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Dreamteck.Splines;
using TileMatching.Utils;
using UnityEngine;

namespace TileMatching {
    public class TileHandler : Singleton<TileHandler> {
        List<TileCrate> tileCrates = new List<TileCrate>();
        
        //tiles that are on splines currently.
        List<SplineFollower> activeFollowers = new List<SplineFollower>();
        
        [SerializeField] float totalDuration = 1f;
        [SerializeField] float startDelay = .2f;


        protected override void Awake() {
            base.Awake();
            tileCrates = FindObjectsByType<TileCrate>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
        }

        public void AnimateTilesToSpline(List<Tile> tiles,Transform toTransform ,Action<Tile> onSingleTileReached = null, Action onComplete = null) {
            if (tiles == null || tiles.Count == 0) {
                return;
            }

            int remainingTiles = tiles.Count;
            for (var i = 0; i < tiles.Count; i++) {
                Tile tile = tiles[i];
                float delay = i * startDelay;
                
                Debug.Log($"AnimateTiles: is tile null : {tile == null} for index : {i}");
                
                var posTween = tile.transform.DOMove(toTransform.position, totalDuration);
                var rotTween = tile.transform.DORotateQuaternion(toTransform.rotation, totalDuration);
                
                Sequence sequence = DOTween.Sequence();
                sequence.Append(posTween).Join(rotTween);
                sequence.SetDelay(delay);
                sequence.OnComplete(() => {
                    onSingleTileReached?.Invoke(tile);
                    activeFollowers.Add(tile.GetComponent<SplineFollower>());
                    remainingTiles--;
                    if (remainingTiles == 0) onComplete?.Invoke();
                });
            }
        }

        public void AnimateTileFromSplineToEnd(Tile tile, Transform toCrate, Action onComplete = null) {
            
        }

        /// <summary>
        /// should be attached to trigger on cross function so that it can check whether is there any tile near it ,
        /// and call the correct callback for that tile
        /// </summary>
        public void OnTileTriggerEnter(int triggerIndex) {
            var currentSpline = GameManager.Instance.LevelDataHolder.SplineComputer;
            var group = currentSpline.triggerGroups[0];
            var splineTrigger = group.triggers[triggerIndex];
            double triggerPos = splineTrigger.position;
            FindClosestFollower(triggerPos, out SplineFollower closestFollower);

            if (closestFollower != null) {
                //AnimateTileFromSplineToEnd();
                Debug.Log($"Tile enter trigger of index {triggerPos} for tile : {closestFollower.gameObject.name}");
            }
        }

        void FindClosestFollower(double triggerPos, out SplineFollower closestFollower) {
            double minDist = double.MaxValue;

            foreach (var follower in activeFollowers) {
                if(follower.triggerGroup != 0) continue;

                double dist = follower.result.percent - triggerPos;

                if (dist < minDist) {
                    minDist = dist;
                    closestFollower = follower;
                }
            }
            closestFollower = null;
        }
    }
}