using System;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using TileMatching.Spawning;
using UnityEngine;

namespace TileMatching {
    public class TileCrate : MonoBehaviour {
        [SerializeField] TilePositionHolder pointsHolder;
        [SerializeField] int triggerIndex = 0; 

        List<Tile> tiles = new List<Tile>();
        int currentAnimatingIndex = 0;

        public int CurrentIndex {
            get; 
            private set;
        }
        
        public SplineTrigger SplineTrigger {
            get; 
            private set;
        }

        public bool ShouldAnimateToCrate {
            get;

            private set;
        } = true;


        
        public int TriggerIndex => triggerIndex;
        
        

        public void Initialize(SplineTrigger trigger) {
            SplineTrigger = trigger;
            var pointsCount = pointsHolder.Points.Count;
            tiles.Capacity = pointsCount;
        }

        public void AddTile(Tile tile) {
            tiles.Insert(CurrentIndex++, tile);
        }

        public void ConsumeTiles() {
            ShouldAnimateToCrate = false;
            StartEndAnimation();
        }

        public bool IsAtMaxIndex() {
            return CurrentIndex >= pointsHolder.Points.Count;
        }

        public int GetAndUpdateAnimatingIndex() {
            return currentAnimatingIndex++;
        }

        private void StartEndAnimation() {
            var startScale = transform.localScale;
            transform.DOScale(Vector3.zero, .25f).SetEase(Ease.InOutBounce).
                OnComplete(() => {
                    ConsumeTile_Internal();
                    transform.DOScale(startScale, .25f).SetEase(Ease.InOutBounce).OnComplete(() => {
                        currentAnimatingIndex = 0;
                    });
                });
        }

        private void ConsumeTile_Internal() {
            foreach (var tile in tiles) {
                tile.ReturnToPool();
                tile.transform.position = Vector3.zero;
            }
            tiles.Clear();
            CurrentIndex = 0;
            currentAnimatingIndex = 0;
        }
    }
}