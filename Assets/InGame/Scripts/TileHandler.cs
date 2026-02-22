using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Dreamteck.Splines;
using TileMatching.Spawning;
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

        public void AnimateTileFromSplineToEnd(Tile tile, TileCrate toCrate, Action onSingleTileComplete = null,Action onTileCrateFull = null) {
            
            var tilePositionHolder = toCrate.GetComponent<TilePositionHolder>();

            var point = tilePositionHolder.Points[toCrate.GetAndUpdateAnimatingIndex()];

            var posTween = tile.transform.DOMove(point.position, totalDuration);
            var rotTween = tile.transform.DORotateQuaternion(point.rotation, totalDuration);
            
            Sequence sequence = DOTween.Sequence();
            sequence.Append(posTween).Join(rotTween);

            sequence.OnComplete(() => {
                toCrate.AddTile(tile);
                onSingleTileComplete?.Invoke();
                if (toCrate.IsAtMaxIndex()) {
                    onTileCrateFull?.Invoke();
                }
            });
        }
    }
}