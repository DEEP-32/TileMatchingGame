using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TileMatching.Utils;
using UnityEngine;

namespace TileMatching {
    public class TileHandler : PersistentSingleton<TileHandler> {

        List<TileCrate> tileCrates = new List<TileCrate>();
        
        [SerializeField] float totalDuration = 1f;
        [SerializeField] float startDelay = .2f;


        protected override void Awake() {
            base.Awake();
            tileCrates = FindObjectsByType<TileCrate>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
        }

        public void AnimateTiles(List<Tile> tiles,Transform toTransform ,Action<Tile> onSingleTileReached = null, Action onComplete = null) {
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
                    remainingTiles--;
                    if (remainingTiles == 0) onComplete?.Invoke();
                });
            }
        }
    }
}